using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreaciónllavecompuestaentretramitevirtualIdyNotariaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualID_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                columns: new[] { "TramiteVirtualID", "NotariaId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualID_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");
        }
    }
}
