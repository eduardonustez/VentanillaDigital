using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreacionIndicestramiteyNotariaUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tramites_IsDeleted_NotariaId",
                schema: "Transaccional",
                table: "Tramites",
                columns: new[] { "IsDeleted", "NotariaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_EstadoTramiteId",
                schema: "Transaccional",
                table: "Tramites",
                columns: new[] { "IsDeleted", "NotariaId", "EstadoTramiteId" })
                .Annotation("SqlServer:Include", new[] { "TramiteId", "FechaCreacion", "CantidadComparecientes", "TipoTramiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_FechaCreacion",
                schema: "Transaccional",
                table: "Tramites",
                columns: new[] { "IsDeleted", "NotariaId", "FechaCreacion" });

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_EstadoTramiteId_FechaCreacion",
                schema: "Transaccional",
                table: "Tramites",
                columns: new[] { "IsDeleted", "NotariaId", "EstadoTramiteId", "FechaCreacion" })
                .Annotation("SqlServer:Include", new[] { "TramiteId", "CantidadComparecientes", "TipoTramiteId" });

            migrationBuilder.CreateIndex(
                name: "IX_NotariasUsuario_IsDeleted",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "IsDeleted")
                .Annotation("SqlServer:Include", new[] { "UsuariosId" });

            migrationBuilder.CreateIndex(
                name: "IX_NotariasUsuario_UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_NotariasUsuario_IsDeleted_UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario",
                columns: new[] { "IsDeleted", "UserEmail" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tramites_IsDeleted_NotariaId",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_EstadoTramiteId",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_FechaCreacion",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropIndex(
                name: "IX_Tramites_IsDeleted_NotariaId_EstadoTramiteId_FechaCreacion",
                schema: "Transaccional",
                table: "Tramites");

            migrationBuilder.DropIndex(
                name: "IX_NotariasUsuario_IsDeleted",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropIndex(
                name: "IX_NotariasUsuario_UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario");

            migrationBuilder.DropIndex(
                name: "IX_NotariasUsuario_IsDeleted_UserEmail",
                schema: "Parametricas",
                table: "NotariasUsuario");
        }
    }
}
