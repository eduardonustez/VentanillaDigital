using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddUrlMiFirmaToConvenioNotaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialCertificadoNotarioEncargado",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlApiMiFirma",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlSubirDocumentosMiFirma",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialCertificadoNotarioEncargado",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "UrlApiMiFirma",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "UrlSubirDocumentosMiFirma",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");
        }
    }
}
