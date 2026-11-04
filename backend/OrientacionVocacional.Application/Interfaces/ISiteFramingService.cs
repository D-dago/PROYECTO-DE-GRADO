using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface ISiteFramingService
{
    /// <summary>Devuelve null si la institucion no existe o no tiene pagina oficial registrada.</summary>
    Task<SiteCheckResponse?> CheckInstitutionAsync(Guid institutionId, CancellationToken cancellationToken);
}
