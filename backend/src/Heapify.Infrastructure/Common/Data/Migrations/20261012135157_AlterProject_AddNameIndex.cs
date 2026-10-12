using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Heapify.Infrastructure.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterProject_AddNameIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_project_name",
                table: "project",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_project_name",
                table: "project");
        }
    }
}
