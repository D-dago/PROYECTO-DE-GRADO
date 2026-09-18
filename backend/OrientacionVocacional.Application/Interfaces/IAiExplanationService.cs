using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface IAiExplanationService
{
    Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken);
}