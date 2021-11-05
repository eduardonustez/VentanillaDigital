using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddActosNotarialesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActosNotariales",
                schema: "Parametricas",
                columns: table => new
                {
                    ActoNotarialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Codigo = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    Nombre = table.Column<string>(unicode: false, maxLength: 512, nullable: false),
                    TipoTramiteVirtualId = table.Column<int>(nullable: true),
                    Activo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActosNotariales", x => x.ActoNotarialId);
                    table.ForeignKey(
                        name: "FK_ActosNotariales_TipoTramiteVirtual_TipoTramiteVirtualId",
                        column: x => x.TipoTramiteVirtualId,
                        principalSchema: "Parametricas",
                        principalTable: "TipoTramiteVirtual",
                        principalColumn: "TipoTramiteID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActosNotariales_TipoTramiteVirtualId",
                schema: "Parametricas",
                table: "ActosNotariales",
                column: "TipoTramiteVirtualId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActosNotariales",
                schema: "Parametricas");
        }
    }
}
