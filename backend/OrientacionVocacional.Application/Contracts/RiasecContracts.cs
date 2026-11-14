namespace OrientacionVocacional.Application.Contracts;

// Answered: la pregunta ya tiene respuesta en este intento (para reanudar en la primera sin responder).
public sealed record QuestionResponse(Guid Id, string Code, string Text, string Dimension, int Order, bool Answered = false);

public sealed record TestAttemptResponse(
    Guid AttemptId,
    int Answered,
    int Total,
    decimal Progress,
    string Status,
    int Coins);

public sealed class SubmitAnswerRequest
{
    public Guid QuestionId { get; init; }
    public int Value { get; init; }
}

public sealed record RiasecResultResponse(
    Guid Id,
    Guid TestAttemptId,
    int RScore,
    int IScore,
    int AScore,
    int SScore,
    int EScore,
    int CScore,
    string PrimaryCode,
    string SecondaryCode,
    DateTime CreatedAtUtc,
    string AlgorithmVersion,
    // Cronometro total (ORION #31, sugerencia de la revision de Ch'iti Wasi): segundos desde que se
    // abrio el intento hasta que se termino. Lo calcula el servidor y solo se muestra con el resultado,
    // nunca mientras se responde. Nulo si el intento no lo registro.
    int? DurationSeconds = null)
{
    /// <summary>Maximo por dimension del test que produjo este resultado (10 con 12 preguntas, 20 con 24).</summary>
    public int MaxScore => OrientacionVocacional.Domain.Constants.RiasecTestConstants.MaxDimensionScoreFor(AlgorithmVersion);
}