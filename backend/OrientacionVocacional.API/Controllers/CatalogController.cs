using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(ICatalogService service, ISiteFramingService siteFraming) : ControllerBase
{
    [HttpGet("institutions")]
    public async Task<ActionResult<IReadOnlyList<InstitutionResponse>>> Institutions(CancellationToken cancellationToken) => Ok(await service.GetInstitutionsAsync(cancellationToken));

    // Indica si la pagina oficial se puede mostrar dentro de una ventana interna o hay que abrirla aparte.
    [HttpGet("institutions/{id:guid}/site-check")]
    [Authorize]
    public async Task<ActionResult<SiteCheckResponse>> SiteCheck(Guid id, CancellationToken cancellationToken) =>
        await siteFraming.CheckInstitutionAsync(id, cancellationToken) is { } result ? Ok(result) : NotFound(new { message = "Esta institución no tiene una página oficial registrada." });

    [HttpGet("careers")]
    public async Task<ActionResult<IReadOnlyList<CareerResponse>>> Careers(CancellationToken cancellationToken) => Ok(await service.GetCareersAsync(cancellationToken));

    [HttpPost("recommendations")]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<ActionResult<RecommendationResponse>> Recommendations(RecommendationFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await service.GetRecommendationsAsync(GetUserId(), request, cancellationToken);
        return result is null ? NotFound(new { message = "Completa el test vocacional antes de solicitar recomendaciones." }) : Ok(result);
    }

    // Solo las carreras, para refrescar una pantalla abierta: no escribe en el historial ni llama a la IA.
    [HttpGet("my-careers")]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<ActionResult<IReadOnlyList<CareerResponse>>> MyCareers(CancellationToken cancellationToken) =>
        await service.GetCareerMatchesAsync(GetUserId(), cancellationToken) is { } result ? Ok(result) : NotFound(new { message = "Completa el test vocacional antes de solicitar recomendaciones." });

    [HttpGet("history")]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<ActionResult<IReadOnlyList<HistoryResponse>>> History(CancellationToken cancellationToken) => Ok(await service.GetHistoryAsync(GetUserId(), cancellationToken));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}