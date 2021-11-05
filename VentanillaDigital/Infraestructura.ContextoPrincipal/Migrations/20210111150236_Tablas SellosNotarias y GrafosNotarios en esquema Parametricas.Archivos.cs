using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class TablasSellosNotariasyGrafosNotariosenesquemaParametricasArchivos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Colaboradores",
                schema: "Parametricas");

            migrationBuilder.EnsureSchema(
                name: "Parametricas.Archivos");

            migrationBuilder.AddColumn<long>(
                name: "GrafoId",
                schema: "Parametricas",
                table: "Notarios",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SelloId",
                schema: "Parametricas",
                table: "Notarias",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GrafosNotarios",
                schema: "Parametricas.Archivos",
                columns: table => new
                {
                    GrafoNotarioId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Contenido = table.Column<byte[]>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Tamanio = table.Column<long>(nullable: false),
                    Ruta = table.Column<string>(nullable: true),
                    NotarioId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrafosNotarios", x => x.GrafoNotarioId);
                });

            migrationBuilder.CreateTable(
                name: "SellosNotarias",
                schema: "Parametricas.Archivos",
                columns: table => new
                {
                    SelloNotariaId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaCreacion = table.Column<DateTime>(nullable: false),
                    FechaModificacion = table.Column<DateTime>(nullable: false),
                    UsuarioCreacion = table.Column<string>(nullable: true),
                    UsuarioModificacion = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Contenido = table.Column<byte[]>(nullable: true),
                    Nombre = table.Column<string>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Tamanio = table.Column<long>(nullable: false),
                    Ruta = table.Column<string>(nullable: true),
                    NotariaId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellosNotarias", x => x.SelloNotariaId);
                });

            var crearFuncion = @"
