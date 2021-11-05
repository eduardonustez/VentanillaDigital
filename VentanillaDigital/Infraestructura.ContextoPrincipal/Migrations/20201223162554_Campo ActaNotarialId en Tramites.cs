using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CampoActaNotarialIdenTramites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 23, 11, 25, 54, 339, DateTimeKind.Local).AddTicks(2612),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 15, 9, 29, 25, 754, DateTimeKind.Local).AddTicks(4599));

            migrationBuilder.AddColumn<long>(
                name: "ActaNotarialId",
                schema: "Transaccional",
                table: "Tramites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_ActaNotarialId",
                schema: "Transaccional",
                table: "Tramites",
                column: "ActaNotarialId",
                unique: true,
                filter: "[ActaNotarialId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Tramites_MetadataActaNotarial",
                schema: "Transaccional",
                table: "Tramites",
                column: "ActaNotarialId",
                principalSchema: "Transaccional",
                principalTable: "MetadataArchivos",
                principalColumn: "MetadataArchivoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tramites_MetadataActaNotarial",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropIndex(
                name: "IX_Tramites_ActaNotarialId",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropColumn(
                name: "ActaNotarialId",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 9, 29, 25, 754, DateTimeKind.Local).AddTicks(4599),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 23, 11, 25, 54, 339, DateTimeKind.Local).AddTicks(2612));
        }
    }
}
