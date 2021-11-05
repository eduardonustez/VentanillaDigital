using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AgregarCamposNuevosDocumentoPendiente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Error",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Estado",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.AddColumn<short>(
                name: "Intentos",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");

            migrationBuilder.DropColumn(
                name: "Estado",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");

            migrationBuilder.DropColumn(
                name: "Intentos",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");
        }
    }
}
