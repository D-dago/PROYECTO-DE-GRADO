namespace OrientacionVocacional.Application.Configuration;

public sealed class AiOptions
{
    public const string SectionName = "AI";
    public string ProviderUrl { get; init; } = "https://api.openai.com/v1/chat/completions";
    public string Model { get; init; } = "";
    public string ApiKey { get; init; } = "";
}