using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddRecaudoTramiteVirtualTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecaudosTramiteVirtual",
                schema: "Transaccional",
                columns: table => new
                {
                    RecaudoTramiteVirtualId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramitePortalVirtualId = table.Column<int>(nullable: false),
                    Estado = table.Column<int>(nullable: false),
                    NombreCompleto = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    TipoIdentificacion = table.Column<int>(nullable: false),
                    NumeroIdenficacion = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Correo = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Observacion = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    CUS = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    FechaPagado = table.Column<DateTime>(nullable: true),
                    ValorPagado = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecaudosTramiteVirtual", x => x.RecaudoTramiteVirtualId);
                    table.ForeignKey(
                        name: "FK_RecaudosTramiteVirtual_TramitesPortalVirtual_TramitePortalVirtualId",
                        column: x => x.TramitePortalVirtualId,
                        principalSchema: "Transaccional",
                        principalTable: "TramitesPortalVirtual",
                        principalColumn: "TramitesPortalVirtualId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecaudosTramiteVirtual_TramitePortalVirtualId",
                schema: "Transaccional",
                table: "RecaudosTramiteVirtual",
                column: "TramitePortalVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecaudosTramiteVirtual",
                schema: "Transaccional");
        }
    }
}
