using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeveloperGoals.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnologyDefinitionsAndIgnoredTechnologies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStart",
                table: "UserTechnologies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TechnologyDefinitionId",
                table: "UserTechnologies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IgnoredTechnologies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tag = table.Column<int>(type: "integer", nullable: false),
                    SystemDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    AiReasoning = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ContextTechnologyId = table.Column<int>(type: "integer", nullable: true),
                    IgnoredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgnoredTechnologies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgnoredTechnologies_UserTechnologies_ContextTechnologyId",
                        column: x => x.ContextTechnologyId,
                        principalTable: "UserTechnologies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_IgnoredTechnologies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnologyDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Prefix = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Tag = table.Column<int>(type: "integer", nullable: false),
                    SystemDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologyDefinitions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TechnologyDefinitions",
                columns: new[] { "Id", "CreatedAt", "Name", "Prefix", "SystemDescription", "Tag" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Start", "System", "Węzeł startowy grafu technologii. Automatycznie tworzony dla każdego użytkownika.", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_UserTechnologies_TechnologyDefinitionId",
                table: "UserTechnologies",
                column: "TechnologyDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyDependencies_UserId",
                table: "TechnologyDependencies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredTechnologies_ContextTechnologyId",
                table: "IgnoredTechnologies",
                column: "ContextTechnologyId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredTechnologies_UserId",
                table: "IgnoredTechnologies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IgnoredTechnologies_UserId_Name_Category",
                table: "IgnoredTechnologies",
                columns: new[] { "UserId", "Name", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologyDefinitions_Name_Prefix_Tag",
                table: "TechnologyDefinitions",
                columns: new[] { "Name", "Prefix", "Tag" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTechnologies_TechnologyDefinitions_TechnologyDefinition~",
                table: "UserTechnologies",
                column: "TechnologyDefinitionId",
                principalTable: "TechnologyDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTechnologies_TechnologyDefinitions_TechnologyDefinition~",
                table: "UserTechnologies");

            migrationBuilder.DropTable(
                name: "IgnoredTechnologies");

            migrationBuilder.DropTable(
                name: "TechnologyDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_UserTechnologies_TechnologyDefinitionId",
                table: "UserTechnologies");

            migrationBuilder.DropIndex(
                name: "IX_TechnologyDependencies_UserId",
                table: "TechnologyDependencies");

            migrationBuilder.DropColumn(
                name: "IsStart",
                table: "UserTechnologies");

            migrationBuilder.DropColumn(
                name: "TechnologyDefinitionId",
                table: "UserTechnologies");
        }
    }
}
