namespace OrientacionVocacional.Domain.Entities;

/// <summary>
/// Perfil vocacional de un intento. El nombre "Riasec" es historico: hoy el test es
/// CHASIDE (7 areas). Las columnas corresponden a C, H, A, S, I, D, E.
/// </summary>
public sealed class RiasecProfile
{
    public Guid Id { get; set; }
    public Guid TestAttemptId { get; set; }
    public int CScore { get; set; }
    public int HScore { get; set; }
    public int AScore { get; set; }
    public int SScore { get; set; }
    public int IScore { get; set; }
    public int DScore { get; set; }
    public int EScore { get; set; }
    public required string PrimaryCode { get; set; }
    public required string SecondaryCode { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public required string AlgorithmVersion { get; set; }
    public TestAttempt TestAttempt { get; set; } = null!;
}