/****** Object:  UserDefinedFunction [dbo].[Base64ABinary]    Script Date: 1/11/2021 10:06:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 

-- Author: Jaime Silva
-- Create date: 11/01/2021
-- Description: Funcion para convertir de base64 a varbinary
-- ============================================= 
--Test:
--SELECT dbo.[Base64ABinary]('3qAAAA==')
-- ============================================= 

CREATE FUNCTION [dbo].[Base64ABinary](@base64 VARCHAR(max)) 
returns VARBINARY(max) 
AS 
  BEGIN 

  RETURN cast(N'' as xml).value('xs:base64Binary(sql:variable(""@base64""))','varbinary(max)');
END";

            migrationBuilder.Sql(crearFuncion);

            var copiarGrafos = @"
  INSERT INTO [Parametricas.Archivos].GrafosNotarios
    SELECT GETDATE(), GETDATE(), 'jsilva', 'jsilva', 0, 
		dbo.Base64ABinary(RIGHT(Grafo,LEN(Grafo)-CHARINDEX(',',Grafo))) as Contenido, 
		CONCAT(nto.NotarioId, '_', nu.UserEmail) as Nombre,
		SUBSTRING(Grafo,12,CHARINDEX(';',Grafo)-12) as Extension,
		DATALENGTH(dbo.Base64ABinary(RIGHT(Grafo,LEN(Grafo)-CHARINDEX(',',Grafo)))) as Tamanio,
		LOWER(
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
				CONCAT('grafos/', nta.NotariaId,'_',nta.Nombre,'/')
				,' - ','-')
				,'.','')
				,' ','_')
				,'á','a')
				,'é','e')
				,'í','i')
				,'ó','o')
				,'ú','u') ) as Ruta,
        nto.NotarioId as NotarioId
  FROM Parametricas.Notarios nto
  JOIN Parametricas.NotariasUsuario nu on nto.NotariaUsuariosId = nu.NotariaUsuariosId
  JOIN Parametricas.Personas pe on nu.PersonaId = pe.PersonaId
  JOIN Parametricas.Notarias nta on nu.NotariaId = nta.NotariaId
  where Grafo Like 'data%'

    UPDATE Parametricas.Notarios
    SET GrafoId = gn.GrafoNotarioId 
    FROM Parametricas.Notarios nto
    JOIN [Parametricas.Archivos].GrafosNotarios gn on gn.NotarioId = nto.NotarioId
";
            migrationBuilder.Sql(copiarGrafos);

            var copiarSellos = @"
  INSERT INTO [Parametricas.Archivos].SellosNotarias
  SELECT GETDATE(), GETDATE(), 'jsilva', 'jsilva', 0, 
		dbo.Base64ABinary(RIGHT(Sello,LEN(Sello)-CHARINDEX(',',Sello))) as Contenido, 
		LOWER(
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
				CONCAT(nta.NotariaId,'_',nta.Nombre)
				,' - ','-')
				,'.','')
				,' ','_')
				,'á','a')
				,'é','e')
				,'í','i')
				,'ó','o')
				,'ú','u') ) as Nombre,
		'png' as Extension,
		DATALENGTH(dbo.Base64ABinary(RIGHT(Sello,LEN(Sello)-CHARINDEX(',',Sello)))) as Tamanio,
		LOWER(
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
				CONCAT('sellos/', CASE WHEN dep.Nombre is NULL THEN mun.Nombre ELSE dep.Nombre END,'/',nta.Nombre,'/')
				,' - ','-')
				,'.','')
				,' ','_')
				,'á','a')
				,'é','e')
				,'í','i')
				,'ó','o')
				,'ú','u') ) as Ruta,
        nta.NotariaId as NotariaId
  FROM Parametricas.Notarias nta
  JOIN Parametricas.Ubicaciones mun on nta.UbicacionId = mun.UbicacionId
  LEFT JOIN Parametricas.Ubicaciones dep on mun.UbicacionPadreId = dep.UbicacionId
  where Sello Like 'data%'

    UPDATE Parametricas.Notarias
    SET SelloId = sn.SelloNotariaId 
    FROM Parametricas.Notarias nta
    JOIN [Parametricas.Archivos].SellosNotarias sn on sn.NotariaId = nta.NotariaId
";

            migrationBuilder.Sql(copiarSellos);



            migrationBuilder.DropColumn(
                name: "NotarioId",
                schema: "Parametricas.Archivos",
                table: "GrafosNotarios");

            migrationBuilder.DropColumn(
                name: "NotariaId",
                schema: "Parametricas.Archivos",
                table: "SellosNotarias");

            migrationBuilder.CreateIndex(
                name: "IX_Notarios_GrafoId",
                schema: "Parametricas",
                table: "Notarios",
                column: "GrafoId",
                unique: true,
                filter: "[GrafoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notarias_SelloId",
                schema: "Parametricas",
                table: "Notarias",
                column: "SelloId",
                unique: true,
                filter: "[SelloId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Notarias_SellosNotarias",
                schema: "Parametricas",
                table: "Notarias",
                column: "SelloId",
                principalSchema: "Parametricas.Archivos",
                principalTable: "SellosNotarias",
                principalColumn: "SelloNotariaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios",
                column: "GrafoId",
                principalSchema: "Parametricas.Archivos",
                principalTable: "GrafosNotarios",
                principalColumn: "GrafoNotarioId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notarias_SellosNotarias",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.DropForeignKey(
                name: "FK_Notarios_GrafosNotarios",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropTable(
                name: "GrafosNotarios",
                schema: "Parametricas.Archivos");

            migrationBuilder.DropTable(
                name: "SellosNotarias",
                schema: "Parametricas.Archivos");

            migrationBuilder.DropIndex(
                name: "IX_Notarios_GrafoId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropIndex(
                name: "IX_Notarias_SelloId",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.DropColumn(
                name: "GrafoId",
                schema: "Parametricas",
                table: "Notarios");

            migrationBuilder.DropColumn(
                name: "SelloId",
                schema: "Parametricas",
                table: "Notarias");

            migrationBuilder.CreateTable(
                name: "Colaboradores",
                schema: "Parametricas",
                columns: table => new
                {
                    ColaboradorId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Area = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cargo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NotariaId = table.Column<long>(type: "bigint", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoDocumentoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioModificacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Colaboradores_NotariaId",
                schema: "Parametricas",
                table: "Colaboradores",
                column: "NotariaId");

            var crearFuncion = @"
DROP FUNCTION [dbo].[Base64ABinary]
GO
";

            migrationBuilder.Sql(crearFuncion);
        }
    }
}
