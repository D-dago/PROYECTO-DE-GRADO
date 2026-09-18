using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReorderAndShortenQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                columns: new[] { "Order", "Text" },
                values: new object[] { 7, "Aprender haciendo, no leyendo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 13, "Desarmar objetos para entenderlos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 19, "Trabajo manual y coordinado" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 25, "Usar máquinas o equipos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 31, "Trabajo práctico al aire libre" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 37, "Construir siguiendo un plano" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 43, "Corregir una falla técnica" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 49, "Manejar herramientas técnicas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 55, "Tareas con resultado visible" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 2, "Descubrir cómo funciona algo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 8, "Resolver acertijos lógicos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 14, "Analizar información antes de decidir" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 20, "Hacer experimentos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 26, "Comparar datos y hallar patrones" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 32, "Preguntar hasta entender" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 38, "Investigar la causa de un problema" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 44, "Ciencia y tecnología" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 50, "Entender el porqué de las cosas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 56, "Aprender temas complejos solo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 3, "Dibujar y diseñar" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 9, "Inventar ideas nuevas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 15, "Combinar colores y formas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 21, "Crear contenido digital" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 27, "Expresar creatividad libremente" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 33, "Escribir historias o ideas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 39, "Fotografía, video o música" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 45, "Convertir una idea en algo visual" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 51, "Buscar soluciones distintas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 57, "Proyectos con estilo personal" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 4, "Explicar algo a alguien" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 10, "Escuchar problemas ajenos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 16, "Enseñar una habilidad" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 22, "Colaborar en equipo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 28, "Apoyar a la comunidad" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 34, "Motivar a un compañero" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 40, "Orientar una decisión difícil" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 46, "Trabajar con personas" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 52, "Organizar actividades grupales" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 58, "Ayudar a alguien a mejorar" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 5, "Tomar la iniciativa" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 11, "Dirigir un equipo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 17, "Presentar una idea en público" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 23, "Crear un negocio" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 29, "Tomar decisiones bajo presión" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 35, "Negociar un acuerdo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 41, "Planificar cómo lograr una meta" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 47, "Convencer a otros" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 53, "Asumir responsabilidad de un proyecto" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 59, "Competir por un objetivo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 6, "Ordenar información" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 12, "Seguir listas y pasos" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 18, "Revisar errores en un documento" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 24, "Registrar datos con orden" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 30, "Planificar antes de actuar" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 36, "Trabajar con tablas y registros" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 42, "Mantener archivos organizados" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 48, "Comprobar que todo esté completo" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 54, "Seguir normas claras" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Llevar control de tareas y fechas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "Me gustaría usar herramientas para reparar algo.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 2, "Prefiero aprender haciendo que solo leyendo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 3, "Me interesa montar o desarmar objetos para entenderlos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 4, "Disfruto realizar trabajos que requieren coordinación manual." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 5, "Me gustaría trabajar con máquinas o equipos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 6, "Me atraen las actividades al aire libre con tareas prácticas." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 7, "Me interesa construir algo siguiendo un plano." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 8, "Disfruto encontrar y corregir una falla técnica." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 9, "Me gustaría aprender a manejar instrumentos o herramientas técnicas." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 10, "Prefiero una tarea práctica con un resultado visible." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 11, "Me gusta descubrir cómo funciona algo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 12, "Disfruto resolver acertijos o problemas lógicos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 13, "Me interesa analizar información antes de decidir." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 14, "Me gustaría realizar experimentos para comprobar una idea." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 15, "Me interesa comparar datos para encontrar patrones." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 16, "Disfruto hacer preguntas para llegar a una explicación." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 17, "Me gustaría investigar un problema hasta encontrar su causa." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 18, "Me atraen las actividades relacionadas con ciencia o tecnología." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 19, "Prefiero entender el porqué antes de aplicar una solución." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 20, "Me gusta aprender temas complejos por mi cuenta." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 21, "Me gusta crear dibujos, diseños o composiciones." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 22, "Disfruto inventar ideas nuevas para un proyecto." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 23, "Me interesa combinar colores, formas o estilos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 24, "Me gustaría crear contenido para redes o medios digitales." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 25, "Prefiero una actividad donde pueda expresar mi creatividad." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 26, "Me gusta escribir historias, textos o ideas originales." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 27, "Me interesa la fotografía, video, música o diseño." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 28, "Disfruto transformar una idea en una propuesta visual." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 29, "Me gusta buscar soluciones diferentes a las habituales." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 30, "Me gustaría trabajar en proyectos donde el estilo personal importe." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 31, "Me gusta explicar algo cuando otra persona no lo entiende." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 32, "Disfruto escuchar los problemas de otras personas." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 33, "Me gustaría enseñar una habilidad a alguien." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 34, "Prefiero colaborar antes que trabajar completamente solo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 35, "Me interesa participar en actividades de apoyo a la comunidad." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 36, "Me gusta motivar a compañeros cuando tienen dificultades." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 37, "Me gustaría orientar a alguien para que tome una buena decisión." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 38, "Me interesa trabajar directamente con personas." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 39, "Disfruto organizar actividades que beneficien a un grupo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 40, "Me siento satisfecho cuando ayudo a alguien a mejorar." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 41, "Me gusta tomar la iniciativa en un proyecto." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 42, "Me interesa dirigir o coordinar un equipo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 43, "Disfruto presentar una idea frente a otras personas." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 44, "Me gustaría crear un negocio o emprendimiento." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 45, "Me atraen los retos donde hay que tomar decisiones." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 46, "Me gusta negociar para llegar a un acuerdo." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 47, "Me interesa planificar cómo alcanzar una meta." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 48, "Disfruto convencer a otros de una propuesta." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 49, "Me gustaría asumir responsabilidad por un proyecto." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 50, "Me motiva competir por alcanzar objetivos concretos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 51, "Me gusta ordenar información para encontrarla fácilmente." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 52, "Prefiero trabajar con listas y pasos definidos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 53, "Disfruto revisar que un documento no tenga errores." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 54, "Me gusta registrar datos de manera ordenada." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 55, "Prefiero planificar antes de comenzar una actividad." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 56, "Me siento cómodo trabajando con tablas o registros." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 57, "Me gusta mantener mis archivos y materiales organizados." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 58, "Disfruto comprobar que todos los datos estén completos." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 59, "Prefiero seguir normas claras para realizar una tarea." });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                column: "Text",
                value: "Me gusta llevar control de fechas, tareas y pendientes.");
        }
    }
}
