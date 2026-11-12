namespace OrientacionVocacional.Application.Configuration;

/// <summary>
/// "Continuar con Google" mediante Firebase Authentication (plan gratuito Spark). Son los datos de la app
/// web de Firebase (Configuracion del proyecto > Tus apps). No son secretos: Firebase los publica en
/// cada pagina que lo usa; lo que protege el acceso es la lista de dominios autorizados del proyecto.
/// </summary>
public sealed class FirebaseOptions
{
    public const string SectionName = "Firebase";
    public string ProjectId { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string AuthDomain { get; init; } = string.Empty;
    public string AppId { get; init; } = string.Empty;

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(ProjectId) && !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(AuthDomain);
}
