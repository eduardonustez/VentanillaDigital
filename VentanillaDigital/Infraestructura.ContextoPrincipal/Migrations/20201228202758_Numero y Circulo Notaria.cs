using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class NumeroyCirculoNotaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CirculoNotaria",
                schema: "Parametricas",
                table: "Notarias",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumeroNotaria",
                schema: "Parametricas",
                table: "Notarias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NumeroNotariaEnLetras",
                schema: "Parametricas",
                table: "Notarias",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CirculoNotaria",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.DropColumn(
                name: "NumeroNotaria",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.DropColumn(
                name: "NumeroNotariaEnLetras",
                schema: "Parametricas",
                table: "Notarias");
        }
    }
}
