using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class Relacióntablapersonasconcomparecientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 19, 52, 15, 524, DateTimeKind.Local).AddTicks(5978),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 23, 16, 3, 26, 413, DateTimeKind.Local).AddTicks(5817));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 16, 3, 26, 413, DateTimeKind.Local).AddTicks(5817),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 23, 19, 52, 15, 524, DateTimeKind.Local).AddTicks(5978));
        }
    }
}
