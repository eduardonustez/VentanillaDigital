using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióncolumnaTramiteVirtualGuidenlatablaTramitesPortalVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TramiteVirtualGuid",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TramiteVirtualGuid",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");
        }
    }
}
