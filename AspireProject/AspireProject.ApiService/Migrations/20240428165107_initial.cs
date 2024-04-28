using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireProject.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    ExternalId = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            // Insert seed data
            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Title", "ExternalId" },
                values: new object[,]
                {
                    { Guid.NewGuid(), "Buy groceries", "1" },
                    { Guid.NewGuid(), "Walk the dog", "2" },
                    { Guid.NewGuid(), "Finish work project", "3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
