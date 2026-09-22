using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChasideTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RScore",
                table: "riasec_profiles",
                newName: "HScore");

            migrationBuilder.AddColumn<int>(
                name: "DScore",
                table: "riasec_profiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1060 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1061 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1062 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1063 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1064 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1065 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1066 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1067 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1068 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1069 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 5, "¿Cuando eras chico, te interesaba saber cómo estaban construidos tus juguetes?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 12, "Cuando resuelves un problema matemático, ¿perseveras hasta encontrar la solución?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 19, "¿Entablas una relación casi personal con tu computadora?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 26, "¿Harías un curso para aprender a fabricar las piezas de las máquinas o aparatos con que trabajas?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 33, "Cuando se descompone un artefacto en tu casa, ¿te dispones enseguida a repararlo?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 40, "¿Te gustaría dirigir la construcción de una central hidroeléctrica?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 47, "¿Planificas detalladamente tus trabajos antes de empezar?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 54, "¿Trabajar con objetos y máquinas te resulta más gratificante que trabajar con personas?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1070 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1071 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Te atrae armar rompecabezas o puzzles?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 10, "¿Disfrutas modelando con arcilla?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 17, "Cuando trabajas en grupo, ¿te entusiasma aportar ideas originales y que las tomen en cuenta?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 24, "¿Te gustaría que te regalen un instrumento musical para tu cumpleaños?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 31, "¿Harías el diseño de un cartel o afiche para una campaña de salud?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 38, "Cuando eliges tu ropa o decoras un ambiente, ¿te fijas en la combinación de colores, telas o estilo?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 45, "¿Harías un nuevo diseño de una prenda pasada de moda?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 52, "¿Dirigirías un grupo de teatro independiente?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 59, "¿Crees que los detalles son tan importantes como el todo?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1072 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0001-4000-8000-000000000000"),
                column: "Text",
                value: "¿Escuchas atentamente los problemas que te cuentan tus amigos?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 11, "¿Te sentirías a gusto trabajando en un hospital o centro de salud?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 18, "¿Te agradaría hacer un curso de primeros auxilios?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 25, "¿Te resultó interesante el estudio de las ciencias biológicas?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 32, "¿Leerías revistas sobre los últimos avances científicos y tecnológicos en el área de la salud?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 39, "¿Te interesaría investigar sobre una nueva vacuna?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 46, "Ante un llamado solidario, ¿te ofrecerías para cuidar a un enfermo?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 53, "¿Ayudas habitualmente a las personas no videntes a cruzar la calle?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 60, "¿Renunciarías a un momento placentero para ayudar a alguien con tu trabajo?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1073 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 7, "¿Te gustaría crear nuevas técnicas para descubrir enfermedades a través del microscopio?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 14, "¿Te incluirías en un proyecto de investigación sobre los movimientos sísmicos y sus consecuencias?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 21, "¿Te gustaría trabajar en un laboratorio mientras estudias?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 28, "¿Formarías parte de un equipo que trabaje por proteger la flora y la fauna en peligro de extinción?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 35, "¿Visitarías un observatorio astronómico para ver cómo funcionan sus aparatos?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 42, "¿Te atrae investigar los misterios del universo, como los agujeros negros?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 49, "¿Te interesan más los misterios de la naturaleza que los secretos de la tecnología?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 56, "¿Te interesa saber las causas de ciertos fenómenos, aunque saberlo no cambie tu vida?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1074 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1075 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 1, "¿Aceptarías trabajar escribiendo artículos en la sección económica de un diario?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 8, "¿Te gustaría conocer la diferencia entre macroeconomía y microeconomía?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 15, "¿Te sentirías a gusto como gerente de comercialización de una gran empresa?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 22, "¿Dirigirías el área de importación y exportación de una empresa?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 29, "¿Te costearías los estudios trabajando en una auditoría (revisión de cuentas)?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0006-4000-8000-000000000000"),
                column: "Text",
                value: "¿Te resultaría gratificante ser asesor contable en una empresa reconocida?");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 43, "¿Te gustan los temas económicos, como saber qué es el PIB?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 50, "¿Organizas tu dinero de manera que te alcance hasta el próximo cobro?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 57, "¿Distribuyes tus horarios del día para poder hacer todo lo que planeaste?" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { false, 1076 });

            migrationBuilder.InsertData(
                table: "questions",
                columns: new[] { "Id", "Code", "Dimension", "IsActive", "Order", "Text" },
                values: new object[,]
                {
                    { new Guid("70000000-0001-4000-8000-000000000000"), "H01", "H", true, 2, "¿Te ofrecerías a explicar un tema a tus compañeros que no lo entendieron?" },
                    { new Guid("70000000-0002-4000-8000-000000000000"), "H02", "H", true, 9, "¿Pasarías varias horas leyendo un libro de tu interés?" },
                    { new Guid("70000000-0003-4000-8000-000000000000"), "H03", "H", true, 16, "¿Te interesan los temas del pasado y de la evolución del ser humano?" },
                    { new Guid("70000000-0004-4000-8000-000000000000"), "H04", "H", true, 23, "¿La libertad y la justicia son valores fundamentales en tu vida?" },
                    { new Guid("70000000-0005-4000-8000-000000000000"), "H05", "H", true, 30, "¿Lucharías por una causa justa hasta las últimas consecuencias?" },
                    { new Guid("70000000-0006-4000-8000-000000000000"), "H06", "H", true, 37, "¿Te gustaría trabajar con niños?" },
                    { new Guid("70000000-0007-4000-8000-000000000000"), "H07", "H", true, 44, "¿Elegirías una carrera cuya herramienta de trabajo fuera un idioma extranjero?" },
                    { new Guid("70000000-0008-4000-8000-000000000000"), "H08", "H", true, 51, "¿Consideras importante que en el colegio se fomente la actitud crítica y la participación activa?" },
                    { new Guid("70000000-0009-4000-8000-000000000000"), "H09", "H", true, 58, "¿En una discusión entre amigos, te ofreces como mediador?" },
                    { new Guid("80000000-0001-4000-8000-000000000000"), "D01", "D", true, 6, "¿Te dedicarías a socorrer a personas accidentadas o atacadas por asaltantes?" },
                    { new Guid("80000000-0002-4000-8000-000000000000"), "D02", "D", true, 13, "¿Participarías como profesional en un espectáculo de acrobacia aérea?" },
                    { new Guid("80000000-0003-4000-8000-000000000000"), "D03", "D", true, 20, "¿Te gustaría colaborar para mantener el orden ante grandes desórdenes y catástrofes?" },
                    { new Guid("80000000-0004-4000-8000-000000000000"), "D04", "D", true, 27, "¿Te interesan las actividades de mucha acción y reacción rápida en situaciones imprevistas o de peligro?" },
                    { new Guid("80000000-0005-4000-8000-000000000000"), "D05", "D", true, 34, "¿Elegirías una profesión en la que tuvieras que estar meses lejos de tu familia, como la de marino?" },
                    { new Guid("80000000-0006-4000-8000-000000000000"), "D06", "D", true, 41, "¿Aceptarías colaborar con el cumplimiento de las normas en lugares públicos?" },
                    { new Guid("80000000-0007-4000-8000-000000000000"), "D07", "D", true, 48, "¿Ante una situación de emergencia actúas rápidamente?" },
                    { new Guid("80000000-0008-4000-8000-000000000000"), "D08", "D", true, 55, "¿Arriesgarías tu vida para salvar la de otra persona que no conoces?" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0001-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0002-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0003-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0004-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0005-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0006-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0007-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0008-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("70000000-0009-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0001-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0002-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0003-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0004-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0005-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0006-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0007-4000-8000-000000000000"));

            migrationBuilder.DeleteData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("80000000-0008-4000-8000-000000000000"));

            migrationBuilder.DropColumn(
                name: "DScore",
                table: "riasec_profiles");

            migrationBuilder.RenameColumn(
                name: "HScore",
                table: "riasec_profiles",
                newName: "RScore");

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0001-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 1 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0002-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 7 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0003-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 13 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0004-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 19 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0005-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 25 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0006-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 31 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0007-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 37 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0008-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 43 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 49 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 55 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 2, "Descubrir cómo funciona algo (ej.: cómo una app guarda tus datos)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 8, "Resolver acertijos o problemas de lógica (ej.: sudoku, ajedrez)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 14, "Analizar información antes de decidir (ej.: comparar precios y calidad)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 20, "Hacer experimentos para probar una idea (ej.: qué abono hace crecer mejor una planta)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 26, "Comparar datos y encontrar patrones (ej.: en qué meses suben las ventas)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 32, "Hacer preguntas hasta entender el porqué (ej.: por qué ocurre un terremoto)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 38, "Investigar la causa de un problema (ej.: por qué bajan las notas en un curso)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 44, "Temas de ciencia y tecnología (ej.: robótica, biología, astronomía)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 50 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 56 });

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
                columns: new[] { "Order", "Text" },
                values: new object[] { 9, "Inventar ideas nuevas para un proyecto (ej.: el tema de una feria del colegio)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 15, "Combinar colores, formas o estilos (ej.: decorar un espacio, armar un outfit)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 21, "Crear contenido para redes o video (ej.: editar un reel, grabar un podcast)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 27, "Expresar tu creatividad con libertad (ej.: pintar, bailar, componer)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 33, "Escribir historias, poemas o textos propios (ej.: un cuento, letras de canciones)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 39, "Fotografía, video, música o diseño (ej.: sacar fotos, mezclar una canción)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 45, "Convertir una idea en algo visual (ej.: un boceto, una maqueta, una presentación)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 51, "Buscar soluciones distintas a las de siempre (ej.: reusar objetos para hacer algo nuevo)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 57 });

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
                columns: new[] { "Order", "Text" },
                values: new object[] { 10, "Escuchar los problemas de otras personas (ej.: aconsejar a un amigo)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 16, "Enseñar una habilidad a alguien (ej.: tocar guitarra, cocinar)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 22, "Trabajar en equipo (ej.: un grupo de estudio, un proyecto grupal)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 28, "Apoyar a tu comunidad (ej.: voluntariado, campañas de ayuda)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 34, "Animar a un compañero que se siente mal (ej.: motivarlo antes de un examen)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 40, "Orientar a alguien en una decisión difícil (ej.: ayudarle a elegir colegio o carrera)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 46, "Trabajar directamente con personas (ej.: atender, acompañar, cuidar)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 52, "Organizar actividades para un grupo (ej.: una kermés, un campeonato)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 58 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 5, "Tomar la iniciativa en un proyecto (ej.: proponer y arrancar una actividad)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 11, "Dirigir o coordinar un equipo (ej.: ser capitán o jefe de grupo)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 17, "Presentar una idea frente a otras personas (ej.: exponer ante todo el curso)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 23, "Crear tu propio negocio (ej.: vender ropa, comida o servicios)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 29, "Tomar decisiones bajo presión (ej.: resolver un imprevisto a último momento)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0006-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 35, "Negociar para llegar a un acuerdo (ej.: regatear un precio, resolver un conflicto)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0007-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 41, "Planificar cómo lograr una meta (ej.: reunir dinero para el viaje de curso)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 47, "Convencer a otros de tu propuesta (ej.: lograr que apoyen tu idea)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0009-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 53 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 59 });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0001-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 6, "Ordenar información para encontrarla fácil (ej.: carpetas, cuadernos, archivos)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0002-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 12, "Seguir listas y pasos definidos (ej.: una receta, un manual)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0003-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 18, "Revisar que un documento no tenga errores (ej.: corregir ortografía o cifras)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0004-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 24, "Registrar datos de forma ordenada (ej.: llevar las cuentas en una planilla)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0005-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 30, "Planificar antes de empezar una actividad (ej.: armar un cronograma de estudio)" });

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
                columns: new[] { "Order", "Text" },
                values: new object[] { 42, "Mantener tus materiales organizados (ej.: útiles, archivos del celular)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0008-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 48, "Comprobar que todos los datos estén completos (ej.: revisar un formulario)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0009-4000-8000-000000000000"),
                columns: new[] { "Order", "Text" },
                values: new object[] { 54, "Seguir normas claras para hacer una tarea (ej.: reglamentos, procedimientos)" });

            migrationBuilder.UpdateData(
                table: "questions",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0010-4000-8000-000000000000"),
                columns: new[] { "IsActive", "Order" },
                values: new object[] { true, 60 });
        }
    }
}
