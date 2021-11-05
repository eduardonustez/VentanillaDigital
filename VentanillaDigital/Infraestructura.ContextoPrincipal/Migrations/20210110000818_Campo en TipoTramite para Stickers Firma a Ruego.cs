using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CampoenTipoTramiteparaStickersFirmaaRuego : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposTramites_PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaDosStickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TiposTramites_PlantillasActas_PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaDosStickerId",
                principalSchema: "Parametricas",
                principalTable: "PlantillasActas",
                principalColumn: "PlantillaActaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiposTramites_PlantillasActas_PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropIndex(
                name: "IX_TiposTramites_PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropColumn(
                name: "PlantillaDosStickerId",
                schema: "Parametricas",
                table: "TiposTramites");

           
        }
    }
}
