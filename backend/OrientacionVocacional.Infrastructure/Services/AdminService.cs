using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Enums;
using OrientacionVocacional.Infrastructure.Persistence;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class AdminService(OrientacionVocacionalDbContext db) : IAdminService
{
    // Las preguntas de versiones anteriores del test (Order >= 1000) se conservan inactivas y no se muestran.
    private const int LegacyOrderStart = 1000;

    public async Task<IReadOnlyList<AdminQuestionResponse>> GetQuestionsAsync(CancellationToken cancellationToken) =>
        await db.Questions.AsNoTracking()
            .Where(question => question.Order < LegacyOrderStart)
            .OrderBy(question => question.Order)
            .Select(question => new AdminQuestionResponse(question.Id, question.Code, question.Text, question.Dimension, question.IsActive, question.Order))
            .ToListAsync(cancellationToken);

    public async Task<AdminQuestionResponse> CreateQuestionAsync(AdminQuestionRequest request, CancellationToken cancellationToken)
    {
        var count = await db.Questions.CountAsync(question => question.Dimension == request.Dimension, cancellationToken);
        var code = $"{request.Dimension}{count + 1:00}";
        while (await db.Questions.AnyAsync(question => question.Code == code, cancellationToken)) code = $"{request.Dimension}{++count + 1:00}";
        var order = (await db.Questions.Where(question => question.Order < LegacyOrderStart).MaxAsync(question => (int?)question.Order, cancellationToken) ?? 0) + 1;
        var question = new Question { Id = Guid.NewGuid(), Code = code, Text = request.Text.Trim(), Dimension = request.Dimension, Order = order, IsActive = request.IsActive };
        db.Questions.Add(question);
        await db.SaveChangesAsync(cancellationToken);
        return ToResponse(question);
    }

    public async Task<(bool Success, string? Error, AdminQuestionResponse? Question)> UpdateQuestionAsync(Guid id, AdminQuestionRequest request, CancellationToken cancellationToken)
    {
        var question = await db.Questions.SingleOrDefaultAsync(candidate => candidate.Id == id, cancellationToken);
        if (question is null) return (false, "La pregunta no existe.", null);

        // Cada intento sortea 4 por area: con menos de 4 activas el test no se podria armar.
        var remainingActive = await db.Questions.CountAsync(candidate => candidate.Dimension == question.Dimension && candidate.IsActive && candidate.Id != id, cancellationToken);
        var staysActiveInSameArea = request.IsActive && request.Dimension == question.Dimension;
        if (!staysActiveInSameArea && question.IsActive && remainingActive < RiasecTestConstants.QuestionsPerDimensionPerAttempt)
            return (false, $"El área {question.Dimension} necesita al menos {RiasecTestConstants.QuestionsPerDimensionPerAttempt} preguntas activas.", null);

        question.Text = request.Text.Trim();
        question.Dimension = request.Dimension;
        question.IsActive = request.IsActive;
        await db.SaveChangesAsync(cancellationToken);
        return (true, null, ToResponse(question));
    }

    public async Task<IReadOnlyList<AdminRegisteredUser>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var users = await db.Users.AsNoTracking().OrderByDescending(user => user.CreatedAtUtc)
            .Select(user => new { user.Id, user.Email, user.FirstName, user.LastName, Role = user.Role.Name, user.IsActive, user.CreatedAtUtc, user.Coins })
            .ToListAsync(cancellationToken);
        var completed = await db.TestAttempts.AsNoTracking().Where(attempt => attempt.Status == TestAttemptStatus.Completed)
            .GroupBy(attempt => attempt.UserId).Select(group => new { UserId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(item => item.UserId, item => item.Count, cancellationToken);
        return users.Select(user => new AdminRegisteredUser(user.Id, user.Email, $"{user.FirstName} {user.LastName}", user.Role, user.IsActive, user.CreatedAtUtc, user.Coins, completed.GetValueOrDefault(user.Id))).ToList();
    }

    public async Task<IReadOnlyList<AdminResultRow>> GetResultsAsync(CancellationToken cancellationToken)
    {
        // Tambien los de los tests anteriores (24 y 12 preguntas): son RIASEC, solo cambia el maximo.
        var versions = RiasecTestConstants.InterpretableVersions;
        var rows = await db.RiasecProfiles.AsNoTracking()
            .Where(profile => versions.Contains(profile.AlgorithmVersion))
            .OrderByDescending(profile => profile.CreatedAtUtc)
            .Select(profile => new
            {
                profile.TestAttemptId, profile.TestAttempt.User.FirstName, profile.TestAttempt.User.LastName, profile.TestAttempt.User.Email,
                profile.CreatedAtUtc, profile.PrimaryCode, profile.SecondaryCode, profile.AlgorithmVersion,
                profile.RScore, profile.IScore, profile.AScore, profile.SScore, profile.EScore, profile.CScore
            })
            .ToListAsync(cancellationToken);
        return rows.Select(row => new AdminResultRow(row.TestAttemptId, $"{row.FirstName} {row.LastName}", row.Email, row.CreatedAtUtc, row.PrimaryCode, row.SecondaryCode,
            row.RScore, row.IScore, row.AScore, row.SScore, row.EScore, row.CScore,
            RiasecInterpretationBuilder.Build(row.PrimaryCode, row.SecondaryCode),
            RiasecTestConstants.MaxDimensionScoreFor(row.AlgorithmVersion))).ToList();
    }

    public async Task<IReadOnlyList<AdminAnswerRow>> GetAnswersAsync(CancellationToken cancellationToken)
    {
        var versions = RiasecTestConstants.InterpretableVersions;
        var rows = await db.Answers.AsNoTracking()
            .Where(answer => answer.TestAttempt.Status == TestAttemptStatus.Completed && versions.Contains(answer.TestAttempt.TestVersion))
            .OrderBy(answer => answer.TestAttempt.CompletedAtUtc).ThenBy(answer => answer.Question.Order)
            .Select(answer => new
            {
                answer.TestAttemptId, answer.TestAttempt.User.FirstName, answer.TestAttempt.User.LastName, answer.TestAttempt.User.Email,
                answer.Question.Code, answer.Question.Dimension, answer.Question.Text, answer.Value
            })
            .ToListAsync(cancellationToken);
        return rows.Select(row => new AdminAnswerRow(row.TestAttemptId, $"{row.FirstName} {row.LastName}", row.Email, row.Code, row.Dimension, row.Text, row.Value)).ToList();
    }

    public byte[] BuildExcel(IReadOnlyList<AdminResultRow> results, IReadOnlyList<AdminAnswerRow> answers)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Resultados");
        string[] header = ["Estudiante", "Correo", "Fecha", "Perfil", "R", "I", "A", "S", "E", "C", "Sobre"];
        for (var column = 0; column < header.Length; column++) sheet.Cell(1, column + 1).Value = header[column];
        for (var index = 0; index < results.Count; index++)
        {
            var row = results[index];
            object[] values = [row.StudentName, row.Email, row.CompletedAtUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm"), PerfilTexto(row.PrimaryCode, row.SecondaryCode), row.R, row.I, row.A, row.S, row.E, row.C, row.MaxScore];
            for (var column = 0; column < values.Length; column++) sheet.Cell(index + 2, column + 1).Value = XLCellValue.FromObject(values[column]);
        }
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Columns().AdjustToContents();

        var detail = workbook.Worksheets.Add("Respuestas");
        string[] detailHeader = ["Estudiante", "Correo", "Pregunta", "Área", "Texto", "Respuesta (1-5)"];
        for (var column = 0; column < detailHeader.Length; column++) detail.Cell(1, column + 1).Value = detailHeader[column];
        for (var index = 0; index < answers.Count; index++)
        {
            var row = answers[index];
            object[] values = [row.StudentName, row.Email, row.QuestionCode, row.Dimension, row.QuestionText, row.Value];
            for (var column = 0; column < values.Length; column++) detail.Cell(index + 2, column + 1).Value = XLCellValue.FromObject(values[column]);
        }
        detail.Row(1).Style.Font.Bold = true;
        detail.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] BuildPdf(IReadOnlyList<AdminResultRow> results)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        return Document.Create(container => container.Page(page =>
        {
            page.Size(PageSizes.A4.Landscape());
            page.Margin(30);
            page.DefaultTextStyle(style => style.FontSize(9));
            page.Header().Column(column =>
            {
                column.Item().Text("Resultados del test vocacional RIASEC").FontSize(16).Bold();
                column.Item().Text($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm} · {results.Count} resultado(s)").FontColor(Colors.Grey.Darken1);
            });
            page.Content().PaddingTop(10).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); columns.RelativeColumn(4); columns.RelativeColumn(2.2f); columns.RelativeColumn(1.5f);
                    for (var index = 0; index < 7; index++) columns.RelativeColumn(0.8f);
                });
                string[] header = ["Estudiante", "Correo", "Fecha", "Perfil", "R", "I", "A", "S", "E", "C", "Sobre"];
                table.Header(row => { foreach (var title in header) row.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text(title).Bold(); });
                foreach (var item in results)
                {
                    string[] cells = [item.StudentName, item.Email, item.CompletedAtUtc.ToLocalTime().ToString("dd/MM/yyyy HH:mm"), PerfilTexto(item.PrimaryCode, item.SecondaryCode),
                        item.R.ToString(), item.I.ToString(), item.A.ToString(), item.S.ToString(), item.E.ToString(), item.C.ToString(), item.MaxScore.ToString()];
                    foreach (var cell in cells) table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(cell);
                }
            });
            page.Footer().AlignCenter().Text(text => { text.Span("Página "); text.CurrentPageNumber(); });
        })).GeneratePdf();
    }

    private static AdminQuestionResponse ToResponse(Question question) => new(question.Id, question.Code, question.Text, question.Dimension, question.IsActive, question.Order);

    // En el test O*NET un perfil sin ninguna area sobre el promedio no tiene codigo: se exporta como "Perfil plano".
    private static string PerfilTexto(string primaryCode, string secondaryCode) =>
        string.IsNullOrEmpty(primaryCode + secondaryCode) ? "Perfil plano" : primaryCode + secondaryCode;
}
