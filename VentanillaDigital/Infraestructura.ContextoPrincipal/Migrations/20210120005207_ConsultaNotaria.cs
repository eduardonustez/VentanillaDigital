using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ConsultaNotaria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioModificacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "UsuarioModificacion",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");
        }
    }
}
