using System.ComponentModel.DataAnnotations;

namespace OrientacionVocacional.Application.Contracts;

public sealed class RegisterRequest
{
    [Required, EmailAddress(ErrorMessage = "El correo no es válido."), MaxLength(254)]
    public required string Email { get; init; }

    [Required, RegularExpression(InputRules.Password, ErrorMessage = InputRules.PasswordMessage)]
    public required string Password { get; init; }

    [Required, MinLength(2, ErrorMessage = "Mínimo 2 letras."), MaxLength(50, ErrorMessage = "Máximo 50 letras."), RegularExpression(InputRules.LettersOnly, ErrorMessage = InputRules.LettersOnlyMessage)]
    public required string FirstName { get; init; }

    [Required, MinLength(2, ErrorMessage = "Mínimo 2 letras."), MaxLength(50, ErrorMessage = "Máximo 50 letras."), RegularExpression(InputRules.LettersOnly, ErrorMessage = InputRules.LettersOnlyMessage)]
    public required string LastName { get; init; }
}

public sealed class LoginRequest
{
    [Required, EmailAddress, MaxLength(254)]
    public required string Email { get; init; }

    [Required, MaxLength(64)]
    public required string Password { get; init; }
}

public sealed record AuthResponse(string AccessToken, DateTime ExpiresAtUtc, string Role);

public sealed record UserProfileResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    string? Phone,
    string? City,
    string? School,
    int? GraduationYear,
    int Coins,
    string EquippedSkin);

public sealed class UpdateStudentProfileRequest
{
    [RegularExpression(InputRules.DigitsOnly, ErrorMessage = InputRules.DigitsOnlyMessage)]
    public string? Phone { get; init; }

    [MaxLength(60), RegularExpression(InputRules.Place, ErrorMessage = InputRules.LettersOnlyMessage)]
    public string? City { get; init; }

    [MaxLength(100), RegularExpression(InputRules.Place, ErrorMessage = InputRules.LettersOnlyMessage)]
    public string? School { get; init; }

    [Range(2000, 2050, ErrorMessage = "El año debe estar entre 2000 y 2050.")]
    public int? GraduationYear { get; init; }
}