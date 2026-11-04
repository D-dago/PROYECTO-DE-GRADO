using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrientacionVocacional.API.Live;

namespace OrientacionVocacional.API.Controllers;

[ApiController]
[Route("api/live")]
[Authorize]
public sealed class LiveController(DataVersion version) : ControllerBase
{
    // Fuera del limite anti-abuso: un salon entero comparte la misma IP, y 30 pantallas preguntando cada 5 s
    // ya son 360 peticiones por minuto, mas que el tope de 300. Solo lee dos numeros en memoria.
    [HttpGet("version")]
    [DisableRateLimiting]
    public IActionResult Version()
    {
        Response.Headers.CacheControl = "no-store";
        return Ok(new { catalog = version.Catalog, activity = version.Activity });
    }
}
