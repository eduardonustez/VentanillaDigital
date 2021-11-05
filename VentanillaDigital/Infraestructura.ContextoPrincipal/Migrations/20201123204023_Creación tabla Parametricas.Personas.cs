using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacióntablaParametricasPersonas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 23, 15, 40, 23, 213, DateTimeKind.Local).AddTicks(3637),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 17, 15, 11, 9, 103, DateTimeKind.Local).AddTicks(1797));

            migrationBuilder.CreateTable(
                name: "Personas",
                schema: "Parametricas",
                columns: table => new
                {
                    PersonaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AspNetUserId = table.Column<string>(maxLength: 900, nullable: false),
                    UserName = table.Column<string>(maxLength: 512, nullable: false),
                    TipoDocumentoId = table.Column<int>(nullable: false),
                    NumeroDocumento = table.Column<string>(maxLength: 20, nullable: false),
                    Nombres = table.Column<string>(maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(maxLength: 120, nullable: false),
                    Email = table.Column<string>(maxLength: 512, nullable: true),
                    NumeroCelular = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.PersonaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Personas_TipoDocumentoId_NumeroDocumento",
                schema: "Parametricas",
                table: "Personas",
                columns: new[] { "TipoDocumentoId", "NumeroDocumento" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personas",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 17, 15, 11, 9, 103, DateTimeKind.Local).AddTicks(1797),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 23, 15, 40, 23, 213, DateTimeKind.Local).AddTicks(3637));
        }
    }
}
