using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.API.Live;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

/// <summary>Preguntas del test, usuarios registrados y descargas de resultados. Solo ADMINISTRADOR.</summary>
[ApiController]
[Route("api/admin")]
[Authorize(Roles = RoleNames.Administrator)]
[NotifyDataChange(DataArea.Activity)]
public sealed class AdminManagementController(IAdminService service) : ControllerBase
{
    private const string ExcelType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpGet("questions")]
    public async Task<ActionResult<IReadOnlyList<AdminQuestionResponse>>> Questions(CancellationToken cancellationToken) => Ok(await service.GetQuestionsAsync(cancellationToken));

    [HttpPost("questions")]
    public async Task<ActionResult<AdminQuestionResponse>> CreateQuestion(AdminQuestionRequest request, CancellationToken cancellationToken) => Ok(await service.CreateQuestionAsync(request, cancellationToken));

    [HttpPut("questions/{id:guid}")]
    public async Task<ActionResult<AdminQuestionResponse>> UpdateQuestion(Guid id, AdminQuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await service.UpdateQuestionAsync(id, request, cancellationToken);
        if (result.Success) return Ok(result.Question);
        return result.Question is null && result.Error == "La pregunta no existe." ? NotFound(new { message = result.Error }) : BadRequest(new { message = result.Error });
    }

    [HttpGet("users")]
    public async Task<ActionResult<IReadOnlyList<AdminRegisteredUser>>> Users(CancellationToken cancellationToken) => Ok(await service.GetUsersAsync(cancellationToken));

    [HttpGet("results")]
    public async Task<ActionResult<IReadOnlyList<AdminResultRow>>> Results(CancellationToken cancellationToken) => Ok(await service.GetResultsAsync(cancellationToken));

    [HttpGet("results/export/excel")]
    public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken)
    {
        var bytes = service.BuildExcel(await service.GetResultsAsync(cancellationToken), await service.GetAnswersAsync(cancellationToken));
        return File(bytes, ExcelType, $"resultados-{DateTime.Now:yyyyMMdd-HHmm}.xlsx");
    }

    [HttpGet("results/export/pdf")]
    public async Task<IActionResult> ExportPdf(CancellationToken cancellationToken) =>
        File(service.BuildPdf(await service.GetResultsAsync(cancellationToken)), "application/pdf", $"resultados-{DateTime.Now:yyyyMMdd-HHmm}.pdf");
}
