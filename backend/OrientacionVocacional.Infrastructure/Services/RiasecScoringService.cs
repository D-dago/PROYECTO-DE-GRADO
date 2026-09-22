using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class RiasecScoringService : IRiasecScoringService
{
    private static readonly string[] DimensionOrder = RiasecTestConstants.DimensionOrder;

    public RiasecScoreResult Calculate(IEnumerable<Answer> answers)
    {
        var scores = DimensionOrder.ToDictionary(
            dimension => dimension,
            dimension => answers
                .Where(answer => answer.Question.Dimension == dimension)
                .Sum(answer => answer.Value));
        // Empates: gana la letra que va primero en DimensionOrder (C,H,A,S,I,D,E).
        var ordered = DimensionOrder
            .Select((dimension, index) => (dimension, score: scores[dimension], index))
            .OrderByDescending(item => item.score)
            .ThenBy(item => item.index)
            .ToArray();

        return new RiasecScoreResult(
            scores["C"], scores["H"], scores["A"], scores["S"], scores["I"], scores["D"], scores["E"],
            ordered[0].dimension + ordered[1].dimension,
            ordered[2].dimension);
    }
}
