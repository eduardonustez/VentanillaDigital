using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ActualizartablaCompareciente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroDocumento",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 12, 45, 46, 802, DateTimeKind.Local).AddTicks(8649),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 58, 54, 635, DateTimeKind.Local).AddTicks(3603));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 58, 54, 635, DateTimeKind.Local).AddTicks(3603),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 24, 12, 45, 46, 802, DateTimeKind.Local).AddTicks(8649));

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumento",
                schema: "Transaccional",
                table: "Comparecientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
