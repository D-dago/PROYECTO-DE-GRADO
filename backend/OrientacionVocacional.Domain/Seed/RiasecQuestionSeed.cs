using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("R", [
            "Me gustaría usar herramientas para reparar algo.",
            "Prefiero aprender haciendo que solo leyendo.",
            "Me interesa montar o desarmar objetos para entenderlos.",
            "Disfruto realizar trabajos que requieren coordinación manual.",
            "Me gustaría trabajar con máquinas o equipos.",
            "Me atraen las actividades al aire libre con tareas prácticas.",
            "Me interesa construir algo siguiendo un plano.",
            "Disfruto encontrar y corregir una falla técnica.",
            "Me gustaría aprender a manejar instrumentos o herramientas técnicas.",
            "Prefiero una tarea práctica con un resultado visible."
        ]),
        ("I", [
            "Me gusta descubrir cómo funciona algo.",
            "Disfruto resolver acertijos o problemas lógicos.",
            "Me interesa analizar información antes de decidir.",
            "Me gustaría realizar experimentos para comprobar una idea.",
            "Me interesa comparar datos para encontrar patrones.",
            "Disfruto hacer preguntas para llegar a una explicación.",
            "Me gustaría investigar un problema hasta encontrar su causa.",
            "Me atraen las actividades relacionadas con ciencia o tecnología.",
            "Prefiero entender el porqué antes de aplicar una solución.",
            "Me gusta aprender temas complejos por mi cuenta."
        ]),
        ("A", [
            "Me gusta crear dibujos, diseños o composiciones.",
            "Disfruto inventar ideas nuevas para un proyecto.",
            "Me interesa combinar colores, formas o estilos.",
            "Me gustaría crear contenido para redes o medios digitales.",
            "Prefiero una actividad donde pueda expresar mi creatividad.",
            "Me gusta escribir historias, textos o ideas originales.",
            "Me interesa la fotografía, video, música o diseño.",
            "Disfruto transformar una idea en una propuesta visual.",
            "Me gusta buscar soluciones diferentes a las habituales.",
            "Me gustaría trabajar en proyectos donde el estilo personal importe."
        ]),
        ("S", [
            "Me gusta explicar algo cuando otra persona no lo entiende.",
            "Disfruto escuchar los problemas de otras personas.",
            "Me gustaría enseñar una habilidad a alguien.",
            "Prefiero colaborar antes que trabajar completamente solo.",
            "Me interesa participar en actividades de apoyo a la comunidad.",
            "Me gusta motivar a compañeros cuando tienen dificultades.",
            "Me gustaría orientar a alguien para que tome una buena decisión.",
            "Me interesa trabajar directamente con personas.",
            "Disfruto organizar actividades que beneficien a un grupo.",
            "Me siento satisfecho cuando ayudo a alguien a mejorar."
        ]),
        ("E", [
            "Me gusta tomar la iniciativa en un proyecto.",
            "Me interesa dirigir o coordinar un equipo.",
            "Disfruto presentar una idea frente a otras personas.",
            "Me gustaría crear un negocio o emprendimiento.",
            "Me atraen los retos donde hay que tomar decisiones.",
            "Me gusta negociar para llegar a un acuerdo.",
            "Me interesa planificar cómo alcanzar una meta.",
            "Disfruto convencer a otros de una propuesta.",
            "Me gustaría asumir responsabilidad por un proyecto.",
            "Me motiva competir por alcanzar objetivos concretos."
        ]),
        ("C", [
            "Me gusta ordenar información para encontrarla fácilmente.",
            "Prefiero trabajar con listas y pasos definidos.",
            "Disfruto revisar que un documento no tenga errores.",
            "Me gusta registrar datos de manera ordenada.",
            "Prefiero planificar antes de comenzar una actividad.",
            "Me siento cómodo trabajando con tablas o registros.",
            "Me gusta mantener mis archivos y materiales organizados.",
            "Disfruto comprobar que todos los datos estén completos.",
            "Prefiero seguir normas claras para realizar una tarea.",
            "Me gusta llevar control de fechas, tareas y pendientes."
        ])
    ];

    public static IReadOnlyList<Question> Create()
    {
        var order = 1;
        return Dimensions
            .SelectMany(dimension => dimension.Texts.Select((text, index) => new Question
            {
                Id = QuestionId(dimension.Dimension, index + 1),
                Code = $"{dimension.Dimension}{index + 1:00}",
                Text = text,
                Dimension = dimension.Dimension,
                Order = order++,
                IsActive = true
            }))
            .ToArray();
    }

    public static Guid QuestionId(string dimension, int number)
    {
        var prefix = dimension switch
        {
            "R" => "10000000",
            "I" => "20000000",
            "A" => "30000000",
            "S" => "40000000",
            "E" => "50000000",
            "C" => "60000000",
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };

        return Guid.Parse($"{prefix}-{number:0000}-4000-8000-000000000000");
    }
}