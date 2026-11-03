using System.ComponentModel.DataAnnotations;

namespace OrientacionVocacional.Application.Contracts;

public sealed record AdminQuestionResponse(Guid Id, string Code, string Text, string Dimension, bool IsActive, int Order);

public sealed class AdminQuestionRequest
{
    [Required, MinLength(5), MaxLength(300)]
    public required string Text { get; init; }

    [Required, RegularExpression("^[RIASEC]$", ErrorMessage = "La dimensión debe ser R, I, A, S, E o C.")]
    public required string Dimension { get; init; }

    public bool IsActive { get; init; } = true;
}

public sealed record AdminResultRow(
    Guid AttemptId, string StudentName, string Email, DateTime CompletedAtUtc,
    string PrimaryCode, string SecondaryCode,
    int R, int I, int A, int S, int E, int C,
    ProfileInterpretationResponse Interpretation);

public sealed record AdminAnswerRow(Guid AttemptId, string StudentName, string Email, string QuestionCode, string Dimension, string QuestionText, int Value);

public sealed record AdminRegisteredUser(Guid Id, string Email, string FullName, string Role, bool IsActive, DateTime CreatedAtUtc, int Coins, int CompletedTests);
