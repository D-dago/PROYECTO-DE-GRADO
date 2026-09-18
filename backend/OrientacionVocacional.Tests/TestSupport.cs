using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Tests;

internal static class TestSupport
{
    public static OrientacionVocacionalDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OrientacionVocacionalDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new OrientacionVocacionalDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    /// <summary>
    /// Crea y persiste un estudiante minimo. TestService necesita que el userId
    /// corresponda a una fila real en Users (para leer/otorgar monedas), a
    /// diferencia de antes donde un Guid arbitrario alcanzaba para las pruebas.
    /// </summary>
    public static async Task<Guid> CreatePersistedStudentAsync(OrientacionVocacionalDbContext context)
    {
        var role = await context.Roles.SingleAsync(candidate => candidate.Name == RoleNames.Student);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = $"{Guid.NewGuid()}@test.local",
            FirstName = "Test",
            LastName = "Student",
            PasswordHash = "hash",
            RoleId = role.Id,
            Role = role,
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.Id;
    }
}
