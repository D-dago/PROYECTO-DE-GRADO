using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Enums;
using OrientacionVocacional.Infrastructure.Services;

namespace OrientacionVocacional.Tests;

public class AdminServiceTests
{
    [Fact]
    public async Task GetQuestionsAsync_returns_only_current_questions_ordered()
    {
        await using var context = TestSupport.CreateContext();
        var totalInBank = await context.Questions.CountAsync(question => question.Order < 1000);
        var service = new AdminService(context);

        var questions = await service.GetQuestionsAsync(CancellationToken.None);

        Assert.Equal(totalInBank, questions.Count); // banco actual, sin las legacy (Order >= 1000)
        Assert.All(questions, question => Assert.True(question.Order < 1000));
        Assert.Equal(questions.Select(question => question.Order).OrderBy(order => order), questions.Select(question => question.Order));
    }

    [Fact]
    public async Task CreateQuestionAsync_assigns_next_code_and_order()
    {
        await using var context = TestSupport.CreateContext();
        var service = new AdminService(context);
        var existingInC = await context.Questions.CountAsync(question => question.Dimension == "C");
        var maxOrder = await context.Questions.Where(question => question.Order < 1000).MaxAsync(question => question.Order);

        var created = await service.CreateQuestionAsync(new AdminQuestionRequest { Text = "¿Pregunta nueva de prueba?", Dimension = "C", IsActive = true }, CancellationToken.None);

        Assert.Equal("C", created.Dimension);
        Assert.Equal($"C{existingInC + 1:00}", created.Code);
        Assert.Equal(maxOrder + 1, created.Order);
        Assert.True(created.IsActive);
    }

    [Fact]
    public async Task UpdateQuestionAsync_rejects_leaving_a_dimension_with_less_than_minimum_active()
    {
        await using var context = TestSupport.CreateContext();
        var service = new AdminService(context);
        // Desactiva todas las preguntas de "E" menos 2, para dejar la dimension justo en el minimo.
        var dQuestions = await context.Questions.Where(question => question.Dimension == "E" && question.IsActive).OrderBy(question => question.Order).ToListAsync();
        foreach (var question in dQuestions.Skip(RiasecTestConstants.QuestionsPerDimensionPerAttempt)) question.IsActive = false;
        await context.SaveChangesAsync();
        var target = dQuestions[0];

        var (success, error, updated) = await service.UpdateQuestionAsync(target.Id, new AdminQuestionRequest { Text = target.Text, Dimension = "E", IsActive = false }, CancellationToken.None);

        Assert.False(success);
        Assert.Null(updated);
        Assert.Contains("E", error);
        Assert.True((await context.Questions.SingleAsync(question => question.Id == target.Id)).IsActive); // no se guardo el cambio
    }

    [Fact]
    public async Task UpdateQuestionAsync_allows_editing_text_when_dimension_keeps_enough_active()
    {
        await using var context = TestSupport.CreateContext();
        var service = new AdminService(context);
        var target = await context.Questions.FirstAsync(question => question.Dimension == "E" && question.IsActive);

        var (success, error, updated) = await service.UpdateQuestionAsync(target.Id, new AdminQuestionRequest { Text = "¿Texto editado?", Dimension = "E", IsActive = true }, CancellationToken.None);

        Assert.True(success);
        Assert.Null(error);
        Assert.Equal("¿Texto editado?", updated!.Text);
    }

    [Fact]
    public async Task UpdateQuestionAsync_returns_error_for_missing_question()
    {
        await using var context = TestSupport.CreateContext();
        var service = new AdminService(context);

        var (success, error, updated) = await service.UpdateQuestionAsync(Guid.NewGuid(), new AdminQuestionRequest { Text = "x", Dimension = "C", IsActive = true }, CancellationToken.None);

        Assert.False(success);
        Assert.NotNull(error);
        Assert.Null(updated);
    }

