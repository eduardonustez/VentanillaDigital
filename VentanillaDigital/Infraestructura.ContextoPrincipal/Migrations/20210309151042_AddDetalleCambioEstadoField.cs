using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddDetalleCambioEstadoField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetalleCambioEstado",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                unicode: false,
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DetalleCambioEstado",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");
        }
    }
}
