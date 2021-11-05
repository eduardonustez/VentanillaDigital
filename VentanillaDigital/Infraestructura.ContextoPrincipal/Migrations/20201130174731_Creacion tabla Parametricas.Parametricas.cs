using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreaciontablaParametricasParametricas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 30, 12, 47, 30, 738, DateTimeKind.Local).AddTicks(3732),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 27, 17, 43, 11, 736, DateTimeKind.Local).AddTicks(2271));

            migrationBuilder.CreateTable(
                name: "Parametricas",
                schema: "Parametricas",
                columns: table => new
                {
                    ParametricaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Codigo = table.Column<string>(maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 200, nullable: false),
                    Valor = table.Column<string>(maxLength: 1000, nullable: false),
                    Longitud = table.Column<int>(nullable: false),
                    Expresion = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametricas", x => x.ParametricaId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parametricas",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 27, 17, 43, 11, 736, DateTimeKind.Local).AddTicks(2271),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 30, 12, 47, 30, 738, DateTimeKind.Local).AddTicks(3732));
        }
    }
}
