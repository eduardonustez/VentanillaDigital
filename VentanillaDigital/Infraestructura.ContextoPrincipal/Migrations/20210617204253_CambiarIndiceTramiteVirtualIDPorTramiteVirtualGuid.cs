using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CambiarIndiceTramiteVirtualIDPorTramiteVirtualGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualID_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualGuid_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                columns: new[] { "TramiteVirtualGuid", "NotariaId" },
                unique: true,
                filter: "[TramiteVirtualGuid] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualGuid_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_TramiteVirtualID_NotariaId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                columns: new[] { "TramiteVirtualID", "NotariaId" },
                unique: true);
        }
    }
}
