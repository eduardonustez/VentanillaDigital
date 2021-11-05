using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposCertificadoNotario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Certificadoid",
                schema: "Parametricas",
                table: "Notarios",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioCertificado",
                schema: "Parametricas",
                table: "Notarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certificadoid",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "UsuarioCertificado",
                schema: "Parametricas",
                table: "Notarios");
        }
    }
}
