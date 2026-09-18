using System.ComponentModel.DataAnnotations;

namespace OrientacionVocacional.Application.Contracts;

public sealed class RegisterRequest
{
    [Required, EmailAddress, MaxLength(320)]
    public required string Email { get; init; }

    [Required, MinLength(8), MaxLength(100)]
    public required string Password { get; init; }

    [Required, MaxLength(100)]
    public required string FirstName { get; init; }

    [Required, MaxLength(100)]
    public required string LastName { get; init; }
}

public sealed class LoginRequest
{
    [Required, EmailAddress]
    public required string Email { get; init; }

    [Required]
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
    int? GraduationYear);

public sealed class UpdateStudentProfileRequest
{
    [MaxLength(30)]
    public string? Phone { get; init; }

    [MaxLength(100)]
    public string? City { get; init; }

    [MaxLength(200)]
    public string? School { get; init; }

    [Range(1900, 2200)]
    public int? GraduationYear { get; init; }
}