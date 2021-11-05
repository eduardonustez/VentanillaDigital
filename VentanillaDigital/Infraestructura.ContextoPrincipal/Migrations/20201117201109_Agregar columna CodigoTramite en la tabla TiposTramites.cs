using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AgregarcolumnaCodigoTramiteenlatablaTiposTramites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 17, 15, 11, 9, 103, DateTimeKind.Local).AddTicks(1797),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 13, 23, 34, 30, 763, DateTimeKind.Local).AddTicks(3168));

            migrationBuilder.AddColumn<long>(
                name: "CodigoTramite",
                schema: "Parametricas",
                table: "TiposTramites",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoTramite",
                schema: "Parametricas",
                table: "TiposTramites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 13, 23, 34, 30, 763, DateTimeKind.Local).AddTicks(3168),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 17, 15, 11, 9, 103, DateTimeKind.Local).AddTicks(1797));
        }
    }
}
