using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSkins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquippedSkin",
                table: "users",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "basic");

            migrationBuilder.CreateTable(
                name: "user_skins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkinId = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    PurchasedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_skins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_skins_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_skins_UserId_SkinId",
                table: "user_skins",
                columns: new[] { "UserId", "SkinId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_skins");

            migrationBuilder.DropColumn(
                name: "EquippedSkin",
                table: "users");
        }
    }
}
