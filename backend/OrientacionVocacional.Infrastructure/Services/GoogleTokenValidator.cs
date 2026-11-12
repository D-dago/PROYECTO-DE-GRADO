using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Interfaces;

namespace OrientacionVocacional.Infrastructure.Services;

/// <summary>Claves publicas con las que Google firma los tokens de Firebase.</summary>
public interface IFirebaseSigningKeys
{
    Task<IReadOnlyCollection<SecurityKey>> GetAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Descarga las claves publicas de Firebase (JWKS) y las guarda en memoria unas horas: Google las rota
/// cada pocos dias, asi que no hace falta pedirlas en cada inicio de sesion.
/// </summary>
public sealed class FirebaseSigningKeys(IHttpClientFactory httpClientFactory, IMemoryCache cache) : IFirebaseSigningKeys
{
    public const string HttpClientName = "firebase-keys";
    private const string JwksUrl = "https://www.googleapis.com/service_accounts/v1/jwk/securetoken@system.gserviceaccount.com";
    private const string CacheKey = "firebase-jwks";

    public async Task<IReadOnlyCollection<SecurityKey>> GetAsync(CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(CacheKey, out IReadOnlyCollection<SecurityKey>? cached) && cached is not null) return cached;
        var json = await httpClientFactory.CreateClient(HttpClientName).GetStringAsync(JwksUrl, cancellationToken);
        IReadOnlyCollection<SecurityKey> keys = new JsonWebKeySet(json).GetSigningKeys().ToList();
        cache.Set(CacheKey, keys, TimeSpan.FromHours(6));
        return keys;
    }
}

/// <summary>
/// Valida el ID token que entrega Firebase Authentication tras "Continuar con Google". Se comprueba
/// aqui mismo, sin secretos ni el SDK de administrador: la firma (claves publicas de Google), el emisor y
/// la audiencia (nuestro proyecto de Firebase), la vigencia y que el correo este verificado.
/// </summary>
public sealed class GoogleTokenValidator(IOptions<FirebaseOptions> options, IFirebaseSigningKeys signingKeys) : IGoogleTokenValidator
{
    public bool IsConfigured => options.Value.IsConfigured;

    public async Task<GoogleIdentity?> ValidateAsync(string idToken, CancellationToken cancellationToken)
    {
        if (!IsConfigured || string.IsNullOrWhiteSpace(idToken)) return null;
        var projectId = options.Value.ProjectId.Trim();
        try
        {
            var keys = await signingKeys.GetAsync(cancellationToken);
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = $"https://securetoken.google.com/{projectId}",
                ValidAudience = projectId,
                IssuerSigningKeys = keys,
                ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.FromMinutes(2),
            };
            var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var principal = handler.ValidateToken(idToken, parameters, out _);

            var email = principal.FindFirst("email")?.Value;
            var verified = principal.FindFirst("email_verified")?.Value;
            if (string.IsNullOrWhiteSpace(email) || !string.Equals(verified, "true", StringComparison.OrdinalIgnoreCase)) return null;

            // Firebase entrega el nombre completo en "name": la primera palabra es el nombre y el resto el apellido.
            var fullName = (principal.FindFirst("name")?.Value ?? string.Empty).Trim();
            var parts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var first = parts.Length > 0 ? parts[0] : email.Split('@')[0];
            var last = parts.Length > 1 ? parts[1] : string.Empty;
            return new GoogleIdentity(email, first, last);
        }
        catch (Exception exception) when (exception is SecurityTokenException or ArgumentException or HttpRequestException or TaskCanceledException)
        {
            return null;
        }
    }
}
