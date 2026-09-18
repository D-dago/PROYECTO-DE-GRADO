using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface IStudentProfileService
{
    Task<UserProfileResponse?> GetAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserProfileResponse?> UpdateAsync(Guid userId, UpdateStudentProfileRequest request, CancellationToken cancellationToken);
}