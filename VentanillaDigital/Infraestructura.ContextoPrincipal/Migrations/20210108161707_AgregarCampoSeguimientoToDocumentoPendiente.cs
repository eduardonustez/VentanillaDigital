using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AgregarCampoSeguimientoToDocumentoPendiente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seguimiento",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seguimiento",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");
        }
    }
}
