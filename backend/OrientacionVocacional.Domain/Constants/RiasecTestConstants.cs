namespace OrientacionVocacional.Domain.Constants;

public static class RiasecTestConstants
{
    // "V1" administraba las 60 preguntas del banco completo. "V2" sorteaba 12 por intento
    // (2 por dimension) y "V3" sorteaba 24 (4 por dimension). Con el sorteo cada estudiante
    // respondia un test distinto y los puntajes no se podian comparar entre estudiantes.
    // "V4" aplico las 60 del banco corregido (revision de Gabriela Lino, ORION #27) en forma fija.
    // "V5" mantiene la forma fija pero con 24: las 4 primeras activas de cada dimension, las mismas
    // para todos e intercaladas (ORION #28: el usuario prefirio un test mas corto).
    public const string Version = "RIASEC-24-V5";
    // Los resultados de versiones anteriores siguen siendo RIASEC (mismas letras y escala 1-5):
    // se siguen mostrando y recomendando, medidos sobre su propio maximo.
    public const string Fixed60Version = "RIASEC-60-V4";
    public const string PreviousVersion = "RIASEC-24-V3";
    public const string OlderVersion = "RIASEC-12-V2";
    public static readonly string[] InterpretableVersions = [Version, Fixed60Version, PreviousVersion, OlderVersion];
    public const int QuestionsPerDimension = 10; // tamano del banco por dimension
    public const int QuestionsPerDimensionPerAttempt = 4; // forma fija: las 4 primeras activas, sin sorteo
    public static readonly string[] DimensionOrder = ["R", "I", "A", "S", "E", "C"];
    // Puntaje maximo posible por dimension (4 preguntas x valor 5). Las compatibilidades
    // se calculan contra este maximo.
    public static int MaxDimensionScore => QuestionsPerDimensionPerAttempt * 5;
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;

    public static bool IsInterpretable(string? version) => version is not null && InterpretableVersions.Contains(version);

    /// <summary>Maximo por dimension del test con el que se obtuvo un resultado (20 en V5 y V3, 50 en V4, 10 en V2).</summary>
    public static int MaxDimensionScoreFor(string? version) => version switch
    {
        OlderVersion => 2 * 5,
        PreviousVersion => 4 * 5,
        Fixed60Version => 10 * 5,
        _ => MaxDimensionScore
    };
}
