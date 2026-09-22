using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Infrastructure.Services;

/// <summary>
/// Motor de reglas de interpretacion: traduce el codigo CHASIDE (p. ej. "HSA") a
/// lenguaje claro y a AREAS de estudio afines. Son areas genericas, no datos
/// institucionales: las carreras y universidades concretas salen unicamente del
/// catalogo cargado por el administrador.
/// </summary>
public static class RiasecInterpretationBuilder
{
    private sealed record Info(string Name, string Traits, string[] Areas);

    private static readonly Dictionary<string, Info> Dimensions = new()
    {
        ["C"] = new("Administrativa", "organizar, planificar y dirigir, y trabajar con números, empresas y economía",
            ["Administración de empresas", "Contaduría y auditoría", "Comercio exterior y marketing", "Economía y finanzas"]),
        ["H"] = new("Humanidades, Ciencias Sociales y Jurídicas", "la justicia, la lectura, los idiomas, la educación y entender a las personas y la sociedad",
            ["Derecho y ciencias políticas", "Educación y pedagogía", "Comunicación social y periodismo", "Psicología, historia e idiomas"]),
        ["A"] = new("Artística", "crear, diseñar y expresarte con sensibilidad, estilo e ideas propias",
            ["Diseño gráfico e industrial", "Arquitectura", "Artes, música y teatro", "Comunicación audiovisual y moda"]),
        ["S"] = new("Ciencias de la Salud", "cuidar, asistir y ayudar a las personas a estar sanas",
            ["Medicina", "Enfermería", "Odontología y fisioterapia", "Nutrición, bioquímica y farmacia"]),
        ["I"] = new("Enseñanzas Técnicas", "calcular, construir, reparar y trabajar con máquinas y tecnología",
            ["Ingenierías (civil, mecánica, electrónica, industrial)", "Ingeniería de sistemas y computación", "Construcción y arquitectura técnica", "Electricidad, electrónica y mecánica técnica"]),
        ["D"] = new("Defensa y Seguridad", "la acción, la disciplina y proteger a otras personas en situaciones de riesgo",
            ["Fuerzas Armadas y Policía", "Bomberos, rescate y protección civil", "Seguridad y prevención de riesgos", "Marina, aviación y logística de defensa"]),
        ["E"] = new("Ciencias Experimentales", "investigar, observar la naturaleza y descubrir el porqué de los fenómenos",
            ["Biología y ciencias ambientales", "Química y física", "Agronomía, veterinaria y ciencias agropecuarias", "Geología y astronomía"]),
    };

    public static ProfileInterpretationResponse Build(string primaryCode, string secondaryCode)
    {
        var code = (primaryCode + secondaryCode).ToUpperInvariant();
        var letters = code.Select(letter => letter.ToString()).Where(Dimensions.ContainsKey).Distinct().ToList();

        var dimensions = letters
            .Select(letter => new DimensionInfoResponse(letter, Dimensions[letter].Name, $"Te atrae {Dimensions[letter].Traits}.", Dimensions[letter].Areas))
            .ToList();

        // Las areas de la letra principal primero, sin repetir, maximo 6.
        var areas = letters.SelectMany((letter, index) => Dimensions[letter].Areas.Take(index == 0 ? 3 : 2)).Distinct().Take(6).ToList();

        var parts = letters.Select(letter => $"{Dimensions[letter].Name} ({Dimensions[letter].Traits})").ToList();
        var summary = parts.Count switch
        {
            0 => "No hay suficiente información para interpretar tu perfil.",
            1 => $"Tu perfil es {code}: lo que más destaca en ti es el área {parts[0]}.",
            _ => $"Tu perfil es {code}. Lo que más destaca en ti es el área {parts[0]}; le sigue {string.Join(", y luego ", parts.Skip(1))}."
        };
        if (areas.Count > 0)
        {
            summary += $" Con esta combinación conviene explorar carreras como {string.Join("; ", areas)}.";
        }

        return new ProfileInterpretationResponse(code, dimensions, summary, areas);
    }
}
