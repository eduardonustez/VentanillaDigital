using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class TablaTiposIdentificacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 5, 17, 6, 22, 662, DateTimeKind.Local).AddTicks(3227),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 5, 16, 57, 48, 108, DateTimeKind.Local).AddTicks(27));

            migrationBuilder.CreateTable(
                name: "TiposIdentificacion",
                schema: "Parametricas",
                columns: table => new
                {
                    TipoIdentificacionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Abreviatura = table.Column<string>(maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposIdentificacion", x => x.TipoIdentificacionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TiposIdentificacion",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 5, 16, 57, 48, 108, DateTimeKind.Local).AddTicks(27),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 5, 17, 6, 22, 662, DateTimeKind.Local).AddTicks(3227));
        }
    }
}
