using System.ComponentModel.DataAnnotations;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public sealed class InstitutionWebsiteTests
{
    private sealed class SinIa : IAiExplanationService
    {
        public Task<string> ExplainAsync(RiasecResultResponse profile, IReadOnlyList<CareerResponse> alternatives, CancellationToken cancellationToken) => Task.FromResult(string.Empty);
    }

    private static bool IsValid(string? website)
    {
        var model = new InstitutionRequest { Name = "Universidad", Type = "Universidad", Status = "PENDIENTE", Website = website };
        return Validator.TryValidateObject(model, new ValidationContext(model), [], validateAllProperties: true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("https://www.umss.edu.bo")]
    [InlineData("http://ejemplo.edu.bo/carreras?id=1")]
    [InlineData("HTTPS://EJEMPLO.EDU.BO")]
    public void Web_addresses_accepted(string? website) => Assert.True(IsValid(website));

    [Theory]
    [InlineData("javascript:alert(1)")]
    [InlineData("JaVaScRiPt:alert(1)")]
    [InlineData("data:text/html,<script>alert(1)</script>")]
    [InlineData("ftp://ejemplo.edu.bo")]
    [InlineData("www.ejemplo.edu.bo")]
    [InlineData("https://ejemplo.edu.bo/\"onload=\"alert(1)")]
    [InlineData("https://ejemplo.edu.bo/con espacio")]
    public void Dangerous_or_malformed_addresses_rejected(string website) => Assert.False(IsValid(website));

    [Fact]
    public async Task Website_is_saved_trimmed_and_blank_becomes_null()
    {
        await using var context = TestSupport.CreateContext();
        var service = new CatalogService(context, new SinIa());

        var created = await service.CreateInstitutionAsync(new InstitutionRequest { Name = "U", Type = "Universidad", Status = "PENDIENTE", Website = " https://u.edu.bo " }, CancellationToken.None);
        var cleared = await service.UpdateInstitutionAsync(created.Id, new InstitutionRequest { Name = "U", Type = "Universidad", Status = "PENDIENTE", Website = "  " }, CancellationToken.None);

        Assert.Equal("https://u.edu.bo", created.Website);
        Assert.Null(cleared!.Website);
    }

    [Fact]
    public void Site_without_restrictions_can_open_inside() =>
        Assert.True(SiteFramingService.AllowsFraming([], ["upgrade-insecure-requests"]));

    [Theory]
    [InlineData("SAMEORIGIN")]
    [InlineData("DENY")]
    public void X_frame_options_blocks_opening_inside(string value) =>
        Assert.False(SiteFramingService.AllowsFraming([value], []));

    [Theory]
    [InlineData("frame-ancestors 'self'", false)]
    [InlineData("default-src 'self'; frame-ancestors https://otro.edu.bo", false)]
    [InlineData("frame-ancestors 'none'", false)]
    [InlineData("FRAME-ANCESTORS *", true)]
    public void Frame_ancestors_rule_decides(string policy, bool expected) =>
        Assert.Equal(expected, SiteFramingService.AllowsFraming([], [policy]));

    [Fact]
    public void Frame_ancestors_takes_priority_over_x_frame_options() =>
        Assert.True(SiteFramingService.AllowsFraming(["SAMEORIGIN"], ["frame-ancestors *"]));
}
