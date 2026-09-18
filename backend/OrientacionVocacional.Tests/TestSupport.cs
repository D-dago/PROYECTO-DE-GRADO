using Microsoft.EntityFrameworkCore;
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
}
