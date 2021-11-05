using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddActoPrincipalFieldToTramitePortalVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "ActoPrincipalId");

            migrationBuilder.AddForeignKey(
                name: "FK_TramitesPortalVirtual_ActosNotariales_ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "ActoPrincipalId",
                principalSchema: "Parametricas",
                principalTable: "ActosNotariales",
                principalColumn: "ActoNotarialId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TramitesPortalVirtual_ActosNotariales_ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.DropIndex(
                name: "IX_TramitesPortalVirtual_ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");

            migrationBuilder.DropColumn(
                name: "ActoPrincipalId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual");
        }
    }
}
