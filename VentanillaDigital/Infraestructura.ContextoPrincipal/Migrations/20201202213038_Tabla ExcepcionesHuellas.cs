using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class TablaExcepcionesHuellas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 2, 16, 30, 37, 559, DateTimeKind.Local).AddTicks(2765),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 30, 12, 47, 30, 738, DateTimeKind.Local).AddTicks(3732));

            migrationBuilder.CreateTable(
                name: "ExcepcionesHuellas",
                schema: "Transaccional",
                columns: table => new
                {
                    ExcepcionHuellaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Descripcion = table.Column<string>(nullable: false),
                    DedosExceptuados = table.Column<int>(nullable: false),
                    ComparecienteId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcepcionesHuellas", x => x.ExcepcionHuellaId);
                    table.ForeignKey(
                        name: "FK_ExcepcionesHuellas_Comparecientes",
                        column: x => x.ComparecienteId,
                        principalSchema: "Transaccional",
                        principalTable: "Comparecientes",
                        principalColumn: "ComparecienteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcepcionesHuellas_ComparecienteId",
                schema: "Transaccional",
                table: "ExcepcionesHuellas",
                column: "ComparecienteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcepcionesHuellas",
                schema: "Transaccional");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 30, 12, 47, 30, 738, DateTimeKind.Local).AddTicks(3732),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 2, 16, 30, 37, 559, DateTimeKind.Local).AddTicks(2765));
        }
    }
}
