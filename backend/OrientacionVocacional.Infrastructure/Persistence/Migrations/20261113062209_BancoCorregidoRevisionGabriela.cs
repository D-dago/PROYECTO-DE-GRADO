using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BancoCorregidoRevisionGabriela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa reparar objetos utilizando herramientas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa aprender una actividad mediante la práctica? Ejemplo: aprender a armar un objeto mientras sigues instrucciones.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa desarmar objetos para descubrir cómo funcionan? Ejemplo: explorar piezas de una radio vieja o un celular.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar trabajos manuales que requieren precisión? Ejemplo: cortar, soldar o crear piezas para fabricar un objeto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa manejar máquinas o equipos para realizar una tarea? Ejemplo: utilizar una impresora 3D, un torno o un tractor.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar trabajos prácticos al aire libre? Ejemplo: cultivar plantas o colaborar en una construcción.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa construir objetos siguiendo un plano o diseño? Ejemplo: fabricar una mesa a partir de un dibujo con medidas.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa encontrar y solucionar fallas en aparatos? Ejemplo: investigar por qué una computadora no enciende.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa utilizar instrumentos para medir con precisión? Ejemplo: medir dimensiones con una cinta métrica, un multímetro o un calibrador.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar tareas prácticas cuyo resultado puedas ver y tocar?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa descubrir cómo funcionan las cosas? Ejemplo: comprender cómo una app guarda información.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa resolver acertijos o problemas de lógica? Ejemplo: resolver un sudoku, un problema de ajedrez o un desafío de razonamiento.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa analizar información para tomar decisiones? Ejemplo: comparar características, precios y calidad antes de elegir un producto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa hacer experimentos para comprobar una idea? Ejemplo: probar qué condiciones ayudan a crecer a una planta.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comparar datos para descubrir patrones? Ejemplo: observar en qué meses aumenta la venta de un producto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa investigar por qué ocurren ciertos fenómenos? Ejemplo: buscar las causas de un terremoto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa investigar las causas de un problema? Ejemplo: averiguar por qué bajan las calificaciones de un curso.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa explorar temas científicos? Ejemplo: aprender sobre biología, astronomía o robótica.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo? Ejemplo: conocer cómo trabaja un motor antes de ponerlo en marcha.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa aprender por tu cuenta temas complejos que requieren investigación? Ejemplo: investigar un concepto de programación o estadística.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa dibujar o crear diseños propios? Ejemplo: diseñar un logotipo o un afiche.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa proponer ideas originales para un proyecto? Ejemplo: imaginar una temática novedosa para una feria escolar.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa combinar colores y formas para crear una composición? Ejemplo: diseñar la decoración de un espacio o armar un “outfit”.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa crear contenido audiovisual? Ejemplo: grabar y editar un video corto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa expresar tu creatividad mediante actividades artísticas? Ejemplo: pintar, bailar o componer música.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa escribir textos creativos? Ejemplo: inventar cuentos, poemas o letras de canciones.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa crear imágenes mediante la fotografía, el video o el diseño? Ejemplo: tomar fotografías buscando un encuadre original o editar videos.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa transformar tus ideas en representaciones visuales? Ejemplo: hacer un boceto, una maqueta o una presentación.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa encontrar maneras creativas de hacer o solucionar las cosas? Ejemplo: reutilizar materiales para crear un objeto diferente.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa desarrollar proyectos en los que puedas expresar tu estilo personal? Ejemplo: crear diseños propios para tu colección de ropa.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa explicar algo para ayudar a otra persona a comprenderlo? Ejemplo: enseñar a un compañero un tema de matemáticas.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa escuchar con atención las preocupaciones o problemas de otras personas? Ejemplo: escuchar a un amigo que atraviesa una dificultad.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa enseñar a alguien una habilidad que tú tienes? Ejemplo: mostrarle cómo tocar un acorde o preparar una receta.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa colaborar con otras personas para trabajar en equipo? Ejemplo: participar en un proyecto grupal o en un grupo de estudio.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa participar en actividades de ayuda a tu comunidad? Ejemplo: colaborar en una campaña solidaria o ser voluntaria/o.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa animar a alguien que está pasando por un momento difícil o que se siente mal? Ejemplo: apoyar a un compañero nervioso antes de un examen.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa ayudar a otras personas a evaluar sus opciones ante una decisión? Ejemplo: conversar con un amigo que necesita elegir un curso.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar actividades de atención y apoyo directo a personas? Ejemplo: orientar, cuidar o acompañar a alguien que necesita ayuda.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa organizar actividades para que un grupo participe? Ejemplo: coordinar una actividad recreativa escolar, un campeonato o una kermés.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa acompañar a alguien mientras aprende o mejora en algo? Ejemplo: ayudar a un compañero a practicar hasta que comprenda un tema.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa tomar la iniciativa para poner en marcha una actividad? Ejemplo: proponer una idea y comenzar a organizarla.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa dirigir o coordinar un equipo para alcanzar una meta? Ejemplo: asumir la coordinación de un trabajo grupal.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa presentar tus ideas ante un grupo de personas? Ejemplo: exponer una propuesta frente a tu curso.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa emprender una actividad comercial propia? Ejemplo: organizar la venta de un producto o un servicio, o tu propio emprendimiento.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa tomar decisiones cuando surge un problema inesperado? Ejemplo: resolver un imprevisto en una actividad o decidir bajo presión.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa negociar con otras personas para alcanzar un acuerdo? Ejemplo: regatear un precio o resolver un conflicto.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa planificar estrategias para alcanzar una meta? Ejemplo: organizar acciones para reunir fondos para una actividad escolar o el viaje de curso.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa convencer a otras personas para que apoyen tu propuesta? Ejemplo: explicar las ventajas de una idea para conseguir apoyo.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa asumir la responsabilidad de llevar adelante un proyecto? Ejemplo: estar a cargo de un evento.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa participar en actividades donde compites por alcanzar un objetivo? Ejemplo: participar en un concurso con una meta definida o ser el mejor vendedor.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa organizar información para encontrarla fácilmente? Ejemplo: ordenar carpetas, cuadernos o archivos digitales.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar actividades siguiendo instrucciones paso a paso? Ejemplo: seguir un manual o una receta para completar una tarea.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa revisar documentos para detectar errores? Ejemplo: comprobar la ortografía o las cifras de un informe.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa registrar datos de forma ordenada? Ejemplo: anotar ingresos y gastos en una hoja de cálculo.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa planificar los pasos de una actividad antes de comenzar? Ejemplo: preparar un horario o cronograma de estudio.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa trabajar con tablas y registros de información? Ejemplo: completar una lista de asistencia o una hoja de cálculo (Excel).");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa mantener ordenados tus materiales y archivos? Ejemplo: organizar tus útiles o los archivos de tu celular.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprobar que los datos estén completos? Ejemplo: revisar que un formulario no tenga campos vacíos.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar tareas siguiendo normas y procedimientos claros? Ejemplo: aplicar reglamentos o instrucciones establecidas para un trabajo.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa llevar un control de fechas y tareas pendientes? Ejemplo: usar una agenda o un calendario para registrar entregas.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
