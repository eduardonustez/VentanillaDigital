using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CampoNotariaSegura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "idTramite",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<long>(
                name: "idTramite",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
