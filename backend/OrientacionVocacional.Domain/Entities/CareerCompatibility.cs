namespace OrientacionVocacional.Domain.Entities;

public sealed class CareerCompatibility
{
    public Guid Id { get; set; }
    public Guid CareerId { get; set; }
    public required string Dimension { get; set; }
    public int CompatibilityLevel { get; set; }
    public required string Rule { get; set; }
    public Career Career { get; set; } = null!;
}