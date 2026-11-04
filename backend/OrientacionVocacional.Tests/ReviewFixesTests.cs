using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class ReviewFixesTests
{
    private sealed class SinIa : IAiExplanationService
    {
        public Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    [Fact]
    public async Task Attempt_can_finish_when_a_question_is_deactivated_midway()
    {
        await using var context = TestSupport.CreateContext();
        var service = new TestService(context, new RiasecScoringService());
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await service.CreateAttemptAsync(userId, CancellationToken.None);
        var questions = await service.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None);

        var deactivated = await context.Questions.SingleAsync(question => question.Id == questions![0].Id);
        deactivated.IsActive = false;
        await context.SaveChangesAsync();

        foreach (var question in questions!)
        {
            var (success, error, _) = await service.SubmitAnswerAsync(userId, attempt.AttemptId, new SubmitAnswerRequest { QuestionId = question.Id, Value = 3 }, CancellationToken.None);
            Assert.True(success, error);
        }
        var (completed, _, result) = await service.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);

        Assert.True(completed);
        Assert.NotNull(result);
    }

    private static async Task<(CatalogService Service, Guid CareerId)> CatalogWithCareerAsync(Microsoft.EntityFrameworkCore.DbContext context)
    {
        var institution = new Institution { Id = Guid.NewGuid(), Name = "Universidad", Type = "Universidad", Status = "PENDIENTE" };
        var career = new Career { Id = Guid.NewGuid(), Name = "Carrera", Status = "PENDIENTE", InstitutionId = institution.Id, Institution = institution };
        context.Add(institution);
        context.Add(career);
        await context.SaveChangesAsync();
        return (new CatalogService((OrientacionVocacional.Infrastructure.Persistence.OrientacionVocacionalDbContext)context, new SinIa()), career.Id);
    }

    [Fact]
    public async Task Duplicate_compatibility_rule_is_rejected_instead_of_failing()
    {
        await using var context = TestSupport.CreateContext();
        var (service, careerId) = await CatalogWithCareerAsync(context);
        var rule = new CompatibilityRequest { CareerId = careerId, Dimension = "R", CompatibilityLevel = 80, Rule = "Regla" };

        Assert.True(await service.AddCompatibilityAsync(rule, CancellationToken.None));
        Assert.False(await service.AddCompatibilityAsync(rule, CancellationToken.None));
        Assert.False(await service.AddCompatibilityAsync(new CompatibilityRequest { CareerId = careerId, Dimension = "r", CompatibilityLevel = 50, Rule = "Otra" }, CancellationToken.None));
        Assert.Equal(1, await context.CareerCompatibilities.CountAsync());
    }

    [Fact]
    public async Task Rule_cannot_be_changed_to_a_dimension_already_used_by_that_career()
    {
        await using var context = TestSupport.CreateContext();
        var (service, careerId) = await CatalogWithCareerAsync(context);
        await service.AddCompatibilityAsync(new CompatibilityRequest { CareerId = careerId, Dimension = "R", CompatibilityLevel = 80, Rule = "Regla R" }, CancellationToken.None);
        await service.AddCompatibilityAsync(new CompatibilityRequest { CareerId = careerId, Dimension = "I", CompatibilityLevel = 60, Rule = "Regla I" }, CancellationToken.None);
        var ruleI = await context.CareerCompatibilities.SingleAsync(item => item.Dimension == "I");

        Assert.False(await service.UpdateCompatibilityAsync(ruleI.Id, new CompatibilityRequest { CareerId = careerId, Dimension = "R", CompatibilityLevel = 60, Rule = "Regla I" }, CancellationToken.None));
        Assert.True(await service.UpdateCompatibilityAsync(ruleI.Id, new CompatibilityRequest { CareerId = careerId, Dimension = "I", CompatibilityLevel = 90, Rule = "Regla I" }, CancellationToken.None));
    }
}
