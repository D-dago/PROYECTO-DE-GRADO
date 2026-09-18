using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.RegisterAsync(request, cancellationToken);
        return response is null
            ? Conflict(new { message = "El correo ya esta registrado." })
            : Created("api/auth/me", response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return response is null
            ? Unauthorized(new { message = "Credenciales invalidas." })
            : Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout() => NoContent();
}