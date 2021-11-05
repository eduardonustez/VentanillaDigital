using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ColumnasHitDedo1y2yVigenciaentablaComparecientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HitDedo1",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HitDedo2",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vigencia",
                schema: "Transaccional",
                table: "Comparecientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HitDedo1",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "HitDedo2",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.DropColumn(
                name: "Vigencia",
                schema: "Transaccional",
                table: "Comparecientes");
        }
    }
}
