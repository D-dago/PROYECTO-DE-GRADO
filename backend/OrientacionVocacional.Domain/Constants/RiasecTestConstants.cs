namespace OrientacionVocacional.Domain.Constants;

/// <summary>
/// Parametros del test vocacional. El nombre "Riasec" es historico: desde la version
/// CHASIDE el test mide 7 areas (C, H, A, S, I, D, E) tomadas del test "Chaside".
/// </summary>
public static class RiasecTestConstants
{
    // V1/V2 eran RIASEC (60 y 12 items); "CHASIDE-14-V1" sortea 14 por intento
    // (2 por cada una de las 7 areas, al azar dentro del banco de 60).
    public const string Version = "CHASIDE-14-V1";
    public const int QuestionsPerDimensionPerAttempt = 2; // cuantas preguntas de cada area se sortean por intento
    public static readonly string[] DimensionOrder = ["C", "H", "A", "S", "I", "D", "E"];
    // Puntaje maximo posible por area (2 preguntas x valor 5). Las compatibilidades se calculan contra este maximo.
    public static int MaxDimensionScore => QuestionsPerDimensionPerAttempt * 5;
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;
}
