using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class LiveCatalogTests
{
    private sealed class IaQueCuenta : IAiExplanationService
    {
        public int Llamadas { get; private set; }
        public Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken)
        {
            Llamadas++;
            return Task.FromResult(string.Empty);
        }
    }

    private static async Task<Guid> StudentWithProfileAsync(OrientacionVocacionalDbContext context)
    {
        var tests = new TestService(context, new RiasecScoringService());
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var attempt = await tests.CreateAttemptAsync(userId, CancellationToken.None);
        foreach (var question in (await tests.GetAssignedQuestionsAsync(userId, attempt.AttemptId, CancellationToken.None))!)
        {
            await tests.SubmitAnswerAsync(userId, attempt.AttemptId, new SubmitAnswerRequest { QuestionId = question.Id, Value = 5 }, CancellationToken.None);
        }
        await tests.CompleteAsync(userId, attempt.AttemptId, CancellationToken.None);
        return userId;
    }

    private static async Task<(Institution Institution, Career Career)> CareerWithRuleAsync(OrientacionVocacionalDbContext context, string name)
    {
        var institution = new Institution { Id = Guid.NewGuid(), Name = name, Type = "Universidad", Status = "PENDIENTE", Website = "https://u.edu.bo" };
        var career = new Career { Id = Guid.NewGuid(), Name = $"Carrera de {name}", Status = "PENDIENTE", InstitutionId = institution.Id, Institution = institution };
        context.Add(institution);
        context.Add(career);
        context.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = career.Id, Dimension = "R", CompatibilityLevel = 80, Rule = "Regla" });
        await context.SaveChangesAsync();
        return (institution, career);
    }

    [Fact]
    public async Task Matches_refresh_without_history_row_or_ai_call()
    {
        await using var context = TestSupport.CreateContext();
        var ai = new IaQueCuenta();
        var service = new CatalogService(context, ai);
        var userId = await StudentWithProfileAsync(context);
        await CareerWithRuleAsync(context, "Univalle");

        var matches = await service.GetCareerMatchesAsync(userId, CancellationToken.None);

        Assert.Single(matches!);
        Assert.Equal("https://u.edu.bo", matches![0].InstitutionWebsite);
        Assert.Equal(0, ai.Llamadas);
        Assert.Equal(0, await context.RecommendationHistories.CountAsync());
    }

    [Fact]
    public async Task Matches_equal_the_recommendations_shown_after_the_test()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new IaQueCuenta());
        var userId = await StudentWithProfileAsync(context);
        await CareerWithRuleAsync(context, "Univalle");
        await CareerWithRuleAsync(context, "UMSS");

        var recommendation = await service.GetRecommendationsAsync(userId, new RecommendationFilterRequest(), CancellationToken.None);
        var matches = await service.GetCareerMatchesAsync(userId, CancellationToken.None);

        Assert.Equal(recommendation!.Items.Select(item => (item.Id, item.Compatibility)), matches!.Select(item => (item.Id, item.Compatibility)));
    }

    [Fact]
    public async Task Deactivating_an_institution_removes_it_from_the_next_refresh()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new IaQueCuenta());
        var userId = await StudentWithProfileAsync(context);
        var (institution, _) = await CareerWithRuleAsync(context, "UPEA");
        await CareerWithRuleAsync(context, "UMSS");
        Assert.Equal(2, (await service.GetCareerMatchesAsync(userId, CancellationToken.None))!.Count);

        await service.SetInstitutionActiveAsync(institution.Id, false, CancellationToken.None);

        var matches = await service.GetCareerMatchesAsync(userId, CancellationToken.None);
        Assert.Single(matches!);
        Assert.Equal("UMSS", matches![0].InstitutionName);
    }

    [Fact]
    public async Task Without_a_finished_test_there_are_no_matches()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new IaQueCuenta());
        var userId = await TestSupport.CreatePersistedStudentAsync(context);

        Assert.Null(await service.GetCareerMatchesAsync(userId, CancellationToken.None));
    }
}
