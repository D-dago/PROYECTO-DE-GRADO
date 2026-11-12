using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Application.Interfaces;

/// <summary>
/// Puntajes por dimension y codigo del perfil. En el test O*NET el codigo tiene de 0 a 3 letras:
/// solo las dimensiones por encima del promedio del propio estudiante. PrimaryCode lleva las dos
/// primeras y SecondaryCode la tercera; si ambas vienen vacias, el perfil es plano.
/// </summary>
public sealed record RiasecScoreResult(
    int RScore,
    int IScore,
    int AScore,
    int SScore,
    int EScore,
    int CScore,
    string PrimaryCode,
    string SecondaryCode);

public interface IRiasecScoringService
{
    /// <param name="answers">Respuestas con su pregunta cargada (para saber la dimension).</param>
    /// <param name="testVersion">Version del intento: decide si se puntua como O*NET o como las versiones anteriores.</param>
    RiasecScoreResult Calculate(IEnumerable<Answer> answers, string testVersion);
}
