namespace OrientacionVocacional.Domain.Entities;

/// <summary>Skin comprada por un estudiante (la skin basica es gratis y no se registra).</summary>
public sealed class UserSkin
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public required string SkinId { get; set; }
    public DateTime PurchasedAtUtc { get; set; } = DateTime.UtcNow;
}
