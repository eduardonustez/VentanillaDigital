using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class Adicionartablausuariotokenportaladmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioTokenPortalAdministrador",
                schema: "Parametricas",
                columns: table => new
                {
                    UsuarioTokenPortalAdministradorId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UsuarioAdministracionId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 150, nullable: false),
                    Token = table.Column<string>(maxLength: 3000, nullable: false),
                    Nombre = table.Column<int>(maxLength: 40, nullable: false),
                    FechaExpiracion = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioTokenPortalAdministrador", x => x.UsuarioTokenPortalAdministradorId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTokenPortalAdministrador_UsuarioAdministracionId_LoginProvider_Nombre",
                schema: "Parametricas",
                table: "UsuarioTokenPortalAdministrador",
                columns: new[] { "UsuarioAdministracionId", "LoginProvider", "Nombre" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioTokenPortalAdministrador",
                schema: "Parametricas");
        }
    }
}
