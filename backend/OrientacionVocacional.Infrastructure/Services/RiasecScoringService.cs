using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class RiasecScoringService : IRiasecScoringService
{
    private static readonly string[] DimensionOrder = RiasecTestConstants.DimensionOrder;

    // Desempate de O*NET: despues de la primera dimension se prefiere la mas cercana en el hexagono
    // (las dos vecinas, luego las alternas y al final la opuesta). Para la primera, el orden RIASEC.
    private static readonly Dictionary<string, string[]> TieOrder = new()
    {
        [""] = ["R", "I", "A", "S", "E", "C"],
        ["R"] = ["C", "I", "E", "A", "S"],
        ["I"] = ["R", "A", "C", "S", "E"],
        ["A"] = ["I", "S", "R", "E", "C"],
        ["S"] = ["A", "E", "I", "C", "R"],
        ["E"] = ["S", "C", "A", "R", "I"],
        ["C"] = ["E", "R", "S", "I", "A"],
    };

    public RiasecScoreResult Calculate(IEnumerable<Answer> answers, string testVersion)
    {
        var list = answers.ToList();
        return RiasecTestConstants.UsesOnetScoring(testVersion) ? CalculateOnet(list) : CalculatePrevious(list);
    }

    /// <summary>
    /// Perfil de Intereses O*NET: cada respuesta 1-5 vale 0-4 y se suma por dimension (0-20). Solo son
    /// intereses principales las dimensiones por encima del promedio de las seis del propio estudiante
    /// (puntaje z mayor que 0), hasta tres, de mayor a menor; los empates se resuelven por cercania en el
    /// hexagono. Si ninguna supera el promedio (todas iguales), el perfil es plano y no hay codigo.
    /// </summary>
    private static RiasecScoreResult CalculateOnet(List<Answer> answers)
    {
        var scores = DimensionOrder.ToDictionary(
            dimension => dimension,
            dimension => answers
                .Where(answer => answer.Question.Dimension == dimension)
                .Sum(answer => Math.Max(answer.Value - 1, 0)));
        var top = TopInterests(scores);
        return new RiasecScoreResult(
            scores["R"], scores["I"], scores["A"], scores["S"], scores["E"], scores["C"],
            string.Concat(top.Take(2)),
            top.Count > 2 ? top[2] : string.Empty);
    }

    /// <summary>Intereses principales segun O*NET (0 a 3 letras, de mayor a menor).</summary>
    public static IReadOnlyList<string> TopInterests(IReadOnlyDictionary<string, int> scores)
    {
        // z > 0 equivale a puntaje > promedio; se compara en enteros (puntaje x 6 > total) para no
        // depender del redondeo de los decimales.
        var total = scores.Values.Sum();
        var above = scores.Where(pair => pair.Value * scores.Count > total).ToDictionary(pair => pair.Key, pair => pair.Value);

        List<string> Ranked(string after) => above.Keys
            .Where(dimension => dimension != after)
            .OrderByDescending(dimension => above[dimension])
            .ThenBy(dimension => Array.IndexOf(TieOrder[after], dimension))
            .ToList();

        var first = Ranked("");
        if (first.Count == 0) return [];
        var rest = Ranked(first[0]);
        return [first[0], .. rest.Take(2)];
    }

    /// <summary>Versiones anteriores (V2, V3): suma de 1 a 5 y codigo con las tres mas altas, empates en orden RIASEC.</summary>
    private static RiasecScoreResult CalculatePrevious(List<Answer> answers)
    {
        var scores = DimensionOrder.ToDictionary(
            dimension => dimension,
            dimension => answers
                .Where(answer => answer.Question.Dimension == dimension)
                .Sum(answer => answer.Value));
        var ordered = DimensionOrder
            .Select((dimension, index) => (dimension, score: scores[dimension], index))
            .OrderByDescending(item => item.score)
            .ThenBy(item => item.index)
            .ToArray();

        return new RiasecScoreResult(
            scores["R"], scores["I"], scores["A"], scores["S"], scores["E"], scores["C"],
            ordered[0].dimension + ordered[1].dimension,
            ordered[2].dimension);
    }
}
