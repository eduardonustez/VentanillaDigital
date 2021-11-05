using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Parametricas");

            migrationBuilder.EnsureSchema(
                name: "Transaccional");

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "Parametricas",
                columns: table => new
                {
                    CategoriaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosTramites",
                schema: "Parametricas",
                columns: table => new
                {
                    EstadoTramiteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosTramites", x => x.EstadoTramiteId);
                });

            migrationBuilder.CreateTable(
                name: "Ubicaciones",
                schema: "Parametricas",
                columns: table => new
                {
                    UbicacionId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(nullable: false),
                    UbicacionPadreId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubicaciones", x => x.UbicacionId);
                    table.ForeignKey(
                        name: "FK_Ubicacion_UbicacionPadre",
                        column: x => x.UbicacionPadreId,
                        principalSchema: "Parametricas",
                        principalTable: "Ubicaciones",
                        principalColumn: "UbicacionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MetadataArchivos",
                schema: "Transaccional",
                columns: table => new
                {
                    MetadataArchivoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 250, nullable: false),
                    Extension = table.Column<string>(maxLength: 20, nullable: false),
                    Tamanio = table.Column<long>(nullable: false),
                    Ruta = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetadataArchivos", x => x.MetadataArchivoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposTramites",
                schema: "Parametricas",
                columns: table => new
                {
                    TipoTramiteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(maxLength: 250, nullable: true),
                    CategoriaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTramites", x => x.TipoTramiteId);
                    table.ForeignKey(
                        name: "FK_TiposTramites_Categorias",
                        column: x => x.CategoriaId,
                        principalSchema: "Parametricas",
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notarias",
                schema: "Parametricas",
                columns: table => new
                {
                    NotariaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Nombre = table.Column<string>(maxLength: 60, nullable: false),
                    Direccion = table.Column<string>(maxLength: 250, nullable: true),
                    Telefono = table.Column<string>(maxLength: 20, nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    UbicacionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notarias", x => x.NotariaId);
                    table.ForeignKey(
                        name: "FK_Notarias_Ubicaciones",
                        column: x => x.UbicacionId,
                        principalSchema: "Parametricas",
                        principalTable: "Ubicaciones",
                        principalColumn: "UbicacionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Archivos",
                schema: "Transaccional",
                columns: table => new
                {
                    ArchivoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MetadataArchivoId = table.Column<long>(nullable: false),
                    Contenido = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivos", x => x.ArchivoId);
                    table.ForeignKey(
                        name: "FK_Archivos_MetadataArchivos",
                        column: x => x.MetadataArchivoId,
                        principalSchema: "Transaccional",
                        principalTable: "MetadataArchivos",
                        principalColumn: "MetadataArchivoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                schema: "Parametricas",
                columns: table => new
                {
                    ColaboradorId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TipoDocumentoId = table.Column<int>(nullable: false),
                    NumeroDocumento = table.Column<string>(maxLength: 20, nullable: false),
                    NotariaId = table.Column<long>(nullable: false),
                    Cargo = table.Column<string>(maxLength: 100, nullable: true),
                    Area = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaboradores", x => x.ColaboradorId);
                    table.ForeignKey(
                        name: "FK_Colaboradores_Notarias",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notarios",
                schema: "Parametricas",
                columns: table => new
                {
                    NotarioId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TipoDocumentoId = table.Column<int>(nullable: false),
                    NumeroDocumento = table.Column<string>(maxLength: 20, nullable: false),
                    Nit = table.Column<string>(maxLength: 20, nullable: false),
                    NotariaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notarios", x => x.NotarioId);
                    table.ForeignKey(
                        name: "FK_Notarios_Notarias",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tramites",
                schema: "Transaccional",
                columns: table => new
                {
                    TramiteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CantidadComparecientes = table.Column<int>(nullable: false),
                    TipoTramiteId = table.Column<long>(nullable: false),
                    NotariaId = table.Column<long>(nullable: false),
                    EstadoTramiteId = table.Column<long>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2020, 11, 4, 17, 45, 28, 978, DateTimeKind.Local).AddTicks(9100))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tramites", x => x.TramiteId);
                    table.ForeignKey(
                        name: "FK_Tramites_EstadosTramites",
                        column: x => x.EstadoTramiteId,
                        principalSchema: "Parametricas",
                        principalTable: "EstadosTramites",
                        principalColumn: "EstadoTramiteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tramites_Notarias",
                        column: x => x.NotariaId,
                        principalSchema: "Parametricas",
                        principalTable: "Notarias",
                        principalColumn: "NotariaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tramites_TiposTramites",
                        column: x => x.TipoTramiteId,
                        principalSchema: "Parametricas",
                        principalTable: "TiposTramites",
                        principalColumn: "TipoTramiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comparecientes",
                schema: "Transaccional",
                columns: table => new
                {
                    ComparecienteId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TramiteId = table.Column<long>(nullable: false),
                    TipoDocumentoId = table.Column<int>(nullable: false),
                    NumeroDocumento = table.Column<string>(maxLength: 20, nullable: false),
                    FotoId = table.Column<long>(nullable: false),
                    FirmaId = table.Column<long>(nullable: false),
                    TransaccionRNECId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comparecientes", x => x.ComparecienteId);
                    table.ForeignKey(
                        name: "FK_Firmas_Comparecientes",
                        column: x => x.FirmaId,
                        principalSchema: "Transaccional",
                        principalTable: "MetadataArchivos",
                        principalColumn: "MetadataArchivoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fotos_Comparecientes",
                        column: x => x.FotoId,
                        principalSchema: "Transaccional",
                        principalTable: "MetadataArchivos",
                        principalColumn: "MetadataArchivoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comparecientes_Tramites",
                        column: x => x.TramiteId,
                        principalSchema: "Transaccional",
                        principalTable: "Tramites",
                        principalColumn: "TramiteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_NotariaId",
                schema: "Parametricas",
                table: "Colaboradores",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Notarias_UbicacionId",
                schema: "Parametricas",
                table: "Notarias",
                column: "UbicacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Notarios_NotariaId",
                schema: "Parametricas",
                table: "Notarios",
                column: "NotariaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposTramites_CategoriaId",
                schema: "Parametricas",
                table: "TiposTramites",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ubicaciones_UbicacionPadreId",
                schema: "Parametricas",
                table: "Ubicaciones",
                column: "UbicacionPadreId");

            migrationBuilder.CreateIndex(
                name: "IX_Archivos_MetadataArchivoId",
                schema: "Transaccional",
                table: "Archivos",
                column: "MetadataArchivoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comparecientes_FirmaId",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "FirmaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comparecientes_FotoId",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "FotoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comparecientes_TramiteId",
                schema: "Transaccional",
                table: "Comparecientes",
                column: "TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_EstadoTramiteId",
                schema: "Transaccional",
                table: "Tramites",
                column: "EstadoTramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_NotariaId",
                schema: "Transaccional",
                table: "Tramites",
                column: "NotariaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_TipoTramiteId",
                schema: "Transaccional",
                table: "Tramites",
                column: "TipoTramiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colaboradores",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Notarios",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Archivos",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "Comparecientes",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "MetadataArchivos",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "Tramites",
                schema: "Transaccional");

            migrationBuilder.DropTable(
                name: "EstadosTramites",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Notarias",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "TiposTramites",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Ubicaciones",
                schema: "Parametricas");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "Parametricas");
        }
    }
}
