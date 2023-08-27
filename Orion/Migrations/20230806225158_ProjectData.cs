using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orion.Migrations
{
    /// <inheritdoc />
    public partial class ProjectData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectDueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectStatus = table.Column<bool>(type: "bit", nullable: false),
                    ProjectCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_Id",
                table: "AdminUsers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectData_Id",
                table: "ProjectData",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectData");

            migrationBuilder.DropIndex(
                name: "IX_AdminUsers_Id",
                table: "AdminUsers");
        }
    }
}
