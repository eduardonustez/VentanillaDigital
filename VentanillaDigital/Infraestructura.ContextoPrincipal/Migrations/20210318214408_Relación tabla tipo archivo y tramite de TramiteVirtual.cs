using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class RelacióntablatipoarchivoytramitedeTramiteVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ArchivosPortalVirtual_TipoArchivo",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                column: "TipoArchivo");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivosPortalVirtual_TipoArchivoTramiteVirtual",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                column: "TipoArchivo",
                principalSchema: "Parametricas",
                principalTable: "TipoArchivoTramiteVirtual",
                principalColumn: "TipoArchivoTramiteVirtualId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivosPortalVirtual_TipoArchivoTramiteVirtual",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual");

            migrationBuilder.DropIndex(
                name: "IX_ArchivosPortalVirtual_TipoArchivo",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual");
        }
    }
}
