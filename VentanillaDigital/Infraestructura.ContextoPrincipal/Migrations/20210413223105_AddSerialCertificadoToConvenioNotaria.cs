using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddSerialCertificadoToConvenioNotaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialCertificado",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialCertificado",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");
        }
    }
}
