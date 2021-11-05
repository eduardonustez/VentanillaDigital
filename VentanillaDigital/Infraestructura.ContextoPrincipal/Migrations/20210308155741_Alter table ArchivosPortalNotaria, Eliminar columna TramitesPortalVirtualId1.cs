using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AltertableArchivosPortalNotariaEliminarcolumnaTramitesPortalVirtualId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivosPortalVirtual_TramitesPortalVirtual_TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual");

            migrationBuilder.DropIndex(
                name: "IX_ArchivosPortalVirtual_TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual");

            migrationBuilder.DropColumn(
                name: "TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosPortalVirtual_TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                column: "TramitesPortalVirtualId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivosPortalVirtual_TramitesPortalVirtual_TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                column: "TramitesPortalVirtualId1",
                principalSchema: "Transaccional",
                principalTable: "TramitesPortalVirtual",
                principalColumn: "TramitesPortalVirtualId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
