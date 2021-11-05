using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class Camposnonecesarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grafo",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "UsarSticker",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "Sello",
                schema: "Parametricas",
                table: "Notarias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grafo",
                schema: "Parametricas",
                table: "Notarios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsarSticker",
                schema: "Parametricas",
                table: "Notarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Sello",
                schema: "Parametricas",
                table: "Notarias",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
