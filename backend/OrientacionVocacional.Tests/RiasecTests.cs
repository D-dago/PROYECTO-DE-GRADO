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
    public void Seed_has_exactly_sixty_questions_and_ten_per_dimension()
    {
        var questions = RiasecQuestionSeed.Create();

        Assert.Equal(60, questions.Count);
        Assert.Equal(60, questions.Select(question => question.Code).Distinct().Count());
        foreach (var dimension in new[] { "R", "I", "A", "S", "E", "C" })
        {
            Assert.Equal(10, questions.Count(question => question.Dimension == dimension));
        }
    }

    [Fact]
    public void Scoring_is_deterministic_and_uses_fixed_tie_order()
    {
        var questions = RiasecQuestionSeed.Create();
        var answers = questions.Select(question => new Answer
        {
            QuestionId = question.Id,
            Question = question,
            Value = 5
        }).ToArray();
        var scoring = new RiasecScoringService();

        var first = scoring.Calculate(answers);
        var second = scoring.Calculate(answers);

        Assert.Equal(first, second);
        Assert.Equal("RI", first.PrimaryCode);
        Assert.Equal("A", first.SecondaryCode);
        Assert.Equal(50, first.RScore);
    }

    [Fact]
    public async Task Answer_value_must_be_between_one_and_five()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var question = await context.Questions.FirstAsync();

        var zero = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = question.Id, Value = 0 }, CancellationToken.None);
        var six = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = question.Id, Value = 6 }, CancellationToken.None);

        Assert.False(zero.Success);
        Assert.False(six.Success);
    }

    [Fact]
    public async Task Answer_values_one_and_five_are_accepted()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var questions = await context.Questions.OrderBy(question => question.Order).Take(2).ToListAsync();

        var one = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = questions[0].Id, Value = 1 }, CancellationToken.None);
        var five = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = questions[1].Id, Value = 5 }, CancellationToken.None);

        Assert.True(one.Success);
        Assert.True(five.Success);
    }

    [Fact]
    public async Task Duplicate_question_answer_is_rejected()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var question = await context.Questions.FirstAsync();
        var request = new OrientacionVocacional.Application.Contracts.SubmitAnswerRequest { QuestionId = question.Id, Value = 1 };

        var first = await service.SubmitAnswerAsync(userId, attempt.AttemptId, request, CancellationToken.None);
        var duplicate = await service.SubmitAnswerAsync(userId, attempt.AttemptId, request, CancellationToken.None);

        Assert.True(first.Success);
        Assert.False(duplicate.Success);
    }

    [Fact]
    public async Task Cannot_complete_with_fifty_nine_answers_but_can_with_sixty()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var userId = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var questions = await context.Questions.OrderBy(question => question.Order).ToListAsync();

        foreach (var question in questions.Take(59))
        {
            var response = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = question.Id, Value = 3 }, CancellationToken.None);
            Assert.True(response.Success);
        }

        var incomplete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        Assert.False(incomplete.Success);
        Assert.Equal("El test debe tener 60 respuestas antes de finalizar.", incomplete.Error);

        var last = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new() { QuestionId = questions[59].Id, Value = 3 }, CancellationToken.None);
        var complete = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.True(last.Success);
        Assert.True(complete.Success);
        Assert.NotNull(complete.Result);
        Assert.Equal(30, complete.Result!.RScore);
        Assert.Equal("RI", complete.Result.PrimaryCode);
        Assert.Equal("A", complete.Result.SecondaryCode);
    }

    [Fact]
    public async Task Student_cannot_read_another_students_attempt()
    {
        await using var context = CreateContext();
        var service = CreateService(context);
        var firstUser = Guid.NewGuid();
        var secondUser = Guid.NewGuid();
        var attempt = await service.CreateAttemptAsync(firstUser, CancellationToken.None);

        var visible = await service.GetAttemptAsync(firstUser, attempt.AttemptId, CancellationToken.None);
        var hidden = await service.GetAttemptAsync(secondUser, attempt.AttemptId, CancellationToken.None);

        Assert.NotNull(visible);
        Assert.Null(hidden);
    }

    [Fact]
    public async Task Inactive_questions_are_not_returned()
    {
        await using var context = CreateContext();
        var question = await context.Questions.FirstAsync();
        question.IsActive = false;
        await context.SaveChangesAsync();
        var service = CreateService(context);

        var questions = await service.GetQuestionsAsync(CancellationToken.None);

        Assert.Equal(59, questions.Count);
        Assert.DoesNotContain(questions, item => item.Id == question.Id);
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
        var attempt = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = "RIASEC-60-V1" };
        var profile = new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = attempt.Id, TestAttempt = attempt, RScore = 50, IScore = 20, AScore = 10, SScore = 10, EScore = 10, CScore = 10, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = "RIASEC-60-V1" };
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
        Assert.Equal(2, adminCareers.Count);
        Assert.Contains(adminCareers, item => item.Name == "Carrera inactiva");
    }

    private static TestService CreateService(OrientacionVocacionalDbContext context) =>
        new(context, new RiasecScoringService());

    private static OrientacionVocacionalDbContext CreateContext() => TestSupport.CreateContext();
}