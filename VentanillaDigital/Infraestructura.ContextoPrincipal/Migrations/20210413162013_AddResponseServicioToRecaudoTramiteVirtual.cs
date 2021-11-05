using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddResponseServicioToRecaudoTramiteVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RespuestaServicio",
                schema: "Transaccional",
                table: "RecaudosTramiteVirtual",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespuestaServicio",
                schema: "Transaccional",
                table: "RecaudosTramiteVirtual");
        }
    }
}
