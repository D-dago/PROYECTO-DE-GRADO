using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    // Formato de cada texto: "Actividad concreta (ej.: ejemplo corto)". El frontend separa
    // el ejemplo y lo muestra en un tono mas suave debajo de la actividad.
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("R", [
            "Reparar cosas con herramientas (ej.: arreglar una bicicleta o un enchufe)",
            "Aprender haciendo, no leyendo (ej.: armar algo siguiendo pasos)",
            "Desarmar objetos para entender cómo funcionan (ej.: un celular viejo, un radio)",
            "Trabajo manual que exige precisión (ej.: carpintería, soldadura)",
            "Manejar máquinas o equipos (ej.: un torno, un tractor, una impresora 3D)",
            "Trabajar al aire libre con las manos (ej.: cultivar, construir, cuidar animales)",
            "Construir algo siguiendo un plano (ej.: una mesa o una casita de madera)",
            "Encontrar y corregir una falla técnica (ej.: por qué no enciende una computadora)",
            "Usar herramientas técnicas o de medición (ej.: multímetro, nivel, calibrador)",
            "Terminar una tarea con un resultado que se ve y se toca (ej.: una pared pintada)"
        ]),
        ("I", [
            "Descubrir cómo funciona algo (ej.: cómo una app guarda tus datos)",
            "Resolver acertijos o problemas de lógica (ej.: sudoku, ajedrez)",
            "Analizar información antes de decidir (ej.: comparar precios y calidad)",
            "Hacer experimentos para probar una idea (ej.: qué abono hace crecer mejor una planta)",
            "Comparar datos y encontrar patrones (ej.: en qué meses suben las ventas)",
            "Hacer preguntas hasta entender el porqué (ej.: por qué ocurre un terremoto)",
            "Investigar la causa de un problema (ej.: por qué bajan las notas en un curso)",
            "Temas de ciencia y tecnología (ej.: robótica, biología, astronomía)",
            "Entender cómo funciona algo antes de usarlo (ej.: leer cómo trabaja un motor)",
            "Aprender por tu cuenta temas complejos (ej.: programación, estadística)"
        ]),
        ("A", [
            "Dibujar o diseñar (ej.: un logotipo, un afiche)",
            "Inventar ideas nuevas para un proyecto (ej.: el tema de una feria del colegio)",
            "Combinar colores, formas o estilos (ej.: decorar un espacio, armar un outfit)",
            "Crear contenido para redes o video (ej.: editar un reel, grabar un podcast)",
            "Expresar tu creatividad con libertad (ej.: pintar, bailar, componer)",
            "Escribir historias, poemas o textos propios (ej.: un cuento, letras de canciones)",
            "Fotografía, video, música o diseño (ej.: sacar fotos, mezclar una canción)",
            "Convertir una idea en algo visual (ej.: un boceto, una maqueta, una presentación)",
            "Buscar soluciones distintas a las de siempre (ej.: reusar objetos para hacer algo nuevo)",
            "Proyectos donde importe tu estilo personal (ej.: tu propia marca de ropa)"
        ]),
        ("S", [
            "Explicar algo a quien no lo entiende (ej.: ayudar a un compañero con matemáticas)",
            "Escuchar los problemas de otras personas (ej.: aconsejar a un amigo)",
            "Enseñar una habilidad a alguien (ej.: tocar guitarra, cocinar)",
            "Trabajar en equipo (ej.: un grupo de estudio, un proyecto grupal)",
            "Apoyar a tu comunidad (ej.: voluntariado, campañas de ayuda)",
            "Animar a un compañero que se siente mal (ej.: motivarlo antes de un examen)",
            "Orientar a alguien en una decisión difícil (ej.: ayudarle a elegir colegio o carrera)",
            "Trabajar directamente con personas (ej.: atender, acompañar, cuidar)",
            "Organizar actividades para un grupo (ej.: una kermés, un campeonato)",
            "Sentirte bien al ayudar a alguien a mejorar (ej.: ver que aprende gracias a ti)"
        ]),
        ("E", [
            "Tomar la iniciativa en un proyecto (ej.: proponer y arrancar una actividad)",
            "Dirigir o coordinar un equipo (ej.: ser capitán o jefe de grupo)",
            "Presentar una idea frente a otras personas (ej.: exponer ante todo el curso)",
            "Crear tu propio negocio (ej.: vender ropa, comida o servicios)",
            "Tomar decisiones bajo presión (ej.: resolver un imprevisto a último momento)",
            "Negociar para llegar a un acuerdo (ej.: regatear un precio, resolver un conflicto)",
            "Planificar cómo lograr una meta (ej.: reunir dinero para el viaje de curso)",
            "Convencer a otros de tu propuesta (ej.: lograr que apoyen tu idea)",
            "Asumir la responsabilidad de un proyecto (ej.: ser el encargado de un evento)",
            "Competir para lograr un objetivo (ej.: ganar un concurso, ser el mejor vendedor)"
        ]),
        ("C", [
            "Ordenar información para encontrarla fácil (ej.: carpetas, cuadernos, archivos)",
            "Seguir listas y pasos definidos (ej.: una receta, un manual)",
            "Revisar que un documento no tenga errores (ej.: corregir ortografía o cifras)",
            "Registrar datos de forma ordenada (ej.: llevar las cuentas en una planilla)",
            "Planificar antes de empezar una actividad (ej.: armar un cronograma de estudio)",
            "Trabajar con tablas y registros (ej.: Excel, listas de asistencia)",
            "Mantener tus materiales organizados (ej.: útiles, archivos del celular)",
            "Comprobar que todos los datos estén completos (ej.: revisar un formulario)",
            "Seguir normas claras para hacer una tarea (ej.: reglamentos, procedimientos)",
            "Llevar control de fechas y pendientes (ej.: agenda, calendario de entregas)"
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
