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
    public async Task<IReadOnlyList<QuestionResponse>> GetQuestionsAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Questions
            .Where(question => question.IsActive)
            .OrderBy(question => question.Order)
            .Select(question => new QuestionResponse(
                question.Id, question.Code, question.Text, question.Dimension, question.Order))
            .ToListAsync(cancellationToken);
    }

    public async Task<TestAttemptResponse> CreateAttemptAsync(Guid userId, CancellationToken cancellationToken)
    {
        var current = await dbContext.TestAttempts
            .Include(attempt => attempt.Answers)
            .SingleOrDefaultAsync(
                attempt => attempt.UserId == userId && attempt.Status == TestAttemptStatus.InProgress,
                cancellationToken);

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
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        return attempt is null ? null : ToAttemptResponse(attempt, await GetCoinsAsync(userId, cancellationToken));
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
        if (attempt.Answers.Count != RiasecTestConstants.TotalQuestions)
        {
            return (false, "El test debe tener 60 respuestas antes de finalizar.", null);
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

    private async Task<int> GetCoinsAsync(Guid userId, CancellationToken cancellationToken) =>
        await dbContext.Users.Where(candidate => candidate.Id == userId).Select(candidate => candidate.Coins).SingleAsync(cancellationToken);

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

    private static TestAttemptResponse ToAttemptResponse(TestAttempt attempt, int coins)
    {
        var answered = attempt.Answers.Count;
        return new TestAttemptResponse(
            attempt.Id,
            answered,
            RiasecTestConstants.TotalQuestions,
            Math.Round(answered * 100m / RiasecTestConstants.TotalQuestions, 2),
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