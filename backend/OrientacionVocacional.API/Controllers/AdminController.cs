using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Domain.Constants;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = RoleNames.Administrator)]
public sealed class AdminController : ControllerBase
{
    [HttpGet("access")]
    public IActionResult Access() => Ok(new
    {
        message = "Acceso de administrador habilitado.",
        ready = true
    });
}