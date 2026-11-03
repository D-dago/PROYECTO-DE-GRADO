using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(IAuthService authService, IGoogleTokenValidator googleValidator) : ControllerBase
{
    public sealed record GoogleLoginRequest(string IdToken);

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> GoogleLogin(
        GoogleLoginRequest request,
        CancellationToken cancellationToken)
    {
        if (!googleValidator.IsConfigured)
        {
            return StatusCode(503, new { message = "El inicio con Google no está configurado en el servidor (falta Google:ClientId)." });
        }

        var identity = await googleValidator.ValidateAsync(request.IdToken, cancellationToken);
        if (identity is null)
        {
            return Unauthorized(new { message = "No se pudo validar tu cuenta de Google." });
        }

        try
        {
            var response = await authService.LoginExternalAsync(identity.Email, identity.FirstName, identity.LastName, cancellationToken);
            return response is null ? Unauthorized(new { message = "Tu cuenta está desactivada." }) : Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(503, new { message = exception.Message });
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var emailProblem = RegistrationPolicy.CheckEmail(request.Email.Trim());
        if (emailProblem is not null) return BadRequest(new { message = emailProblem });

        try
        {
            var response = await authService.RegisterAsync(request, cancellationToken);
            return response is null
                ? Conflict(new { message = "El correo ya está registrado." })
                : Created("api/auth/me", response);
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(503, new { message = exception.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return response is null
            ? Unauthorized(new { message = "Credenciales inválidas." })
            : Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout() => NoContent();
}