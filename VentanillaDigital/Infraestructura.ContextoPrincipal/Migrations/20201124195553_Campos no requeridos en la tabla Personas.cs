using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CamposnorequeridosenlatablaPersonas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 14, 55, 53, 303, DateTimeKind.Local).AddTicks(4334),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 12, 45, 46, 802, DateTimeKind.Local).AddTicks(8649));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Parametricas",
                table: "Personas",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUserId",
                schema: "Parametricas",
                table: "Personas",
                maxLength: 900,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(900)",
                oldMaxLength: 900);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 12, 45, 46, 802, DateTimeKind.Local).AddTicks(8649),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 24, 14, 55, 53, 303, DateTimeKind.Local).AddTicks(4334));

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "Parametricas",
                table: "Personas",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AspNetUserId",
                schema: "Parametricas",
                table: "Personas",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 900,
                oldNullable: true);
        }
    }
}
