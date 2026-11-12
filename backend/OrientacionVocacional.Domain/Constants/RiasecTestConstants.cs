namespace OrientacionVocacional.Domain.Constants;

public static class RiasecTestConstants
{
    // "V1" administraba las 60 preguntas del banco propio completo. "V2" sorteaba 12 por intento
    // (2 por dimension) y "V3" sorteaba 24 (4 por dimension). "V4" aplica el Perfil de Intereses
    // O*NET en espanol (30 actividades, 5 por dimension): una forma fija, igual para todos, con la
    // escala "Me disgusta mucho ... Me gusta mucho" puntuada de 0 a 4 y el perfil calculado como en
    // O*NET. Con el sorteo cada estudiante respondia un test distinto y los puntajes no se podian
    // comparar (ver docs/decisiones.md, 2026-11-12).
    public const string Version = "ONET-30-V4";
    // Los resultados de versiones anteriores siguen siendo RIASEC (mismas letras): se siguen
    // mostrando y recomendando, medidos sobre su propio maximo.
    public const string PreviousVersion = "RIASEC-24-V3";
    public const string OlderVersion = "RIASEC-12-V2";
    public static readonly string[] InterpretableVersions = [Version, PreviousVersion, OlderVersion];
    public const int QuestionsPerDimension = 5; // actividades O*NET por dimension
    public const int QuestionsPerDimensionPerAttempt = 5; // forma fija: todas, sin sorteo
    public static readonly string[] DimensionOrder = ["R", "I", "A", "S", "E", "C"];
    // Cada respuesta (1-5) vale de 0 a 4 en O*NET: 5 actividades x 4 = 20 por dimension.
    // Las compatibilidades se calculan contra este maximo.
    public const int MaxItemScore = 4;
    public static int MaxDimensionScore => QuestionsPerDimensionPerAttempt * MaxItemScore;
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;

    public static bool IsInterpretable(string? version) => version is not null && InterpretableVersions.Contains(version);

    /// <summary>Si el intento se puntua como O*NET (respuesta - 1) o como las versiones anteriores (suma de 1 a 5).</summary>
    public static bool UsesOnetScoring(string? version) => version == Version;

    /// <summary>Maximo por dimension del test con el que se obtuvo un resultado (20 en V4 y V3, 10 en V2).</summary>
    public static int MaxDimensionScoreFor(string? version) => version switch
    {
        OlderVersion => 2 * 5,
        PreviousVersion => 4 * 5,
        _ => MaxDimensionScore
    };
}
