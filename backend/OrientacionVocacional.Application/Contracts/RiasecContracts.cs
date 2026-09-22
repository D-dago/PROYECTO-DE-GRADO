namespace OrientacionVocacional.Application.Contracts;

public sealed record QuestionResponse(Guid Id, string Code, string Text, string Dimension, int Order);

public sealed record TestAttemptResponse(
    Guid AttemptId,
    int Answered,
    int Total,
    decimal Progress,
    string Status,
    int Coins);

public sealed class SubmitAnswerRequest
{
    public Guid QuestionId { get; init; }
    public int Value { get; init; }
}

public sealed record RiasecResultResponse(
    Guid Id,
    Guid TestAttemptId,
    int CScore,
    int HScore,
    int AScore,
    int SScore,
    int IScore,
    int DScore,
    int EScore,
    string PrimaryCode,
    string SecondaryCode,
    DateTime CreatedAtUtc,
    string AlgorithmVersion);