using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface ICatalogService
{
    Task<IReadOnlyList<InstitutionResponse>> GetInstitutionsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<CareerResponse>> GetCareersAsync(CancellationToken cancellationToken);
    Task<InstitutionResponse> CreateInstitutionAsync(InstitutionRequest request, CancellationToken cancellationToken);
    Task<CareerResponse?> CreateCareerAsync(CareerRequest request, CancellationToken cancellationToken);
    Task<bool> SetInstitutionActiveAsync(Guid id, bool active, CancellationToken cancellationToken);
    Task<bool> SetCareerActiveAsync(Guid id, bool active, CancellationToken cancellationToken);
    Task<bool> AddCompatibilityAsync(CompatibilityRequest request, CancellationToken cancellationToken);
    Task<RecommendationResponse?> GetRecommendationsAsync(Guid userId, RecommendationFilterRequest filter, CancellationToken cancellationToken);
    Task<IReadOnlyList<HistoryResponse>> GetHistoryAsync(Guid userId, CancellationToken cancellationToken);
    Task<AdminDashboardResponse> GetDashboardAsync(CancellationToken cancellationToken);
    Task<InstitutionResponse?> UpdateInstitutionAsync(Guid id, InstitutionRequest request, CancellationToken cancellationToken);
    Task<CareerResponse?> UpdateCareerAsync(Guid id, CareerRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<CompatibilityResponse>> GetCompatibilitiesAsync(CancellationToken cancellationToken);
    Task<bool> UpdateCompatibilityAsync(Guid id, CompatibilityRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteCompatibilityAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<StudentAdminResponse>> GetStudentsAsync(CancellationToken cancellationToken);
    Task<bool> SetStudentActiveAsync(Guid id, bool active, CancellationToken cancellationToken);
}