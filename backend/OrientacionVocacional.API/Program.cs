using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrientacionVocacional.Application.Configuration;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Infrastructure.DependencyInjection;
using OrientacionVocacional.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Documentacion interactiva de la API (solo en desarrollo: no tiene sentido exponer esto
// si algun dia se publica en un hosting real de verdad).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Orientación Vocacional API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pega el token asi: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

// Endpoint de salud: confirma que la API responde y que puede llegar a la base de datos.
// Util para monitoreo y para verificar rapido si "el sistema esta encendido".
builder.Services.AddHealthChecks().AddDbContextCheck<OrientacionVocacionalDbContext>();

// Ninguna peticion legitima de esta app necesita mas de 100 KB: corta cuerpos gigantes.
builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = 100 * 1024);

// Anti-abuso: limita cuantas peticiones puede hacer una misma IP. Registrar cuentas en
// masa (llenar la base con correos) o adivinar contrasenas queda muy lento. Los topes
// son holgados a proposito: un salon entero comparte la misma IP al registrarse.
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync("{\"message\":\"Demasiados intentos. Espera un momento e inténtalo de nuevo.\"}", token);
    };
    static string Ip(HttpContext context) => context.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
    options.AddPolicy("register", context => RateLimitPartition.GetFixedWindowLimiter(Ip(context), _ => new FixedWindowRateLimiterOptions { PermitLimit = 40, Window = TimeSpan.FromHours(1) }));
    options.AddPolicy("login", context => RateLimitPartition.GetFixedWindowLimiter(Ip(context), _ => new FixedWindowRateLimiterOptions { PermitLimit = 30, Window = TimeSpan.FromMinutes(1) }));
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(Ip(context), _ => new FixedWindowRateLimiterOptions { PermitLimit = 300, Window = TimeSpan.FromMinutes(1) }));
});

var jwtSection = builder.Configuration.GetSection(JwtOptions.SectionName);
var jwtKey = jwtSection["Key"];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("Jwt:Key debe configurarse con al menos 32 bytes.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorization();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        // Ademas de los origenes configurados, se aceptan paginas abiertas desde la red local
        // (p. ej. el celular en http://192.168.0.151:5173). La API usa token en el header, no
        // cookies, y desde internet estas direcciones privadas no son alcanzables.
        policy.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin) || IsLocalNetworkOrigin(origin))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

// Detras de un tunel o proxy (Cloudflare, Nginx...) la conexion llega desde la propia PC. Estas
// cabeceras traen la IP real del visitante: sin ellas todos compartirian el mismo limite anti-abuso.
// Por defecto solo se confia en proxies que se conectan desde localhost.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Red de seguridad: una excepcion no manejada (un bug real, una falla de la base de datos...)
// nunca debe devolver un stack trace ni una pagina HTML al cliente. Se registra el detalle en
// el log del servidor y se responde siempre el mismo JSON generico.
app.UseExceptionHandler(exceptionApp => exceptionApp.Run(async context =>
{
    var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
    if (error is not null)
    {
        context.RequestServices.GetRequiredService<ILogger<Program>>().LogError(error, "Error no controlado en {Path}", context.Request.Path);
    }
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsync("{\"message\":\"Ocurrió un error inesperado. Intenta de nuevo en un momento.\"}");
}));

app.MapHealthChecks("/health");

app.UseForwardedHeaders();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    await next();
});

// La API tambien sirve la web ya compilada (carpeta wwwroot): un solo puerto y un solo enlace.
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        // Los archivos con hash (assets/) pueden cachearse; el index.html no, para ver siempre la ultima version.
        context.Context.Response.Headers.CacheControl = context.File.Name == "index.html" ? "no-cache" : "public,max-age=86400";
    }
});

// UseRouting va DESPUES de los archivos estaticos: si el enrutador corre antes, elige la ruta comodin
// de abajo (index.html) para cualquier archivo y los .js/.css/.jpg nunca llegan a servirse.
app.UseRouting();
app.UseCors("Frontend");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Cualquier ruta que no sea de la API (/registro, /test...) devuelve la web; las rutas /api/ inexistentes siguen dando 404.
app.MapFallbackToFile("{*path:regex(^(?!api/).*$)}", "index.html");

app.Run();

// localhost, 127.x y los rangos privados 10.x, 172.16-31.x y 192.168.x.
static bool IsLocalNetworkOrigin(string origin)
{
    if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri)) return false;
    if (uri.Host == "localhost") return true;
    if (!System.Net.IPAddress.TryParse(uri.Host, out var ip) || ip.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) return false;
    var bytes = ip.GetAddressBytes();
    return bytes[0] is 10 or 127 || (bytes[0] == 172 && bytes[1] is >= 16 and <= 31) || (bytes[0] == 192 && bytes[1] == 168);
}
