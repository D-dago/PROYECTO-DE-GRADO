namespace OrientacionVocacional.Domain.Entities;

public sealed class Institution
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? Location { get; set; }
    public string? Municipality { get; set; }
    public required string Status { get; set; }
    public string? Source { get; set; }
    public DateTime? LastUpdatedUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Career> Careers { get; } = [];
}