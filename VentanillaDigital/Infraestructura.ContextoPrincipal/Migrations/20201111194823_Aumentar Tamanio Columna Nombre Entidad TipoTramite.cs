using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AumentarTamanioColumnaNombreEntidadTipoTramite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 11, 14, 48, 23, 309, DateTimeKind.Local).AddTicks(7454),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 6, 11, 48, 5, 219, DateTimeKind.Local).AddTicks(8306));

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "Parametricas",
                table: "TiposTramites",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 6, 11, 48, 5, 219, DateTimeKind.Local).AddTicks(8306),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 11, 14, 48, 23, 309, DateTimeKind.Local).AddTicks(7454));

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                schema: "Parametricas",
                table: "TiposTramites",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);
        }
    }
}
