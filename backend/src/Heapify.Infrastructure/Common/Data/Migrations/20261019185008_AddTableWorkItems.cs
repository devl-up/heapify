using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Heapify.Infrastructure.Common.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableWorkItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "next_number",
                table: "project",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateTable(
                name: "work_item",
                columns: table => new
                {
                    project_id = table.Column<Guid>(type: "uuid", nullable: false),
                    number = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_work_item", x => new { x.project_id, x.number });
                    table.ForeignKey(
                        name: "fk_work_item_project_project_id",
                        column: x => x.project_id,
                        principalTable: "project",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_item");

            migrationBuilder.DropColumn(
                name: "next_number",
                table: "project");
        }
    }
}
