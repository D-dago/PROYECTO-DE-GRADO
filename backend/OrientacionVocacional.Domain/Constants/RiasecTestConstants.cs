namespace OrientacionVocacional.Domain.Constants;

public static class RiasecTestConstants
{
    // "V1" administraba las 60 preguntas del banco completo. "V2" sorteaba 12 por intento
    // (2 por dimension). "V3" sortea 24 (4 por dimension, al azar dentro del banco de 10):
    // el doble de items por dimension da un perfil mas estable sin volver a las 60 preguntas,
    // y todas las dimensiones siguen con la misma cantidad de items (comparables entre si).
    public const string Version = "RIASEC-24-V3";
    // Los resultados de la version anterior siguen siendo RIASEC (mismas letras y escala 1-5),
    // solo cambia la cantidad de preguntas: se siguen mostrando y recomendando, medidos sobre
    // su propio maximo.
    public const string PreviousVersion = "RIASEC-12-V2";
    public static readonly string[] InterpretableVersions = [Version, PreviousVersion];
    public const int QuestionsPerDimension = 10; // tamano del banco por dimension
    public const int QuestionsPerDimensionPerAttempt = 4; // cuantas de esas 10 se sortean por intento
    private const int PreviousQuestionsPerDimensionPerAttempt = 2;
    public static readonly string[] DimensionOrder = ["R", "I", "A", "S", "E", "C"];
    // Puntaje maximo posible por dimension (4 preguntas x valor 5). Las compatibilidades
    // se calculan contra este maximo.
    public static int MaxDimensionScore => QuestionsPerDimensionPerAttempt * 5;
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;

    public static bool IsInterpretable(string? version) => version is not null && InterpretableVersions.Contains(version);

    /// <summary>Maximo por dimension del test con el que se obtuvo un resultado (10 en V2, 20 en V3).</summary>
    public static int MaxDimensionScoreFor(string? version) =>
        version == PreviousVersion ? PreviousQuestionsPerDimensionPerAttempt * 5 : MaxDimensionScore;
}
