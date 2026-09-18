namespace OrientacionVocacional.Domain.Entities;

public sealed class StudentProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string? Phone { get; set; }
    public string? City { get; set; }
    public string? School { get; set; }
    public int? GraduationYear { get; set; }
    public User User { get; set; } = null!;
}