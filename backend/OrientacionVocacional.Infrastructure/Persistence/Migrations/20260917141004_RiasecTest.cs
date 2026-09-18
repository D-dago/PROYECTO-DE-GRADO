using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RiasecTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Text = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Dimension = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "test_attempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TestVersion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_attempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_test_attempts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    AnsweredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.Id);
                    table.CheckConstraint("CK_answers_value", "\"Value\" BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_answers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_answers_test_attempts_TestAttemptId",
                        column: x => x.TestAttemptId,
                        principalTable: "test_attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "riasec_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    RScore = table.Column<int>(type: "integer", nullable: false),
                    IScore = table.Column<int>(type: "integer", nullable: false),
                    AScore = table.Column<int>(type: "integer", nullable: false),
                    SScore = table.Column<int>(type: "integer", nullable: false),
                    EScore = table.Column<int>(type: "integer", nullable: false),
                    CScore = table.Column<int>(type: "integer", nullable: false),
                    PrimaryCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    SecondaryCode = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AlgorithmVersion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_riasec_profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_riasec_profiles_test_attempts_TestAttemptId",
                        column: x => x.TestAttemptId,
                        principalTable: "test_attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "questions",
                columns: new[] { "Id", "Code", "Dimension", "IsActive", "Order", "Text" },
                values: new object[,]
                {
                    { new Guid("10000000-0001-4000-8000-000000000000"), "R01", "R", true, 1, "Me gustaría usar herramientas para reparar algo." },
                    { new Guid("10000000-0002-4000-8000-000000000000"), "R02", "R", true, 2, "Prefiero aprender haciendo que solo leyendo." },
                    { new Guid("10000000-0003-4000-8000-000000000000"), "R03", "R", true, 3, "Me interesa montar o desarmar objetos para entenderlos." },
                    { new Guid("10000000-0004-4000-8000-000000000000"), "R04", "R", true, 4, "Disfruto realizar trabajos que requieren coordinación manual." },
                    { new Guid("10000000-0005-4000-8000-000000000000"), "R05", "R", true, 5, "Me gustaría trabajar con máquinas o equipos." },
                    { new Guid("10000000-0006-4000-8000-000000000000"), "R06", "R", true, 6, "Me atraen las actividades al aire libre con tareas prácticas." },
                    { new Guid("10000000-0007-4000-8000-000000000000"), "R07", "R", true, 7, "Me interesa construir algo siguiendo un plano." },
                    { new Guid("10000000-0008-4000-8000-000000000000"), "R08", "R", true, 8, "Disfruto encontrar y corregir una falla técnica." },
                    { new Guid("10000000-0009-4000-8000-000000000000"), "R09", "R", true, 9, "Me gustaría aprender a manejar instrumentos o herramientas técnicas." },
                    { new Guid("10000000-0010-4000-8000-000000000000"), "R10", "R", true, 10, "Prefiero una tarea práctica con un resultado visible." },
                    { new Guid("20000000-0001-4000-8000-000000000000"), "I01", "I", true, 11, "Me gusta descubrir cómo funciona algo." },
                    { new Guid("20000000-0002-4000-8000-000000000000"), "I02", "I", true, 12, "Disfruto resolver acertijos o problemas lógicos." },
                    { new Guid("20000000-0003-4000-8000-000000000000"), "I03", "I", true, 13, "Me interesa analizar información antes de decidir." },
                    { new Guid("20000000-0004-4000-8000-000000000000"), "I04", "I", true, 14, "Me gustaría realizar experimentos para comprobar una idea." },
                    { new Guid("20000000-0005-4000-8000-000000000000"), "I05", "I", true, 15, "Me interesa comparar datos para encontrar patrones." },
                    { new Guid("20000000-0006-4000-8000-000000000000"), "I06", "I", true, 16, "Disfruto hacer preguntas para llegar a una explicación." },
                    { new Guid("20000000-0007-4000-8000-000000000000"), "I07", "I", true, 17, "Me gustaría investigar un problema hasta encontrar su causa." },
                    { new Guid("20000000-0008-4000-8000-000000000000"), "I08", "I", true, 18, "Me atraen las actividades relacionadas con ciencia o tecnología." },
                    { new Guid("20000000-0009-4000-8000-000000000000"), "I09", "I", true, 19, "Prefiero entender el porqué antes de aplicar una solución." },
                    { new Guid("20000000-0010-4000-8000-000000000000"), "I10", "I", true, 20, "Me gusta aprender temas complejos por mi cuenta." },
                    { new Guid("30000000-0001-4000-8000-000000000000"), "A01", "A", true, 21, "Me gusta crear dibujos, diseños o composiciones." },
                    { new Guid("30000000-0002-4000-8000-000000000000"), "A02", "A", true, 22, "Disfruto inventar ideas nuevas para un proyecto." },
                    { new Guid("30000000-0003-4000-8000-000000000000"), "A03", "A", true, 23, "Me interesa combinar colores, formas o estilos." },
                    { new Guid("30000000-0004-4000-8000-000000000000"), "A04", "A", true, 24, "Me gustaría crear contenido para redes o medios digitales." },
                    { new Guid("30000000-0005-4000-8000-000000000000"), "A05", "A", true, 25, "Prefiero una actividad donde pueda expresar mi creatividad." },
                    { new Guid("30000000-0006-4000-8000-000000000000"), "A06", "A", true, 26, "Me gusta escribir historias, textos o ideas originales." },
                    { new Guid("30000000-0007-4000-8000-000000000000"), "A07", "A", true, 27, "Me interesa la fotografía, video, música o diseño." },
                    { new Guid("30000000-0008-4000-8000-000000000000"), "A08", "A", true, 28, "Disfruto transformar una idea en una propuesta visual." },
                    { new Guid("30000000-0009-4000-8000-000000000000"), "A09", "A", true, 29, "Me gusta buscar soluciones diferentes a las habituales." },
                    { new Guid("30000000-0010-4000-8000-000000000000"), "A10", "A", true, 30, "Me gustaría trabajar en proyectos donde el estilo personal importe." },
                    { new Guid("40000000-0001-4000-8000-000000000000"), "S01", "S", true, 31, "Me gusta explicar algo cuando otra persona no lo entiende." },
                    { new Guid("40000000-0002-4000-8000-000000000000"), "S02", "S", true, 32, "Disfruto escuchar los problemas de otras personas." },
                    { new Guid("40000000-0003-4000-8000-000000000000"), "S03", "S", true, 33, "Me gustaría enseñar una habilidad a alguien." },
                    { new Guid("40000000-0004-4000-8000-000000000000"), "S04", "S", true, 34, "Prefiero colaborar antes que trabajar completamente solo." },
                    { new Guid("40000000-0005-4000-8000-000000000000"), "S05", "S", true, 35, "Me interesa participar en actividades de apoyo a la comunidad." },
                    { new Guid("40000000-0006-4000-8000-000000000000"), "S06", "S", true, 36, "Me gusta motivar a compañeros cuando tienen dificultades." },
                    { new Guid("40000000-0007-4000-8000-000000000000"), "S07", "S", true, 37, "Me gustaría orientar a alguien para que tome una buena decisión." },
                    { new Guid("40000000-0008-4000-8000-000000000000"), "S08", "S", true, 38, "Me interesa trabajar directamente con personas." },
                    { new Guid("40000000-0009-4000-8000-000000000000"), "S09", "S", true, 39, "Disfruto organizar actividades que beneficien a un grupo." },
                    { new Guid("40000000-0010-4000-8000-000000000000"), "S10", "S", true, 40, "Me siento satisfecho cuando ayudo a alguien a mejorar." },
                    { new Guid("50000000-0001-4000-8000-000000000000"), "E01", "E", true, 41, "Me gusta tomar la iniciativa en un proyecto." },
                    { new Guid("50000000-0002-4000-8000-000000000000"), "E02", "E", true, 42, "Me interesa dirigir o coordinar un equipo." },
                    { new Guid("50000000-0003-4000-8000-000000000000"), "E03", "E", true, 43, "Disfruto presentar una idea frente a otras personas." },
                    { new Guid("50000000-0004-4000-8000-000000000000"), "E04", "E", true, 44, "Me gustaría crear un negocio o emprendimiento." },
                    { new Guid("50000000-0005-4000-8000-000000000000"), "E05", "E", true, 45, "Me atraen los retos donde hay que tomar decisiones." },
                    { new Guid("50000000-0006-4000-8000-000000000000"), "E06", "E", true, 46, "Me gusta negociar para llegar a un acuerdo." },
                    { new Guid("50000000-0007-4000-8000-000000000000"), "E07", "E", true, 47, "Me interesa planificar cómo alcanzar una meta." },
                    { new Guid("50000000-0008-4000-8000-000000000000"), "E08", "E", true, 48, "Disfruto convencer a otros de una propuesta." },
                    { new Guid("50000000-0009-4000-8000-000000000000"), "E09", "E", true, 49, "Me gustaría asumir responsabilidad por un proyecto." },
                    { new Guid("50000000-0010-4000-8000-000000000000"), "E10", "E", true, 50, "Me motiva competir por alcanzar objetivos concretos." },
                    { new Guid("60000000-0001-4000-8000-000000000000"), "C01", "C", true, 51, "Me gusta ordenar información para encontrarla fácilmente." },
                    { new Guid("60000000-0002-4000-8000-000000000000"), "C02", "C", true, 52, "Prefiero trabajar con listas y pasos definidos." },
                    { new Guid("60000000-0003-4000-8000-000000000000"), "C03", "C", true, 53, "Disfruto revisar que un documento no tenga errores." },
                    { new Guid("60000000-0004-4000-8000-000000000000"), "C04", "C", true, 54, "Me gusta registrar datos de manera ordenada." },
                    { new Guid("60000000-0005-4000-8000-000000000000"), "C05", "C", true, 55, "Prefiero planificar antes de comenzar una actividad." },
                    { new Guid("60000000-0006-4000-8000-000000000000"), "C06", "C", true, 56, "Me siento cómodo trabajando con tablas o registros." },
                    { new Guid("60000000-0007-4000-8000-000000000000"), "C07", "C", true, 57, "Me gusta mantener mis archivos y materiales organizados." },
                    { new Guid("60000000-0008-4000-8000-000000000000"), "C08", "C", true, 58, "Disfruto comprobar que todos los datos estén completos." },
                    { new Guid("60000000-0009-4000-8000-000000000000"), "C09", "C", true, 59, "Prefiero seguir normas claras para realizar una tarea." },
                    { new Guid("60000000-0010-4000-8000-000000000000"), "C10", "C", true, 60, "Me gusta llevar control de fechas, tareas y pendientes." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuestionId",
                table: "answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_answers_TestAttemptId_QuestionId",
                table: "answers",
                columns: new[] { "TestAttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_questions_Code",
                table: "questions",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_questions_IsActive_Order",
                table: "questions",
                columns: new[] { "IsActive", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_riasec_profiles_TestAttemptId",
                table: "riasec_profiles",
                column: "TestAttemptId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_test_attempts_UserId",
                table: "test_attempts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "riasec_profiles");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "test_attempts");
        }
    }
}
