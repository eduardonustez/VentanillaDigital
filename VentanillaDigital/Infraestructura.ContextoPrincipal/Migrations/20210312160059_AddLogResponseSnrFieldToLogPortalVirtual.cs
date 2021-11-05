using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AddLogResponseSnrFieldToLogPortalVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogResponseSNR",
                schema: "Log",
                table: "LogTramitePortalVirtual",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogResponseSNR",
                schema: "Log",
                table: "LogTramitePortalVirtual");
        }
    }
}
