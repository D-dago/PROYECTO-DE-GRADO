using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class AiExplanationService(IHttpClientFactory httpClientFactory, IOptions<AiOptions> options) : IAiExplanationService
{
    public const string UnavailableMessage = "Explicación mediante IA no disponible actualmente.";

    public async Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken)
    {
        var config = options.Value;
        if (string.IsNullOrWhiteSpace(config.ApiKey) || string.IsNullOrWhiteSpace(config.Model)) return UnavailableMessage;
        var payload = new { model = config.Model, temperature = 0.2, messages = new[] { new { role = "system", content = "Eres un orientador vocacional para bachilleres de Cochabamba, Bolivia. Responde en español sencillo y cercano, en 2 o 3 párrafos cortos. Primero explica qué significa el perfil RIASEC recibido (qué es cada letra, sin dar por hecho que el estudiante la conoce). Luego explica por qué las carreras recibidas en 'alternatives' son compatibles y qué considerar al elegir entre ellas. Si 'alternatives' viene vacío, explica solo el perfil y dile que aún no hay carreras cargadas. Explica solamente los datos recibidos. No inventes carreras, instituciones, costos, modalidades, ubicaciones ni requisitos, y no menciones costos ni ubicaciones: el estudiante los consulta en la página oficial de cada institución. No modifiques puntuaciones ni reglas. Recuerda que es una guía de intereses, no una decisión obligatoria." }, new { role = "user", content = JsonSerializer.Serialize(new { profile.RScore, profile.IScore, profile.AScore, profile.SScore, profile.EScore, profile.CScore, profile.PrimaryCode, profile.SecondaryCode, alternatives = alternatives.Take(10).Select(item => new { carrera = item.Name, institucion = item.InstitutionName, compatibilidad = item.Compatibility, motivo = item.CompatibilityReason }) }) } } };
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, config.ProviderUrl) { Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json") };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.ApiKey);
            using var response = await httpClientFactory.CreateClient("ai").SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode) return UnavailableMessage;
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
            return document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? UnavailableMessage;
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException or InvalidOperationException or KeyNotFoundException)
        {
            // La IA es un complemento opcional: si el proveedor tarda, se cae la red o
            // devuelve un formato inesperado, las recomendaciones estructuradas no deben
            // fallar por eso.
            return UnavailableMessage;
        }
    }
}

