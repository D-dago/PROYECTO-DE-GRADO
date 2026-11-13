namespace OrientacionVocacional.Domain.Constants;

public static class RiasecTestConstants
{
    // "V1" administraba las 60 preguntas del banco completo. "V2" sorteaba 12 por intento
    // (2 por dimension) y "V3" sorteaba 24 (4 por dimension). Con el sorteo cada estudiante
    // respondia un test distinto y los puntajes no se podian comparar entre estudiantes.
    // "V4" vuelve a la forma fija: las mismas 60 preguntas para todos (10 por dimension),
    // intercaladas, con el banco corregido segun la revision de Gabriela Lino (ORION #27).
    public const string Version = "RIASEC-60-V4";
    // Los resultados de versiones anteriores siguen siendo RIASEC (mismas letras y escala 1-5):
    // se siguen mostrando y recomendando, medidos sobre su propio maximo.
    public const string PreviousVersion = "RIASEC-24-V3";
    public const string OlderVersion = "RIASEC-12-V2";
    public static readonly string[] InterpretableVersions = [Version, PreviousVersion, OlderVersion];
    public const int QuestionsPerDimension = 10; // tamano del banco por dimension
    public const int QuestionsPerDimensionPerAttempt = 10; // forma fija: todas, sin sorteo
    public static readonly string[] DimensionOrder = ["R", "I", "A", "S", "E", "C"];
    // Puntaje maximo posible por dimension (10 preguntas x valor 5). Las compatibilidades
    // se calculan contra este maximo.
    public static int MaxDimensionScore => QuestionsPerDimensionPerAttempt * 5;
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;

    public static bool IsInterpretable(string? version) => version is not null && InterpretableVersions.Contains(version);

    /// <summary>Maximo por dimension del test con el que se obtuvo un resultado (50 en V4, 20 en V3, 10 en V2).</summary>
    public static int MaxDimensionScoreFor(string? version) => version switch
    {
        OlderVersion => 2 * 5,
        PreviousVersion => 4 * 5,
        _ => MaxDimensionScore
    };
}
