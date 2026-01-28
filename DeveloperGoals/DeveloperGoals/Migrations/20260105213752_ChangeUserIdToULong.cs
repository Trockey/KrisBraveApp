using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeveloperGoals.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToULong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Usuń wszystkie klucze obce związane z UserId przed zmianą typu
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Users_UserId",
                table: "UserTechnologies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_Users_UserId",
                table: "TechnologyDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_IgnoredTechnologies_Users_UserId",
                table: "IgnoredTechnologies");

            // Usuń identity z kolumny Id przed zmianą typu (PostgreSQL nie obsługuje identity dla numeric)
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Id\" DROP IDENTITY IF EXISTS;");
            
            // Zmień typ kolumny Id w tabeli Users (główna tabela)
            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Users",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            // Zmień typ kolumny UserId we wszystkich tabelach zależnych
            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "UserTechnologies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "UserProfiles",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "TechnologyDependencies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "IgnoredTechnologies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            // Przywróć wszystkie klucze obce
            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Users_UserId",
                table: "UserTechnologies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_Users_UserId",
                table: "TechnologyDependencies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IgnoredTechnologies_Users_UserId",
                table: "IgnoredTechnologies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usuń wszystkie klucze obce przed zmianą typu
            migrationBuilder.DropForeignKey(
                name: "FK_Technologies_Users_UserId",
                table: "UserTechnologies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_Users_UserId",
                table: "TechnologyDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_IgnoredTechnologies_Users_UserId",
                table: "IgnoredTechnologies");

            // Zmień typ kolumny UserId we wszystkich tabelach zależnych z powrotem na bigint
            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserTechnologies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "UserProfiles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "TechnologyDependencies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "IgnoredTechnologies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            // Zmień typ kolumny Id w tabeli Users z powrotem na bigint
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");
            
            // Przywróć identity dla kolumny Id
            migrationBuilder.Sql("ALTER TABLE \"Users\" ALTER COLUMN \"Id\" ADD GENERATED BY DEFAULT AS IDENTITY;");

            // Przywróć wszystkie klucze obce
            migrationBuilder.AddForeignKey(
                name: "FK_Technologies_Users_UserId",
                table: "UserTechnologies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_UserId",
                table: "UserProfiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_Users_UserId",
                table: "TechnologyDependencies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IgnoredTechnologies_Users_UserId",
                table: "IgnoredTechnologies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
