using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class spTransaccionalTramites_Obtener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 11, 9, 33, 1, 988, DateTimeKind.Local).AddTicks(8438),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 10, 14, 55, 15, 234, DateTimeKind.Local).AddTicks(4935));

            var sp = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* 
-----------------------------------------------
* Elaborado por     : Miguel Barahona
* Descripción       : Filtra los registros de la tabla Transaccional.Tramites, según los parametros especificados.
* Fecha creación    : Martes, 10 de noviembre de 2020
* Parámetros        : @TramiteId: Filtro tramiteId
                      @TipoTramite: Filtro por tipo de tramite
					  @NotariaId: Filtro por Id de la notaria
					  @EstadoTramiteId: Filtro por estado del tramite
-----------------------------------------------
* Modificado por    : 
* Fecha modificación: 
* Descripción       :
-----------------------------------------------
Test                :

    DECLARE
    @TramiteId BIGINT,
    @TipoTramite BIGINT,
	@NotariaId BIGINT,
	@EstadoTramiteId BIGINT;

	--SET
		--@TramiteId = 13,
		--@TipoTramite = 0,
		--@NotariaId = 0,
		--@EstadoTramiteId = 0;

    EXEC [Transaccional].[Tramites_Obtener] @TramiteId, @TipoTramite, @NotariaId, @EstadoTramiteId;

*/
CREATE PROCEDURE [Transaccional].[Tramites_Obtener]
	(
		@TramiteId BIGINT = NULL,
		@TipoTramite BIGINT = NULL,
		@NotariaId BIGINT = NULL,
		@EstadoTramiteId BIGINT = NULL
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
            [Tramites].[TramiteId]
			,[Tramites].[FechaCreacion]
			,[Tramites].[FechaModificacion]
			,[Tramites].[UsuarioCreacion]
			,[Tramites].[UsuarioModificacion]
			,[Tramites].[IsDeleted]
			,[Tramites].[CantidadComparecientes]
			,[Tramites].[TipoTramiteId]
			,[Tramites].[NotariaId]
			,[Tramites].[EstadoTramiteId]
			,[Tramites].[Fecha]
        FROM [Transaccional].[Tramites]
        WHERE (
			(@TramiteId IS NULL OR ([Tramites].[TramiteId] = @TramiteId))
			AND (@TipoTramite IS NULL OR ([Tramites].[TipoTramiteId] = @TipoTramite))
			AND (@NotariaId IS NULL OR ([Tramites].[NotariaId] = @NotariaId))
			AND (@EstadoTramiteId IS NULL OR ([Tramites].[EstadoTramiteId] = @EstadoTramiteId))
		)
END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 10, 14, 55, 15, 234, DateTimeKind.Local).AddTicks(4935),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 11, 9, 33, 1, 988, DateTimeKind.Local).AddTicks(8438));
        }
    }
}
