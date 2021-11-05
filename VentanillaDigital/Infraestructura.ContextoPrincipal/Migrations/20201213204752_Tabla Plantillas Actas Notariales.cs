using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class TablaPlantillasActasNotariales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 13, 15, 47, 52, 405, DateTimeKind.Local).AddTicks(1908),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 10, 10, 47, 27, 116, DateTimeKind.Local).AddTicks(4457));

            migrationBuilder.AddColumn<long>(
                name: "PlantillaActaId",
                schema: "Parametricas",
                table: "TiposTramites",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PlantillasActas",
                schema: "Parametricas",
                columns: table => new
                {
                    PlantillaActaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Contenido = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantillasActas", x => x.PlantillaActaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TiposTramites_PlantillaActaId",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaActaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TiposTramites_PlantillasActas",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "PlantillaActaId",
                principalSchema: "Parametricas",
                principalTable: "PlantillasActas",
                principalColumn: "PlantillaActaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TiposTramites_PlantillasActas",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropTable(
                name: "PlantillasActas",
                schema: "Parametricas");

            migrationBuilder.DropIndex(
                name: "IX_TiposTramites_PlantillaActaId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.DropColumn(
                name: "PlantillaActaId",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 10, 10, 47, 27, 116, DateTimeKind.Local).AddTicks(4457),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 13, 15, 47, 52, 405, DateTimeKind.Local).AddTicks(1908));
        }
    }
}
