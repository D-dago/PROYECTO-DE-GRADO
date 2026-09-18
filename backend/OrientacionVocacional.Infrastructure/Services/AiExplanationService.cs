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
    public async Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken)
    {
        var config = options.Value;
        if (string.IsNullOrWhiteSpace(config.ApiKey) || string.IsNullOrWhiteSpace(config.Model)) return "Explicación mediante IA no disponible actualmente.";
        var payload = new { model = config.Model, temperature = 0.2, messages = new[] { new { role = "system", content = "Explica solamente los datos recibidos. No inventes carreras, instituciones, costos, modalidades, ubicaciones ni requisitos. No modifiques puntuaciones ni reglas." }, new { role = "user", content = JsonSerializer.Serialize(new { profile.RScore, profile.IScore, profile.AScore, profile.SScore, profile.EScore, profile.CScore, profile.PrimaryCode, profile.SecondaryCode, alternatives }) } } };
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, config.ProviderUrl) { Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json") };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", config.ApiKey);
            using var response = await httpClientFactory.CreateClient("ai").SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode) return "Explicación mediante IA no disponible actualmente.";
            using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync(cancellationToken));
            return document.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "Explicación mediante IA no disponible actualmente.";
        }
        catch (HttpRequestException) { return "Explicación mediante IA no disponible actualmente."; }
    }
}