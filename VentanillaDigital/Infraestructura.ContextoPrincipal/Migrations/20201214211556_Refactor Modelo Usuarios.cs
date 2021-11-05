using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class RefactorModeloUsuarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarias_Usuarios",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_Notarias",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropIndex(
                name: "IX_Notarios_NotariaId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "AspNetUserId",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "Parametricas",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "NotariaId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "NumeroDocumento",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 14, 16, 15, 56, 554, DateTimeKind.Local).AddTicks(6622),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 13, 15, 47, 52, 405, DateTimeKind.Local).AddTicks(1908));

            migrationBuilder.AddColumn<string>(
                name: "Grafo",
                schema: "Parametricas",
                table: "Notarios",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NotariaUsuariosId",
                schema: "Parametricas",
                table: "Notarios",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                schema: "Parametricas",
                table: "Notarios",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoNotario",
                schema: "Parametricas",
                table: "Notarios",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                schema: "Parametricas",
                table: "NotariasUsuario",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cargo",
                schema: "Parametricas",
                table: "NotariasUsuario",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Celular",
                schema: "Parametricas",
                table: "NotariasUsuario",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PersonaId",
                schema: "Parametricas",
                table: "NotariasUsuario",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "SincronizoRNEC",
                schema: "Parametricas",
                table: "NotariasUsuario",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "NotarioId",
                schema: "Parametricas",
                table: "Notarias",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ConveniosRNEC",
                schema: "Parametricas",
                columns: table => new
                {
                    ConvenioRNECId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false),
                    Convenio = table.Column<string>(nullable: true),
                    IdCliente = table.Column<long>(nullable: false),
                    IdZona = table.Column<long>(nullable: false),
                    IdRol = table.Column<long>(nullable: false),
                    IdOficina = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConveniosRNEC", x => x.ConvenioRNECId);
                    table.ForeignKey(
                        name: "FK_ConveniosRNEC_Notarias",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maquinas",
                schema: "Parametricas",
                columns: table => new
                {
                    MaquinaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 200, nullable: false),
                    MAC = table.Column<string>(maxLength: 50, nullable: false),
                    DireccionIP = table.Column<string>(maxLength: 50, nullable: false),
                    TipoMaquina = table.Column<int>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maquinas", x => x.MaquinaId);
                    table.ForeignKey(
                        name: "FK_Maquinas_Notarias",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TiposDatos",
                schema: "Parametricas",
                columns: table => new
                {
                    TipoDatoId = table.Column<int>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDatos", x => x.TipoDatoId);
                });

            migrationBuilder.CreateTable(
                name: "PersonasDatos",
                schema: "Parametricas",
                columns: table => new
                {
                    PersonaDatosId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PersonaId = table.Column<long>(nullable: false),
                    TipoDatoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonasDatos", x => x.PersonaDatosId);
                    table.ForeignKey(
                        name: "FK_PersonaDatos_Personas",
                        column: x => x.PersonaId,
                        principalSchema: "Parametricas",
                        principalTable: "Personas",
                        principalColumn: "PersonaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonasDatos_TiposDatos_TipoDatoId",
                        column: x => x.TipoDatoId,
                        principalSchema: "Parametricas",
                        principalTable: "TiposDatos",
                        principalColumn: "TipoDatoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Parametricas",
                table: "TiposDatos",
                columns: new[] { "TipoDatoId", "Nombre" },
                values: new object[] { 1, "Direccion" });

            migrationBuilder.InsertData(
                schema: "Parametricas",
                table: "TiposDatos",
                columns: new[] { "TipoDatoId", "Nombre" },
                values: new object[] { 2, "NumeroCelular" });

            migrationBuilder.InsertData(
                schema: "Parametricas",
                table: "TiposDatos",
                columns: new[] { "TipoDatoId", "Nombre" },
                values: new object[] { 3, "Email" });

            migrationBuilder.CreateIndex(
                name: "IX_NotariasUsuario_PersonaId",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_ConveniosRNEC_NotariaId",
                schema: "Parametricas",
                table: "ConveniosRNEC",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Maquinas_NotariaId",
                schema: "Parametricas",
                table: "Maquinas",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonasDatos_PersonaId",
                schema: "Parametricas",
                table: "PersonasDatos",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonasDatos_TipoDatoId",
                schema: "Parametricas",
                table: "PersonasDatos",
                column: "TipoDatoId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotariasUsuarios_Notarias",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "NotariaId",
                principalSchema: "Parametricas",
                principalTable: "Notarias",
                principalColumn: "NotariaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "NotariaUsuariosId",
                principalSchema: "Parametricas",
                principalTable: "Notarios",
                principalColumn: "NotarioId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotariasUsuarios_Personas",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "PersonaId",
                principalSchema: "Parametricas",
                principalTable: "Personas",
                principalColumn: "PersonaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarias_Notarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotarioId",
                principalSchema: "Parametricas",
                principalTable: "Notarias",
                principalColumn: "NotariaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotariasUsuarios_Notarias",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_NotariasUsuarios",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_NotariasUsuarios_Personas",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropForeignKey(
                name: "FK_Notarias_Notarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropTable(
                name: "ConveniosRNEC",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Maquinas",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "PersonasDatos",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "TiposDatos",
                schema: "Parametricas");

            migrationBuilder.DropIndex(
                name: "IX_NotariasUsuario_PersonaId",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "Grafo",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "NotariaUsuariosId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "Pin",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "TipoNotario",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "Area",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "Cargo",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "Celular",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "PersonaId",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "SincronizoRNEC",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropColumn(
                name: "NotarioId",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 13, 15, 47, 52, 405, DateTimeKind.Local).AddTicks(1908),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 14, 16, 15, 56, 554, DateTimeKind.Local).AddTicks(6622));

            migrationBuilder.AddColumn<string>(
                name: "AspNetUserId",
                schema: "Parametricas",
                table: "Personas",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "Parametricas",
                table: "Personas",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NotariaId",
                schema: "Parametricas",
                table: "Notarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NumeroDocumento",
                schema: "Parametricas",
                table: "Notarios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentoId",
                schema: "Parametricas",
                table: "Notarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 60);

            migrationBuilder.CreateIndex(
                name: "IX_Notarios_NotariaId",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotariaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarias_Usuarios",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "NotariaId",
                principalSchema: "Parametricas",
                principalTable: "Notarias",
                principalColumn: "NotariaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_Notarias",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotariaId",
                principalSchema: "Parametricas",
                principalTable: "Notarias",
                principalColumn: "NotariaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
