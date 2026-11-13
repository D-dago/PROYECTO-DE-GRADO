using OrientacionVocacional.Application.Contracts;

namespace OrientacionVocacional.Infrastructure.Services;

/// <summary>
/// Motor de reglas de interpretacion: traduce el codigo RIASEC (p. ej. "RIA") a
/// lenguaje claro y a AREAS profesionales afines (teoria de Holland). Son areas
/// genericas, no datos institucionales: las carreras y universidades concretas
/// salen unicamente del catalogo cargado por el administrador.
/// </summary>
public static class RiasecInterpretationBuilder
{
    private sealed record Info(string Name, string Traits, string[] Areas);

    private static readonly Dictionary<string, Info> Dimensions = new()
    {
        ["R"] = new("Realista", "lo práctico y concreto: trabajar con herramientas, máquinas, tecnología o al aire libre",
            ["Ingeniería civil, mecánica o electrónica", "Construcción y arquitectura técnica", "Agronomía y veterinaria", "Técnico en mantenimiento o redes"]),
        ["I"] = new("Investigador", "entender, analizar y resolver problemas con lógica, datos y ciencia",
            ["Medicina y ciencias de la salud", "Ingeniería de sistemas y ciencia de datos", "Biología, química o física", "Investigación científica"]),
        ["A"] = new("Artístico", "crear, imaginar y expresarte con libertad e ideas propias",
            ["Diseño gráfico e industrial", "Arquitectura", "Comunicación audiovisual y publicidad", "Artes, música y literatura"]),
        ["S"] = new("Social", "ayudar, enseñar, acompañar y trabajar con personas",
            ["Psicología", "Educación", "Enfermería y trabajo social", "Fisioterapia y nutrición"]),
        ["E"] = new("Emprendedor", "liderar, convencer, negociar y tomar la iniciativa en proyectos",
            ["Administración de empresas", "Marketing y comercio", "Derecho y ciencias políticas", "Emprendimiento e innovación"]),
        ["C"] = new("Convencional", "organizar, ordenar información y seguir procesos claros",
            ["Contaduría y auditoría", "Administración y finanzas", "Gestión documental y secretariado", "Informática de gestión"]),
    };

    public static ProfileInterpretationResponse Build(string primaryCode, string secondaryCode)
    {
        var code = (primaryCode + secondaryCode).ToUpperInvariant();
        var letters = code.Select(letter => letter.ToString()).Where(Dimensions.ContainsKey).Distinct().ToList();

        var dimensions = letters
            .Select(letter => new DimensionInfoResponse(letter, Dimensions[letter].Name, $"Te interesa {Dimensions[letter].Traits}.", Dimensions[letter].Areas))
            .ToList();

        // Las areas de la letra principal primero, sin repetir, maximo 6.
        var areas = letters.SelectMany((letter, index) => Dimensions[letter].Areas.Take(index == 0 ? 3 : 2)).Distinct().Take(6).ToList();

        var parts = letters.Select(letter => $"{Dimensions[letter].Name} ({Dimensions[letter].Traits})").ToList();
        var summary = parts.Count switch
        {
            0 => "No hay suficiente información para interpretar tu perfil.",
            1 => $"Tu perfil es {code}: lo que más destaca en ti es el tipo {parts[0]}.",
            _ => $"Tu perfil es {code}. Lo que más destaca en ti es el tipo {parts[0]}; le sigue {string.Join(", y luego ", parts.Skip(1))}."
        };
        if (areas.Count > 0)
        {
            summary += $" Con esta combinación conviene explorar áreas como {string.Join("; ", areas)}.";
        }

        return new ProfileInterpretationResponse(code, dimensions, summary, areas);
    }
}
