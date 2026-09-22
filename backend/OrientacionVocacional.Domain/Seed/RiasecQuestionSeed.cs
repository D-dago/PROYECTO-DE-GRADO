using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

/// <summary>
/// Banco de preguntas del test vocacional CHASIDE. Las 60 preguntas activas estan
/// adaptadas del cuestionario "Chaside" (98 preguntas Si/No, 7 areas: C, H, A, S, I, D, E),
/// tomando 8-9 por area entre las de intereses y las de aptitudes. Cada intento sortea 2
/// por area de este banco. Se conservan como INACTIVAS las preguntas del test anterior
/// (RIASEC) para no romper respuestas ya guardadas que apuntan a ellas.
/// </summary>
public static class RiasecQuestionSeed
{
    // El orden de este arreglo debe coincidir con RiasecTestConstants.DimensionOrder.
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("C", [
            "¿Aceptarías trabajar escribiendo artículos en la sección económica de un diario?",
            "¿Te gustaría conocer la diferencia entre macroeconomía y microeconomía?",
            "¿Te sentirías a gusto como gerente de comercialización de una gran empresa?",
            "¿Dirigirías el área de importación y exportación de una empresa?",
            "¿Te costearías los estudios trabajando en una auditoría (revisión de cuentas)?",
            "¿Te resultaría gratificante ser asesor contable en una empresa reconocida?",
            "¿Te gustan los temas económicos, como saber qué es el PIB?",
            "¿Organizas tu dinero de manera que te alcance hasta el próximo cobro?",
            "¿Distribuyes tus horarios del día para poder hacer todo lo que planeaste?"
        ]),
        ("H", [
            "¿Te ofrecerías a explicar un tema a tus compañeros que no lo entendieron?",
            "¿Pasarías varias horas leyendo un libro de tu interés?",
            "¿Te interesan los temas del pasado y de la evolución del ser humano?",
            "¿La libertad y la justicia son valores fundamentales en tu vida?",
            "¿Lucharías por una causa justa hasta las últimas consecuencias?",
            "¿Te gustaría trabajar con niños?",
            "¿Elegirías una carrera cuya herramienta de trabajo fuera un idioma extranjero?",
            "¿Consideras importante que en el colegio se fomente la actitud crítica y la participación activa?",
            "¿En una discusión entre amigos, te ofreces como mediador?"
        ]),
        ("A", [
            "¿Te atrae armar rompecabezas o puzzles?",
            "¿Disfrutas modelando con arcilla?",
            "Cuando trabajas en grupo, ¿te entusiasma aportar ideas originales y que las tomen en cuenta?",
            "¿Te gustaría que te regalen un instrumento musical para tu cumpleaños?",
            "¿Harías el diseño de un cartel o afiche para una campaña de salud?",
            "Cuando eliges tu ropa o decoras un ambiente, ¿te fijas en la combinación de colores, telas o estilo?",
            "¿Harías un nuevo diseño de una prenda pasada de moda?",
            "¿Dirigirías un grupo de teatro independiente?",
            "¿Crees que los detalles son tan importantes como el todo?"
        ]),
        ("S", [
            "¿Escuchas atentamente los problemas que te cuentan tus amigos?",
            "¿Te sentirías a gusto trabajando en un hospital o centro de salud?",
            "¿Te agradaría hacer un curso de primeros auxilios?",
            "¿Te resultó interesante el estudio de las ciencias biológicas?",
            "¿Leerías revistas sobre los últimos avances científicos y tecnológicos en el área de la salud?",
            "¿Te interesaría investigar sobre una nueva vacuna?",
            "Ante un llamado solidario, ¿te ofrecerías para cuidar a un enfermo?",
            "¿Ayudas habitualmente a las personas no videntes a cruzar la calle?",
            "¿Renunciarías a un momento placentero para ayudar a alguien con tu trabajo?"
        ]),
        ("I", [
            "¿Cuando eras chico, te interesaba saber cómo estaban construidos tus juguetes?",
            "Cuando resuelves un problema matemático, ¿perseveras hasta encontrar la solución?",
            "¿Entablas una relación casi personal con tu computadora?",
            "¿Harías un curso para aprender a fabricar las piezas de las máquinas o aparatos con que trabajas?",
            "Cuando se descompone un artefacto en tu casa, ¿te dispones enseguida a repararlo?",
            "¿Te gustaría dirigir la construcción de una central hidroeléctrica?",
            "¿Planificas detalladamente tus trabajos antes de empezar?",
            "¿Trabajar con objetos y máquinas te resulta más gratificante que trabajar con personas?"
        ]),
        ("D", [
            "¿Te dedicarías a socorrer a personas accidentadas o atacadas por asaltantes?",
            "¿Participarías como profesional en un espectáculo de acrobacia aérea?",
            "¿Te gustaría colaborar para mantener el orden ante grandes desórdenes y catástrofes?",
            "¿Te interesan las actividades de mucha acción y reacción rápida en situaciones imprevistas o de peligro?",
            "¿Elegirías una profesión en la que tuvieras que estar meses lejos de tu familia, como la de marino?",
            "¿Aceptarías colaborar con el cumplimiento de las normas en lugares públicos?",
            "¿Ante una situación de emergencia actúas rápidamente?",
            "¿Arriesgarías tu vida para salvar la de otra persona que no conoces?"
        ]),
        ("E", [
            "¿Te gustaría crear nuevas técnicas para descubrir enfermedades a través del microscopio?",
            "¿Te incluirías en un proyecto de investigación sobre los movimientos sísmicos y sus consecuencias?",
            "¿Te gustaría trabajar en un laboratorio mientras estudias?",
            "¿Formarías parte de un equipo que trabaje por proteger la flora y la fauna en peligro de extinción?",
            "¿Visitarías un observatorio astronómico para ver cómo funcionan sus aparatos?",
            "¿Te atrae investigar los misterios del universo, como los agujeros negros?",
            "¿Te interesan más los misterios de la naturaleza que los secretos de la tecnología?",
            "¿Te interesa saber las causas de ciertos fenómenos, aunque saberlo no cambie tu vida?"
        ])
    ];

    // Preguntas del test RIASEC anterior que ya no se usan. Se mantienen (inactivas) porque
    // hay respuestas historicas que las referencian y la clave foranea es restrictiva.
    private static readonly (string Dimension, int Number, string Text)[] Legacy =
    [
        ("R", 1, "Reparar cosas con herramientas (ej.: arreglar una bicicleta o un enchufe)"),
        ("R", 2, "Aprender haciendo, no leyendo (ej.: armar algo siguiendo pasos)"),
        ("R", 3, "Desarmar objetos para entender cómo funcionan (ej.: un celular viejo, un radio)"),
        ("R", 4, "Trabajo manual que exige precisión (ej.: carpintería, soldadura)"),
        ("R", 5, "Manejar máquinas o equipos (ej.: un torno, un tractor, una impresora 3D)"),
        ("R", 6, "Trabajar al aire libre con las manos (ej.: cultivar, construir, cuidar animales)"),
        ("R", 7, "Construir algo siguiendo un plano (ej.: una mesa o una casita de madera)"),
        ("R", 8, "Encontrar y corregir una falla técnica (ej.: por qué no enciende una computadora)"),
        ("R", 9, "Usar herramientas técnicas o de medición (ej.: multímetro, nivel, calibrador)"),
        ("R", 10, "Terminar una tarea con un resultado que se ve y se toca (ej.: una pared pintada)"),
        ("I", 9, "Entender cómo funciona algo antes de usarlo (ej.: leer cómo trabaja un motor)"),
        ("I", 10, "Aprender por tu cuenta temas complejos (ej.: programación, estadística)"),
        ("A", 10, "Proyectos donde importe tu estilo personal (ej.: tu propia marca de ropa)"),
        ("S", 10, "Sentirte bien al ayudar a alguien a mejorar (ej.: ver que aprende gracias a ti)"),
        ("E", 9, "Asumir la responsabilidad de un proyecto (ej.: ser el encargado de un evento)"),
        ("E", 10, "Competir para lograr un objetivo (ej.: ganar un concurso, ser el mejor vendedor)"),
        ("C", 10, "Llevar control de fechas y pendientes (ej.: agenda, calendario de entregas)"),
    ];

    public static IReadOnlyList<Question> Create()
    {
        // El orden se intercala entre areas (C,H,A,S,I,D,E,C,H,...) en vez de agrupar las
        // preguntas de cada area seguidas: si el estudiante ve varias parecidas en fila
        // reconoce el patron y responde "para ser consistente" en vez de genuino.
        var byDimension = Dimensions
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
        var all = new List<Question>(80);
        for (var index = 0; index < byDimension.Max(questions => questions.Length); index++)
        {
            foreach (var questions in byDimension)
            {
                if (index >= questions.Length) continue;
                questions[index].Order = order++;
                all.Add(questions[index]);
            }
        }

        foreach (var (dimension, number, text) in Legacy)
        {
            all.Add(new Question
            {
                Id = QuestionId(dimension, number),
                Code = $"{dimension}{number:00}",
                Text = text,
                Dimension = dimension,
                Order = 1000 + all.Count,
                IsActive = false
            });
        }

        return all;
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
