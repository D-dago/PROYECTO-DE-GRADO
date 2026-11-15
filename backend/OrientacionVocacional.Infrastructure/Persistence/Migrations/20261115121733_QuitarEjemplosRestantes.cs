using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class QuitarEjemplosRestantes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa aprender una actividad mediante la práctica?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa desarmar objetos para descubrir cómo funcionan?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa imaginar ideas nuevas y diferentes para crear algo?");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa comprender cómo funciona un equipo antes de utilizarlo? Ejemplo: leer el manual antes de utilizar un aparato.");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                column: "Text",
                value: "¿Qué tanto te interesa imaginar ideas nuevas y diferentes para crear algo? Ejemplo: inventar alguna dinámica para una reunión.");
        }
    }
}
