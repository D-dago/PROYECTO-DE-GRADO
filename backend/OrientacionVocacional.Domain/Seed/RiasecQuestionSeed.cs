using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("R", [
            "Reparar cosas con herramientas",
            "Aprender haciendo, no leyendo",
            "Desarmar objetos para entenderlos",
            "Trabajo manual y coordinado",
            "Usar máquinas o equipos",
            "Trabajo práctico al aire libre",
            "Construir siguiendo un plano",
            "Corregir una falla técnica",
            "Manejar herramientas técnicas",
            "Tareas con resultado visible"
        ]),
        ("I", [
            "Descubrir cómo funciona algo",
            "Resolver acertijos lógicos",
            "Analizar información antes de decidir",
            "Hacer experimentos",
            "Comparar datos y hallar patrones",
            "Preguntar hasta entender",
            "Investigar la causa de un problema",
            "Ciencia y tecnología",
            "Entender el porqué de las cosas",
            "Aprender temas complejos solo"
        ]),
        ("A", [
            "Dibujar y diseñar",
            "Inventar ideas nuevas",
            "Combinar colores y formas",
            "Crear contenido digital",
            "Expresar creatividad libremente",
            "Escribir historias o ideas",
            "Fotografía, video o música",
            "Convertir una idea en algo visual",
            "Buscar soluciones distintas",
            "Proyectos con estilo personal"
        ]),
        ("S", [
            "Explicar algo a alguien",
            "Escuchar problemas ajenos",
            "Enseñar una habilidad",
            "Colaborar en equipo",
            "Apoyar a la comunidad",
            "Motivar a un compañero",
            "Orientar una decisión difícil",
            "Trabajar con personas",
            "Organizar actividades grupales",
            "Ayudar a alguien a mejorar"
        ]),
        ("E", [
            "Tomar la iniciativa",
            "Dirigir un equipo",
            "Presentar una idea en público",
            "Crear un negocio",
            "Tomar decisiones bajo presión",
            "Negociar un acuerdo",
            "Planificar cómo lograr una meta",
            "Convencer a otros",
            "Asumir responsabilidad de un proyecto",
            "Competir por un objetivo"
        ]),
        ("C", [
            "Ordenar información",
            "Seguir listas y pasos",
            "Revisar errores en un documento",
            "Registrar datos con orden",
            "Planificar antes de actuar",
            "Trabajar con tablas y registros",
            "Mantener archivos organizados",
            "Comprobar que todo esté completo",
            "Seguir normas claras",
            "Llevar control de tareas y fechas"
        ])
    ];

    public static IReadOnlyList<Question> Create()
    {
        // El orden se intercala entre dimensiones (R,I,A,S,E,C,R,I,A,S,E,C,...) en vez
        // de agrupar las 10 preguntas de cada dimension seguidas. Si el estudiante ve
        // 10 preguntas "parecidas" en fila reconoce el patron y empieza a responder
        // "para ser consistente" en vez de genuino, lo que sesga el resultado.
        var questionsByDimension = Dimensions
            .Select(dimension => dimension.Texts
                .Select((text, index) => new Question
                {
                    Id = QuestionId(dimension.Dimension, index + 1),
                    Code = $"{dimension.Dimension}{index + 1:00}",
                    Text = text,
                    Dimension = dimension.Dimension,
                    IsActive = true
                })
                .ToArray())
            .ToArray();

        var order = 1;
        var interleaved = new List<Question>(60);
        for (var index = 0; index < 10; index++)
        {
            foreach (var dimension in questionsByDimension)
            {
                var question = dimension[index];
                question.Order = order++;
                interleaved.Add(question);
            }
        }

        return interleaved;
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
