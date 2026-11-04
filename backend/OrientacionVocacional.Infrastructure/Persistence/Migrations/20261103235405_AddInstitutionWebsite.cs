using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInstitutionWebsite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "institutions",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            // Direcciones comprobadas el 2026-11-03 abriendo cada sitio. INCOS e Instituto Alvarez Plata
            // quedan vacios a proposito: no se encontro una pagina oficial que se pudiera confirmar.
            foreach (var (name, website) in new[]
            {
                ("Universidad Mayor de San Simón (UMSS)", "https://www.umss.edu.bo"),
                ("Universidad del Valle (Univalle)", "https://www.univalle.edu"),
                ("Universidad Privada Boliviana (UPB)", "https://www.upb.edu"),
                ("Universidad Pública de El Alto (UPEA)", "https://www.upea.edu.bo"),
                ("Universidad Simón I. Patiño (USIP)", "https://usip.edu.bo"),
                ("Tecnológico Boliviano Alemán (TECBA)", "https://tecba.edu.bo"),
                ("Instituto Tecnológico Puerto de Mejillones", "https://itpm.edu.bo"),
            })
            {
                migrationBuilder.Sql($"UPDATE institutions SET \"Website\" = '{website}' WHERE \"Name\" = '{name}';");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Website",
                table: "institutions");
        }
    }
}
