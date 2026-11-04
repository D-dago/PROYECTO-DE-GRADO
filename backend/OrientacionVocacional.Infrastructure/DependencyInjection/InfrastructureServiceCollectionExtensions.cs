using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ConnectionStrings:PostgreSQL no está configurada.");
        }

        services.AddDbContext<OrientacionVocacionalDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<AiOptions>(configuration.GetSection(AiOptions.SectionName));
        services.Configure<GoogleOptions>(configuration.GetSection(GoogleOptions.SectionName));
        // Sin este tope, un proveedor de IA lento dejaba al estudiante hasta 100 s esperando su resultado.
        services.AddHttpClient("ai", client => client.Timeout = TimeSpan.FromSeconds(15));
        services.AddHttpClient("google");
        services.AddHttpClient(SiteFramingService.HttpClientName, client => client.Timeout = TimeSpan.FromSeconds(6));
        services.AddMemoryCache();
        services.AddScoped<ISiteFramingService, SiteFramingService>();
        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IStudentProfileService, StudentProfileService>();
        services.AddScoped<ISkinService, SkinService>();
        services.AddScoped<IRiasecScoringService, RiasecScoringService>();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<IAiExplanationService, AiExplanationService>();
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IAdminService, AdminService>();

        return services;
    }
}