using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Enums;
using OrientacionVocacional.Domain.Seed;

namespace OrientacionVocacional.Infrastructure.Persistence;

public sealed class OrientacionVocacionalDbContext(DbContextOptions<OrientacionVocacionalDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<TestAttempt> TestAttempts => Set<TestAttempt>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<RiasecProfile> RiasecProfiles => Set<RiasecProfile>();
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<Career> Careers => Set<Career>();
    public DbSet<CareerCompatibility> CareerCompatibilities => Set<CareerCompatibility>();
    public DbSet<RecommendationHistory> RecommendationHistories => Set<RecommendationHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(role => role.Id);
            entity.Property(role => role.Name).HasMaxLength(40).IsRequired();
            entity.HasIndex(role => role.Name).IsUnique();
            entity.HasData(
                new Role { Id = RoleIds.Student, Name = RoleNames.Student },
                new Role { Id = RoleIds.Administrator, Name = RoleNames.Administrator });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Email).HasMaxLength(320).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(user => user.LastName).HasMaxLength(100).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
            entity.HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.ToTable("student_profiles");
            entity.HasKey(profile => profile.Id);
            entity.Property(profile => profile.Phone).HasMaxLength(30);
            entity.Property(profile => profile.City).HasMaxLength(100);
            entity.Property(profile => profile.School).HasMaxLength(200);
            entity.HasIndex(profile => profile.UserId).IsUnique();
            entity.HasOne(profile => profile.User)
                .WithOne(user => user.StudentProfile)
                .HasForeignKey<StudentProfile>(profile => profile.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");
            entity.HasKey(question => question.Id);
            entity.Property(question => question.Code).HasMaxLength(3).IsRequired();
            entity.Property(question => question.Text).HasMaxLength(300).IsRequired();
            entity.Property(question => question.Dimension).HasMaxLength(1).IsRequired();
            entity.HasIndex(question => question.Code).IsUnique();
            entity.HasIndex(question => new { question.IsActive, question.Order });
            entity.HasData(RiasecQuestionSeed.Create());
        });

        modelBuilder.Entity<TestAttempt>(entity =>
        {
            entity.ToTable("test_attempts");
            entity.HasKey(attempt => attempt.Id);
            entity.Property(attempt => attempt.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(attempt => attempt.TestVersion).HasMaxLength(30).IsRequired();
            entity.HasOne(attempt => attempt.User)
                .WithMany()
                .HasForeignKey(attempt => attempt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.ToTable("answers");
            entity.HasKey(answer => answer.Id);
            entity.HasIndex(answer => new { answer.TestAttemptId, answer.QuestionId }).IsUnique();
            entity.ToTable(table => table.HasCheckConstraint("CK_answers_value", "\"Value\" BETWEEN 1 AND 5"));
            entity.HasOne(answer => answer.TestAttempt)
                .WithMany(attempt => attempt.Answers)
                .HasForeignKey(answer => answer.TestAttemptId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(answer => answer.Question)
                .WithMany(question => question.Answers)
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RiasecProfile>(entity =>
        {
            entity.ToTable("riasec_profiles");
            entity.HasKey(profile => profile.Id);
            entity.Property(profile => profile.PrimaryCode).HasMaxLength(2).IsRequired();
            entity.Property(profile => profile.SecondaryCode).HasMaxLength(1).IsRequired();
            entity.Property(profile => profile.AlgorithmVersion).HasMaxLength(30).IsRequired();
            entity.HasIndex(profile => profile.TestAttemptId).IsUnique();
            entity.HasOne(profile => profile.TestAttempt)
                .WithOne(attempt => attempt.RiasecProfile)
                .HasForeignKey<RiasecProfile>(profile => profile.TestAttemptId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Institution>(entity =>
        {
            entity.ToTable("institutions");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Type).HasMaxLength(80).IsRequired();
            entity.Property(item => item.Location).HasMaxLength(200);
            entity.Property(item => item.Municipality).HasMaxLength(100);
            entity.Property(item => item.Status).HasMaxLength(40).IsRequired();
            entity.Property(item => item.Source).HasMaxLength(500);
        });

        modelBuilder.Entity<Career>(entity =>
        {
            entity.ToTable("careers");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Name).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Area).HasMaxLength(120);
            entity.Property(item => item.Level).HasMaxLength(80);
            entity.Property(item => item.Modality).HasMaxLength(80);
            entity.Property(item => item.Status).HasMaxLength(40).IsRequired();
            entity.Property(item => item.Source).HasMaxLength(500);
            entity.Property(item => item.Cost).HasPrecision(12, 2);
            entity.HasOne(item => item.Institution).WithMany(item => item.Careers)
                .HasForeignKey(item => item.InstitutionId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CareerCompatibility>(entity =>
        {
            entity.ToTable("career_compatibilities");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Dimension).HasMaxLength(1).IsRequired();
            entity.Property(item => item.Rule).HasMaxLength(500).IsRequired();
            entity.HasIndex(item => new { item.CareerId, item.Dimension }).IsUnique();
            entity.HasOne(item => item.Career).WithMany(item => item.Compatibilities)
                .HasForeignKey(item => item.CareerId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RecommendationHistory>(entity =>
        {
            entity.ToTable("recommendation_history");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.FilterSummary).HasMaxLength(1000);
            entity.HasOne(item => item.User).WithMany().HasForeignKey(item => item.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(item => item.TestAttempt).WithMany().HasForeignKey(item => item.TestAttemptId).OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static class RoleIds
    {
        public static readonly Guid Student = Guid.Parse("5b2c4f1e-37ae-4b7b-9d73-3a72e7c3af01");
        public static readonly Guid Administrator = Guid.Parse("d9d0c6c4-baf4-4f77-9d2c-2ca2afeb4f02");
    }
}