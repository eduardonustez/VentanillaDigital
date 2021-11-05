using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class RestrictFKNotariosGrafosNotarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "GrafoId",
                principalSchema: "Parametricas.Archivos",
                principalTable: "GrafosNotarios",
                principalColumn: "GrafoNotarioId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "GrafoId",
                principalSchema: "Parametricas.Archivos",
                principalTable: "GrafosNotarios",
                principalColumn: "GrafoNotarioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
