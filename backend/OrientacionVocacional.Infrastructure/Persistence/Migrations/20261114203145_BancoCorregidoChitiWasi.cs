using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class BancoCorregidoChitiWasi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa reparar y/o construir objetos utilizando herramientas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa aprender una actividad mediante la práctica? Ejemplo: aprender a utilizar un celular sin leer el manual.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa desarmar objetos para descubrir cómo funcionan? Ejemplo: explorar piezas o mecanismos de algún aparato electrónico.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa hacer trabajos manuales que requieren cuidado y atención?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa utilizar máquinas o equipos para realizar diferentes actividades?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa trabajar al aire libre de forma manual o práctica?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa construir objetos siguiendo instrucciones visuales?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa descubrir por qué un aparato no funciona y cómo arreglarlo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa utilizar herramientas para medir objetos o materiales?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar tareas con tus propias manos cuyo resultado puedas ver y tocar?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa descubrir cómo funcionan las cosas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa resolver problemas que requieren pensar y encontrar una solución?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa analizar información para tomar decisiones?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa hacer pruebas para comprobar si una idea es correcta?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comparar datos para descubrir patrones?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa investigar por qué ocurren las cosas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa investigar qué causa un problema antes de buscar una solución?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa explorar temas relacionados con la ciencia?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo? Ejemplo: leer el manual antes de utilizar un aparato.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa buscar información para conocer más sobre un tema que te causa curiosidad?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa crear dibujos o diseños a partir de tus propias ideas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa imaginar ideas nuevas y diferentes para crear algo? Ejemplo: inventar alguna dinámica para una reunión.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa combinar colores, formas y figuras para crear diseños?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa crear videos para contar una historia o transmitir una idea?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa expresar tus ideas mediante actividades artísticas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa inventar historias o escribir textos a partir de tus propias ideas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa compartir tus ideas a través de fotografías, videos o diseños propios?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa desarrollar tus ideas hasta convertirlas en una creación propia?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa recurrir a tu creatividad para hacer cosas y solucionar problemas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa utilizar tus gustos y estilo personal para crear proyectos propios?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa explicar algo para ayudar a otra persona a comprenderlo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa escuchar las preocupaciones o problemas de otras personas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa enseñar a otra persona a realizar una actividad que tú sabes hacer?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar actividades junto con otras personas para alcanzar un objetivo en común?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa participar en actividades que buscan ayudar a otras personas de tu comunidad?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa brindar ánimo a alguien que está pasando por un momento difícil?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa ayudar a otras personas a evaluar sus opciones para poder tomar una decisión?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa cuidar o acompañar a personas que necesitan ayuda?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa ayudar a que otras personas se integren y participen en una actividad grupal?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa acompañar a otra persona mientras aprende y mejora en algo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa proponer una actividad y dar los primeros pasos para realizarla?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa dirigir o coordinar un equipo para alcanzar una meta?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa presentar propuestas o ideas frente a un grupo de personas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa iniciar un negocio o una actividad propia para vender un producto o servicio?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa tomar decisiones cuando una actividad no sale como estaba planeada?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa negociar con otras personas para llegar a un acuerdo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa crear un plan para lograr una meta?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa convencer a otras personas para que apoyen una idea o propuesta tuya?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa hacerte responsable de llevar adelante un proyecto?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa participar en actividades donde compites por alcanzar un objetivo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa ordenar y clasificar información para encontrarla fácilmente?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar tareas siguiendo instrucciones ordenadas paso a paso?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa revisar documentos para detectar errores?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa anotar y registrar información de manera ordenada?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa planificar los pasos de una actividad antes de comenzar?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa organizar información utilizando tablas o listas?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa mantener tus materiales organizados en su lugar?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprobar que los datos estén completos?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa realizar actividades que tienen normas y procedimientos claros?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa utilizar una agenda o calendario para organizar tus tareas y fechas pendientes?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
