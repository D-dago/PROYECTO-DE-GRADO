using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Enums;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class TestService(
    OrientacionVocacionalDbContext dbContext,
    IRiasecScoringService scoringService) : ITestService
{
    public async Task<TestAttemptResponse> CreateAttemptAsync(Guid userId, CancellationToken cancellationToken)
    {
        var current = await dbContext.TestAttempts
            .Include(attempt => attempt.Answers)
            .Include(attempt => attempt.AssignedQuestions)
            .Where(attempt => attempt.UserId == userId && attempt.Status == TestAttemptStatus.InProgress)
            .OrderByDescending(attempt => attempt.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        if (current is not null)
        {
            return ToAttemptResponse(current, await GetCoinsAsync(userId, cancellationToken));
        }

        var attempt = new TestAttempt
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TestVersion = RiasecTestConstants.Version
        };
        dbContext.TestAttempts.Add(attempt);
        await AssignQuestionsAsync(attempt, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToAttemptResponse(attempt, await GetCoinsAsync(userId, cancellationToken));
    }

    public async Task<TestAttemptResponse?> GetAttemptAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await dbContext.TestAttempts
            .Include(candidate => candidate.Answers)
            .Include(candidate => candidate.AssignedQuestions)
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        return attempt is null ? null : ToAttemptResponse(attempt, await GetCoinsAsync(userId, cancellationToken));
    }

    public async Task<IReadOnlyList<QuestionResponse>?> GetAssignedQuestionsAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attemptExists = await dbContext.TestAttempts
            .AnyAsync(candidate => candidate.Id == attemptId && candidate.UserId == userId, cancellationToken);
        if (!attemptExists) return null;

        return await dbContext.AssignedQuestions
            .Where(assigned => assigned.TestAttemptId == attemptId)
            .Include(assigned => assigned.Question)
            .OrderBy(assigned => assigned.Order)
            .Select(assigned => new QuestionResponse(
                assigned.Question.Id, assigned.Question.Code, assigned.Question.Text,
                assigned.Question.Dimension, assigned.Order))
            .ToListAsync(cancellationToken);
    }

    public async Task<(bool Success, string? Error, TestAttemptResponse? Attempt)> SubmitAnswerAsync(
        Guid userId,
        Guid attemptId,
        SubmitAnswerRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Value is < 1 or > 5)
        {
            return (false, "La respuesta debe estar entre 1 y 5.", null);
        }

        var attempt = await dbContext.TestAttempts
            .Include(candidate => candidate.Answers)
            .Include(candidate => candidate.AssignedQuestions)
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        if (attempt is null)
        {
            return (false, "El intento no existe.", null);
        }
        if (attempt.Status != TestAttemptStatus.InProgress)
        {
            return (false, "El intento ya fue completado.", null);
        }
        if (attempt.AssignedQuestions.All(assigned => assigned.QuestionId != request.QuestionId))
        {
            return (false, "Esa pregunta no forma parte de este intento.", null);
        }

        var question = await dbContext.Questions
            .SingleOrDefaultAsync(candidate => candidate.Id == request.QuestionId && candidate.IsActive, cancellationToken);
        if (question is null)
        {
            return (false, "La pregunta no existe o no está activa.", null);
        }
        if (attempt.Answers.Any(answer => answer.QuestionId == request.QuestionId))
        {
            return (false, "La pregunta ya fue respondida en este intento.", null);
        }

        dbContext.Answers.Add(new Answer
        {
            Id = Guid.NewGuid(),
            TestAttemptId = attemptId,
            QuestionId = question.Id,
            Value = request.Value
        });
        var user = await dbContext.Users.SingleAsync(candidate => candidate.Id == userId, cancellationToken);
        user.Coins += RiasecTestConstants.CoinsPerAnswer;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (true, null, ToAttemptResponse(attempt, user.Coins));
    }

    public async Task<(bool Success, string? Error, RiasecResultResponse? Result)> CompleteAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await dbContext.TestAttempts
            .Include(candidate => candidate.Answers)
                .ThenInclude(answer => answer.Question)
            .Include(candidate => candidate.AssignedQuestions)
            .Include(candidate => candidate.RiasecProfile)
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        if (attempt is null)
        {
            return (false, "El intento no existe.", null);
        }
        if (attempt.Status == TestAttemptStatus.Completed && attempt.RiasecProfile is not null)
        {
            return (true, null, ToResultResponse(attempt.RiasecProfile));
        }
        if (attempt.Answers.Count != attempt.AssignedQuestions.Count)
        {
            return (false, $"El test debe tener {attempt.AssignedQuestions.Count} respuestas antes de finalizar.", null);
        }

        var score = scoringService.Calculate(attempt.Answers);
        var profile = new RiasecProfile
        {
            Id = Guid.NewGuid(),
            TestAttemptId = attempt.Id,
            RScore = score.RScore,
            IScore = score.IScore,
            AScore = score.AScore,
            SScore = score.SScore,
            EScore = score.EScore,
            CScore = score.CScore,
            PrimaryCode = score.PrimaryCode,
            SecondaryCode = score.SecondaryCode,
            AlgorithmVersion = RiasecTestConstants.Version
        };
        attempt.Status = TestAttemptStatus.Completed;
        attempt.CompletedAtUtc = DateTime.UtcNow;
        dbContext.RiasecProfiles.Add(profile);
        var user = await dbContext.Users.SingleAsync(candidate => candidate.Id == userId, cancellationToken);
        user.Coins += RiasecTestConstants.CompletionBonusCoins;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (true, null, ToResultResponse(profile));
    }

    public async Task<RiasecResultResponse?> GetResultAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.RiasecProfiles
            .Include(candidate => candidate.TestAttempt)
            .SingleOrDefaultAsync(
                candidate => candidate.TestAttemptId == attemptId && candidate.TestAttempt.UserId == userId,
                cancellationToken);
        return profile is null ? null : ToResultResponse(profile);
    }

    /// <summary>
    /// Sortea 2 preguntas por dimension (12 en total) del banco de 10 por dimension,
    /// intercaladas R,I,A,S,E,C,R,I,A,S,E,C para que nunca aparezcan dos preguntas
    /// seguidas de la misma dimension dentro del intento.
    /// </summary>
    private async Task AssignQuestionsAsync(TestAttempt attempt, CancellationToken cancellationToken)
    {
        var pool = await dbContext.Questions
            .Where(question => question.IsActive)
            .ToListAsync(cancellationToken);
        var byDimension = RiasecTestConstants.DimensionOrder.ToDictionary(
            dimension => dimension,
            dimension => pool.Where(question => question.Dimension == dimension)
                .OrderBy(_ => Random.Shared.Next())
                .Take(RiasecTestConstants.QuestionsPerDimensionPerAttempt)
                .ToList());

        var order = 1;
        for (var round = 0; round < RiasecTestConstants.QuestionsPerDimensionPerAttempt; round++)
        {
            foreach (var dimension in RiasecTestConstants.DimensionOrder)
            {
                var question = byDimension[dimension][round];
                dbContext.AssignedQuestions.Add(new AssignedQuestion
                {
                    Id = Guid.NewGuid(),
                    TestAttemptId = attempt.Id,
                    QuestionId = question.Id,
                    Order = order++
                });
            }
        }
    }

    private async Task<int> GetCoinsAsync(Guid userId, CancellationToken cancellationToken) =>
        await dbContext.Users.Where(candidate => candidate.Id == userId).Select(candidate => candidate.Coins).SingleAsync(cancellationToken);

    private static TestAttemptResponse ToAttemptResponse(TestAttempt attempt, int coins)
    {
        var answered = attempt.Answers.Count;
        var total = attempt.AssignedQuestions.Count;
        return new TestAttemptResponse(
            attempt.Id,
            answered,
            total,
            total == 0 ? 0 : Math.Round(answered * 100m / total, 2),
            attempt.Status.ToString().ToUpperInvariant(),
            coins);
    }

    private static RiasecResultResponse ToResultResponse(RiasecProfile profile) => new(
        profile.Id,
        profile.TestAttemptId,
        profile.RScore,
        profile.IScore,
        profile.AScore,
        profile.SScore,
        profile.EScore,
        profile.CScore,
        profile.PrimaryCode,
        profile.SecondaryCode,
        profile.CreatedAtUtc,
        profile.AlgorithmVersion);
}
