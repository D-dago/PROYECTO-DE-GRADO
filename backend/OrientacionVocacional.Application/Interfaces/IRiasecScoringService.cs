using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Application.Interfaces;

public sealed record RiasecScoreResult(
    int CScore,
    int HScore,
    int AScore,
    int SScore,
    int IScore,
    int DScore,
    int EScore,
    string PrimaryCode,
    string SecondaryCode);

public interface IRiasecScoringService
{
    RiasecScoreResult Calculate(IEnumerable<Answer> answers);
}
