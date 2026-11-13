using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    // Banco corregido segun la revision de Gabriela Lino (13-nov-2026, ORION #27): todas empiezan con
    // "¿Que tanto te interesa...?" (preguntan por intereses, no por rasgos) y el ejemplo va despues de
    // "Ejemplo:". La pantalla del test muestra el ejemplo debajo, en letra secundaria.
    private static readonly (string Dimension, string[] Texts)[] Dimensions =
    [
        ("R", [
            "¿Qué tanto te interesa reparar objetos utilizando herramientas?",
            "¿Qué tanto te interesa aprender una actividad mediante la práctica? Ejemplo: aprender a armar un objeto mientras sigues instrucciones.",
            "¿Qué tanto te interesa desarmar objetos para descubrir cómo funcionan? Ejemplo: explorar piezas de una radio vieja o un celular.",
            "¿Qué tanto te interesa realizar trabajos manuales que requieren precisión? Ejemplo: cortar, soldar o crear piezas para fabricar un objeto.",
            "¿Qué tanto te interesa manejar máquinas o equipos para realizar una tarea? Ejemplo: utilizar una impresora 3D, un torno o un tractor.",
            "¿Qué tanto te interesa realizar trabajos prácticos al aire libre? Ejemplo: cultivar plantas o colaborar en una construcción.",
            "¿Qué tanto te interesa construir objetos siguiendo un plano o diseño? Ejemplo: fabricar una mesa a partir de un dibujo con medidas.",
            "¿Qué tanto te interesa encontrar y solucionar fallas en aparatos? Ejemplo: investigar por qué una computadora no enciende.",
            "¿Qué tanto te interesa utilizar instrumentos para medir con precisión? Ejemplo: medir dimensiones con una cinta métrica, un multímetro o un calibrador.",
            "¿Qué tanto te interesa realizar tareas prácticas cuyo resultado puedas ver y tocar?"
        ]),
        ("I", [
            "¿Qué tanto te interesa descubrir cómo funcionan las cosas? Ejemplo: comprender cómo una app guarda información.",
            "¿Qué tanto te interesa resolver acertijos o problemas de lógica? Ejemplo: resolver un sudoku, un problema de ajedrez o un desafío de razonamiento.",
            "¿Qué tanto te interesa analizar información para tomar decisiones? Ejemplo: comparar características, precios y calidad antes de elegir un producto.",
            "¿Qué tanto te interesa hacer experimentos para comprobar una idea? Ejemplo: probar qué condiciones ayudan a crecer a una planta.",
            "¿Qué tanto te interesa comparar datos para descubrir patrones? Ejemplo: observar en qué meses aumenta la venta de un producto.",
            "¿Qué tanto te interesa investigar por qué ocurren ciertos fenómenos? Ejemplo: buscar las causas de un terremoto.",
            "¿Qué tanto te interesa investigar las causas de un problema? Ejemplo: averiguar por qué bajan las calificaciones de un curso.",
            "¿Qué tanto te interesa explorar temas científicos? Ejemplo: aprender sobre biología, astronomía o robótica.",
            "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo? Ejemplo: conocer cómo trabaja un motor antes de ponerlo en marcha.",
            "¿Qué tanto te interesa aprender por tu cuenta temas complejos que requieren investigación? Ejemplo: investigar un concepto de programación o estadística."
        ]),
        ("A", [
            "¿Qué tanto te interesa dibujar o crear diseños propios? Ejemplo: diseñar un logotipo o un afiche.",
            "¿Qué tanto te interesa proponer ideas originales para un proyecto? Ejemplo: imaginar una temática novedosa para una feria escolar.",
            "¿Qué tanto te interesa combinar colores y formas para crear una composición? Ejemplo: diseñar la decoración de un espacio o armar un “outfit”.",
            "¿Qué tanto te interesa crear contenido audiovisual? Ejemplo: grabar y editar un video corto.",
            "¿Qué tanto te interesa expresar tu creatividad mediante actividades artísticas? Ejemplo: pintar, bailar o componer música.",
            "¿Qué tanto te interesa escribir textos creativos? Ejemplo: inventar cuentos, poemas o letras de canciones.",
            "¿Qué tanto te interesa crear imágenes mediante la fotografía, el video o el diseño? Ejemplo: tomar fotografías buscando un encuadre original o editar videos.",
            "¿Qué tanto te interesa transformar tus ideas en representaciones visuales? Ejemplo: hacer un boceto, una maqueta o una presentación.",
            "¿Qué tanto te interesa encontrar maneras creativas de hacer o solucionar las cosas? Ejemplo: reutilizar materiales para crear un objeto diferente.",
            "¿Qué tanto te interesa desarrollar proyectos en los que puedas expresar tu estilo personal? Ejemplo: crear diseños propios para tu colección de ropa."
        ]),
        ("S", [
            "¿Qué tanto te interesa explicar algo para ayudar a otra persona a comprenderlo? Ejemplo: enseñar a un compañero un tema de matemáticas.",
            "¿Qué tanto te interesa escuchar con atención las preocupaciones o problemas de otras personas? Ejemplo: escuchar a un amigo que atraviesa una dificultad.",
            "¿Qué tanto te interesa enseñar a alguien una habilidad que tú tienes? Ejemplo: mostrarle cómo tocar un acorde o preparar una receta.",
            "¿Qué tanto te interesa colaborar con otras personas para trabajar en equipo? Ejemplo: participar en un proyecto grupal o en un grupo de estudio.",
            "¿Qué tanto te interesa participar en actividades de ayuda a tu comunidad? Ejemplo: colaborar en una campaña solidaria o ser voluntaria/o.",
            "¿Qué tanto te interesa animar a alguien que está pasando por un momento difícil o que se siente mal? Ejemplo: apoyar a un compañero nervioso antes de un examen.",
            "¿Qué tanto te interesa ayudar a otras personas a evaluar sus opciones ante una decisión? Ejemplo: conversar con un amigo que necesita elegir un curso.",
            "¿Qué tanto te interesa realizar actividades de atención y apoyo directo a personas? Ejemplo: orientar, cuidar o acompañar a alguien que necesita ayuda.",
            "¿Qué tanto te interesa organizar actividades para que un grupo participe? Ejemplo: coordinar una actividad recreativa escolar, un campeonato o una kermés.",
            "¿Qué tanto te interesa acompañar a alguien mientras aprende o mejora en algo? Ejemplo: ayudar a un compañero a practicar hasta que comprenda un tema."
        ]),
        ("E", [
            "¿Qué tanto te interesa tomar la iniciativa para poner en marcha una actividad? Ejemplo: proponer una idea y comenzar a organizarla.",
            "¿Qué tanto te interesa dirigir o coordinar un equipo para alcanzar una meta? Ejemplo: asumir la coordinación de un trabajo grupal.",
            "¿Qué tanto te interesa presentar tus ideas ante un grupo de personas? Ejemplo: exponer una propuesta frente a tu curso.",
            "¿Qué tanto te interesa emprender una actividad comercial propia? Ejemplo: organizar la venta de un producto o un servicio, o tu propio emprendimiento.",
            "¿Qué tanto te interesa tomar decisiones cuando surge un problema inesperado? Ejemplo: resolver un imprevisto en una actividad o decidir bajo presión.",
            "¿Qué tanto te interesa negociar con otras personas para alcanzar un acuerdo? Ejemplo: regatear un precio o resolver un conflicto.",
            "¿Qué tanto te interesa planificar estrategias para alcanzar una meta? Ejemplo: organizar acciones para reunir fondos para una actividad escolar o el viaje de curso.",
            "¿Qué tanto te interesa convencer a otras personas para que apoyen tu propuesta? Ejemplo: explicar las ventajas de una idea para conseguir apoyo.",
            "¿Qué tanto te interesa asumir la responsabilidad de llevar adelante un proyecto? Ejemplo: estar a cargo de un evento.",
            "¿Qué tanto te interesa participar en actividades donde compites por alcanzar un objetivo? Ejemplo: participar en un concurso con una meta definida o ser el mejor vendedor."
        ]),
        ("C", [
            "¿Qué tanto te interesa organizar información para encontrarla fácilmente? Ejemplo: ordenar carpetas, cuadernos o archivos digitales.",
            "¿Qué tanto te interesa realizar actividades siguiendo instrucciones paso a paso? Ejemplo: seguir un manual o una receta para completar una tarea.",
            "¿Qué tanto te interesa revisar documentos para detectar errores? Ejemplo: comprobar la ortografía o las cifras de un informe.",
            "¿Qué tanto te interesa registrar datos de forma ordenada? Ejemplo: anotar ingresos y gastos en una hoja de cálculo.",
            "¿Qué tanto te interesa planificar los pasos de una actividad antes de comenzar? Ejemplo: preparar un horario o cronograma de estudio.",
            "¿Qué tanto te interesa trabajar con tablas y registros de información? Ejemplo: completar una lista de asistencia o una hoja de cálculo (Excel).",
            "¿Qué tanto te interesa mantener ordenados tus materiales y archivos? Ejemplo: organizar tus útiles o los archivos de tu celular.",
            "¿Qué tanto te interesa comprobar que los datos estén completos? Ejemplo: revisar que un formulario no tenga campos vacíos.",
            "¿Qué tanto te interesa realizar tareas siguiendo normas y procedimientos claros? Ejemplo: aplicar reglamentos o instrucciones establecidas para un trabajo.",
            "¿Qué tanto te interesa llevar un control de fechas y tareas pendientes? Ejemplo: usar una agenda o un calendario para registrar entregas."
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
