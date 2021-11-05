using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class RelaciónTiposIdentificacionconPersonas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_TipoDocumentoId_NumeroDocumento",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 25, 10, 24, 42, 175, DateTimeKind.Local).AddTicks(6162),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 14, 55, 53, 303, DateTimeKind.Local).AddTicks(4334));

            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificacionId",
                schema: "Parametricas",
                table: "Personas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoIdentificacionId_NumeroDocumento",
                schema: "Parametricas",
                table: "Personas",
                columns: new[] { "TipoIdentificacionId", "NumeroDocumento" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TipoIdentificacion_Personas",
                schema: "Parametricas",
                table: "Personas",
                column: "TipoIdentificacionId",
                principalSchema: "Parametricas",
                principalTable: "TiposIdentificacion",
                principalColumn: "TipoIdentificacionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TipoIdentificacion_Personas",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_TipoIdentificacionId_NumeroDocumento",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "TipoIdentificacionId",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 14, 55, 53, 303, DateTimeKind.Local).AddTicks(4334),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 25, 10, 24, 42, 175, DateTimeKind.Local).AddTicks(6162));

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentoId",
                schema: "Parametricas",
                table: "Personas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoDocumentoId_NumeroDocumento",
                schema: "Parametricas",
                table: "Personas",
                columns: new[] { "TipoDocumentoId", "NumeroDocumento" },
                unique: true);
        }
    }
}
