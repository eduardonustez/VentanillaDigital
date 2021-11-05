using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AjustesUsarSticker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UsarSticker",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UsarSticker",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsarSticker",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropColumn(
                name: "UsarSticker",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");
        }
    }
}
