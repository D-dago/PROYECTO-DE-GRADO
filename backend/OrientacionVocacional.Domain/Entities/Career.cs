namespace OrientacionVocacional.Domain.Entities;

public sealed class Career
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Area { get; set; }
    public string? Level { get; set; }
    public string? Modality { get; set; }
    public int? DurationMonths { get; set; }
    public decimal? Cost { get; set; }
    public required string Status { get; set; }
    public string? Source { get; set; }
    public DateTime? LastUpdatedUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid InstitutionId { get; set; }
    public Institution Institution { get; set; } = null!;
    public ICollection<CareerCompatibility> Compatibilities { get; } = [];
}