using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperGoals.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToBigInteger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<BigInteger>(
                name: "UserId",
                table: "UserTechnologies",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<BigInteger>(
                name: "Id",
                table: "Users",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<BigInteger>(
                name: "UserId",
                table: "UserProfiles",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<BigInteger>(
                name: "UserId",
                table: "TechnologyDependencies",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<BigInteger>(
                name: "UserId",
                table: "IgnoredTechnologies",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "UserTechnologies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(BigInteger),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "Users",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(BigInteger),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "UserProfiles",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(BigInteger),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "TechnologyDependencies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(BigInteger),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "IgnoredTechnologies",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(BigInteger),
                oldType: "numeric");
        }
    }
}
