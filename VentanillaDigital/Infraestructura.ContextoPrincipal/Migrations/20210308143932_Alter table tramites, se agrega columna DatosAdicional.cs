using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AltertabletramitesseagregacolumnaDatosAdicional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TramitesPortalVirtual_TipoTramiteVirtual",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.AddColumn<string>(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                maxLength: 1500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_TramitesPortalVirtual_TipoTramiteVirtual_TipoTramiteVirtualId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "TipoTramiteVirtualId",
                principalSchema: "Parametricas",
                principalTable: "TipoTramiteVirtual",
                principalColumn: "TipoTramiteID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TramitesPortalVirtual_TipoTramiteVirtual_TipoTramiteVirtualId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.DropColumn(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.AddForeignKey(
                name: "FK_TramitesPortalVirtual_TipoTramiteVirtual",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "TipoTramiteVirtualId",
                principalSchema: "Parametricas",
                principalTable: "TipoTramiteVirtual",
                principalColumn: "TipoTramiteID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
