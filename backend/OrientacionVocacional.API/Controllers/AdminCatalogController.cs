using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/admin/catalog")]
[Authorize(Roles = RoleNames.Administrator)]
public sealed class AdminCatalogController(ICatalogService service) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<AdminDashboardResponse>> Dashboard(CancellationToken cancellationToken) => Ok(await service.GetDashboardAsync(cancellationToken));

    [HttpPost("institutions")]
    public async Task<ActionResult<InstitutionResponse>> CreateInstitution(InstitutionRequest request, CancellationToken cancellationToken) => Ok(await service.CreateInstitutionAsync(request, cancellationToken));

    [HttpGet("careers")]
    public async Task<ActionResult<IReadOnlyList<CareerResponse>>> Careers(CancellationToken cancellationToken) => Ok(await service.GetAllCareersForAdminAsync(cancellationToken));

    [HttpPost("careers")]
    public async Task<ActionResult<CareerResponse>> CreateCareer(CareerRequest request, CancellationToken cancellationToken)
    {
        var result = await service.CreateCareerAsync(request, cancellationToken);
        return result is null ? BadRequest(new { message = "La institucion no existe o esta inactiva." }) : Ok(result);
    }

    [HttpPut("institutions/{id:guid}")]
    public async Task<ActionResult<InstitutionResponse>> UpdateInstitution(Guid id, InstitutionRequest request, CancellationToken cancellationToken) => (await service.UpdateInstitutionAsync(id, request, cancellationToken)) is { } result ? Ok(result) : NotFound();

    [HttpPut("careers/{id:guid}")]
    public async Task<ActionResult<CareerResponse>> UpdateCareer(Guid id, CareerRequest request, CancellationToken cancellationToken) => (await service.UpdateCareerAsync(id, request, cancellationToken)) is { } result ? Ok(result) : NotFound();

    [HttpGet("compatibilities")]
    public async Task<ActionResult<IReadOnlyList<CompatibilityResponse>>> Compatibilities(CancellationToken cancellationToken) => Ok(await service.GetCompatibilitiesAsync(cancellationToken));

    [HttpPost("compatibilities")]
    public async Task<IActionResult> AddCompatibility(CompatibilityRequest request, CancellationToken cancellationToken) => await service.AddCompatibilityAsync(request, cancellationToken) ? NoContent() : BadRequest(new { message = "Regla invalida o carrera inexistente." });

    [HttpPut("compatibilities/{id:guid}")]
    public async Task<IActionResult> UpdateCompatibility(Guid id, CompatibilityRequest request, CancellationToken cancellationToken) => await service.UpdateCompatibilityAsync(id, request, cancellationToken) ? NoContent() : NotFound();

    [HttpDelete("compatibilities/{id:guid}")]
    public async Task<IActionResult> DeleteCompatibility(Guid id, CancellationToken cancellationToken) => await service.DeleteCompatibilityAsync(id, cancellationToken) ? NoContent() : NotFound();

    [HttpGet("students")]
    public async Task<ActionResult<IReadOnlyList<StudentAdminResponse>>> Students(CancellationToken cancellationToken) => Ok(await service.GetStudentsAsync(cancellationToken));

    [HttpPatch("students/{id:guid}/active")]
    public async Task<IActionResult> SetStudentActive(Guid id, [FromQuery] bool active, CancellationToken cancellationToken) => await service.SetStudentActiveAsync(id, active, cancellationToken) ? NoContent() : NotFound();

    [HttpPatch("institutions/{id:guid}/active")]
    public async Task<IActionResult> SetInstitutionActive(Guid id, [FromQuery] bool active, CancellationToken cancellationToken) => await service.SetInstitutionActiveAsync(id, active, cancellationToken) ? NoContent() : NotFound();

    [HttpPatch("careers/{id:guid}/active")]
    public async Task<IActionResult> SetCareerActive(Guid id, [FromQuery] bool active, CancellationToken cancellationToken) => await service.SetCareerActiveAsync(id, active, cancellationToken) ? NoContent() : NotFound();
}