using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddSNRFieldsToConvenioNotariaVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKeySNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiUserSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AutorizacionAutenticacionSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdNotariaSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKeySNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "ApiUserSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "AutorizacionAutenticacionSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "IdNotariaSNR",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");
        }
    }
}
