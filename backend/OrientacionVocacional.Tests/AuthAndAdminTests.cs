using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class AuthAndAdminTests
{
    private static AuthService CreateAuthService(OrientacionVocacionalDbContext context) =>
        new(context, new PasswordHasher<User>(), Options.Create(new JwtOptions
        {
            Issuer = "test-issuer",
            Audience = "test-audience",
            Key = new string('k', 32),
        }));

    [Fact]
    public async Task Register_creates_student_and_returns_token()
    {
        await using var context = TestSupport.CreateContext();
        var service = CreateAuthService(context);

        var response = await service.RegisterAsync(new RegisterRequest
        {
            Email = "nuevo@test.local",
            Password = "clave12345",
            FirstName = "Ana",
            LastName = "Perez",
        }, CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal("ESTUDIANTE", response!.Role);
        Assert.False(string.IsNullOrWhiteSpace(response.AccessToken));
        var stored = await context.Users.SingleAsync();
        Assert.NotEqual("clave12345", stored.PasswordHash);
    }

    [Fact]
    public async Task Register_rejects_duplicate_email_case_insensitively()
    {
        await using var context = TestSupport.CreateContext();
        var service = CreateAuthService(context);
        await service.RegisterAsync(new RegisterRequest { Email = "Dup@Test.local", Password = "clave12345", FirstName = "Ana", LastName = "Perez" }, CancellationToken.None);

        var second = await service.RegisterAsync(new RegisterRequest { Email = "dup@test.local", Password = "otraclave99", FirstName = "Otro", LastName = "Usuario" }, CancellationToken.None);

        Assert.Null(second);
        Assert.Equal(1, await context.Users.CountAsync());
    }

    [Fact]
    public async Task Login_rejects_wrong_password_and_unknown_email()
    {
        await using var context = TestSupport.CreateContext();
        var service = CreateAuthService(context);
        await service.RegisterAsync(new RegisterRequest { Email = "user@test.local", Password = "clave12345", FirstName = "Ana", LastName = "Perez" }, CancellationToken.None);

        var wrongPassword = await service.LoginAsync(new LoginRequest { Email = "user@test.local", Password = "incorrecta" }, CancellationToken.None);
        var unknownUser = await service.LoginAsync(new LoginRequest { Email = "otro@test.local", Password = "clave12345" }, CancellationToken.None);

        Assert.Null(wrongPassword);
        Assert.Null(unknownUser);
    }

    [Fact]
    public async Task Login_rejects_deactivated_user_even_with_correct_password()
    {
        await using var context = TestSupport.CreateContext();
        var service = CreateAuthService(context);
        await service.RegisterAsync(new RegisterRequest { Email = "user@test.local", Password = "clave12345", FirstName = "Ana", LastName = "Perez" }, CancellationToken.None);
        var user = await context.Users.SingleAsync();
        user.IsActive = false;
        await context.SaveChangesAsync();

        var result = await service.LoginAsync(new LoginRequest { Email = "user@test.local", Password = "clave12345" }, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task StudentProfile_update_persists_and_get_reflects_changes()
    {
        await using var context = TestSupport.CreateContext();
        var authService = CreateAuthService(context);
        await authService.RegisterAsync(new RegisterRequest { Email = "user@test.local", Password = "clave12345", FirstName = "Ana", LastName = "Perez" }, CancellationToken.None);
        var userId = (await context.Users.SingleAsync()).Id;
        var service = new StudentProfileService(context);

        var updated = await service.UpdateAsync(userId, new UpdateStudentProfileRequest { Phone = "70000000", City = "Cochabamba", School = "Colegio X", GraduationYear = 2024 }, CancellationToken.None);
        var fetched = await service.GetAsync(userId, CancellationToken.None);

        Assert.NotNull(updated);
        Assert.Equal("Cochabamba", fetched!.City);
        Assert.Equal(2024, fetched.GraduationYear);
    }

    [Fact]
    public async Task StudentProfile_returns_null_for_unknown_user()
    {
        await using var context = TestSupport.CreateContext();
        var service = new StudentProfileService(context);

        var result = await service.GetAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Admin_can_create_update_and_toggle_institution()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new NoAiExplanationService());

        var created = await service.CreateInstitutionAsync(new InstitutionRequest { Name = "Universidad X", Type = "Publica", Status = "PENDIENTE" }, CancellationToken.None);
        var updated = await service.UpdateInstitutionAsync(created.Id, new InstitutionRequest { Name = "Universidad X Actualizada", Type = "Publica", Status = "ACTIVA" }, CancellationToken.None);
        var deactivated = await service.SetInstitutionActiveAsync(created.Id, false, CancellationToken.None);
        var list = await service.GetInstitutionsAsync(CancellationToken.None);

        Assert.Equal("Universidad X Actualizada", updated!.Name);
        Assert.True(deactivated);
        Assert.False(list.Single(item => item.Id == created.Id).IsActive);
    }

    [Fact]
    public async Task Admin_cannot_create_career_under_inactive_institution()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new NoAiExplanationService());
        var institution = await service.CreateInstitutionAsync(new InstitutionRequest { Name = "Universidad Y", Type = "Privada", Status = "PENDIENTE", IsActive = false }, CancellationToken.None);

        var result = await service.CreateCareerAsync(new CareerRequest { Name = "Ingenieria", Status = "PENDIENTE", InstitutionId = institution.Id }, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task Admin_compatibility_rules_can_be_created_updated_and_deleted()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new NoAiExplanationService());
        var institution = await service.CreateInstitutionAsync(new InstitutionRequest { Name = "Universidad Z", Type = "Publica", Status = "ACTIVA" }, CancellationToken.None);
        var career = await service.CreateCareerAsync(new CareerRequest { Name = "Sistemas", Status = "ACTIVA", InstitutionId = institution.Id }, CancellationToken.None);

        var added = await service.AddCompatibilityAsync(new CompatibilityRequest { CareerId = career!.Id, Dimension = "i", CompatibilityLevel = 80, Rule = "Alta afinidad investigativa." }, CancellationToken.None);
        var rules = await service.GetCompatibilitiesAsync(CancellationToken.None);
        var rejectedInvalidLevel = await service.AddCompatibilityAsync(new CompatibilityRequest { CareerId = career.Id, Dimension = "I", CompatibilityLevel = 150, Rule = "Invalida." }, CancellationToken.None);
        var updated = await service.UpdateCompatibilityAsync(rules[0].Id, new CompatibilityRequest { CareerId = career.Id, Dimension = "I", CompatibilityLevel = 95, Rule = "Actualizada." }, CancellationToken.None);
        var deleted = await service.DeleteCompatibilityAsync(rules[0].Id, CancellationToken.None);

        Assert.True(added);
        Assert.Equal("I", rules[0].Dimension);
        Assert.Equal("Sistemas", rules[0].CareerName);
        Assert.False(rejectedInvalidLevel);
        Assert.True(updated);
        Assert.True(deleted);
        Assert.Empty(await service.GetCompatibilitiesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task Admin_dashboard_counts_reflect_persisted_data()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new NoAiExplanationService());
        var institution = await service.CreateInstitutionAsync(new InstitutionRequest { Name = "Universidad W", Type = "Publica", Status = "ACTIVA" }, CancellationToken.None);
        await service.CreateCareerAsync(new CareerRequest { Name = "Derecho", Status = "ACTIVA", InstitutionId = institution.Id }, CancellationToken.None);

        var dashboard = await service.GetDashboardAsync(CancellationToken.None);

        Assert.Equal(1, dashboard.Institutions);
        Assert.Equal(1, dashboard.Careers);
    }

    [Fact]
    public async Task Admin_can_list_students_and_toggle_active_state()
    {
        await using var context = TestSupport.CreateContext();
        var authService = CreateAuthService(context);
        await authService.RegisterAsync(new RegisterRequest { Email = "estudiante@test.local", Password = "clave12345", FirstName = "Luis", LastName = "Gomez" }, CancellationToken.None);
        var userId = (await context.Users.SingleAsync()).Id;
        var catalogService = new CatalogService(context, new NoAiExplanationService());

        var deactivated = await catalogService.SetStudentActiveAsync(userId, false, CancellationToken.None);
        var students = await catalogService.GetStudentsAsync(CancellationToken.None);
        var loginAfterDeactivation = await authService.LoginAsync(new LoginRequest { Email = "estudiante@test.local", Password = "clave12345" }, CancellationToken.None);

        Assert.True(deactivated);
        Assert.False(students.Single().IsActive);
        Assert.Null(loginAfterDeactivation);
    }

    private sealed class NoAiExplanationService : Application.Interfaces.IAiExplanationService
    {
        public Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken) =>
            Task.FromResult("Explicación mediante IA no disponible actualmente.");
    }
}
