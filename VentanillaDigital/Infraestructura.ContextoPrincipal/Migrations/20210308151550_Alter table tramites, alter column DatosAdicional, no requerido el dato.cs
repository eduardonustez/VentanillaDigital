using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AltertabletramitesaltercolumnDatosAdicionalnorequeridoeldato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AddColumn<int>(
                name: "TramitesPortalVirtualId1",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1500,
                oldNullable: true);
        }
    }
}
