namespace OrientacionVocacional.Domain.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public bool IsActive { get; set; } = true;
    public int Coins { get; set; }
    public string EquippedSkin { get; set; } = "basic";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public StudentProfile? StudentProfile { get; set; }
}