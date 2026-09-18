namespace OrientacionVocacional.Domain.Entities;

public sealed class RecommendationHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TestAttemptId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string? FilterSummary { get; set; }
    public User User { get; set; } = null!;
    public TestAttempt TestAttempt { get; set; } = null!;
}