using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CambioesquemaporParametricas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SellosNotarias",
                schema: "Parametricas.Archivos",
                newName: "SellosNotarias",
                newSchema: "Parametricas");

            migrationBuilder.RenameTable(
                name: "GrafosNotarios",
                schema: "Parametricas.Archivos",
                newName: "GrafosNotarios",
                newSchema: "Parametricas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Parametricas.Archivos");

            migrationBuilder.RenameTable(
                name: "SellosNotarias",
                schema: "Parametricas",
                newName: "SellosNotarias",
                newSchema: "Parametricas.Archivos");

            migrationBuilder.RenameTable(
                name: "GrafosNotarios",
                schema: "Parametricas",
                newName: "GrafosNotarios",
                newSchema: "Parametricas.Archivos");
        }
    }
}
