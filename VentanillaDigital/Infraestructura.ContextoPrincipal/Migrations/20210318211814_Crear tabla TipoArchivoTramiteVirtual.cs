using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class CreartablaTipoArchivoTramiteVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "TipoArchivo",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "TipoArchivoTramiteVirtual",
                schema: "Parametricas",
                columns: table => new
                {
                    TipoArchivoTramiteVirtualId = table.Column<short>(nullable: false),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoArchivoTramiteVirtual", x => x.TipoArchivoTramiteVirtualId);
                });

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 0, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Documento Autenticar", "Autenticación de documento" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 1, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Documento", "Documento" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 2, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Liquidación", "Constancia de pago" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 3, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Minuta Abierta", "Minuta" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 4, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Minuta Cerrada", "Minuta" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 5, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Minuta Firmada", "Minuta" }, "Parametricas");

            migrationBuilder.InsertData("TipoArchivoTramiteVirtual", new string[] { "TipoArchivoTramiteVirtualId", "FechaCreacion", "FechaModificacion", "UsuarioCreacion", "UsuarioModificacion", "IsDeleted", "Nombre", "Descripcion" }, new object[] { 9, DateTime.Now, DateTime.Now, "admin@olimpiait.com", "admin@olimpiait.com", false, "Testamento Cerrado", "Testamento Cerrado" }, "Parametricas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoArchivoTramiteVirtual",
                schema: "Parametricas");

            migrationBuilder.AlterColumn<int>(
                name: "TipoArchivo",
                schema: "Transaccional",
                table: "ArchivosPortalVirtual",
                type: "int",
                nullable: false,
                oldClrType: typeof(short));
        }
    }
}
