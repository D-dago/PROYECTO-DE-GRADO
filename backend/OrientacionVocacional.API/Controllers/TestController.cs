using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/test")]
[Authorize(Roles = RoleNames.Student)]
public sealed class TestController(ITestService testService) : ControllerBase
{
    [HttpGet("questions")]
    public async Task<ActionResult<IReadOnlyList<QuestionResponse>>> GetQuestions(
        CancellationToken cancellationToken) =>
        Ok(await testService.GetQuestionsAsync(cancellationToken));

    [HttpPost("attempts")]
    public async Task<ActionResult<TestAttemptResponse>> CreateAttempt(CancellationToken cancellationToken) =>
        Ok(await testService.CreateAttemptAsync(GetUserId(), cancellationToken));

    [HttpGet("attempts/{attemptId:guid}")]
    public async Task<ActionResult<TestAttemptResponse>> GetAttempt(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await testService.GetAttemptAsync(GetUserId(), attemptId, cancellationToken);
        return attempt is null ? NotFound() : Ok(attempt);
    }

    [HttpPost("attempts/{attemptId:guid}/answers")]
    public async Task<ActionResult<TestAttemptResponse>> SubmitAnswer(
        Guid attemptId,
        SubmitAnswerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await testService.SubmitAnswerAsync(
            GetUserId(), attemptId, request, cancellationToken);
        return result.Success ? Ok(result.Attempt) : BadRequest(new { message = result.Error });
    }

    [HttpPost("attempts/{attemptId:guid}/complete")]
    public async Task<ActionResult<RiasecResultResponse>> Complete(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var result = await testService.CompleteAsync(GetUserId(), attemptId, cancellationToken);
        if (result.Success)
        {
            return Ok(result.Result);
        }

        return BadRequest(new { message = result.Error });
    }

    [HttpGet("attempts/{attemptId:guid}/result")]
    public async Task<ActionResult<RiasecResultResponse>> GetResult(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var result = await testService.GetResultAsync(GetUserId(), attemptId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}