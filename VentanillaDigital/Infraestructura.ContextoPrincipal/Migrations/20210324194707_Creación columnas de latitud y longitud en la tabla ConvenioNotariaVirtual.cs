using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióncolumnasdelatitudylongitudenlatablaConvenioNotariaVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Latitud1",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                type: "decimal (10,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud2",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                type: "decimal (10,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud1",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                type: "decimal (10,6)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud2",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                type: "decimal (10,6)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud1",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "Latitud2",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "Longitud1",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "Longitud2",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");
        }
    }
}
