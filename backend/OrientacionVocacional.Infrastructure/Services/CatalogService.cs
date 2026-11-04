using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class CatalogService(OrientacionVocacionalDbContext db, IAiExplanationService ai) : ICatalogService
{
    // Listado publico: solo activas. Una institucion desactivada no aporta ninguna carrera a las
    // recomendaciones, asi que ofrecerla en el filtro del estudiante solo da "sin resultados".
    public async Task<IReadOnlyList<InstitutionResponse>> GetInstitutionsAsync(CancellationToken cancellationToken) =>
        await db.Institutions.AsNoTracking().Where(item => item.IsActive).OrderBy(item => item.Name).Select(item => new InstitutionResponse(item.Id, item.Name, item.Type, item.Location, item.Municipality, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive, item.Website)).ToListAsync(cancellationToken);

    // El administrador si necesita ver las inactivas: son las que puede volver a activar.
    public async Task<IReadOnlyList<InstitutionResponse>> GetAllInstitutionsForAdminAsync(CancellationToken cancellationToken) =>
        await db.Institutions.AsNoTracking().OrderBy(item => item.Name).Select(item => new InstitutionResponse(item.Id, item.Name, item.Type, item.Location, item.Municipality, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive, item.Website)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerResponse>> GetCareersAsync(CancellationToken cancellationToken)
    {
        var careers = await db.Careers.AsNoTracking().Include(item => item.Institution)
            .Where(item => item.IsActive && item.Institution.IsActive).OrderBy(item => item.Name).ToListAsync(cancellationToken);
        return careers.Select(item => ToCareer(item, 0, "Compatibilidad pendiente de regla.")).ToList();
    }

    public async Task<IReadOnlyList<CareerResponse>> GetAllCareersForAdminAsync(CancellationToken cancellationToken)
    {
        var careers = await db.Careers.AsNoTracking().Include(item => item.Institution).OrderBy(item => item.Name).ToListAsync(cancellationToken);
        return careers.Select(item => ToCareer(item, 0, "Compatibilidad pendiente de regla.")).ToList();
    }

    public async Task<InstitutionResponse> CreateInstitutionAsync(InstitutionRequest request, CancellationToken cancellationToken)
    {
        var item = new Institution { Id = Guid.NewGuid(), Name = request.Name.Trim(), Type = request.Type.Trim(), Location = request.Location?.Trim(), Municipality = request.Municipality?.Trim(), Status = request.Status.Trim(), Source = request.Source?.Trim(), Website = LimpiarWeb(request.Website), IsActive = request.IsActive, LastUpdatedUtc = DateTime.UtcNow };
        db.Institutions.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return ToInstitution(item);
    }

    public async Task<CareerResponse?> CreateCareerAsync(CareerRequest request, CancellationToken cancellationToken)
    {
        var institution = await db.Institutions.SingleOrDefaultAsync(item => item.Id == request.InstitutionId && item.IsActive, cancellationToken);
        if (institution is null) return null;
        var item = new Career { Id = Guid.NewGuid(), Name = request.Name.Trim(), Area = request.Area?.Trim(), Level = request.Level?.Trim(), Modality = request.Modality?.Trim(), DurationMonths = request.DurationMonths, Cost = request.Cost, Status = request.Status.Trim(), Source = request.Source?.Trim(), IsActive = request.IsActive, LastUpdatedUtc = DateTime.UtcNow, InstitutionId = institution.Id, Institution = institution };
        db.Careers.Add(item);
        await db.SaveChangesAsync(cancellationToken);
        return ToCareer(item, 0, "Compatibilidad pendiente de regla.");
    }

    public async Task<bool> SetInstitutionActiveAsync(Guid id, bool active, CancellationToken cancellationToken) => await SetActive(db.Institutions, id, active, cancellationToken);
    public async Task<bool> SetCareerActiveAsync(Guid id, bool active, CancellationToken cancellationToken) => await SetActive(db.Careers, id, active, cancellationToken);

    public async Task<bool> AddCompatibilityAsync(CompatibilityRequest request, CancellationToken cancellationToken)
    {
        if (request.CompatibilityLevel is < 0 or > 100 || request.Dimension.Length != 1) return false;
        if (!await db.Careers.AnyAsync(item => item.Id == request.CareerId, cancellationToken)) return false;
        // La base solo admite una regla por carrera y dimension: sin esta comprobacion, un duplicado llegaba como error 500.
        var dimension = request.Dimension.ToUpperInvariant();
        if (await db.CareerCompatibilities.AnyAsync(item => item.CareerId == request.CareerId && item.Dimension == dimension, cancellationToken)) return false;
        db.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = request.CareerId, Dimension = request.Dimension.ToUpperInvariant(), CompatibilityLevel = request.CompatibilityLevel, Rule = request.Rule.Trim() });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<RecommendationResponse?> GetRecommendationsAsync(Guid userId, RecommendationFilterRequest filter, CancellationToken cancellationToken)
    {
        var profile = await GetCurrentProfileAsync(userId, cancellationToken);
        if (profile is null) return null;
        var recommendations = await MatchCareersAsync(profile, filter, cancellationToken);
        var history = new RecommendationHistory { Id = Guid.NewGuid(), UserId = userId, TestAttemptId = profile.TestAttemptId, FilterSummary = JsonSerializer.Serialize(filter) };
        db.RecommendationHistories.Add(history);
        await db.SaveChangesAsync(cancellationToken);
        var result = new RiasecResultResponse(profile.Id, profile.TestAttemptId, profile.RScore, profile.IScore, profile.AScore, profile.SScore, profile.EScore, profile.CScore, profile.PrimaryCode, profile.SecondaryCode, profile.CreatedAtUtc, profile.AlgorithmVersion);
        var aiMessage = await ai.ExplainAsync(result, recommendations, cancellationToken);
        return new RecommendationResponse(
            recommendations, history.Id, aiMessage,
            RiasecInterpretationBuilder.Build(profile.PrimaryCode, profile.SecondaryCode),
            aiMessage != AiExplanationService.UnavailableMessage);
    }

    // Lo que usa una pantalla abierta para ponerse al dia. Sin historial ni IA: se repite cada vez que el
    // administrador cambia algo, y cada llamada a la IA tarda y cuesta dinero.
    public async Task<IReadOnlyList<CareerResponse>?> GetCareerMatchesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await GetCurrentProfileAsync(userId, cancellationToken);
        return profile is null ? null : await MatchCareersAsync(profile, new RecommendationFilterRequest(), cancellationToken);
    }

    // Solo cuenta un perfil del test vigente (RIASEC-12-V2): los perfiles de otras versiones
    // (el CHASIDE que se aplico un tiempo) usan letras con otro significado y darian
    // recomendaciones sin sentido; hay que repetir el test.
    private Task<RiasecProfile?> GetCurrentProfileAsync(Guid userId, CancellationToken cancellationToken) =>
        db.RiasecProfiles.AsNoTracking().Where(item => item.TestAttempt.UserId == userId && item.AlgorithmVersion == RiasecTestConstants.Version).OrderByDescending(item => item.CreatedAtUtc).FirstOrDefaultAsync(cancellationToken);

    private async Task<List<CareerResponse>> MatchCareersAsync(RiasecProfile profile, RecommendationFilterRequest filter, CancellationToken cancellationToken)
    {
        var scores = new Dictionary<string, int> { ["R"] = profile.RScore, ["I"] = profile.IScore, ["A"] = profile.AScore, ["S"] = profile.SScore, ["E"] = profile.EScore, ["C"] = profile.CScore };
        // Solo lo traducible de forma segura a SQL (igualdad de Id, comparacion numerica) se
        // filtra en la base. El resto (texto libre) se compara despues, en memoria, para poder
        // ignorar mayusculas Y tildes sin depender de una funcion especifica de Postgres que
        // ademas rompe las pruebas con el proveedor InMemory.
        var query = db.Careers.AsNoTracking().Include(item => item.Institution).Include(item => item.Compatibilities).Where(item => item.IsActive && item.Institution.IsActive);
        if (filter.InstitutionId.HasValue) query = query.Where(item => item.InstitutionId == filter.InstitutionId);
        if (filter.MaximumCost.HasValue) query = query.Where(item => item.Cost == null || item.Cost <= filter.MaximumCost);
        if (filter.MaximumDurationMonths.HasValue) query = query.Where(item => item.DurationMonths == null || item.DurationMonths <= filter.MaximumDurationMonths);
        var careers = await query.ToListAsync(cancellationToken);

        // Un estudiante que escribe "sacaba" o "informatico" (sin mayuscula inicial, sin tilde)
        // no tiene forma de saber si el admin cargo "Sacaba" o "Informático"; exigir el texto
        // exacto daba "sin resultados" aunque si hubiera datos que coincidian.
        if (!string.IsNullOrWhiteSpace(filter.Municipality)) careers = careers.Where(item => Coincide(item.Institution.Municipality, filter.Municipality)).ToList();
        if (!string.IsNullOrWhiteSpace(filter.Modality)) careers = careers.Where(item => Coincide(item.Modality, filter.Modality)).ToList();
        if (!string.IsNullOrWhiteSpace(filter.Level)) careers = careers.Where(item => Coincide(item.Level, filter.Level)).ToList();
        // "Area" tambien revisa el nombre de la carrera: el campo es una categoria amplia
        // ("Tecnologia"), pero es normal que alguien escriba el nombre puntual de la carrera
        // que busca ("sistemas informatico") en vez de la categoria.
        if (!string.IsNullOrWhiteSpace(filter.Area)) careers = careers.Where(item => Coincide(item.Area, filter.Area) || Coincide(item.Name, filter.Area)).ToList();

        return careers.Select(item => { var match = item.Compatibilities.OrderByDescending(rule => scores.GetValueOrDefault(rule.Dimension) * rule.CompatibilityLevel).FirstOrDefault(); var level = match is null ? 0 : Math.Clamp(scores.GetValueOrDefault(match.Dimension) * match.CompatibilityLevel / RiasecTestConstants.MaxDimensionScore, 0, 100); return ToCareer(item, level, match?.Rule ?? "No existe una regla de compatibilidad cargada."); }).OrderByDescending(item => item.Compatibility).ToList();
    }

    // Una entrada por test terminado, no por busqueda. RecommendationHistories guarda una fila
    // cada vez que se piden recomendaciones (y la pantalla de resultado las pide sola al abrirse),
    // asi que listar esa tabla mostraba el mismo perfil repetido muchas veces. La fuente es el
    // perfil del intento; de las busquedas solo se rescatan los filtros de la ultima.
    public async Task<IReadOnlyList<HistoryResponse>> GetHistoryAsync(Guid userId, CancellationToken cancellationToken)
    {
        var rows = await db.RiasecProfiles.AsNoTracking()
            .Where(profile => profile.TestAttempt.UserId == userId)
            .OrderByDescending(profile => profile.CreatedAtUtc)
            .Select(profile => new
            {
                profile.Id, profile.TestAttemptId, profile.CreatedAtUtc, profile.AlgorithmVersion,
                profile.PrimaryCode, profile.SecondaryCode,
                profile.RScore, profile.IScore, profile.AScore, profile.SScore, profile.EScore, profile.CScore,
                LastFilter = db.RecommendationHistories
                    .Where(history => history.TestAttemptId == profile.TestAttemptId)
                    .OrderByDescending(history => history.CreatedAtUtc)
                    .Select(history => history.FilterSummary)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        // Los perfiles de otras versiones del test (el CHASIDE que se aplico un tiempo) comparten
        // letras con RIASEC pero significan otra cosa: se listan para no perder el registro, sin
        // puntajes ni interpretacion que enganen. Ademas RiasecInterpretationBuilder.Build no es
        // traducible a SQL, se calcula aqui.
        return rows.Select(row => row.AlgorithmVersion == RiasecTestConstants.Version
            ? new HistoryResponse(
                row.Id, row.TestAttemptId, row.CreatedAtUtc, row.LastFilter,
                row.PrimaryCode, row.SecondaryCode,
                row.RScore, row.IScore, row.AScore, row.SScore, row.EScore, row.CScore,
                RiasecInterpretationBuilder.Build(row.PrimaryCode, row.SecondaryCode))
            : new HistoryResponse(row.Id, row.TestAttemptId, row.CreatedAtUtc, row.LastFilter, null, null, null, null, null, null, null, null, null))
            .ToList();
    }

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken) => new(
        await db.Users.CountAsync(item => item.Role.Name == RoleNames.Student, cancellationToken),
        await db.Institutions.CountAsync(cancellationToken),
        await db.Careers.CountAsync(cancellationToken),
        await db.CareerCompatibilities.CountAsync(cancellationToken));

    public async Task<InstitutionResponse?> UpdateInstitutionAsync(Guid id, InstitutionRequest request, CancellationToken cancellationToken)
    {
        var item = await db.Institutions.FindAsync([id], cancellationToken);
        if (item is null) return null;
        item.Name = request.Name.Trim(); item.Type = request.Type.Trim(); item.Location = request.Location?.Trim(); item.Municipality = request.Municipality?.Trim(); item.Status = request.Status.Trim(); item.Source = request.Source?.Trim(); item.Website = LimpiarWeb(request.Website); item.IsActive = request.IsActive; item.LastUpdatedUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken); return ToInstitution(item);
    }

    public async Task<CareerResponse?> UpdateCareerAsync(Guid id, CareerRequest request, CancellationToken cancellationToken)
    {
        var item = await db.Careers.Include(x => x.Institution).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (item is null || !await db.Institutions.AnyAsync(x => x.Id == request.InstitutionId, cancellationToken)) return null;
        item.Name = request.Name.Trim(); item.Area = request.Area?.Trim(); item.Level = request.Level?.Trim(); item.Modality = request.Modality?.Trim(); item.DurationMonths = request.DurationMonths; item.Cost = request.Cost; item.Status = request.Status.Trim(); item.Source = request.Source?.Trim(); item.IsActive = request.IsActive; item.InstitutionId = request.InstitutionId; item.LastUpdatedUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken); await db.Entry(item).Reference(x => x.Institution).LoadAsync(cancellationToken); return ToCareer(item, 0, "Compatibilidad pendiente de regla.");
    }

    public async Task<IReadOnlyList<CompatibilityResponse>> GetCompatibilitiesAsync(CancellationToken cancellationToken) => await db.CareerCompatibilities.AsNoTracking().Include(x => x.Career).OrderBy(x => x.Dimension).Select(x => new CompatibilityResponse(x.Id, x.CareerId, x.Career.Name, x.Dimension, x.CompatibilityLevel, x.Rule)).ToListAsync(cancellationToken);
    public async Task<bool> UpdateCompatibilityAsync(Guid id, CompatibilityRequest request, CancellationToken cancellationToken)
    {
        var item = await db.CareerCompatibilities.FindAsync([id], cancellationToken); if (item is null) return false; var dimension = request.Dimension.ToUpperInvariant(); if (await db.CareerCompatibilities.AnyAsync(other => other.Id != id && other.CareerId == item.CareerId && other.Dimension == dimension, cancellationToken)) return false; item.Dimension = dimension; item.CompatibilityLevel = request.CompatibilityLevel; item.Rule = request.Rule.Trim(); await db.SaveChangesAsync(cancellationToken); return true;
    }
    public async Task<bool> DeleteCompatibilityAsync(Guid id, CancellationToken cancellationToken) { var item = await db.CareerCompatibilities.FindAsync([id], cancellationToken); if (item is null) return false; db.CareerCompatibilities.Remove(item); await db.SaveChangesAsync(cancellationToken); return true; }
    public async Task<IReadOnlyList<StudentAdminResponse>> GetStudentsAsync(CancellationToken cancellationToken) => await db.Users.AsNoTracking().Where(x => x.Role.Name == RoleNames.Student).OrderBy(x => x.LastName).Select(x => new StudentAdminResponse(x.Id, x.Email, x.FirstName, x.LastName, x.IsActive, x.CreatedAtUtc)).ToListAsync(cancellationToken);
    public async Task<bool> SetStudentActiveAsync(Guid id, bool active, CancellationToken cancellationToken) { var item = await db.Users.SingleOrDefaultAsync(x => x.Id == id && x.Role.Name == RoleNames.Student, cancellationToken); if (item is null) return false; item.IsActive = active; await db.SaveChangesAsync(cancellationToken); return true; }

    private async Task<bool> SetActive<TEntity>(DbSet<TEntity> set, Guid id, bool active, CancellationToken cancellationToken) where TEntity : class
    {
        var item = await set.FindAsync([id], cancellationToken);
        if (item is null) return false;
        var property = typeof(TEntity).GetProperty(nameof(Institution.IsActive));
        property?.SetValue(item, active);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static InstitutionResponse ToInstitution(Institution item) => new(item.Id, item.Name, item.Type, item.Location, item.Municipality, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive, item.Website);
    private static CareerResponse ToCareer(Career item, int compatibility, string reason) => new(item.Id, item.Name, item.Area, item.Level, item.Modality, item.DurationMonths, item.Cost, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive, item.InstitutionId, item.Institution?.Name ?? "Información pendiente de actualización", compatibility, reason, item.Institution?.Website);

    private static string? LimpiarWeb(string? web) => string.IsNullOrWhiteSpace(web) ? null : web.Trim();

    // Coincidencia de texto libre "de verdad": ignora mayusculas/minusculas y tildes (muy comun
    // escribir "informatico" sin la tilde de "informático"), y es un "contiene", no exacta.
    private static bool Coincide(string? valor, string busqueda) => valor is not null && Normalizar(valor).Contains(Normalizar(busqueda));

    private static string Normalizar(string texto)
    {
        var descompuesto = texto.Normalize(System.Text.NormalizationForm.FormD);
        var sinTildes = new System.Text.StringBuilder(descompuesto.Length);
        foreach (var caracter in descompuesto)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(caracter) != System.Globalization.UnicodeCategory.NonSpacingMark)
                sinTildes.Append(caracter);
        }
        return sinTildes.ToString().ToLowerInvariant();
    }
}