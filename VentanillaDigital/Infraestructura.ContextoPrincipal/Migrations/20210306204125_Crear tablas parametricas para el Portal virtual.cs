using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreartablasparametricasparaelPortalvirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadoTramiteVirtual",
                schema: "Parametricas",
                columns: table => new
                {
                    EstadoTramiteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoTramiteVirtual", x => x.EstadoTramiteID);
                });

            migrationBuilder.CreateTable(
                name: "TipoTramiteVirtual",
                schema: "Parametricas",
                columns: table => new
                {
                    TipoTramiteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoTramiteVirtual", x => x.TipoTramiteID);
                });

            migrationBuilder.CreateTable(
                name: "TramitesPortalVirtual",
                schema: "Transaccional",
                columns: table => new
                {
                    TramitesPortalVirtualId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NotariaId = table.Column<int>(nullable: false),
                    TipoTramiteVirtualId = table.Column<int>(nullable: false),
                    EstadoTramiteVirtualId = table.Column<int>(nullable: false),
                    TipoDocumento = table.Column<int>(nullable: false),
                    NumeroDocumento = table.Column<string>(maxLength: 50, nullable: false),
                    TramiteVirtualID = table.Column<int>(nullable: false),
                    CUANDI = table.Column<string>(maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TramitesPortalVirtual", x => x.TramitesPortalVirtualId);
                    table.ForeignKey(
                        name: "FK_TramitesPortalVirtual_EstadoTramiteVirtual",
                        column: x => x.EstadoTramiteVirtualId,
                        principalSchema: "Parametricas",
                        principalTable: "EstadoTramiteVirtual",
                        principalColumn: "EstadoTramiteID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TramitesPortalVirtual_TipoTramiteVirtual",
                        column: x => x.TipoTramiteVirtualId,
                        principalSchema: "Parametricas",
                        principalTable: "TipoTramiteVirtual",
                        principalColumn: "TipoTramiteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_EstadoTramiteVirtualId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "EstadoTramiteVirtualId");

            migrationBuilder.CreateIndex(
                name: "IX_TramitesPortalVirtual_TipoTramiteVirtualId",
                schema: "Transaccional",
                table: "TramitesPortalVirtual",
                column: "TipoTramiteVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TramitesPortalVirtual",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "EstadoTramiteVirtual",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "TipoTramiteVirtual",
                schema: "Parametricas");
        }
    }
}
