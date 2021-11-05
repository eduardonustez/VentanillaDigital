using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ColumnaPosicionComparecientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Posicion",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Posicion",
                schema: "Transaccional",
                table: "Comparecientes");
        }
    }
}
