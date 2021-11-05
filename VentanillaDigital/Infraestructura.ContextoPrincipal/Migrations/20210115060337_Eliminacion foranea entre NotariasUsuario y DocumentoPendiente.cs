using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class EliminacionforaneaentreNotariasUsuarioyDocumentoPendiente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentosPendienteAutorizar_NotariasUsuario_NotarioUsuarioId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");

            migrationBuilder.DropIndex(
                name: "IX_DocumentosPendienteAutorizar_NotarioUsuarioId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPendienteAutorizar_NotarioUsuarioId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                column: "NotarioUsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentosPendienteAutorizar_NotariasUsuario_NotarioUsuarioId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                column: "NotarioUsuarioId",
                principalSchema: "Parametricas",
                principalTable: "NotariasUsuario",
                principalColumn: "NotariaUsuariosId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
