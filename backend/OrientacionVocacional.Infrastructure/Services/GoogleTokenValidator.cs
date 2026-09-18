using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Interfaces;

namespace OrientacionVocacional.Infrastructure.Services;

/// <summary>
/// Valida el ID token de "Iniciar sesion con Google" preguntandole a Google
/// (endpoint tokeninfo) y comprobando que fue emitido para NUESTRA app (aud)
/// y que el correo esta verificado.
/// </summary>
public sealed class GoogleTokenValidator(IHttpClientFactory httpClientFactory, IOptions<GoogleOptions> options) : IGoogleTokenValidator
{
    private sealed record TokenInfo(
        [property: JsonPropertyName("aud")] string? Audience,
        [property: JsonPropertyName("email")] string? Email,
        [property: JsonPropertyName("email_verified")] string? EmailVerified,
        [property: JsonPropertyName("given_name")] string? GivenName,
        [property: JsonPropertyName("family_name")] string? FamilyName,
        [property: JsonPropertyName("name")] string? Name);

    public bool IsConfigured => !string.IsNullOrWhiteSpace(options.Value.ClientId);

    public async Task<GoogleIdentity?> ValidateAsync(string idToken, CancellationToken cancellationToken)
    {
        if (!IsConfigured || string.IsNullOrWhiteSpace(idToken)) return null;
        try
        {
            var client = httpClientFactory.CreateClient("google");
            var info = await client.GetFromJsonAsync<TokenInfo>(
                $"https://oauth2.googleapis.com/tokeninfo?id_token={Uri.EscapeDataString(idToken)}", cancellationToken);
            if (info is null || info.Audience != options.Value.ClientId) return null;
            if (string.IsNullOrWhiteSpace(info.Email) || !string.Equals(info.EmailVerified, "true", StringComparison.OrdinalIgnoreCase)) return null;

            var first = info.GivenName ?? info.Name ?? info.Email.Split('@')[0];
            return new GoogleIdentity(info.Email, first, info.FamilyName ?? string.Empty);
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or System.Text.Json.JsonException)
        {
            return null;
        }
    }
}
