using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióncolumnaDatosAdicionalesenlatablaTramite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 24, 8, 31, 47, 76, DateTimeKind.Local).AddTicks(449),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 23, 11, 25, 54, 339, DateTimeKind.Local).AddTicks(2612));

            migrationBuilder.AddColumn<string>(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "Tramites",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatosAdicionales",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 23, 11, 25, 54, 339, DateTimeKind.Local).AddTicks(2612),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 24, 8, 31, 47, 76, DateTimeKind.Local).AddTicks(449));
        }
    }
}
