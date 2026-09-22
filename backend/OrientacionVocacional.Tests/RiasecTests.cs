using Microsoft.EntityFrameworkCore;
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
    public void Seed_has_sixty_active_chaside_questions_and_keeps_legacy_ones_inactive()
    {
        var questions = RiasecQuestionSeed.Create();
        var active = questions.Where(question => question.IsActive).ToList();

        Assert.Equal(60, active.Count);
        Assert.Equal(questions.Count, questions.Select(question => question.Code).Distinct().Count());
        Assert.Equal(questions.Count, questions.Select(question => question.Id).Distinct().Count());
        var expected = new Dictionary<string, int> { ["C"] = 9, ["H"] = 9, ["A"] = 9, ["S"] = 9, ["I"] = 8, ["D"] = 8, ["E"] = 8 };
        foreach (var (dimension, count) in expected)
        {
            Assert.Equal(count, active.Count(question => question.Dimension == dimension));
        }
        // Las preguntas del test RIASEC anterior (incluida toda la dimension R) quedan inactivas.
        Assert.DoesNotContain(active, question => question.Dimension == "R");
        Assert.All(questions.Where(question => !question.IsActive), question => Assert.False(question.IsActive));
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
        // C, H, A, S empatan con 9 preguntas (45): gana el orden fijo C,H,A. I, D, E tienen 8 (40).
        Assert.Equal("CH", first.PrimaryCode);
        Assert.Equal("A", first.SecondaryCode);
        Assert.Equal(45, first.CScore);
        Assert.Equal(40, first.EScore);
    }

    [Fact]
    public async Task Attempt_gets_fourteen_questions_two_per_area()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);

        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.NotNull(assigned);
        Assert.Equal(14, assigned!.Count);
        Assert.Equal(14, assigned.Select(question => question.Id).Distinct().Count());
        foreach (var dimension in new[] { "C", "H", "A", "S", "I", "D", "E" })
        {
            Assert.Equal(2, assigned.Count(question => question.Dimension == dimension));
        }
        // Nunca dos preguntas seguidas de la misma area.
        for (var index = 1; index < assigned.Count; index++)
        {
            Assert.NotEqual(assigned[index - 1].Dimension, assigned[index].Dimension);
        }
        Assert.Equal(14, attempt.Total);
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

        Assert.Equal(2, assigned!.Count(question => question.Dimension == "C"));
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
    public async Task Cannot_complete_with_thirteen_answers_but_can_with_fourteen()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var assigned = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        foreach (var question in assigned!.Take(13))
        {
            var response = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = question.Id, Value = 3 }, CancellationToken.None);
            Assert.True(response.Success);
        }

        var incomplete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.False(incomplete.Success);
        Assert.Equal("El test debe tener 14 respuestas antes de finalizar.", incomplete.Error);

        var last = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = assigned![13].Id, Value = 3 }, CancellationToken.None);
        var complete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.True(last.Success);
        Assert.True(complete.Success);
        Assert.NotNull(complete.Result);
        Assert.Equal(6, complete.Result!.CScore); // 2 preguntas x valor 3
        Assert.Equal("CH", complete.Result.PrimaryCode); // empate total: orden fijo C,H,A,S,I,D,E
        Assert.Equal("A", complete.Result.SecondaryCode);
        Assert.Equal(14, last.Attempt!.Coins);

        var afterCompletion = await service.GetAttemptAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.Equal(64, afterCompletion!.Coins); // 14 respuestas + 50 de bono

        var completedAgain = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        var afterSecondComplete = await service.GetAttemptAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.True(completedAgain.Success);
        Assert.Equal(64, afterSecondComplete!.Coins);
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
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "C", CompatibilityLevel = 100, Rule = "Coincide con la dimension principal." });
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version };
        var profile = new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, CScore = 10, HScore = 6, AScore = 4, SScore = 4, IScore = 4, DScore = 4, EScore = 4, PrimaryCode = "CH", SecondaryCode = "A", AlgorithmVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version };
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
    public async Task Compatibility_uses_the_twelve_question_scale_max_score_gives_full_match()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var institution = new Institution { Id = Guid.NewGuid(), Name = "DATOS DE PRUEBA", Type = "Privada", Status = "PRUEBA" };
        var career = new Career { Id = Guid.NewGuid(), Name = "Carrera de prueba", Status = "PRUEBA", InstitutionId = institution.Id, Institution = institution };
        context.Institutions.Add(institution);
        context.Careers.Add(career);
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "H", CompatibilityLevel = 100, Rule = "Humanidades." });
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt);
        // Puntaje maximo en H (2 preguntas x 5 = 10) y la mitad en C.
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, CScore = 5, HScore = 10, AScore = 2, SScore = 2, IScore = 2, DScore = 2, EScore = 2, PrimaryCode = "HC", SecondaryCode = "A", AlgorithmVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var result = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);

        Assert.Equal(100, result!.Items.Single().Compatibility);
    }

    [Fact]
    public void Interpretation_explains_each_letter_and_suggests_areas()
    {
        var interpretation = RiasecInterpretationBuilder.Build("HS", "A");

        Assert.Equal("HSA", interpretation.Code);
        Assert.Equal(new[] { "Humanidades, Ciencias Sociales y Jurídicas", "Ciencias de la Salud", "Artística" }, interpretation.Dimensions.Select(item => item.Name));
        Assert.All(interpretation.Dimensions, item => Assert.StartsWith("Te atrae", item.Description));
        Assert.NotEmpty(interpretation.SuggestedAreas);
        Assert.Contains("HSA", interpretation.Summary);
    }

    [Fact]
    public async Task Recommendations_include_profile_interpretation_and_flag_ai_unavailable()
    {
        await using var context = CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version };
        context.TestAttempts.Add(attempt);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, CScore = 8, HScore = 6, AScore = 4, SScore = 2, IScore = 2, DScore = 2, EScore = 2, PrimaryCode = "CH", SecondaryCode = "A", AlgorithmVersion = OrientacionVocacional.Domain.Constants.RiasecTestConstants.Version });
        await context.SaveChangesAsync();
        var service = new Infrastructure.Services.CatalogService(context, new NoAi());

        var result = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("CHA", result!.Interpretation.Code);
        Assert.False(result.AiGenerated);
        Assert.Empty(result.Items);
    }

    private static TestService CreateService(OrientacionVocacionalDbContext context) =>
        new(context, new RiasecScoringService());

    private static OrientacionVocacionalDbContext CreateContext() => TestSupport.CreateContext();
}
