using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ColumnaNotarioDeTurnoenNotarias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NotarioEnTurno",
                schema: "Parametricas",
                table: "Notarias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotarioEnTurno",
                schema: "Parametricas",
                table: "Notarias");
        }
    }
}
