using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface ITestService
{
    Task<IReadOnlyList<QuestionResponse>> GetQuestionsAsync(CancellationToken cancellationToken);
    Task<TestAttemptResponse> CreateAttemptAsync(Guid userId, CancellationToken cancellationToken);
    Task<TestAttemptResponse?> GetAttemptAsync(Guid userId, Guid attemptId, CancellationToken cancellationToken);
    Task<(bool Success, string? Error, TestAttemptResponse? Attempt)> SubmitAnswerAsync(
        Guid userId,
        Guid attemptId,
        SubmitAnswerRequest request,
        CancellationToken cancellationToken);
    Task<(bool Success, string? Error, RiasecResultResponse? Result)> CompleteAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken);
    Task<RiasecResultResponse?> GetResultAsync(Guid userId, Guid attemptId, CancellationToken cancellationToken);
}