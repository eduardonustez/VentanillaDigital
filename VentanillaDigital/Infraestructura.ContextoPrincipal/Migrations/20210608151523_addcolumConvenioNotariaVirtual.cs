using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class addcolumConvenioNotariaVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfigurationGuid",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoginConvenio",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordConvenio",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual",
                unicode: false,
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigurationGuid",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "LoginConvenio",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");

            migrationBuilder.DropColumn(
                name: "PasswordConvenio",
                schema: "Parametricas",
                table: "ConvenioNotariaVirtual");
        }
    }
}
