using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Application.Interfaces;

public sealed record RiasecScoreResult(
    int RScore,
    int IScore,
    int AScore,
    int SScore,
    int EScore,
    int CScore,
    string PrimaryCode,
    string SecondaryCode);

public interface IRiasecScoringService
{
    RiasecScoreResult Calculate(IEnumerable<Answer> answers);
}