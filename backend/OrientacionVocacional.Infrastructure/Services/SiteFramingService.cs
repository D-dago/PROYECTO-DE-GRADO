using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

/// <summary>
/// Muchas instituciones prohiben que su pagina se muestre dentro de otra, y el navegador no le deja a
/// la web averiguarlo: solo veria un recuadro de error. Por eso lo comprueba el servidor leyendo las
/// cabeceras del sitio. Solo consulta direcciones cargadas por el administrador, nunca una que venga
/// en la peticion del estudiante.
/// </summary>
public sealed class SiteFramingService(OrientacionVocacionalDbContext db, IHttpClientFactory httpClientFactory, IMemoryCache cache) : ISiteFramingService
{
    public const string HttpClientName = "site-check";

    public async Task<SiteCheckResponse?> CheckInstitutionAsync(Guid institutionId, CancellationToken cancellationToken)
    {
        var website = await db.Institutions.AsNoTracking()
            .Where(item => item.Id == institutionId && item.IsActive)
            .Select(item => item.Website)
            .SingleOrDefaultAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(website)) return null;

        var cacheKey = $"site-check:{website}";
        if (cache.TryGetValue(cacheKey, out SiteCheckResponse? cached) && cached is not null) return cached;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, website);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (compatible; OrientacionVocacional/1.0)");
            using var response = await httpClientFactory.CreateClient(HttpClientName)
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            var embeddable = AllowsFraming(
                response.Headers.TryGetValues("X-Frame-Options", out var frameOptions) ? frameOptions : [],
                response.Headers.TryGetValues("Content-Security-Policy", out var policies) ? policies : []);
            return cache.Set(cacheKey, new SiteCheckResponse(embeddable), TimeSpan.FromHours(6));
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or UriFormatException or InvalidOperationException)
        {
            // Sin comprobar no se asume que se puede: un sitio lento que prohibe mostrarse dentro de otra
            // pagina dejaba al estudiante frente a un recuadro de error. Abrirlo aparte funciona siempre.
            // Se reintenta pronto para no castigar a un sitio que solo tuvo un mal momento.
            return cache.Set(cacheKey, new SiteCheckResponse(false, Verified: false), TimeSpan.FromMinutes(1));
        }
    }

    /// <summary>
    /// Mismo orden que los navegadores: si hay "frame-ancestors" manda esa regla e ignoran
    /// X-Frame-Options. Se considera permitido solo si admite cualquier origen ("*"), porque el enlace
    /// publico de esta web cambia cada vez y un sitio nunca lo tendria en su lista.
    /// </summary>
    public static bool AllowsFraming(IEnumerable<string> frameOptions, IEnumerable<string> contentSecurityPolicies)
    {
        var ancestors = contentSecurityPolicies
            .SelectMany(policy => policy.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
            .Where(directive => directive.StartsWith("frame-ancestors", StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (ancestors.Count > 0)
            return ancestors.All(directive => directive.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Contains("*"));
        return !frameOptions.Any(value => !string.IsNullOrWhiteSpace(value));
    }
}
