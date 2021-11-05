using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CrearTablaDocumentoPrevio_DocumentosPendientesAutorizar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentosPendienteAutorizar",
                schema: "Transaccional",
                columns: table => new
                {
                    DocumentoPendienteAutorizarId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramiteId = table.Column<long>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false),
                    NotarioUsuarioId = table.Column<long>(nullable: false),
                    Generado = table.Column<bool>(nullable: false),
                    FechaGeneracion = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosPendienteAutorizar", x => x.DocumentoPendienteAutorizarId);
                    table.ForeignKey(
                        name: "FK_DocumentosPendienteAutorizar_Notarias_NotariaId",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentosPendienteAutorizar_NotariasUsuario_NotarioUsuarioId",
                        column: x => x.NotarioUsuarioId,
                        principalSchema: "Parametricas",
                        principalTable: "NotariasUsuario",
                        principalColumn: "NotariaUsuariosId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentosPendienteAutorizar_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalSchema: "Transaccional",
                        principalTable: "Tramites",
                        principalColumn: "TramiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosPrevios",
                schema: "Transaccional",
                columns: table => new
                {
                    DocumentoPrevioId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramiteId = table.Column<long>(nullable: false),
                    Documento = table.Column<byte[]>(nullable: true),
                    Seguimiento = table.Column<string>(unicode: false, maxLength: 150, nullable: true),
                    Tipo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosPrevios", x => x.DocumentoPrevioId);
                    table.ForeignKey(
                        name: "FK_DocumentosPrevios_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalSchema: "Transaccional",
                        principalTable: "Tramites",
                        principalColumn: "TramiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPendienteAutorizar_NotariaId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPendienteAutorizar_NotarioUsuarioId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                column: "NotarioUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPendienteAutorizar_TramiteId",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                column: "TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentosPrevios_TramiteId",
                schema: "Transaccional",
                table: "DocumentosPrevios",
                column: "TramiteId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentosPendienteAutorizar",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "DocumentosPrevios",
                schema: "Transaccional");
        }
    }
}
