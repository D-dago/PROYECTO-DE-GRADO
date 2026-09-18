using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrientacionVocacional.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CatalogAndRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "institutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Municipality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Source = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institutions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "recommendation_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestAttemptId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FilterSummary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recommendation_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recommendation_history_test_attempts_TestAttemptId",
                        column: x => x.TestAttemptId,
                        principalTable: "test_attempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recommendation_history_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "careers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Area = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Level = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    Modality = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    DurationMonths = table.Column<int>(type: "integer", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Source = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_careers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_careers_institutions_InstitutionId",
                        column: x => x.InstitutionId,
                        principalTable: "institutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "career_compatibilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CareerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Dimension = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    CompatibilityLevel = table.Column<int>(type: "integer", nullable: false),
                    Rule = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_career_compatibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_career_compatibilities_careers_CareerId",
                        column: x => x.CareerId,
                        principalTable: "careers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_career_compatibilities_CareerId_Dimension",
                table: "career_compatibilities",
                columns: new[] { "CareerId", "Dimension" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_careers_InstitutionId",
                table: "careers",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_history_TestAttemptId",
                table: "recommendation_history",
                column: "TestAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_recommendation_history_UserId",
                table: "recommendation_history",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "career_compatibilities");

            migrationBuilder.DropTable(
                name: "recommendation_history");

            migrationBuilder.DropTable(
                name: "careers");

            migrationBuilder.DropTable(
                name: "institutions");
        }
    }
}
