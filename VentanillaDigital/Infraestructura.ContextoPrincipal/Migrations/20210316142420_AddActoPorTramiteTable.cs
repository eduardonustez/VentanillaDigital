using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddActoPorTramiteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActosPorTramite",
                schema: "Transaccional",
                columns: table => new
                {
                    ActoPorTramiteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramitePortalVirtualId = table.Column<int>(nullable: false),
                    ActoNotarialId = table.Column<int>(nullable: false),
                    Cuandi = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    Orden = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActosPorTramite", x => x.ActoPorTramiteId);
                    table.ForeignKey(
                        name: "FK_ActosPorTramite_ActosNotariales_ActoNotarialId",
                        column: x => x.ActoNotarialId,
                        principalSchema: "Parametricas",
                        principalTable: "ActosNotariales",
                        principalColumn: "ActoNotarialId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActosPorTramite_TramitesPortalVirtual_TramitePortalVirtualId",
                        column: x => x.TramitePortalVirtualId,
                        principalSchema: "Transaccional",
                        principalTable: "TramitesPortalVirtual",
                        principalColumn: "TramitesPortalVirtualId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActosPorTramite_ActoNotarialId",
                schema: "Transaccional",
                table: "ActosPorTramite",
                column: "ActoNotarialId");

            migrationBuilder.CreateIndex(
                name: "IX_ActosPorTramite_ActoPorTramiteId",
                schema: "Transaccional",
                table: "ActosPorTramite",
                column: "ActoPorTramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActosPorTramite_TramitePortalVirtualId",
                schema: "Transaccional",
                table: "ActosPorTramite",
                column: "TramitePortalVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActosPorTramite",
                schema: "Transaccional");
        }
    }
}
