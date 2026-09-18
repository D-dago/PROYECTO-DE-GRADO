using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ClarifyQuestionsWithExamples : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Reparar cosas con herramientas (ej.: arreglar una bicicleta o un enchufe)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Aprender haciendo, no leyendo (ej.: armar algo siguiendo pasos)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Desarmar objetos para entender cómo funcionan (ej.: un celular viejo, un radio)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajo manual que exige precisión (ej.: carpintería, soldadura)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Manejar máquinas o equipos (ej.: un torno, un tractor, una impresora 3D)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar al aire libre con las manos (ej.: cultivar, construir, cuidar animales)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Construir algo siguiendo un plano (ej.: una mesa o una casita de madera)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Encontrar y corregir una falla técnica (ej.: por qué no enciende una computadora)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Usar herramientas técnicas o de medición (ej.: multímetro, nivel, calibrador)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Terminar una tarea con un resultado que se ve y se toca (ej.: una pared pintada)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Descubrir cómo funciona algo (ej.: cómo una app guarda tus datos)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Resolver acertijos o problemas de lógica (ej.: sudoku, ajedrez)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Analizar información antes de decidir (ej.: comparar precios y calidad)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Hacer experimentos para probar una idea (ej.: qué abono hace crecer mejor una planta)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Comparar datos y encontrar patrones (ej.: en qué meses suben las ventas)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Hacer preguntas hasta entender el porqué (ej.: por qué ocurre un terremoto)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Investigar la causa de un problema (ej.: por qué bajan las notas en un curso)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Temas de ciencia y tecnología (ej.: robótica, biología, astronomía)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Entender cómo funciona algo antes de usarlo (ej.: leer cómo trabaja un motor)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Aprender por tu cuenta temas complejos (ej.: programación, estadística)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Dibujar o diseñar (ej.: un logotipo, un afiche)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Inventar ideas nuevas para un proyecto (ej.: el tema de una feria del colegio)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Combinar colores, formas o estilos (ej.: decorar un espacio, armar un outfit)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Crear contenido para redes o video (ej.: editar un reel, grabar un podcast)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Expresar tu creatividad con libertad (ej.: pintar, bailar, componer)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Escribir historias, poemas o textos propios (ej.: un cuento, letras de canciones)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Fotografía, video, música o diseño (ej.: sacar fotos, mezclar una canción)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Convertir una idea en algo visual (ej.: un boceto, una maqueta, una presentación)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Buscar soluciones distintas a las de siempre (ej.: reusar objetos para hacer algo nuevo)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Proyectos donde importe tu estilo personal (ej.: tu propia marca de ropa)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Explicar algo a quien no lo entiende (ej.: ayudar a un compañero con matemáticas)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Escuchar los problemas de otras personas (ej.: aconsejar a un amigo)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Enseñar una habilidad a alguien (ej.: tocar guitarra, cocinar)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar en equipo (ej.: un grupo de estudio, un proyecto grupal)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Apoyar a tu comunidad (ej.: voluntariado, campañas de ayuda)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Animar a un compañero que se siente mal (ej.: motivarlo antes de un examen)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Orientar a alguien en una decisión difícil (ej.: ayudarle a elegir colegio o carrera)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar directamente con personas (ej.: atender, acompañar, cuidar)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Organizar actividades para un grupo (ej.: una kermés, un campeonato)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Sentirte bien al ayudar a alguien a mejorar (ej.: ver que aprende gracias a ti)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Tomar la iniciativa en un proyecto (ej.: proponer y arrancar una actividad)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Dirigir o coordinar un equipo (ej.: ser capitán o jefe de grupo)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Presentar una idea frente a otras personas (ej.: exponer ante todo el curso)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Crear tu propio negocio (ej.: vender ropa, comida o servicios)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Tomar decisiones bajo presión (ej.: resolver un imprevisto a último momento)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Negociar para llegar a un acuerdo (ej.: regatear un precio, resolver un conflicto)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Planificar cómo lograr una meta (ej.: reunir dinero para el viaje de curso)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Convencer a otros de tu propuesta (ej.: lograr que apoyen tu idea)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Asumir la responsabilidad de un proyecto (ej.: ser el encargado de un evento)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Competir para lograr un objetivo (ej.: ganar un concurso, ser el mejor vendedor)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Ordenar información para encontrarla fácil (ej.: carpetas, cuadernos, archivos)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Seguir listas y pasos definidos (ej.: una receta, un manual)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Revisar que un documento no tenga errores (ej.: corregir ortografía o cifras)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Registrar datos de forma ordenada (ej.: llevar las cuentas en una planilla)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Planificar antes de empezar una actividad (ej.: armar un cronograma de estudio)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar con tablas y registros (ej.: Excel, listas de asistencia)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Mantener tus materiales organizados (ej.: útiles, archivos del celular)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Comprobar que todos los datos estén completos (ej.: revisar un formulario)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Seguir normas claras para hacer una tarea (ej.: reglamentos, procedimientos)");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Llevar control de fechas y pendientes (ej.: agenda, calendario de entregas)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Reparar cosas con herramientas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Aprender haciendo, no leyendo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Desarmar objetos para entenderlos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajo manual y coordinado");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Usar máquinas o equipos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajo práctico al aire libre");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Construir siguiendo un plano");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Corregir una falla técnica");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Manejar herramientas técnicas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Tareas con resultado visible");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Descubrir cómo funciona algo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Resolver acertijos lógicos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Analizar información antes de decidir");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Hacer experimentos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Comparar datos y hallar patrones");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Preguntar hasta entender");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Investigar la causa de un problema");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Ciencia y tecnología");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Entender el porqué de las cosas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Aprender temas complejos solo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Dibujar y diseñar");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Inventar ideas nuevas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Combinar colores y formas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Crear contenido digital");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Expresar creatividad libremente");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Escribir historias o ideas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Fotografía, video o música");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Convertir una idea en algo visual");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Buscar soluciones distintas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Proyectos con estilo personal");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Explicar algo a alguien");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Escuchar problemas ajenos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Enseñar una habilidad");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Colaborar en equipo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Apoyar a la comunidad");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Motivar a un compañero");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Orientar una decisión difícil");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar con personas");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Organizar actividades grupales");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Ayudar a alguien a mejorar");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Tomar la iniciativa");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Dirigir un equipo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Presentar una idea en público");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Crear un negocio");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Tomar decisiones bajo presión");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Negociar un acuerdo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Planificar cómo lograr una meta");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Convencer a otros");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Asumir responsabilidad de un proyecto");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Competir por un objetivo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Ordenar información");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "Seguir listas y pasos");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "Revisar errores en un documento");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "Registrar datos con orden");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "Planificar antes de actuar");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "Trabajar con tablas y registros");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "Mantener archivos organizados");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "Comprobar que todo esté completo");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "Seguir normas claras");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Llevar control de tareas y fechas");
        }
    }
}
