using System.ComponentModel.DataAnnotations;

namespace OrientacionVocacional.Application.Contracts;

public sealed record AdminQuestionResponse(Guid Id, string Code, string Text, string Dimension, bool IsActive, int Order);

public sealed class AdminQuestionRequest
{
    [Required, MinLength(5), MaxLength(300)]
    public required string Text { get; init; }

    [Required, RegularExpression("^[CHASIDE]$", ErrorMessage = "La dimensión debe ser C, H, A, S, I, D o E.")]
    public required string Dimension { get; init; }

    public bool IsActive { get; init; } = true;
}

public sealed record AdminResultRow(
    Guid AttemptId, string StudentName, string Email, DateTime CompletedAtUtc,
    string PrimaryCode, string SecondaryCode,
    int C, int H, int A, int S, int I, int D, int E);

public sealed record AdminAnswerRow(Guid AttemptId, string StudentName, string Email, string QuestionCode, string Dimension, string QuestionText, int Value);

public sealed record AdminRegisteredUser(Guid Id, string Email, string FullName, string Role, bool IsActive, DateTime CreatedAtUtc, int Coins, int CompletedTests);
