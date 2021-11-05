using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposTramiteFueraDeDespacho : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DireccionComparecencia",
                schema: "Transaccional",
                table: "Tramites",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FueraDeDespacho",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DireccionComparecencia",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropColumn(
                name: "FueraDeDespacho",
                schema: "Transaccional",
                table: "Tramites");
        }
    }
}
