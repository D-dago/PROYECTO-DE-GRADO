using OrientacionVocacional.Domain.Enums;

namespace OrientacionVocacional.Domain.Entities;

public sealed class TestAttempt
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }
    public TestAttemptStatus Status { get; set; } = TestAttemptStatus.InProgress;
    public required string TestVersion { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Answer> Answers { get; } = [];
    public RiasecProfile? RiasecProfile { get; set; }
}