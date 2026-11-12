using OrientacionVocacional.Domain.Entities;

namespace OrientacionVocacional.Domain.Seed;

public static class RiasecQuestionSeed
{
    // Perfil de Intereses O*NET, version en espanol (30 actividades, 5 por dimension), en el orden
    // oficial: una actividad de cada dimension en ciclo R, I, A, S, E, C. Textos sin cambios, tal como
    // los presenta https://onetinterestprofiler.org/es/ (consultado el 2026-11-12). Contenido de las
    // O*NET Career Exploration Tools del U.S. Department of Labor, Employment and Training
    // Administration (USDOL/ETA), usado segun la O*NET Tools Developer License. Si se cambia un texto,
    // la licencia exige avisar que USDOL/ETA no aprobo la modificacion y validar la version nueva.
    private static readonly (string Dimension, string Text)[] OnetItems =
    [
        ("R", "Construir gabinetes para cocinas"),
        ("I", "Elaborar nueva medicina"),
        ("A", "Escribir libros u obras de teatro"),
        ("S", "Ayudar a personas con problemas personales o emocionales"),
        ("E", "Administrar un departamento en una compañía grande"),
        ("C", "Instala software en las computadoras en una gran red"),
        ("R", "Reparar electrodomésticos"),
        ("I", "Estudiar maneras para reducir la contaminación del agua"),
        ("A", "Componer y hacer arreglos musicales"),
        ("S", "Orientar a las personas sobre carreras"),
        ("E", "Comenzar su propio negocio"),
        ("C", "Operar una calculadora"),
        ("R", "Ensamblar partes electrónicas"),
        ("I", "Realizar experimentos químicos"),
        ("A", "Crear efectos especiales para películas"),
        ("S", "Realizar terapia de rehabilitación"),
        ("E", "Negociar contratos comerciales"),
        ("C", "Mantener registros de envíos y recepción"),
        ("R", "Manejar un camión para entregar paquetes a oficinas y hogares"),
        ("I", "Examinar muestras de sangre con el uso de un microscopio"),
        ("A", "Pintar escenografías para obras de teatro"),
        ("S", "Hacer trabajo voluntario en organizaciones sin fines de lucro"),
        ("E", "Comercializar una nueva línea de ropa"),
        ("C", "Identificar el inventario de suministros con una computadora de mano"),
        ("R", "Probar la calidad de las partes antes de un envío"),
        ("I", "Desarrollar una mejor manera de predecir el estado del tiempo o clima"),
        ("A", "Escribir libretos para películas o programas de televisión"),
        ("S", "Enseñar una clase de high school"),
        ("E", "Vender mercancía en una tienda por departamentos"),
        ("C", "Colocar sellos, separar y repartir correo para una organización"),
    ];

    // Ids de las actividades O*NET: numero 101-105 dentro de cada dimension (las 1-10 son del banco anterior).
    private const int OnetNumberOffset = 100;
    // El banco propio anterior queda inactivo con Order >= 2000: fuera del test y del banco del administrador.
    private const int PreviousBankOrderStart = 2000;
    private const int ChasideOrderStart = 1060;

    // Banco propio que usaron las versiones V1 a V3 ("Actividad (ej.: ejemplo corto)"). Ya no se
    // aplica, pero se conserva INACTIVO porque hay respuestas guardadas que lo referencian y la
    // clave foranea es restrictiva.
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
        var questions = new List<Question>(OnetItems.Length + 60 + Legacy.Length);

        // 1) Perfil de Intereses O*NET: activas, en el orden oficial (1 a 30). El ciclo R,I,A,S,E,C
        // evita que aparezcan seguidas dos actividades de la misma dimension. Codigo "O" + letra + n
        // (OR1...OC5): el administrador solo genera codigos letra + numero, asi que no chocan.
        var perDimension = new Dictionary<string, int>();
        for (var index = 0; index < OnetItems.Length; index++)
        {
            var (dimension, text) = OnetItems[index];
            var number = perDimension[dimension] = perDimension.GetValueOrDefault(dimension) + 1;
            questions.Add(new Question
            {
                Id = QuestionId(dimension, OnetNumberOffset + number),
                Code = $"O{dimension}{number}",
                Text = text,
                Dimension = dimension,
                Order = index + 1,
                IsActive = true
            });
        }

        // 2) Banco propio anterior (V1-V3): inactivo, con los mismos ids y codigos de siempre.
        var order = PreviousBankOrderStart;
        for (var index = 0; index < 10; index++)
        {
            foreach (var (dimension, texts) in Dimensions)
            {
                questions.Add(new Question
                {
                    Id = QuestionId(dimension, index + 1),
                    Code = $"{dimension}{index + 1:00}",
                    Text = texts[index],
                    Dimension = dimension,
                    Order = ++order,
                    IsActive = false
                });
            }
        }

        // 3) CHASIDE (areas H y D): inactivas, con el mismo orden que ya tenian en la base.
        for (var index = 0; index < Legacy.Length; index++)
        {
            var (dimension, number, text) = Legacy[index];
            questions.Add(new Question
            {
                Id = QuestionId(dimension, number),
                Code = $"{dimension}{number:00}",
                Text = text,
                Dimension = dimension,
                Order = ChasideOrderStart + index,
                IsActive = false
            });
        }

        return questions;
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
