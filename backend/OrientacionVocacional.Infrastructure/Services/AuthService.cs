using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class AuthService(
    OrientacionVocacionalDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    public async Task<AuthResponse?> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var email = NormalizeEmail(request.Email);
        var exists = await dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);

        if (exists)
        {
            return null;
        }

        var studentRole = await dbContext.Roles
            .SingleAsync(role => role.Name == RoleNames.Student, cancellationToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            RoleId = studentRole.Id,
            Role = studentRole,
            PasswordHash = string.Empty
        };
        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
        user.StudentProfile = new StudentProfile { Id = Guid.NewGuid(), User = user };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return CreateToken(user);
    }

    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var email = NormalizeEmail(request.Email);
        var user = await dbContext.Users
            .Include(candidate => candidate.Role)
            .SingleOrDefaultAsync(candidate => candidate.Email == email, cancellationToken);

        if (user is null || !user.IsActive)
        {
            return null;
        }

        var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        return verification == PasswordVerificationResult.Failed ? null : CreateToken(user);
    }

    /// <summary>
    /// Login con un proveedor externo (Google) ya validado: si el correo existe
    /// entra con esa cuenta; si no, crea un estudiante nuevo con una contrasena
    /// aleatoria inutilizable (solo podra entrar por el proveedor).
    /// </summary>
    public async Task<AuthResponse?> LoginExternalAsync(
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var normalized = NormalizeEmail(email);
        var user = await dbContext.Users
            .Include(candidate => candidate.Role)
            .SingleOrDefaultAsync(candidate => candidate.Email == normalized, cancellationToken);

        if (user is not null)
        {
            return user.IsActive ? CreateToken(user) : null;
        }

        var studentRole = await dbContext.Roles
            .SingleAsync(role => role.Name == RoleNames.Student, cancellationToken);
        user = new User
        {
            Id = Guid.NewGuid(),
            Email = normalized,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            RoleId = studentRole.Id,
            Role = studentRole,
            PasswordHash = string.Empty
        };
        user.PasswordHash = passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N"));
        user.StudentProfile = new StudentProfile { Id = Guid.NewGuid(), User = user };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
        return CreateToken(user);
    }

    private AuthResponse CreateToken(User user)
    {
        var options = jwtOptions.Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(options.ExpirationMinutes);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: options.Issuer,
            audience: options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), expiresAt, user.Role.Name);
    }

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
}