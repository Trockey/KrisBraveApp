using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperGoals.Migrations
{
    /// <inheritdoc />
    public partial class AddPrefixToUserTechnology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "UserTechnologies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "UserTechnologies");
        }
    }
}
