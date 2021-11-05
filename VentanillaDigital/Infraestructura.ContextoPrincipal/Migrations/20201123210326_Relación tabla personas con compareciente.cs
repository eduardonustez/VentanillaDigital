using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class Relacióntablapersonasconcompareciente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 16, 3, 26, 413, DateTimeKind.Local).AddTicks(5817),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 23, 15, 40, 23, 213, DateTimeKind.Local).AddTicks(3637));

            migrationBuilder.AddColumn<long>(
                name: "PersonaId",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Comparecientes_PersonaId",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Comparecientes",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "PersonaId",
                principalSchema: "Parametricas",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Comparecientes",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropIndex(
                name: "IX_Comparecientes_PersonaId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "PersonaId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 15, 40, 23, 213, DateTimeKind.Local).AddTicks(3637),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 23, 16, 3, 26, 413, DateTimeKind.Local).AddTicks(5817));
        }
    }
}
