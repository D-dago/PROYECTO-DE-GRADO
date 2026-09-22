using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Application.Interfaces;

public interface IAdminService
{
    Task<IReadOnlyList<AdminQuestionResponse>> GetQuestionsAsync(CancellationToken cancellationToken);
    Task<AdminQuestionResponse> CreateQuestionAsync(AdminQuestionRequest request, CancellationToken cancellationToken);
    Task<(bool Success, string? Error, AdminQuestionResponse? Question)> UpdateQuestionAsync(Guid id, AdminQuestionRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<AdminRegisteredUser>> GetUsersAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AdminResultRow>> GetResultsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<AdminAnswerRow>> GetAnswersAsync(CancellationToken cancellationToken);
    byte[] BuildExcel(IReadOnlyList<AdminResultRow> results, IReadOnlyList<AdminAnswerRow> answers);
    byte[] BuildPdf(IReadOnlyList<AdminResultRow> results);
}
