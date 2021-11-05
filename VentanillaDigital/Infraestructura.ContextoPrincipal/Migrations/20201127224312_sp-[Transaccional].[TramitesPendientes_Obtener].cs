using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class spTransaccionalTramitesPendientes_Obtener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 27, 17, 43, 11, 736, DateTimeKind.Local).AddTicks(2271),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 25, 14, 59, 58, 243, DateTimeKind.Local).AddTicks(4888));

            migrationBuilder.Sql(@"SET QUOTED_IDENTIFIER ON
GO

/* 
-----------------------------------------------
* Elaborado por     : Miguel Ángel Barahona
* Descripción       : Filtra y/o pagina los registros de la tabla Transaccional.Tramites, según los parámetros especificados. Asigna la cantidad de registros que cumplen con los filtros especificados a la variables de salida @O_TotalRegistros.
* Fecha creación    : Martes, 24 noviembre de 2020
* Parámetros        : @I_DefinicionFiltro: JSON con la definición de filtro
                      @O_TotalRegistros: Cantidad de registros asociados a la definición de filtro
-----------------------------------------------
* Modificado por    : 
* Fecha modificación: 
* Descripción       :
-----------------------------------------------
Test                :

    DECLARE
    @I_DefinicionFiltro VARCHAR(MAX),
    @O_TotalRegistros BIGINT;

    EXEC[Transaccional].[TramitesPendientes_Obtener] @I_DefinicionFiltro, @O_TotalRegistros OUTPUT;

        SELECT @O_TotalRegistros;

*/
CREATE PROCEDURE[Transaccional].[TramitesPendientes_Obtener]
        (
   @I_DefinicionFiltro VARCHAR(MAX),
    @O_TotalRegistros BIGINT OUTPUT
)
AS
BEGIN

    SET NOCOUNT ON;

    DECLARE
    @V_SoloTotalRegistros BIT,
    @V_IndicePagina INT,
    @V_CantidadFilasPagina INT,
    @V_Datos VARCHAR(MAX);

        SELECT @V_SoloTotalRegistros = ISNULL(SoloTotalRegistros, 0)
               , @V_IndicePagina = ISNULL(IndicePagina, 1)
               , @V_CantidadFilasPagina = ISNULL(CantidadFilasPagina, 10)
    FROM[dbo].[DefinicionFiltro_ObtenerDatosPaginacion] (@I_DefinicionFiltro);

    WITH CTE_Ordenacion AS
    (
        SELECT OrdenacionCampo
               , OrdenacionDireccion
        FROM [dbo].[DefinicionFiltro_ObtenerDatosOrdenacion] (@I_DefinicionFiltro)
    )
    , CTE_Filtro AS
    (
        SELECT FiltroCampo
               , FiltroValor
        FROM [dbo].[DefinicionFiltro_ObtenerDatosFiltro] (@I_DefinicionFiltro)
    )
    , CTE_TramitesPendientes AS
    (
        SELECT
             Tramites.TramiteId
            , Tramites.FechaCreacion
        FROM Transaccional.Tramites
			--INNER JOIN Transaccional.
        WHERE (
        NOT EXISTS(SELECT FiltroValor FROM CTE_Filtro WHERE FiltroCampo = 'TramiteId') OR Tramites.TramiteId IN (SELECT VALUE FROM STRING_SPLIT((SELECT CAST(FiltroValor AS VARCHAR(MAX)) FROM CTE_Filtro WHERE FiltroCampo = 'TramiteId'), ',')))
            AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'FechaCreacion')

            OR Tramites.FechaCreacion LIKE '%' +  (SELECT CAST(FiltroValor AS VARCHAR(200)) FROM CTE_Filtro WHERE FiltroCampo = 'FechaCreacion') + '%')
            --AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'VerboDescripcion') OR Verbo.VerboDescripcion LIKE '%' +  (SELECT CAST(FiltroValor AS VARCHAR(800)) FROM CTE_Filtro WHERE FiltroCampo = 'VerboDescripcion') + '%')
            --AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'VerboHabilitado') OR Verbo.VerboHabilitado = (SELECT CAST(FiltroValor AS BIT) FROM CTE_Filtro WHERE FiltroCampo = 'VerboHabilitado')
			
    )
    , CTE_TramitesPendientesPaginado AS
    (
        SELECT
              TramiteId
            , FechaCreacion
            --, VerboDescripcion
            --, VerboHabilitado
            , ROW_NUMBER() OVER (
        ORDER BY
            (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'TramiteId' AND OrdenacionDireccion = '1') THEN TramiteId END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'TramiteId' AND OrdenacionDireccion = '2') THEN TramiteId END) DESC
			, (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'FechaCreacion' AND OrdenacionDireccion = '1') THEN FechaCreacion END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'FechaCreacion' AND OrdenacionDireccion = '2') THEN FechaCreacion END) DESC            
			--, (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'VerboDescripcion' AND OrdenacionDireccion = '1') THEN VerboDescripcion END) ASC
   --         , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'VerboDescripcion' AND OrdenacionDireccion = '2') THEN VerboDescripcion END) DESC            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'VerboHabilitado' AND OrdenacionDireccion = '1') THEN VerboHabilitado END) ASC
   --         , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'VerboHabilitado' AND OrdenacionDireccion = '2') THEN VerboHabilitado END) DESC
        ) As RowNumber
        FROM CTE_TramitesPendientes
        WHERE @V_SoloTotalRegistros = 0
    )
    SELECT @O_TotalRegistros =
        (
            SELECT COUNT(1)
            FROM CTE_TramitesPendientes
        ) 
        , @V_Datos =
        (
            SELECT TOP(@V_CantidadFilasPagina)
                TramiteId AS Id
                , FechaCreacion AS FechaCreacion
                --, VerboDescripcion AS Descripcion
                --, VerboHabilitado AS Habilitado
            FROM CTE_TramitesPendientesPaginado
            WHERE(RowNumber BETWEEN ((@V_IndicePagina - 1) * @V_CantidadFilasPagina + 1) AND(@V_IndicePagina* @V_CantidadFilasPagina))
            FOR JSON AUTO
        );
    SELECT @V_Datos;
        END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 25, 14, 59, 58, 243, DateTimeKind.Local).AddTicks(4888),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 27, 17, 43, 11, 736, DateTimeKind.Local).AddTicks(2271));

            migrationBuilder.Sql(@"DROP PROCEDURE [Transaccional].[TramitesPendientes_Obtener]");
        }
    }
}
