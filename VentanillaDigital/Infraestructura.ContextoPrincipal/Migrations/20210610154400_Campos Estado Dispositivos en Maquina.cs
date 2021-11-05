using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposEstadoDispositivosenMaquina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanalWacom",
                schema: "Parametricas",
                table: "Maquinas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoCaptor",
                schema: "Parametricas",
                table: "Maquinas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoDllWacom",
                schema: "Parametricas",
                table: "Maquinas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstadoWacomSigCaptX",
                schema: "Parametricas",
                table: "Maquinas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanalWacom",
                schema: "Parametricas",
                table: "Maquinas");

            migrationBuilder.DropColumn(
                name: "EstadoCaptor",
                schema: "Parametricas",
                table: "Maquinas");

            migrationBuilder.DropColumn(
                name: "EstadoDllWacom",
                schema: "Parametricas",
                table: "Maquinas");

            migrationBuilder.DropColumn(
                name: "EstadoWacomSigCaptX",
                schema: "Parametricas",
                table: "Maquinas");
        }
    }
}
