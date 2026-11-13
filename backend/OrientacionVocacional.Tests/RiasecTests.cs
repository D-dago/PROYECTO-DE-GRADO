using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Seed;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Infrastructure.Persistence;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class RiasecTests
{
    private sealed class NoAi : IAiExplanationService
    {
        public Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken) => Task.FromResult("Explicación mediante IA no disponible actualmente.");
    }
    [Fact]
    public void Seed_has_sixty_active_riasec_questions_ten_per_dimension()
    {
        var questions = RiasecQuestionSeed.Create();
        var active = questions.Where(question => question.IsActive).ToList();

        Assert.Equal(60, active.Count);
        Assert.Equal(questions.Count, questions.Select(question => question.Code).Distinct().Count());
        Assert.Equal(questions.Count, questions.Select(question => question.Id).Distinct().Count());
        foreach (var dimension in new[] { "R", "I", "A", "S", "E", "C" })
        {
            Assert.Equal(10, active.Count(question => question.Dimension == dimension));
        }
        // Las preguntas H y D del CHASIDE que se aplico un tiempo se conservan, pero inactivas.
        var legacy = questions.Where(question => !question.IsActive).ToList();
        Assert.Equal(17, legacy.Count);
        Assert.All(legacy, question => Assert.Contains(question.Dimension, new[] { "H", "D" }));
        Assert.All(legacy, question => Assert.True(question.Order >= 1000));
        // Banco corregido (revision de Gabriela Lino): todas preguntan por intereses y el ejemplo va tras "Ejemplo:".
        Assert.All(active, question => Assert.StartsWith("¿Qué tanto te interesa", question.Text));
        Assert.All(active, question => Assert.DoesNotContain("(ej.:", question.Text));
    }

    [Fact]
    public void Scoring_is_deterministic_and_uses_fixed_tie_order()
    {
        var answers = RiasecQuestionSeed.Create()
            .Where(question => question.IsActive)
            .Select(question => new Answer { QuestionId = question.Id, Question = question, Value = 5 })
            .ToArray();
        var scoring = new RiasecScoringService();

        var first = scoring.Calculate(answers);
        var second = scoring.Calculate(answers);

        Assert.Equal(first, second);
        // Las 6 dimensiones empatan con 10 preguntas (50): gana el orden fijo R,I,A.
        Assert.Equal("RI", first.PrimaryCode);
        Assert.Equal("A", first.SecondaryCode);
        Assert.Equal(50, first.RScore);
        Assert.Equal(50, first.CScore);
    }

    [Fact]
    public async Task Attempt_gets_the_same_twenty_four_questions_four_per_dimension()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);

        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.NotNull(assigned);
        Assert.Equal(24, assigned!.Count);
        Assert.Equal(24, assigned.Select(question => question.Id).Distinct().Count());
        foreach (var dimension in new[] { "R", "I", "A", "S", "E", "C" })
        {
            Assert.Equal(4, assigned.Count(question => question.Dimension == dimension));
        }
        // Nunca dos preguntas seguidas de la misma dimension.
        for (var index = 1; index < assigned.Count; index++)
        {
            Assert.NotEqual(assigned[index - 1].Dimension, assigned[index].Dimension);
        }
        Assert.Equal(24, attempt.Total);
    }

    [Fact]
    public async Task Resuming_an_attempt_keeps_the_same_assigned_questions()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var created = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var firstDraw = await service.GetAssignedQuestionsAsync(userId, created.AttemptId, CancellationToken.None);

        var resumed = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var secondDraw = await service.GetAssignedQuestionsAsync(userId, resumed.AttemptId, CancellationToken.None);

        Assert.Equal(created.AttemptId, resumed.AttemptId);
        Assert.Equal(firstDraw!.Select(q => q.Id), secondDraw!.Select(q => q.Id));
    }

    [Fact]
    public async Task Inactive_questions_are_never_assigned()
    {
        await using var context = CreateContext();
        var inactiveQuestion = await context.Questions.FirstAsync(question => question.Dimension == "C" && question.IsActive);
        inactiveQuestion.IsActive = false;
        await context.SaveChangesAsync();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);

        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.Equal(4, assigned!.Count(question => question.Dimension == "C"));
        Assert.DoesNotContain(assigned!, question => question.Id == inactiveQuestion.Id);
    }

    [Fact]
    public async Task Answer_value_must_be_between_one_and_five()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);
        var questionId = assigned![0].Id;

        var zero = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = questionId, Value = 0 }, CancellationToken.None);
        var six = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = questionId, Value = 6 }, CancellationToken.None);

        Assert.False(zero.Success);
        Assert.False(six.Success);
    }

    [Fact]
    public async Task Answer_values_one_and_five_are_accepted()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        var one = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = assigned![0].Id, Value = 1 }, CancellationToken.None);
        var five = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = assigned[1].Id, Value = 5 }, CancellationToken.None);

        Assert.True(one.Success);
        Assert.True(five.Success);
    }

    [Fact]
    public async Task Duplicate_question_answer_is_rejected()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);
        var request = new OrientacionVocacional.Application.Contracts.SubmitAnswerRequest { QuestionId = assigned![0].Id, Value = 1 };

        var first = await service.SubmitAnswerAsync(userId, attempt.AttemptId, request, CancellationToken.None);
        var duplicate = await service.SubmitAnswerAsync(userId, attempt.AttemptId, request, CancellationToken.None);

        Assert.True(first.Success);
        Assert.False(duplicate.Success);
    }

    [Fact]
    public async Task Answering_a_question_not_assigned_to_the_attempt_is_rejected()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);
        var assignedIds = assigned!.Select(question => question.Id).ToHashSet();
        var allQuestions = await context.Questions.ToListAsync();
        var unassignedQuestion = allQuestions.First(question => !assignedIds.Contains(question.Id));

        var result = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = unassignedQuestion.Id, Value = 3 }, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Esa pregunta no forma parte de este intento.", result.Error);
    }

    [Fact]
    public async Task Cannot_complete_with_twenty_three_answers_but_can_with_twenty_four()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        foreach (var question in assigned!.Take(23))
        {
            var response = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = question.Id, Value = 3 }, CancellationToken.None);
            Assert.True(response.Success);
        }

        var incomplete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.False(incomplete.Success);
        Assert.Equal("El test debe tener 24 respuestas antes de finalizar.", incomplete.Error);

        var last = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = assigned![23].Id, Value = 3 }, CancellationToken.None);
        var complete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.True(last.Success);
        Assert.True(complete.Success);
        Assert.NotNull(complete.Result);
        Assert.Equal(12, complete.Result!.RScore); // 4 preguntas x valor 3
        Assert.Equal(20, complete.Result.MaxScore);
        Assert.Equal(RiasecTestConstants.Version, complete.Result.AlgorithmVersion);
        Assert.Equal("RI", complete.Result.PrimaryCode); // empate total: orden fijo R,I,A,S,E,C
        Assert.Equal("A", complete.Result.SecondaryCode);
        Assert.Equal(24, last.Attempt!.Coins);

        var afterCompletion = await service.GetAttemptAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.Equal(74, afterCompletion!.Coins); // 24 respuestas + 50 de bono

        var completedAgain = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        var afterSecondComplete = await service.GetAttemptAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.True(completedAgain.Success);
        Assert.Equal(74, afterSecondComplete!.Coins);
    }

    [Fact]
    public async Task Student_cannot_read_another_students_attempt()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var firstUser = await TestSupport.CreatePersistedStudentAsync(context);
        var secondUser = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(firstUser, CancellationToken.None);

        var visible = await service.GetAttemptAsync(firstUser, attempt.AttemptId, CancellationToken.None);
        var hidden = await service.GetAttemptAsync(secondUser, attempt.AttemptId, CancellationToken.None);

        Assert.NotNull(visible);
        Assert.Null(hidden);
    }

    [Fact]
    public async Task Catalog_rules_filters_and_history_use_persisted_data()
    {
        await using var context = CreateContext();
        var userId = Guid.NewGuid();
        var role = await context.Roles.FirstAsync();
        var user = new User { Id = userId, Email = "student@test.local", FirstName = "Test", LastName = "Student", PasswordHash = "hash", RoleId = role.Id };
        context.Users.Add(user);
        var institution = new Institution { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Type = "DATOS DE PRUEBA", Municipality = "DATOS DE PRUEBA", Status = "ACTIVA" };
        context.Institutions.Add(institution);
        var career = new Career { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Status = "ACTIVA", InstitutionId = institution.Id, Institution = institution, Area = "Tecnologia", Modality = "Presencial" };
        context.Careers.Add(career);
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "R", CompatibilityLevel = 100, Rule = "Coincide con la dimension principal." });
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version };
        var profile = new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 20, IScore = 12, AScore = 8, SScore = 8, EScore = 8, CScore = 8, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt); context.RiasecProfiles.Add(profile);
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var recommendations = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest { Area = "Tecnologia" }, CancellationToken.None);

        Assert.NotNull(recommendations);
        Assert.Single(recommendations!.Items);
        Assert.Equal(100, recommendations.Items[0].Compatibility);
        Assert.Single(await service.GetHistoryAsync(userId, CancellationToken.None));
    }

    [Fact]
    public async Task History_keeps_one_entry_per_test_however_many_searches_were_made()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 10, IScore = 6, AScore = 4, SScore = 4, EScore = 4, CScore = 4, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        // La pantalla de resultado pide recomendaciones sola al abrirse, y cada busqueda hace otra.
        await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);
        await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest { Area = "Salud" }, CancellationToken.None);
        var history = await service.GetHistoryAsync(userId, CancellationToken.None);

        var entry = Assert.Single(history);
        Assert.Equal(attempt.Id, entry.TestAttemptId);
        Assert.Equal("RI", entry.PrimaryCode);
        Assert.Equal(10, entry.RScore);
        Assert.NotNull(entry.Interpretation);
        Assert.Contains("Salud", entry.FilterSummary); // los filtros son los de la ultima busqueda
    }

    [Fact]
    public async Task History_lists_a_profile_of_an_older_test_version_without_scores_or_interpretation()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        // Un resultado del CHASIDE que el sistema aplico un tiempo (las columnas guardan otras areas).
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = "CHASIDE-14-V1" };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 9, IScore = 1, AScore = 1, SScore = 1, EScore = 1, CScore = 1, PrimaryCode = "CH", SecondaryCode = "A", AlgorithmVersion = "CHASIDE-14-V1" });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var entry = Assert.Single(await service.GetHistoryAsync(userId, CancellationToken.None));

        // Las letras del test viejo significan otra cosa: se muestra el registro, no una lectura falsa.
        Assert.Null(entry.PrimaryCode);
        Assert.Null(entry.RScore);
        Assert.Null(entry.Interpretation);
    }

    [Fact]
    public async Task Public_institution_list_hides_inactive_ones_but_admin_listing_shows_all()
    {
        await using var context = CreateContext();
        context.Institutions.Add(new Institution { Id = Guid.NewGuid(), Name = "Institución activa", Type = "Pública", Status = "ACTIVA", IsActive = true });
        context.Institutions.Add(new Institution { Id = Guid.NewGuid(), Name = "Institución inactiva", Type = "Pública", Status = "ACTIVA", IsActive = false });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var publicList = await service.GetInstitutionsAsync(CancellationToken.None);
        var adminList = await service.GetAllInstitutionsForAdminAsync(CancellationToken.None);

        Assert.Equal("Institución activa", Assert.Single(publicList).Name);
        Assert.Equal(2, adminList.Count);
    }

    [Fact]
    public async Task Public_catalog_hides_inactive_careers_but_admin_listing_shows_all()
    {
        await using var context = CreateContext();
        var institution = new Institution { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Type = "DATOS DE PRUEBA", Status = "ACTIVA" };
        context.Institutions.Add(institution);
        var activeCareer = new Career { Id = Guid.NewGuid(), Name = "Carrera activa", Status = "ACTIVA", InstitutionId = institution.Id, Institution = institution, IsActive = true };
        var inactiveCareer = new Career { Id = Guid.NewGuid(), Name = "Carrera inactiva", Status = "ACTIVA", InstitutionId = institution.Id, Institution = institution, IsActive = false };
        context.Careers.AddRange(activeCareer, inactiveCareer);
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var publicCareers = await service.GetCareersAsync(CancellationToken.None);
        var adminCareers = await service.GetAllCareersForAdminAsync(CancellationToken.None);

        Assert.Single(publicCareers);
        Assert.Equal("Carrera activa", publicCareers[0].Name);
        Assert.True(publicCareers[0].IsActive);
        Assert.Equal(2, adminCareers.Count);
        Assert.Contains(adminCareers, item => item.Name == "Carrera inactiva");
        Assert.False(adminCareers.Single(item => item.Name == "Carrera inactiva").IsActive);
        Assert.True(adminCareers.Single(item => item.Name == "Carrera activa").IsActive);
    }

    [Fact]
    public async Task Compatibility_uses_the_twenty_four_question_scale_max_score_gives_full_match()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var institution = new Institution { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Type = "Privada", Status = "PRUEBA" };
        var career = new Career { Id = Guid.NewGuid(), Name = "Carrera de prueba", Status = "PRUEBA", InstitutionId = institution.Id, Institution = institution };
        context.Institutions.Add(institution);
        context.Careers.Add(career);
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "I", CompatibilityLevel = 100, Rule = "Investigador." });
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt);
        // Puntaje maximo en I (4 preguntas x 5 = 20) y la mitad en R.
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 10, IScore = 20, AScore = 4, SScore = 4, EScore = 4, CScore = 4, PrimaryCode = "IR", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var result = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);

        Assert.Equal(100, result!.Items.Single().Compatibility);
    }

    [Fact]
    public async Task A_result_of_the_previous_twelve_question_test_is_still_recommended_on_its_own_scale()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var institution = new Institution { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Type = "Privada", Status = "PRUEBA" };
        var career = new Career { Id = Guid.NewGuid(), Name = "Carrera de prueba", Status = "PRUEBA", InstitutionId = institution.Id, Institution = institution };
        context.Institutions.Add(institution);
        context.Careers.Add(career);
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "I", CompatibilityLevel = 100, Rule = "Investigador." });
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.OlderVersion };
        context.TestAttempts.Add(attempt);
        // Test anterior: 2 preguntas por dimension, 10 era el maximo.
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 5, IScore = 10, AScore = 2, SScore = 2, EScore = 2, CScore = 2, PrimaryCode = "IR", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.OlderVersion });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var result = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);
        var entry = Assert.Single(await service.GetHistoryAsync(userId, CancellationToken.None));

        Assert.Equal(100, result!.Items.Single().Compatibility);
        Assert.Equal(10, entry.MaxScore);
        Assert.Equal(10, entry.IScore);
        Assert.NotNull(entry.Interpretation);
    }

    [Fact]
    public async Task An_attempt_of_a_previous_version_in_progress_is_not_resumed_a_new_one_starts()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var old = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.PreviousVersion, Status = OrientacionVocacional.Domain.Enums.TestAttemptStatus.InProgress };
        context.TestAttempts.Add(old);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);

        Assert.NotEqual(old.Id, attempt.AttemptId);
        Assert.Equal(24, attempt.Total);
    }

    [Fact]
    public async Task Every_student_gets_exactly_the_same_questions_in_the_same_order()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var first = await TestSupport.CreatePersistedStudentAsync(context);
        var second = await TestSupport.CreatePersistedStudentAsync(context);

        var a = await service.CreateAttemptAsync(first, CancellationToken.None);
        var b = await service.CreateAttemptAsync(second, CancellationToken.None);
        var questionsA = await service.GetAssignedQuestionsAsync(first, a.AttemptId, CancellationToken.None);
        var questionsB = await service.GetAssignedQuestionsAsync(second, b.AttemptId, CancellationToken.None);

        // Forma fija: sin sorteo, los puntajes de distintos estudiantes se pueden comparar.
        Assert.Equal(questionsA!.Select(q => q.Id), questionsB!.Select(q => q.Id));
        Assert.Equal(new[] { "R01", "I01", "A01", "S01", "E01", "C01", "R02" }, questionsA!.Take(7).Select(q => q.Code));
    }

    [Fact]
    public async Task A_result_of_the_sixty_question_test_keeps_its_own_scale()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Fixed60Version };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 50, IScore = 30, AScore = 20, SScore = 20, EScore = 20, CScore = 20, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Fixed60Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var entry = Assert.Single(await service.GetHistoryAsync(userId, CancellationToken.None));

        Assert.Equal(50, entry.MaxScore);
        Assert.NotNull(entry.Interpretation);
    }

    [Fact]
    public async Task A_result_of_the_previous_twenty_four_question_test_keeps_its_own_scale()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.PreviousVersion };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 20, IScore = 12, AScore = 8, SScore = 8, EScore = 8, CScore = 8, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.PreviousVersion });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var entry = Assert.Single(await service.GetHistoryAsync(userId, CancellationToken.None));

        Assert.Equal(20, entry.MaxScore);
        Assert.NotNull(entry.Interpretation);
    }

    [Fact]
    public void Interpretation_explains_each_letter_and_suggests_areas()
    {
        var interpretation = RiasecInterpretationBuilder.Build("RI", "A");

        Assert.Equal("RIA", interpretation.Code);
        Assert.Equal(new[] { "Realista", "Investigador", "Artístico" }, interpretation.Dimensions.Select(item => item.Name));
        Assert.All(interpretation.Dimensions, item => Assert.StartsWith("Te interesa", item.Description));
        Assert.NotEmpty(interpretation.SuggestedAreas);
        Assert.Contains("RIA", interpretation.Summary);
    }

    [Fact]
    public async Task Recommendations_include_profile_interpretation_and_flag_ai_unavailable()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 8, IScore = 6, AScore = 4, SScore = 2, EScore = 2, CScore = 2, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var result = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("RIA", result!.Interpretation.Code);
        Assert.False(result.AiGenerated);
        Assert.Empty(result.Items);
    }

    private static TestService CreateService(OrientacionVocacionalDbContext context) =>
        new(context, new RiasecScoringService());

    private static OrientacionVocacionalDbContext CreateContext() => TestSupport.CreateContext();
}
