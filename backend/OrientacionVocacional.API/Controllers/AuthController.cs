using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.API.Security;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    IAuthService authService,
    IGoogleTokenValidator googleValidator,
    PasswordWorkGate passwordWork,
    LoginAttemptGuard loginGuard,
    IOptions<FirebaseOptions> firebase) : ControllerBase
{
    /// <summary>
    /// Datos publicos de la app web de Firebase para "Continuar con Google". Se piden al abrir el boton, asi
    /// la web no se recompila para cambiarlos: basta con la configuracion del servidor (seccion "Firebase").
    /// </summary>
    [HttpGet("firebase-config")]
    [AllowAnonymous]
    public IActionResult FirebaseConfig()
    {
        var options = firebase.Value;
        if (!options.IsConfigured) return NotFound(new { message = "El inicio con Google todavía no está configurado (faltan los datos de Firebase en el servidor)." });
        Response.Headers.CacheControl = "no-store";
        return Ok(new { apiKey = options.ApiKey.Trim(), authDomain = options.AuthDomain.Trim(), projectId = options.ProjectId.Trim(), appId = options.AppId.Trim() });
    }

    public sealed record GoogleLoginRequest(string IdToken);

    /// <summary>
    /// Si la fila para registrarse o entrar es demasiado larga, se avisa al instante cuantos segundos esperar
    /// (encabezado Retry-After): la pagina reintenta sola y muestra que esta en fila, en vez de quedarse
    /// colgada hasta que internet corte la peticion.
    /// </summary>
    private ObjectResult? QueueIsFull()
    {
        if (passwordWork.RetryAfterSeconds() is not { } seconds) return null;
        Response.Headers.RetryAfter = seconds.ToString(System.Globalization.CultureInfo.InvariantCulture);
        return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = $"Hay muchos estudiantes entrando a la vez. Estás en fila: reintentamos en {seconds} segundos.", retryAfter = seconds });
    }

    [HttpPost("google")]
    [OrientacionVocacional.API.Live.NotifyDataChange(OrientacionVocacional.API.Live.DataArea.Activity)]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> GoogleLogin(
        GoogleLoginRequest request,
        CancellationToken cancellationToken)
    {
        if (!googleValidator.IsConfigured)
        {
            return StatusCode(503, new { message = "El inicio con Google no está configurado en el servidor (faltan los datos de Firebase)." });
        }

        var identity = await googleValidator.ValidateAsync(request.IdToken, cancellationToken);
        if (identity is null)
        {
            return Unauthorized(new { message = "No se pudo validar tu cuenta de Google." });
        }

        if (QueueIsFull() is { } busyGoogle) return busyGoogle;

        try
        {
            // Una cuenta nueva por Google tambien calcula un hash: espera su turno como las demas.
            var response = await passwordWork.RunAsync(() => authService.LoginExternalAsync(identity.Email, identity.FirstName, identity.LastName, cancellationToken), cancellationToken);
            return response is null ? Unauthorized(new { message = "Tu cuenta está desactivada." }) : Ok(response);
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(503, new { message = exception.Message });
        }
    }

    [HttpPost("register")]
    [OrientacionVocacional.API.Live.NotifyDataChange(OrientacionVocacional.API.Live.DataArea.Activity)]
    [AllowAnonymous]
    [EnableRateLimiting("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var emailProblem = RegistrationPolicy.CheckEmail(request.Email.Trim());
        if (emailProblem is not null) return BadRequest(new { message = emailProblem });
        if (QueueIsFull() is { } busy) return busy;

        try
        {
            var response = await passwordWork.RunAsync(() => authService.RegisterAsync(request, cancellationToken), cancellationToken);
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
        if (loginGuard.IsLocked(request.Email))
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, new { message = $"Demasiados intentos fallidos con este correo. Espera {LoginAttemptGuard.LockMinutes} minutos e inténtalo de nuevo." });
        }

        if (QueueIsFull() is { } busy) return busy;

        var response = await passwordWork.RunAsync(() => authService.LoginAsync(request, cancellationToken), cancellationToken);
        if (response is null)
        {
            loginGuard.RegisterFailure(request.Email);
            return Unauthorized(new { message = "Credenciales inválidas." });
        }

        loginGuard.Reset(request.Email);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout() => NoContent();
}