using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacionTablaNotariaUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 13, 10, 57, 7, 967, DateTimeKind.Local).AddTicks(1962),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 11, 14, 48, 23, 309, DateTimeKind.Local).AddTicks(7454));

            migrationBuilder.CreateTable(
                name: "NotariasUsuario",
                schema: "Parametricas",
                columns: table => new
                {
                    NotariaUsuariosId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false),
                    UsuariosId = table.Column<long>(nullable: false),
                    UserEmail = table.Column<string>(maxLength: 34, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotariasUsuario", x => x.NotariaUsuariosId);
                    table.ForeignKey(
                        name: "FK_Notarias_Usuarios",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotariasUsuario_NotariaId",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "NotariaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotariasUsuario",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 11, 14, 48, 23, 309, DateTimeKind.Local).AddTicks(7454),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 13, 10, 57, 7, 967, DateTimeKind.Local).AddTicks(1962));
        }
    }
}