    [Fact]
    public async Task GetUsersAsync_reports_completed_test_count()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        context.TestAttempts.Add(new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version, Status = TestAttemptStatus.Completed, CompletedAtUtc = DateTime.UtcNow });
        context.TestAttempts.Add(new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version, Status = TestAttemptStatus.InProgress });
        await context.SaveChangesAsync();
        var service = new AdminService(context);

        var users = await service.GetUsersAsync(CancellationToken.None);

        var row = users.Single(user => user.Id == userId);
        Assert.Equal(1, row.CompletedTests); // solo cuenta el completado, no el que sigue en progreso
    }

    [Fact]
    public async Task GetResultsAsync_only_includes_profiles_from_the_current_algorithm_version()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var current = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version, Status = TestAttemptStatus.Completed, CompletedAtUtc = DateTime.UtcNow };
        context.TestAttempts.Add(current);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = current.Id, TestAttempt = current, RScore = 10, IScore = 6, AScore = 4, SScore = 4, EScore = 4, CScore = 4, PrimaryCode = "RI", SecondaryCode = "A", AlgorithmVersion = RiasecTestConstants.Version });
        var old = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = "CHASIDE-14-V1", Status = TestAttemptStatus.Completed, CompletedAtUtc = DateTime.UtcNow };
        context.TestAttempts.Add(old);
        context.RiasecProfiles.Add(new RiasecProfile { Id = Guid.NewGuid(), TestAttemptId = old.Id, TestAttempt = old, RScore = 1, IScore = 1, AScore = 1, SScore = 1, EScore = 1, CScore = 1, PrimaryCode = "CH", SecondaryCode = "A", AlgorithmVersion = "CHASIDE-14-V1" });
        await context.SaveChangesAsync();
        var service = new AdminService(context);

        var results = await service.GetResultsAsync(CancellationToken.None);

        var row = Assert.Single(results);
        Assert.Equal(current.Id, row.AttemptId);
        Assert.Equal(10, row.R);
    }

    [Fact]
    public async Task GetAnswersAsync_only_includes_completed_attempts_of_the_current_version()
    {
        await using var context = TestSupport.CreateContext();
        var userId = await TestSupport.CreatePersistedStudentAsync(context);
        var question = await context.Questions.FirstAsync(candidate => candidate.IsActive);
        var completed = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version, Status = TestAttemptStatus.Completed, CompletedAtUtc = DateTime.UtcNow };
        var inProgress = new TestAttempt { Id = Guid.NewGuid(), UserId = userId, TestVersion = RiasecTestConstants.Version, Status = TestAttemptStatus.InProgress };
        context.TestAttempts.AddRange(completed, inProgress);
        context.Answers.Add(new Answer { Id = Guid.NewGuid(), TestAttemptId = completed.Id, TestAttempt = completed, QuestionId = question.Id, Question = question, Value = 4 });
        context.Answers.Add(new Answer { Id = Guid.NewGuid(), TestAttemptId = inProgress.Id, TestAttempt = inProgress, QuestionId = question.Id, Question = question, Value = 5 });
        await context.SaveChangesAsync();
        var service = new AdminService(context);

        var answers = await service.GetAnswersAsync(CancellationToken.None);

        var row = Assert.Single(answers);
        Assert.Equal(completed.Id, row.AttemptId);
        Assert.Equal(4, row.Value);
    }

    [Fact]
    public void BuildExcel_produces_a_non_empty_spreadsheet()
    {
        var context = TestSupport.CreateContext();
        var service = new AdminService(context);
        var results = new[] { new AdminResultRow(Guid.NewGuid(), "Estudiante Prueba", "prueba@test.local", DateTime.UtcNow, "RI", "A", 10, 6, 4, 4, 4, 4, RiasecInterpretationBuilder.Build("RI", "A")) };
        var answers = new[] { new AdminAnswerRow(results[0].AttemptId, "Estudiante Prueba", "prueba@test.local", "R01", "R", "Pregunta de prueba", 4) };

        var bytes = service.BuildExcel(results, answers);

        Assert.NotEmpty(bytes);
        Assert.Equal(0x50, bytes[0]); // firma ZIP ("PK"): un .xlsx es un zip
        Assert.Equal(0x4B, bytes[1]);
    }

    [Fact]
    public void BuildPdf_produces_a_non_empty_pdf()
    {
        var context = TestSupport.CreateContext();
        var service = new AdminService(context);
        var results = new[] { new AdminResultRow(Guid.NewGuid(), "Estudiante Prueba", "prueba@test.local", DateTime.UtcNow, "RI", "A", 10, 6, 4, 4, 4, 4, RiasecInterpretationBuilder.Build("RI", "A")) };

        var bytes = service.BuildPdf(results);

        Assert.NotEmpty(bytes);
        Assert.Equal("%PDF"u8.ToArray(), bytes[..4]);
    }
}
