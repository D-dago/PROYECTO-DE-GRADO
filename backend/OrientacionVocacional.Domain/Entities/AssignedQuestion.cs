namespace OrientacionVocacional.Domain.Entities;

/// <summary>
/// Registra que preguntas se sortearon para un intento especifico (4 por dimension,
/// elegidas al azar del banco de 10) y en que orden le aparecen a ese estudiante.
/// Sin esto no habria forma de saber, al reanudar un intento, cuales 24 preguntas
/// ya se le habian asignado.
/// </summary>
public sealed class AssignedQuestion
{
    public Guid Id { get; set; }
    public Guid TestAttemptId { get; set; }
    public TestAttempt TestAttempt { get; set; } = null!;
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int Order { get; set; }
}
