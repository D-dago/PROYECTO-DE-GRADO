using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/student/profile")]
[Authorize(Roles = RoleNames.Student)]
public sealed class StudentProfileController(IStudentProfileService profileService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserProfileResponse>> Get(CancellationToken cancellationToken)
    {
        var profile = await profileService.GetAsync(GetUserId(), cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPut]
    public async Task<ActionResult<UserProfileResponse>> Update(
        UpdateStudentProfileRequest request,
        CancellationToken cancellationToken)
    {
        var profile = await profileService.UpdateAsync(GetUserId(), request, cancellationToken);
        return profile is null ? NotFound() : Ok(profile);
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}