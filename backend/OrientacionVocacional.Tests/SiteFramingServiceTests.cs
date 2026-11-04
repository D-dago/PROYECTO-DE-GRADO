using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class SiteFramingServiceTests
{
    private sealed class HandlerDePrueba(Func<HttpResponseMessage> respuesta) : HttpMessageHandler
    {
        public int Llamadas { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Llamadas++;
            return Task.FromResult(respuesta());
        }
    }

    private sealed class FabricaDePrueba(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler, disposeHandler: false);
    }

    private static async Task<(SiteFramingService Service, Guid Id)> CrearAsync(OrientacionVocacionalDbContext context, HttpMessageHandler handler)
    {
        var institution = new Institution { Id = Guid.NewGuid(), Name = "U", Type = "Universidad", Status = "VERIFICADA", Website = "https://u.edu.bo" };
        context.Add(institution);
        await context.SaveChangesAsync();
        return (new SiteFramingService(context, new FabricaDePrueba(handler), new MemoryCache(Options.Create(new MemoryCacheOptions()))), institution.Id);
    }

    [Fact]
    public async Task Site_that_cannot_be_checked_is_not_assumed_to_open_inside()
    {
        await using var context = TestSupport.CreateContext();
        var (service, id) = await CrearAsync(context, new HandlerDePrueba(() => throw new TaskCanceledException()));

        var result = await service.CheckInstitutionAsync(id, CancellationToken.None);

        Assert.False(result!.Embeddable);
        Assert.False(result.Verified);
    }

    [Fact]
    public async Task Site_that_forbids_framing_is_reported_as_verified()
    {
        await using var context = TestSupport.CreateContext();
        var (service, id) = await CrearAsync(context, new HandlerDePrueba(() =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.TryAddWithoutValidation("X-Frame-Options", "SAMEORIGIN");
            return response;
        }));

        var result = await service.CheckInstitutionAsync(id, CancellationToken.None);

        Assert.False(result!.Embeddable);
        Assert.True(result.Verified);
    }

    [Fact]
    public async Task Answer_is_reused_without_calling_the_site_again()
    {
        await using var context = TestSupport.CreateContext();
        var handler = new HandlerDePrueba(() => new HttpResponseMessage(HttpStatusCode.OK));
        var (service, id) = await CrearAsync(context, handler);

        var first = await service.CheckInstitutionAsync(id, CancellationToken.None);
        var second = await service.CheckInstitutionAsync(id, CancellationToken.None);

        Assert.True(first!.Embeddable);
        Assert.True(second!.Embeddable);
        Assert.Equal(1, handler.Llamadas);
    }
}
