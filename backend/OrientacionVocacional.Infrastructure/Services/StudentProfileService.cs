using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class StudentProfileService(OrientacionVocacionalDbContext dbContext) : IStudentProfileService
{
    public async Task<UserProfileResponse?> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(candidate => candidate.Role)
            .Include(candidate => candidate.StudentProfile)
            .SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);

        return user is null ? null : Map(user);
    }

    public async Task<UserProfileResponse?> UpdateAsync(
        Guid userId,
        UpdateStudentProfileRequest request,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .Include(candidate => candidate.Role)
            .Include(candidate => candidate.StudentProfile)
            .SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);

        if (user?.StudentProfile is null)
        {
            return null;
        }

        user.StudentProfile.Phone = request.Phone?.Trim();
        user.StudentProfile.City = request.City?.Trim();
        user.StudentProfile.School = request.School?.Trim();
        user.StudentProfile.GraduationYear = request.GraduationYear;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Map(user);
    }

    private static UserProfileResponse Map(Domain.Entities.User user) => new(
        user.Id,
        user.Email,
        user.FirstName,
        user.LastName,
        user.Role.Name,
        user.StudentProfile?.Phone,
        user.StudentProfile?.City,
        user.StudentProfile?.School,
        user.StudentProfile?.GraduationYear);
}