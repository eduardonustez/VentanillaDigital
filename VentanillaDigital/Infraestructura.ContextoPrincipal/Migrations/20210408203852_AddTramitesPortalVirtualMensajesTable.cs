using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddTramitesPortalVirtualMensajesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TramitePortalVirtualMensajes",
                schema: "Transaccional",
                columns: table => new
                {
                    TramitePortalVirtualMensajeId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramitePortalVirtualId = table.Column<int>(nullable: false),
                    Mensaje = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    EsNotario = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TramitePortalVirtualMensajes", x => x.TramitePortalVirtualMensajeId);
                    table.ForeignKey(
                        name: "FK_TramitePortalVirtualMensajes_TramitesPortalVirtual_TramitePortalVirtualId",
                        column: x => x.TramitePortalVirtualId,
                        principalSchema: "Transaccional",
                        principalTable: "TramitesPortalVirtual",
                        principalColumn: "TramitesPortalVirtualId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TramitePortalVirtualMensajes_TramitePortalVirtualId",
                schema: "Transaccional",
                table: "TramitePortalVirtualMensajes",
                column: "TramitePortalVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TramitePortalVirtualMensajes",
                schema: "Transaccional");
        }
    }
}
