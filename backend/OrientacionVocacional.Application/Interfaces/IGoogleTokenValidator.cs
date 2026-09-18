namespace OrientacionVocacional.Application.Interfaces;

public sealed record GoogleIdentity(string Email, string FirstName, string LastName);

public interface IGoogleTokenValidator
{
    bool IsConfigured { get; }
    Task<GoogleIdentity?> ValidateAsync(string idToken, CancellationToken cancellationToken);
}
