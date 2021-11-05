using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class updateCampoNotariaSegura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "idTramite",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idTramite",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");
        }
    }
}
