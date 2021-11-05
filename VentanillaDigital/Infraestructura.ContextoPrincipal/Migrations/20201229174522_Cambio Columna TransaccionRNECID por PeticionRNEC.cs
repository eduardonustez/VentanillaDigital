using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CambioColumnaTransaccionRNECIDporPeticionRNEC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransaccionRNECId",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.AddColumn<string>(
                name: "PeticionRNEC",
                schema: "Transaccional",
                table: "Comparecientes",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeticionRNEC",
                schema: "Transaccional",
                table: "Comparecientes");

            migrationBuilder.AddColumn<long>(
                name: "TransaccionRNECId",
                schema: "Transaccional",
                table: "Comparecientes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
