using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    // Banco corregido por Gabriela Lino (Ch'iti Wasi, 14-nov-2026, ORION #30): todas empiezan con
    // "¿Que tanto te interesa...?" (preguntan por intereses, no por rasgos) y ninguna lleva ejemplo, para
    // que no guien las respuestas (los 4 que quedaban en R02, R03, I09 y A02 se quitaron en ORION #37).
    // I05 conserva la pregunta anterior porque en el Word repetia el texto de I04.
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("R", [
            "¿Qué tanto te interesa reparar y/o construir objetos utilizando herramientas?",
            "¿Qué tanto te interesa aprender una actividad mediante la práctica?",
            "¿Qué tanto te interesa desarmar objetos para descubrir cómo funcionan?",
            "¿Qué tanto te interesa hacer trabajos manuales que requieren cuidado y atención?",
            "¿Qué tanto te interesa utilizar máquinas o equipos para realizar diferentes actividades?",
            "¿Qué tanto te interesa trabajar al aire libre de forma manual o práctica?",
            "¿Qué tanto te interesa construir objetos siguiendo instrucciones visuales?",
            "¿Qué tanto te interesa descubrir por qué un aparato no funciona y cómo arreglarlo?",
            "¿Qué tanto te interesa utilizar herramientas para medir objetos o materiales?",
            "¿Qué tanto te interesa realizar tareas con tus propias manos cuyo resultado puedas ver y tocar?"
        ]),
        ("I", [
            "¿Qué tanto te interesa descubrir cómo funcionan las cosas?",
            "¿Qué tanto te interesa resolver problemas que requieren pensar y encontrar una solución?",
            "¿Qué tanto te interesa analizar información para tomar decisiones?",
            "¿Qué tanto te interesa hacer pruebas para comprobar si una idea es correcta?",
            "¿Qué tanto te interesa comparar datos para descubrir patrones?",
            "¿Qué tanto te interesa investigar por qué ocurren las cosas?",
            "¿Qué tanto te interesa investigar qué causa un problema antes de buscar una solución?",
            "¿Qué tanto te interesa explorar temas relacionados con la ciencia?",
            "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo?",
            "¿Qué tanto te interesa buscar información para conocer más sobre un tema que te causa curiosidad?"
        ]),
        ("A", [
            "¿Qué tanto te interesa crear dibujos o diseños a partir de tus propias ideas?",
            "¿Qué tanto te interesa imaginar ideas nuevas y diferentes para crear algo?",
            "¿Qué tanto te interesa combinar colores, formas y figuras para crear diseños?",
            "¿Qué tanto te interesa crear videos para contar una historia o transmitir una idea?",
            "¿Qué tanto te interesa expresar tus ideas mediante actividades artísticas?",
            "¿Qué tanto te interesa inventar historias o escribir textos a partir de tus propias ideas?",
            "¿Qué tanto te interesa compartir tus ideas a través de fotografías, videos o diseños propios?",
            "¿Qué tanto te interesa desarrollar tus ideas hasta convertirlas en una creación propia?",
            "¿Qué tanto te interesa recurrir a tu creatividad para hacer cosas y solucionar problemas?",
            "¿Qué tanto te interesa utilizar tus gustos y estilo personal para crear proyectos propios?"
        ]),
        ("S", [
            "¿Qué tanto te interesa explicar algo para ayudar a otra persona a comprenderlo?",
            "¿Qué tanto te interesa escuchar las preocupaciones o problemas de otras personas?",
            "¿Qué tanto te interesa enseñar a otra persona a realizar una actividad que tú sabes hacer?",
            "¿Qué tanto te interesa realizar actividades junto con otras personas para alcanzar un objetivo en común?",
            "¿Qué tanto te interesa participar en actividades que buscan ayudar a otras personas de tu comunidad?",
            "¿Qué tanto te interesa brindar ánimo a alguien que está pasando por un momento difícil?",
            "¿Qué tanto te interesa ayudar a otras personas a evaluar sus opciones para poder tomar una decisión?",
            "¿Qué tanto te interesa cuidar o acompañar a personas que necesitan ayuda?",
            "¿Qué tanto te interesa ayudar a que otras personas se integren y participen en una actividad grupal?",
            "¿Qué tanto te interesa acompañar a otra persona mientras aprende y mejora en algo?"
        ]),
        ("E", [
            "¿Qué tanto te interesa proponer una actividad y dar los primeros pasos para realizarla?",
            "¿Qué tanto te interesa dirigir o coordinar un equipo para alcanzar una meta?",
            "¿Qué tanto te interesa presentar propuestas o ideas frente a un grupo de personas?",
            "¿Qué tanto te interesa iniciar un negocio o una actividad propia para vender un producto o servicio?",
            "¿Qué tanto te interesa tomar decisiones cuando una actividad no sale como estaba planeada?",
            "¿Qué tanto te interesa negociar con otras personas para llegar a un acuerdo?",
            "¿Qué tanto te interesa crear un plan para lograr una meta?",
            "¿Qué tanto te interesa convencer a otras personas para que apoyen una idea o propuesta tuya?",
            "¿Qué tanto te interesa hacerte responsable de llevar adelante un proyecto?",
            "¿Qué tanto te interesa participar en actividades donde compites por alcanzar un objetivo?"
        ]),
        ("C", [
            "¿Qué tanto te interesa ordenar y clasificar información para encontrarla fácilmente?",
            "¿Qué tanto te interesa realizar tareas siguiendo instrucciones ordenadas paso a paso?",
            "¿Qué tanto te interesa revisar documentos para detectar errores?",
            "¿Qué tanto te interesa anotar y registrar información de manera ordenada?",
            "¿Qué tanto te interesa planificar los pasos de una actividad antes de comenzar?",
            "¿Qué tanto te interesa organizar información utilizando tablas o listas?",
            "¿Qué tanto te interesa mantener tus materiales organizados en su lugar?",
            "¿Qué tanto te interesa comprobar que los datos estén completos?",
            "¿Qué tanto te interesa realizar actividades que tienen normas y procedimientos claros?",
            "¿Qué tanto te interesa utilizar una agenda o calendario para organizar tus tareas y fechas pendientes?"
        ])
    ];

    // Preguntas de las areas H y D del test CHASIDE, que el sistema aplico durante un tiempo.
    // Ya no se usan, pero se conservan INACTIVAS porque hay respuestas guardadas que las
    // referencian y la clave foranea es restrictiva.
    private static readonly (string Dimension, int Number, string Text)[] Legacy =
    [
        ("H", 1, "¿Te ofrecerías a explicar un tema a tus compañeros que no lo entendieron?"),
        ("H", 2, "¿Pasarías varias horas leyendo un libro de tu interés?"),
        ("H", 3, "¿Te interesan los temas del pasado y de la evolución del ser humano?"),
        ("H", 4, "¿La libertad y la justicia son valores fundamentales en tu vida?"),
        ("H", 5, "¿Lucharías por una causa justa hasta las últimas consecuencias?"),
        ("H", 6, "¿Te gustaría trabajar con niños?"),
        ("H", 7, "¿Elegirías una carrera cuya herramienta de trabajo fuera un idioma extranjero?"),
        ("H", 8, "¿Consideras importante que en el colegio se fomente la actitud crítica y la participación activa?"),
        ("H", 9, "¿En una discusión entre amigos, te ofreces como mediador?"),
        ("D", 1, "¿Te dedicarías a socorrer a personas accidentadas o atacadas por asaltantes?"),
        ("D", 2, "¿Participarías como profesional en un espectáculo de acrobacia aérea?"),
        ("D", 3, "¿Te gustaría colaborar para mantener el orden ante grandes desórdenes y catástrofes?"),
        ("D", 4, "¿Te interesan las actividades de mucha acción y reacción rápida en situaciones imprevistas o de peligro?"),
        ("D", 5, "¿Elegirías una profesión en la que tuvieras que estar meses lejos de tu familia, como la de marino?"),
        ("D", 6, "¿Aceptarías colaborar con el cumplimiento de las normas en lugares públicos?"),
        ("D", 7, "¿Ante una situación de emergencia actúas rápidamente?"),
        ("D", 8, "¿Arriesgarías tu vida para salvar la de otra persona que no conoces?"),
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
        var interleaved = new List<Question>(60 + Legacy.Length);
        for (var index = 0; index < 10; index++)
        {
            foreach (var dimension in questionsByDimension)
            {
                var question = dimension[index];
                question.Order = order++;
                interleaved.Add(question);
            }
        }

        foreach (var (dimension, number, text) in Legacy)
        {
            interleaved.Add(new Question
            {
                Id = QuestionId(dimension, number),
                Code = $"{dimension}{number:00}",
                Text = text,
                Dimension = dimension,
                Order = 1000 + interleaved.Count,
                IsActive = false
            });
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
            "H" => "70000000",
            "D" => "80000000",
            _ => throw new ArgumentOutOfRangeException(nameof(dimension))
        };

        return Guid.Parse($"{prefix}-{number:0000}-4000-8000-000000000000");
    }
}
