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
            ["Contaduría y auditoría", "Administración y finanzas", "Comercio internacional y aduanas", "Informática de gestión"]),
    };

    /// <summary>
    /// Terminos con los que cada area encuentra sus carreras en el catalogo (ORION #32). El nombre del area
    /// es generico ("Agronomia y veterinaria") y no coincide con el nombre de las carreras ("Ingenieria
    /// Agronomica"), asi que al tocar o escribir un area se buscan estos terminos en el nombre de cada carrera:
    /// sin tildes, en minusculas y como parte de una palabra ("agronom" encuentra "agronomica"). Un termino
    /// con "-" delante excluye: "fisica" no debe traer "Ciencias de la Actividad Fisica y del Deporte".
    /// </summary>
    private static readonly Dictionary<string, string[]> AreaTerms = new()
    {
        ["Ingeniería civil, mecánica o electrónica"] = ["civil", "mecanic", "electronic", "electromecanic", "mecatronic", "electric"],
        ["Construcción y arquitectura técnica"] = ["construccion", "arquitect"],
        ["Agronomía y veterinaria"] = ["agronom", "agroindustr", "forestal", "agricol", "veterinar", "zootec"],
        ["Técnico en mantenimiento o redes"] = ["automotriz", "mecanica industrial", "autotronic", "electrotecn", "electromecanica industrial", "redes", "telecomunic", "energias renovables"],
        ["Medicina y ciencias de la salud"] = ["medicina", "bioquim", "odontolog", "enfermer", "nutricion", "fisioterap", "fonoaudiolog"],
        ["Ingeniería de sistemas y ciencia de datos"] = ["sistemas", "informatic", "ciencia de datos", "software"],
        ["Biología, química o física"] = ["biolog", "quimic", "biotecnolog", "fisica", "-deporte"],
        ["Investigación científica"] = ["biolog", "quimic", "fisica", "matematic", "biotecnolog", "biomedic", "sociolog", "antropolog", "historia", "-deporte"],
        ["Diseño gráfico e industrial"] = ["diseno"],
        ["Arquitectura"] = ["arquitect", "planificacion del territorio"],
        ["Comunicación audiovisual y publicidad"] = ["comunicacion", "audiovisual", "cine", "publicidad", "multimedia", "transmedia", "sonido"],
        ["Artes, música y literatura"] = ["artes", "musica", "escultura", "letras", "linguistic"],
        ["Psicología"] = ["psicolog"],
        ["Educación"] = ["educacion", "parvulario", "linguistica aplicada", "actividad fisica"],
        ["Enfermería y trabajo social"] = ["enfermer", "trabajo social"],
        ["Fisioterapia y nutrición"] = ["fisioterap", "kinesiolog", "nutricion"],
        ["Administración de empresas"] = ["administracion", "empresarial", "negocios"],
        ["Marketing y comercio"] = ["marketing", "comercio", "comercial", "publicidad", "mercadotecnia"],
        ["Derecho y ciencias políticas"] = ["derecho", "politic"],
        ["Emprendimiento e innovación"] = ["emprend", "empresarial", "negocios", "comercial", "innovacion"],
        ["Contaduría y auditoría"] = ["contadur", "auditor"],
        ["Administración y finanzas"] = ["financ", "administracion", "economi"],
        // Antes "Gestión documental y secretariado": ninguna carrera vigente de Cochabamba la ofrece (ORION #33).
        ["Comercio internacional y aduanas"] = ["comercio internacional", "finanzas internacionales", "negocios internacionales", "aduan"],
        ["Informática de gestión"] = ["informatic", "sistemas", "software", "ciencia de datos"],
    };

    /// <summary>Todas las areas que puede sugerir el motor, para comprobar que cada una tiene terminos.</summary>
    public static IEnumerable<string> AllAreas => Dimensions.Values.SelectMany(info => info.Areas).Distinct();

    public static IReadOnlyList<string> SearchTermsFor(string area) => AreaTerms.TryGetValue(area, out var terms) ? terms : [];

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

        return new ProfileInterpretationResponse(code, dimensions, summary, areas, areas.ToDictionary(area => area, SearchTermsFor));
    }
}
