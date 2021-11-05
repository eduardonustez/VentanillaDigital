using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddDatosAdicionalesCierreToTramiteVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatosAdicionalesCierre",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatosAdicionalesCierre",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");
        }
    }
}
