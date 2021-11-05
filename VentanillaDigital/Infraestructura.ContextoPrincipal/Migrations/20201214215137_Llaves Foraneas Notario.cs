using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class LlavesForaneasNotario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Notarias_Notarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "NotarioId",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 14, 16, 51, 37, 135, DateTimeKind.Local).AddTicks(204),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 14, 16, 15, 56, 554, DateTimeKind.Local).AddTicks(6622));

            migrationBuilder.CreateIndex(
                name: "IX_Notarios_NotariaUsuariosId",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotariaUsuariosId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotariaUsuariosId",
                principalSchema: "Parametricas",
                principalTable: "NotariasUsuario",
                principalColumn: "NotariaUsuariosId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropIndex(
                name: "IX_Notarios_NotariaUsuariosId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 14, 16, 15, 56, 554, DateTimeKind.Local).AddTicks(6622),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 14, 16, 51, 37, 135, DateTimeKind.Local).AddTicks(204));

            migrationBuilder.AddColumn<long>(
                name: "NotarioId",
                schema: "Parametricas",
                table: "Notarias",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "NotariaUsuariosId",
                principalSchema: "Parametricas",
                principalTable: "Notarios",
                principalColumn: "NotarioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarias_Notarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotarioId",
                principalSchema: "Parametricas",
                principalTable: "Notarias",
                principalColumn: "NotariaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
