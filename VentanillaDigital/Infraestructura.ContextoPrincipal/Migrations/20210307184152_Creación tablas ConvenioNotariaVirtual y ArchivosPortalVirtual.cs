using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióntablasConvenioNotariaVirtualyArchivosPortalVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConvenioNotariaVirtual",
                schema: "Parametricas",
                columns: table => new
                {
                    ConvenioNotariaVirtualId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvenioNotariaVirtual", x => x.ConvenioNotariaVirtualId);
                    table.ForeignKey(
                        name: "FK_ConvenioNotariaVirtual_Notarias_NotariaId",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArchivosPortalVirtual",
                schema: "Transaccional",
                columns: table => new
                {
                    ArchivosPortalVirtualId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramitesPortalVirtualId = table.Column<int>(nullable: false),
                    TipoArchivo = table.Column<int>(nullable: false),
                    Formato = table.Column<string>(maxLength: 20, nullable: false),
                    Base64 = table.Column<string>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivosPortalVirtual", x => x.ArchivosPortalVirtualId);
                    table.ForeignKey(
                        name: "FK_ArchivosPortalVirtual_TramitesPortalVirtual",
                        column: x => x.TramitesPortalVirtualId,
                        principalSchema: "Transaccional",
                        principalTable: "TramitesPortalVirtual",
                        principalColumn: "TramitesPortalVirtualId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvenioNotariaVirtual_NotariaId",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_ArchivosPortalVirtual_TramitesPortalVirtualId",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                column: "TramitesPortalVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvenioNotariaVirtual",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "ArchivosPortalVirtual",
                schema: "Transaccional");
        }
    }
}
