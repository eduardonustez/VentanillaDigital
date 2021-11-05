using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddIvaToRecaudoTramiteVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "IVA",
                schema: "Transaccional",
                table: "RecaudosTramiteVirtual",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IVA",
                schema: "Transaccional",
                table: "RecaudosTramiteVirtual");
        }
    }
}
