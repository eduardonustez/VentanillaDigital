using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiciosDistribuidos.ContextoPrincipal.Migrations
{
    public partial class campoisdiasbledtablaaspnetusertokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceCode",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUserTokens",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUserTokens");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
