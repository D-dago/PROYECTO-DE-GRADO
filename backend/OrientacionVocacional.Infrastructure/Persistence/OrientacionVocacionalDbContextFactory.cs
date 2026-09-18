using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrientacionVocacional.Infrastructure.Persistence;

public sealed class OrientacionVocacionalDbContextFactory : IDesignTimeDbContextFactory<OrientacionVocacionalDbContext>
{
    public OrientacionVocacionalDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQL");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Configure ConnectionStrings__PostgreSQL antes de ejecutar comandos de EF Core.");
        }

        var options = new DbContextOptionsBuilder<OrientacionVocacionalDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new OrientacionVocacionalDbContext(options);
    }
}