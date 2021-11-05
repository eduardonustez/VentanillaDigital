using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposComparecientessinbiometria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoSinBiometria",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TramiteSinBiometria",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoSinBiometria",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "TramiteSinBiometria",
                schema: "Transaccional",
                table: "Comparecientes");
        }
    }
}
