namespace OrientacionVocacional.Domain.Entities;

public sealed class RiasecProfile
{
    public Guid Id { get; set; }
    public Guid TestAttemptId { get; set; }
    public int RScore { get; set; }
    public int IScore { get; set; }
    public int AScore { get; set; }
    public int SScore { get; set; }
    public int EScore { get; set; }
    public int CScore { get; set; }
    public required string PrimaryCode { get; set; }
    public required string SecondaryCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public required string AlgorithmVersion { get; set; }
    public TestAttempt TestAttempt { get; set; } = null!;
}