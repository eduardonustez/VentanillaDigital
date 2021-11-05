using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddLogTramitePortalVirtualTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Log");

            migrationBuilder.CreateTable(
                name: "LogTramitePortalVirtual",
                schema: "Log",
                columns: table => new
                {
                    LogTramiteVirtualPortalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramitePortalVirtualId = table.Column<int>(nullable: false),
                    ClaveTestamentoCerrado = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    EnvioSNR = table.Column<bool>(nullable: false),
                    Lat = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(9,6)", nullable: false),
                    EstadoTramiteVirtualId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTramitePortalVirtual", x => x.LogTramiteVirtualPortalId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogTramitePortalVirtual",
                schema: "Log");
        }
    }
}
