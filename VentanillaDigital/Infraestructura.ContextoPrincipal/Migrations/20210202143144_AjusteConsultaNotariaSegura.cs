using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AjusteConsultaNotariaSegura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultaNotariaSegura",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "ID",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "Encontrado",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "Fecha",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "numeroDocumento",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.AddColumn<long>(
                name: "ConsultaNotariaSeguraId",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultaNotariaSegura",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                column: "ConsultaNotariaSeguraId");

            migrationBuilder.AddColumn<string>(
                name: "TramiteId",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TramiteIdHash",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: "");

            migrationBuilder.RenameColumn(
                name: "numeroNotaria",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                newName: "NotariaId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaConsulta",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "EncontroArchivo",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultaNotariaSegura",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "ConsultaNotariaSeguraId",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "TramiteId",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "TramiteIdHash",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.RenameColumn(
                name: "NotariaId",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                newName: "numeroNotaria");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "FechaConsulta",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.DropColumn(
                name: "EncontroArchivo",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Encontrado",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Fecha",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "numeroDocumento",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                type: "bigint",
                maxLength: 14,
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultaNotariaSegura",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                column: "ID");
        }
    }
}