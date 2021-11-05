using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióncolumnaImagenDocumentoenlatablaComparecienteyrelaciónalatablaMetadataArchivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 6, 11, 48, 5, 219, DateTimeKind.Local).AddTicks(8306),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 5, 17, 6, 22, 662, DateTimeKind.Local).AddTicks(3227));

            migrationBuilder.AddColumn<long>(
                name: "ImagenDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Comparecientes_ImagenDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "ImagenDocumentoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImagenesDocumento_Comparecientes",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "ImagenDocumentoId",
                principalSchema: "Transaccional",
                principalTable: "MetadataArchivos",
                principalColumn: "MetadataArchivoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImagenesDocumento_Comparecientes",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropIndex(
                name: "IX_Comparecientes_ImagenDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "ImagenDocumentoId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 5, 17, 6, 22, 662, DateTimeKind.Local).AddTicks(3227),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 6, 11, 48, 5, 219, DateTimeKind.Local).AddTicks(8306));
        }
    }
}
