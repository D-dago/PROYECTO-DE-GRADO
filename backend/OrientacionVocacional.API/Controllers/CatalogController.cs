using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/catalog")]
public sealed class CatalogController(ICatalogService service) : ControllerBase
{
    [HttpGet("institutions")]
    public async Task<ActionResult<IReadOnlyList<InstitutionResponse>>> Institutions(CancellationToken cancellationToken) => Ok(await service.GetInstitutionsAsync(cancellationToken));

    [HttpGet("careers")]
    public async Task<ActionResult<IReadOnlyList<CareerResponse>>> Careers(CancellationToken cancellationToken) => Ok(await service.GetCareersAsync(cancellationToken));

    [HttpPost("recommendations")]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<ActionResult<RecommendationResponse>> Recommendations(RecommendationFilterRequest request, CancellationToken cancellationToken)
    {
        var result = await service.GetRecommendationsAsync(GetUserId(), request, cancellationToken);
        return result is null ? NotFound(new { message = "Completa el test RIASEC antes de solicitar recomendaciones." }) : Ok(result);
    }

    [HttpGet("history")]
    [Authorize(Roles = RoleNames.Student)]
    public async Task<ActionResult<IReadOnlyList<HistoryResponse>>> History(CancellationToken cancellationToken) => Ok(await service.GetHistoryAsync(GetUserId(), cancellationToken));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}