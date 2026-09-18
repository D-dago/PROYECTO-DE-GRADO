namespace OrientacionVocacional.Application.Contracts;

public sealed record InstitutionResponse(Guid Id, string Name, string Type, string? Location, string? Municipality, string Status, string? Source, DateTime? LastUpdatedUtc, bool IsActive);
public sealed record CareerResponse(Guid Id, string Name, string? Area, string? Level, string? Modality, int? DurationMonths, decimal? Cost, string Status, string? Source, DateTime? LastUpdatedUtc, bool IsActive, Guid InstitutionId, string InstitutionName, int Compatibility, string CompatibilityReason);
public sealed record DimensionInfoResponse(string Code, string Name, string Description, IReadOnlyList<string> CareerAreas);
public sealed record ProfileInterpretationResponse(string Code, IReadOnlyList<DimensionInfoResponse> Dimensions, string Summary, IReadOnlyList<string> SuggestedAreas);
public sealed record RecommendationResponse(IReadOnlyList<CareerResponse> Items, Guid HistoryId, string? AiMessage, ProfileInterpretationResponse Interpretation, bool AiGenerated);
public sealed record HistoryResponse(Guid Id, Guid TestAttemptId, DateTime CreatedAtUtc, string? FilterSummary);
public sealed record AdminDashboardResponse(int Students, int Institutions, int Careers, int CompatibilityRules);
public sealed record CompatibilityResponse(Guid Id, Guid CareerId, string CareerName, string Dimension, int CompatibilityLevel, string Rule);
public sealed record StudentAdminResponse(Guid Id, string Email, string FirstName, string LastName, bool IsActive, DateTime CreatedAtUtc);

public sealed class InstitutionRequest
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public string? Location { get; init; }
    public string? Municipality { get; init; }
    public required string Status { get; init; }
    public string? Source { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed class CareerRequest
{
    public required string Name { get; init; }
    public string? Area { get; init; }
    public string? Level { get; init; }
    public string? Modality { get; init; }
    public int? DurationMonths { get; init; }
    public decimal? Cost { get; init; }
    public required string Status { get; init; }
    public string? Source { get; init; }
    public Guid InstitutionId { get; init; }
    public bool IsActive { get; init; } = true;
}

public sealed class CompatibilityRequest
{
    public Guid CareerId { get; init; }
    public required string Dimension { get; init; }
    public int CompatibilityLevel { get; init; }
    public required string Rule { get; init; }
}

public sealed class RecommendationFilterRequest
{
    public Guid? InstitutionId { get; init; }
    public string? Municipality { get; init; }
    public string? Modality { get; init; }
    public string? Level { get; init; }
    public string? Area { get; init; }
    public decimal? MaximumCost { get; init; }
    public int? MaximumDurationMonths { get; init; }
}