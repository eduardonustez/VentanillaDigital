using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposPlantillaStickers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlantillaStickerId",
                schema: "Parametricas",
                table: "TiposTramites",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsarSticker",
                schema: "Parametricas",
                table: "Notarios",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TiposTramites_PlantillaStickerId",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaStickerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TiposTramites_PlantillasStickers",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaStickerId",
                principalSchema: "Parametricas",
                principalTable: "PlantillasActas",
                principalColumn: "PlantillaActaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiposTramites_PlantillasStickers",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropIndex(
                name: "IX_TiposTramites_PlantillaStickerId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropColumn(
                name: "PlantillaStickerId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropColumn(
                name: "UsarSticker",
                schema: "Parametricas",
                table: "Notarios");
        }
    }
}
