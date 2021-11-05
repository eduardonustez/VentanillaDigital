using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class ConsultaNotariaSegura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Vigencia",
                schema: "Transaccional",
                table: "Comparecientes",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConsultaNotariaSegura",
                schema: "Parametricas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<string>(maxLength: 100, nullable: false),
                    numeroDocumento = table.Column<long>(maxLength: 14, nullable: false),
                    Encontrado = table.Column<bool>(nullable: false),
                    numeroNotaria = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultaNotariaSegura", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsultaNotariaSegura",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<string>(
                name: "Vigencia",
                schema: "Transaccional",
                table: "Comparecientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
