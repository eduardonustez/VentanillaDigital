using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class EliminacionForaneatramitecompareciente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comparecientes_Tramites",
                schema: "Transaccional",
                table: "Comparecientes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Comparecientes_Tramites",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "TramiteId",
                principalSchema: "Transaccional",
                principalTable: "Tramites",
                principalColumn: "TramiteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
