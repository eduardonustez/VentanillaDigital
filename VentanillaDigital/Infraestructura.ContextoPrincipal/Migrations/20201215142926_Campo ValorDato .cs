using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CampoValorDato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 15, 9, 29, 25, 754, DateTimeKind.Local).AddTicks(4599),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 14, 16, 51, 37, 135, DateTimeKind.Local).AddTicks(204));

            migrationBuilder.AddColumn<string>(
                name: "ValorDato",
                schema: "Parametricas",
                table: "PersonasDatos",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorDato",
                schema: "Parametricas",
                table: "PersonasDatos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 14, 16, 51, 37, 135, DateTimeKind.Local).AddTicks(204),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 15, 9, 29, 25, 754, DateTimeKind.Local).AddTicks(4599));
        }
    }
}
