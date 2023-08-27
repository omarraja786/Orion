using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orion.Migrations
{
    /// <inheritdoc />
    public partial class ProjectTaskCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectTasksCompleted",
                table: "ProjectData",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalProjectTasks",
                table: "ProjectData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectTasksCompleted",
                table: "ProjectData");

            migrationBuilder.DropColumn(
                name: "TotalProjectTasks",
                table: "ProjectData");
        }
    }
}
