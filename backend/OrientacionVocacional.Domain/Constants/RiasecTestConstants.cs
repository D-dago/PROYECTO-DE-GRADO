namespace OrientacionVocacional.Domain.Constants;

public static class RiasecTestConstants
{
    // "V1" administraba las 60 preguntas del banco completo. "V2" sortea 12 por
    // intento (2 por dimension, al azar dentro del banco de 10) para acortar el
    // test sin perder representacion de las 6 dimensiones ni comparabilidad entre
    // ellas (todas quedan siempre con la misma cantidad de items).
    public const string Version = "RIASEC-12-V2";
    public const int QuestionsPerDimension = 10; // tamano del banco por dimension
    public const int QuestionsPerDimensionPerAttempt = 2; // cuantas de esas 10 se sortean por intento
    public static readonly string[] DimensionOrder = ["R", "I", "A", "S", "E", "C"];
    public static int TotalQuestions => DimensionOrder.Length * QuestionsPerDimensionPerAttempt;
    public const int CoinsPerAnswer = 1;
    public const int CompletionBonusCoins = 50;
}
