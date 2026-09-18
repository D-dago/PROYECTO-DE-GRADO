using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class CatalogService(OrientacionVocacionalDbContext db, IAiExplanationService ai) : ICatalogService
{
    public async Task<IReadOnlyList<InstitutionResponse>> GetInstitutionsAsync(CancellationToken cancellationToken) =>
        await db.Institutions.AsNoTracking().OrderBy(item => item.Name).Select(item => new InstitutionResponse(item.Id, item.Name, item.Type, item.Location, item.Municipality, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CareerResponse>> GetCareersAsync(CancellationToken cancellationToken)
    {
        var careers = await db.Careers.AsNoTracking().Include(item => item.Institution).Include(item => item.Compatibilities)
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
        var item = new Institution { Id = Guid.NewGuid(), Name = request.Name.Trim(), Type = request.Type.Trim(), Location = request.Location?.Trim(), Municipality = request.Municipality?.Trim(), Status = request.Status.Trim(), Source = request.Source?.Trim(), IsActive = request.IsActive, LastUpdatedUtc = DateTime.UtcNow };
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
        db.CareerCompatibilities.Add(new CareerCompatibility { Id = Guid.NewGuid(), CareerId = request.CareerId, Dimension = request.Dimension.ToUpperInvariant(), CompatibilityLevel = request.CompatibilityLevel, Rule = request.Rule.Trim() });
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<RecommendationResponse?> GetRecommendationsAsync(Guid userId, RecommendationFilterRequest filter, CancellationToken cancellationToken)
    {
        var profile = await db.RiasecProfiles.Include(item => item.TestAttempt).Where(item => item.TestAttempt.UserId == userId).OrderByDescending(item => item.CreatedAtUtc).FirstOrDefaultAsync(cancellationToken);
        if (profile is null) return null;
        var scores = new Dictionary<string, int> { ["R"] = profile.RScore, ["I"] = profile.IScore, ["A"] = profile.AScore, ["S"] = profile.SScore, ["E"] = profile.EScore, ["C"] = profile.CScore };
        var query = db.Careers.AsNoTracking().Include(item => item.Institution).Include(item => item.Compatibilities).Where(item => item.IsActive && item.Institution.IsActive);
        if (filter.InstitutionId.HasValue) query = query.Where(item => item.InstitutionId == filter.InstitutionId);
        if (!string.IsNullOrWhiteSpace(filter.Municipality)) query = query.Where(item => item.Institution.Municipality == filter.Municipality);
        if (!string.IsNullOrWhiteSpace(filter.Modality)) query = query.Where(item => item.Modality == filter.Modality);
        if (!string.IsNullOrWhiteSpace(filter.Level)) query = query.Where(item => item.Level == filter.Level);
        if (!string.IsNullOrWhiteSpace(filter.Area)) query = query.Where(item => item.Area == filter.Area);
        if (filter.MaximumCost.HasValue) query = query.Where(item => item.Cost == null || item.Cost <= filter.MaximumCost);
        if (filter.MaximumDurationMonths.HasValue) query = query.Where(item => item.DurationMonths == null || item.DurationMonths <= filter.MaximumDurationMonths);
        var careers = await query.ToListAsync(cancellationToken);
        var recommendations = careers.Select(item => { var match = item.Compatibilities.OrderByDescending(rule => scores.GetValueOrDefault(rule.Dimension) * rule.CompatibilityLevel).FirstOrDefault(); var level = match is null ? 0 : Math.Clamp(scores.GetValueOrDefault(match.Dimension) * match.CompatibilityLevel / OrientacionVocacional.Domain.Constants.RiasecTestConstants.MaxDimensionScore, 0, 100); return ToCareer(item, level, match?.Rule ?? "No existe una regla de compatibilidad cargada."); }).OrderByDescending(item => item.Compatibility).ToList();
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

    public async Task<IReadOnlyList<HistoryResponse>> GetHistoryAsync(Guid userId, CancellationToken cancellationToken) => await db.RecommendationHistories.AsNoTracking().Where(item => item.UserId == userId).OrderByDescending(item => item.CreatedAtUtc).Select(item => new HistoryResponse(item.Id, item.TestAttemptId, item.CreatedAtUtc, item.FilterSummary)).ToListAsync(cancellationToken);

    public async Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken) => new(
        await db.Users.CountAsync(item => item.Role.Name == "ESTUDIANTE", cancellationToken),
        await db.Institutions.CountAsync(cancellationToken),
        await db.Careers.CountAsync(cancellationToken),
        await db.CareerCompatibilities.CountAsync(cancellationToken));

    public async Task<InstitutionResponse?> UpdateInstitutionAsync(Guid id, InstitutionRequest request, CancellationToken cancellationToken)
    {
        var item = await db.Institutions.FindAsync([id], cancellationToken);
        if (item is null) return null;
        item.Name = request.Name.Trim(); item.Type = request.Type.Trim(); item.Location = request.Location?.Trim(); item.Municipality = request.Municipality?.Trim(); item.Status = request.Status.Trim(); item.Source = request.Source?.Trim(); item.IsActive = request.IsActive; item.LastUpdatedUtc = DateTime.UtcNow;
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
        var item = await db.CareerCompatibilities.FindAsync([id], cancellationToken); if (item is null) return false; item.Dimension = request.Dimension.ToUpperInvariant(); item.CompatibilityLevel = request.CompatibilityLevel; item.Rule = request.Rule.Trim(); await db.SaveChangesAsync(cancellationToken); return true;
    }
    public async Task<bool> DeleteCompatibilityAsync(Guid id, CancellationToken cancellationToken) { var item = await db.CareerCompatibilities.FindAsync([id], cancellationToken); if (item is null) return false; db.CareerCompatibilities.Remove(item); await db.SaveChangesAsync(cancellationToken); return true; }
    public async Task<IReadOnlyList<StudentAdminResponse>> GetStudentsAsync(CancellationToken cancellationToken) => await db.Users.AsNoTracking().Where(x => x.Role.Name == "ESTUDIANTE").OrderBy(x => x.LastName).Select(x => new StudentAdminResponse(x.Id, x.Email, x.FirstName, x.LastName, x.IsActive, x.CreatedAtUtc)).ToListAsync(cancellationToken);
    public async Task<bool> SetStudentActiveAsync(Guid id, bool active, CancellationToken cancellationToken) { var item = await db.Users.SingleOrDefaultAsync(x => x.Id == id && x.Role.Name == "ESTUDIANTE", cancellationToken); if (item is null) return false; item.IsActive = active; await db.SaveChangesAsync(cancellationToken); return true; }

    private async Task<bool> SetActive<TEntity>(DbSet<TEntity> set, Guid id, bool active, CancellationToken cancellationToken) where TEntity : class
    {
        var item = await set.FindAsync([id], cancellationToken);
        if (item is null) return false;
        var property = typeof(TEntity).GetProperty(nameof(Institution.IsActive));
        property?.SetValue(item, active);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static InstitutionResponse ToInstitution(Institution item) => new(item.Id, item.Name, item.Type, item.Location, item.Municipality, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive);
    private static CareerResponse ToCareer(Career item, int compatibility, string reason) => new(item.Id, item.Name, item.Area, item.Level, item.Modality, item.DurationMonths, item.Cost, item.Status, item.Source, item.LastUpdatedUtc, item.IsActive, item.InstitutionId, item.Institution?.Name ?? "Información pendiente de actualización", compatibility, reason);
}