using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class FixConsultaNotariaSegura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TramiteIdHash",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura");

            migrationBuilder.RenameColumn(
                name: "idTramite",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                newName: "TramiteIdHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TramiteIdHash",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                newName: "idTramite");

            migrationBuilder.AddColumn<string>(
                name: "TramiteIdHash",
                schema: "Parametricas",
                table: "ConsultaNotariaSegura",
                nullable: false,
                defaultValue: "");
        }
    }
}
