namespace OrientacionVocacional.Domain.Entities;

public sealed class Answer
{
    public Guid Id { get; set; }
    public Guid TestAttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public int Value { get; set; }
    public DateTime AnsweredAtUtc { get; set; } = DateTime.UtcNow;
    public TestAttempt TestAttempt { get; set; } = null!;
    public Question Question { get; set; } = null!;
}