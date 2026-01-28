using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeveloperGoals.Migrations
{
    /// <inheritdoc />
    public partial class RenameTechnologyToUserTechnology : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_Technologies_FromTechnologyId",
                table: "TechnologyDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_Technologies_ToTechnologyId",
                table: "TechnologyDependencies");

            migrationBuilder.RenameTable(
                name: "Technologies",
                newName: "UserTechnologies");

            migrationBuilder.RenameIndex(
                name: "IX_Technologies_UserId_Name",
                table: "UserTechnologies",
                newName: "IX_UserTechnologies_UserId_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_UserTechnologies_FromTechnologyId",
                table: "TechnologyDependencies",
                column: "FromTechnologyId",
                principalTable: "UserTechnologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_UserTechnologies_ToTechnologyId",
                table: "TechnologyDependencies",
                column: "ToTechnologyId",
                principalTable: "UserTechnologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_UserTechnologies_FromTechnologyId",
                table: "TechnologyDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologyDependencies_UserTechnologies_ToTechnologyId",
                table: "TechnologyDependencies");

            migrationBuilder.RenameTable(
                name: "UserTechnologies",
                newName: "Technologies");

            migrationBuilder.RenameIndex(
                name: "IX_UserTechnologies_UserId_Name",
                table: "Technologies",
                newName: "IX_Technologies_UserId_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_Technologies_FromTechnologyId",
                table: "TechnologyDependencies",
                column: "FromTechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologyDependencies_Technologies_ToTechnologyId",
                table: "TechnologyDependencies",
                column: "ToTechnologyId",
                principalTable: "Technologies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
