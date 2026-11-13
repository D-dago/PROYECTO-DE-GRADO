using Microsoft.EntityFrameworkCore;
using OrientacionVocacional.Application.Contracts;
using OrientacionVocacional.Application.Interfaces;
using OrientacionVocacional.Domain.Constants;
using OrientacionVocacional.Domain.Entities;
using OrientacionVocacional.Domain.Enums;
using OrientacionVocacional.Infrastructure.Persistence;

namespace OrientacionVocacional.Infrastructure.Services;

public sealed class TestService(
    OrientacionVocacionalDbContext dbContext,
    IRiasecScoringService scoringService) : ITestService
{
    public async Task<TestAttemptResponse> CreateAttemptAsync(Guid userId, CancellationToken cancellationToken)
    {
        var current = await dbContext.TestAttempts
            .Include(attempt => attempt.Answers)
            .Include(attempt => attempt.AssignedQuestions)
            // Dos colecciones en una sola consulta multiplican filas (24 respuestas x 24 preguntas = 576);
            // divididas son dos consultas chicas y mucho menos trabajo para el servidor.
            .AsSplitQuery()
            .Where(attempt => attempt.UserId == userId && attempt.Status == TestAttemptStatus.InProgress && attempt.TestVersion == RiasecTestConstants.Version)
            .OrderByDescending(attempt => attempt.StartedAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

        // Un intento a medias de una version anterior del test (p. ej. RIASEC) no se reanuda:
        // sus preguntas ya no estan activas. Se deja tal cual y se empieza uno nuevo.
        if (current is not null)
        {
            return ToAttemptResponse(current, await GetCoinsAsync(userId, cancellationToken));
        }

        var attempt = new TestAttempt
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TestVersion = RiasecTestConstants.Version
        };
        dbContext.TestAttempts.Add(attempt);
        await AssignQuestionsAsync(attempt, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToAttemptResponse(attempt, await GetCoinsAsync(userId, cancellationToken));
    }

    public async Task<TestAttemptResponse?> GetAttemptAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await dbContext.TestAttempts
            .Include(candidate => candidate.Answers)
            .Include(candidate => candidate.AssignedQuestions)
            // Dos colecciones en una sola consulta multiplican filas (24 respuestas x 24 preguntas = 576);
            // divididas son dos consultas chicas y mucho menos trabajo para el servidor.
            .AsSplitQuery()
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        return attempt is null ? null : ToAttemptResponse(attempt, await GetCoinsAsync(userId, cancellationToken));
    }

    public async Task<IReadOnlyList<QuestionResponse>?> GetAssignedQuestionsAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attemptExists = await dbContext.TestAttempts
            .AnyAsync(candidate => candidate.Id == attemptId && candidate.UserId == userId, cancellationToken);
        if (!attemptExists) return null;

        return await dbContext.AssignedQuestions
            .Where(assigned => assigned.TestAttemptId == attemptId)
            .Include(assigned => assigned.Question)
            .OrderBy(assigned => assigned.Order)
            .Select(assigned => new QuestionResponse(
                assigned.Question.Id, assigned.Question.Code, assigned.Question.Text,
                assigned.Question.Dimension, assigned.Order))
            .ToListAsync(cancellationToken);
    }

    public async Task<(bool Success, string? Error, TestAttemptResponse? Attempt)> SubmitAnswerAsync(
        Guid userId,
        Guid attemptId,
        SubmitAnswerRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Value is < 1 or > 5)
        {
            return (false, "La respuesta debe estar entre 1 y 5.", null);
        }

        // Es la peticion que mas se repite (24 por estudiante), asi que no carga el intento con todas sus
        // respuestas y preguntas: una sola consulta trae solo lo que hace falta comprobar. Medido con 120
        // estudiantes, cargar y rastrear todo costaba unas 4 veces mas CPU (ver docs/CAPACIDAD.md).
        var state = await dbContext.TestAttempts
            .AsNoTracking()
            .Where(candidate => candidate.Id == attemptId && candidate.UserId == userId)
            .Select(candidate => new
            {
                candidate.Status,
                Total = candidate.AssignedQuestions.Count,
                Answered = candidate.Answers.Count,
                // Si fue asignada, la pregunta existe (llave foranea). No se exige que siga activa: si el
                // administrador la desactiva mientras alguien hace el test, rechazarla dejaria ese intento
                // imposible de terminar.
                IsAssigned = candidate.AssignedQuestions.Any(assigned => assigned.QuestionId == request.QuestionId),
                IsAnswered = candidate.Answers.Any(answer => answer.QuestionId == request.QuestionId)
            })
            .SingleOrDefaultAsync(cancellationToken);
        if (state is null)
        {
            return (false, "El intento no existe.", null);
        }
        if (state.Status != TestAttemptStatus.InProgress)
        {
            return (false, "El intento ya fue completado.", null);
        }
        if (!state.IsAssigned)
        {
            return (false, "Esa pregunta no forma parte de este intento.", null);
        }
        if (state.IsAnswered)
        {
            return (false, "La pregunta ya fue respondida en este intento.", null);
        }

        dbContext.Answers.Add(new Answer
        {
            Id = Guid.NewGuid(),
            TestAttemptId = attemptId,
            QuestionId = request.QuestionId,
            Value = request.Value
        });
        var user = await dbContext.Users.SingleAsync(candidate => candidate.Id == userId, cancellationToken);
        user.Coins += RiasecTestConstants.CoinsPerAnswer;
        await dbContext.SaveChangesAsync(cancellationToken);
        var answered = state.Answered + 1;
        return (true, null, new TestAttemptResponse(
            attemptId,
            answered,
            state.Total,
            state.Total == 0 ? 0 : Math.Round(answered * 100m / state.Total, 2),
            TestAttemptStatus.InProgress.ToString().ToUpperInvariant(),
            user.Coins));
    }

    public async Task<(bool Success, string? Error, RiasecResultResponse? Result)> CompleteAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await dbContext.TestAttempts
            .Include(candidate => candidate.Answers)
                .ThenInclude(answer => answer.Question)
            .Include(candidate => candidate.AssignedQuestions)
            .Include(candidate => candidate.RiasecProfile)
            // Dos colecciones en una sola consulta multiplican filas (24 respuestas x 24 preguntas = 576);
            // divididas son dos consultas chicas y mucho menos trabajo para el servidor.
            .AsSplitQuery()
            .SingleOrDefaultAsync(
                candidate => candidate.Id == attemptId && candidate.UserId == userId,
                cancellationToken);
        if (attempt is null)
        {
            return (false, "El intento no existe.", null);
        }
        if (attempt.Status == TestAttemptStatus.Completed && attempt.RiasecProfile is not null)
        {
            return (true, null, ToResultResponse(attempt.RiasecProfile));
        }
        if (attempt.Answers.Count != attempt.AssignedQuestions.Count)
        {
            return (false, $"El test debe tener {attempt.AssignedQuestions.Count} respuestas antes de finalizar.", null);
        }

        var score = scoringService.Calculate(attempt.Answers);
        var profile = new RiasecProfile
        {
            Id = Guid.NewGuid(),
            TestAttemptId = attempt.Id,
            RScore = score.RScore,
            IScore = score.IScore,
            AScore = score.AScore,
            SScore = score.SScore,
            EScore = score.EScore,
            CScore = score.CScore,
            PrimaryCode = score.PrimaryCode,
            SecondaryCode = score.SecondaryCode,
            // La version del intento, no la vigente: asi un intento de 12 preguntas terminado
            // despues del cambio a 24 se sigue midiendo sobre su propio maximo.
            AlgorithmVersion = attempt.TestVersion
        };
        attempt.Status = TestAttemptStatus.Completed;
        attempt.CompletedAtUtc = DateTime.UtcNow;
        dbContext.RiasecProfiles.Add(profile);
        var user = await dbContext.Users.SingleAsync(candidate => candidate.Id == userId, cancellationToken);
        user.Coins += RiasecTestConstants.CompletionBonusCoins;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (true, null, ToResultResponse(profile));
    }

    public async Task<RiasecResultResponse?> GetResultAsync(
        Guid userId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.RiasecProfiles
            .Include(candidate => candidate.TestAttempt)
            .SingleOrDefaultAsync(
                candidate => candidate.TestAttemptId == attemptId && candidate.TestAttempt.UserId == userId,
                cancellationToken);
        return profile is null ? null : ToResultResponse(profile);
    }

    /// <summary>
    /// Forma fija: las mismas preguntas para todos, las 10 primeras activas de cada dimension por su
    /// orden (60 en total), intercaladas R,I,A,S,E,C,R,I,A,S,E,C,... para que nunca aparezcan dos
    /// seguidas de la misma dimension. Antes se sorteaban 4 de 10 y cada estudiante respondia un
    /// test distinto, asi que sus puntajes no se podian comparar.
    /// </summary>
    private async Task AssignQuestionsAsync(TestAttempt attempt, CancellationToken cancellationToken)
    {
        var pool = await dbContext.Questions
            .Where(question => question.IsActive)
            .ToListAsync(cancellationToken);
        var byDimension = RiasecTestConstants.DimensionOrder.ToDictionary(
            dimension => dimension,
            dimension => pool.Where(question => question.Dimension == dimension)
                .OrderBy(question => question.Order)
                .ThenBy(question => question.Code)
                .Take(RiasecTestConstants.QuestionsPerDimensionPerAttempt)
                .ToList());

        var order = 1;
        for (var round = 0; round < RiasecTestConstants.QuestionsPerDimensionPerAttempt; round++)
        {
            foreach (var dimension in RiasecTestConstants.DimensionOrder)
            {
                // El administrador no puede dejar un area con menos de 10 activas; si aun asi faltara
                // alguna (datos antiguos), el intento sigue con las que hay en vez de fallar.
                if (round >= byDimension[dimension].Count) continue;
                var question = byDimension[dimension][round];
                dbContext.AssignedQuestions.Add(new AssignedQuestion
                {
                    Id = Guid.NewGuid(),
                    TestAttemptId = attempt.Id,
                    QuestionId = question.Id,
                    Order = order++
                });
            }
        }
    }

    private async Task<int> GetCoinsAsync(Guid userId, CancellationToken cancellationToken) =>
        await dbContext.Users.Where(candidate => candidate.Id == userId).Select(candidate => candidate.Coins).SingleAsync(cancellationToken);

    private static TestAttemptResponse ToAttemptResponse(TestAttempt attempt, int coins)
    {
        var answered = attempt.Answers.Count;
        var total = attempt.AssignedQuestions.Count;
        return new TestAttemptResponse(
            attempt.Id,
            answered,
            total,
            total == 0 ? 0 : Math.Round(answered * 100m / total, 2),
            attempt.Status.ToString().ToUpperInvariant(),
            coins);
    }

    private static RiasecResultResponse ToResultResponse(RiasecProfile profile) => new(
        profile.Id,
        profile.TestAttemptId,
        profile.RScore,
        profile.IScore,
        profile.AScore,
        profile.SScore,
        profile.EScore,
        profile.CScore,
        profile.PrimaryCode,
        profile.SecondaryCode,
        profile.CreatedAtUtc,
        profile.AlgorithmVersion);
}
