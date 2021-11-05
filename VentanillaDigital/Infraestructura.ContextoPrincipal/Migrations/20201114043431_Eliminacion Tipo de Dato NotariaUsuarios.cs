using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class EliminacionTipodeDatoNotariaUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 13, 23, 34, 30, 763, DateTimeKind.Local).AddTicks(3168),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 13, 10, 57, 7, 967, DateTimeKind.Local).AddTicks(1962));

            migrationBuilder.AlterColumn<string>(
                name: "UsuariosId",
                schema: "Parametricas",
                table: "NotariasUsuario",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 13, 10, 57, 7, 967, DateTimeKind.Local).AddTicks(1962),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 13, 23, 34, 30, 763, DateTimeKind.Local).AddTicks(3168));

            migrationBuilder.AlterColumn<long>(
                name: "UsuariosId",
                schema: "Parametricas",
                table: "NotariasUsuario",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
