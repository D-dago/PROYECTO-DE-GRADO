using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

/// <summary>
/// "Continuar con Google" mediante Firebase: el servidor solo acepta tokens firmados por Google para
/// NUESTRO proyecto, vigentes y con el correo verificado. Se firma con una clave de prueba en vez de la real.
/// </summary>
public sealed class FirebaseTokenTests
{
    private const string Project = "orientacion-prueba";
    private static readonly RsaSecurityKey GoogleKey = new(RSA.Create(2048)) { KeyId = "clave-google" };
    private static readonly RsaSecurityKey OtherKey = new(RSA.Create(2048)) { KeyId = "clave-google" };

    private sealed class FixedKeys(SecurityKey key) : IFirebaseSigningKeys
    {
        public Task<IReadOnlyCollection<SecurityKey>> GetAsync(CancellationToken cancellationToken) => Task.FromResult<IReadOnlyCollection<SecurityKey>>([key]);
    }

    private static GoogleTokenValidator Validator(string project = Project) =>
        new(Options.Create(new FirebaseOptions { ProjectId = project, ApiKey = "clave-publica", AuthDomain = $"{project}.firebaseapp.com", AppId = "1:2:web:3" }), new FixedKeys(GoogleKey));

    private static string Token(
        SecurityKey? key = null, string audience = Project, string? issuer = null, bool verified = true,
        DateTime? expires = null, string name = "Ana María Rojas", string email = "ana@gmail.com")
    {
        var now = DateTime.UtcNow;
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer ?? $"https://securetoken.google.com/{Project}",
            Audience = audience,
            IssuedAt = now.AddMinutes(-10),
            NotBefore = now.AddMinutes(-10),
            Expires = expires ?? now.AddMinutes(50),
            Subject = new ClaimsIdentity([
                new Claim("sub", "uid-123"),
                new Claim("email", email),
                new Claim("email_verified", verified ? "true" : "false", ClaimValueTypes.Boolean),
                new Claim("name", name),
            ]),
            SigningCredentials = new SigningCredentials(key ?? GoogleKey, SecurityAlgorithms.RsaSha256),
        };
        return new JwtSecurityTokenHandler().CreateEncodedJwt(descriptor);
    }

    [Fact]
    public async Task A_valid_firebase_token_gives_the_google_identity()
    {
        var identity = await Validator().ValidateAsync(Token(), CancellationToken.None);

        Assert.NotNull(identity);
        Assert.Equal("ana@gmail.com", identity!.Email);
        Assert.Equal("Ana", identity.FirstName);
        Assert.Equal("María Rojas", identity.LastName);
    }

    [Fact]
    public async Task Tokens_of_another_project_signed_by_someone_else_expired_or_unverified_are_rejected()
    {
        var validator = Validator();

        Assert.Null(await validator.ValidateAsync(Token(audience: "otro-proyecto"), CancellationToken.None));
        Assert.Null(await validator.ValidateAsync(Token(issuer: "https://securetoken.google.com/otro-proyecto"), CancellationToken.None));
        Assert.Null(await validator.ValidateAsync(Token(key: OtherKey), CancellationToken.None)); // firma falsificada
        Assert.Null(await validator.ValidateAsync(Token(expires: DateTime.UtcNow.AddMinutes(-5)), CancellationToken.None));
        Assert.Null(await validator.ValidateAsync(Token(verified: false), CancellationToken.None));
        Assert.Null(await validator.ValidateAsync("esto-no-es-un-token", CancellationToken.None));
    }

    [Fact]
    public async Task Without_firebase_settings_google_sign_in_is_off()
    {
        var validator = new GoogleTokenValidator(Options.Create(new FirebaseOptions()), new FixedKeys(GoogleKey));

        Assert.False(validator.IsConfigured);
        Assert.Null(await validator.ValidateAsync(Token(), CancellationToken.None));
    }
}
