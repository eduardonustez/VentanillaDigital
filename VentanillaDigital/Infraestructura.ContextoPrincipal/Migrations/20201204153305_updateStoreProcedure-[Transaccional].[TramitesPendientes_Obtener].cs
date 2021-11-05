using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class updateStoreProcedureTransaccionalTramitesPendientes_Obtener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 4, 10, 33, 4, 822, DateTimeKind.Local).AddTicks(3761),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 2, 16, 30, 37, 559, DateTimeKind.Local).AddTicks(2765));

            migrationBuilder.Sql(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
-----------------------------------------------
* Elaborado por     : Miguel Ángel Barahona
* Descripción       : Filtra y/o pagina los registros de la tabla Transaccional.Tramites, según los parámetros especificados. Asigna la cantidad de registros que cumplen con los filtros especificados a la variables de salida @O_TotalRegistros.
* Fecha creación    : Martes, 24 noviembre de 2020
* Parámetros        : @I_DefinicionFiltro: JSON con la definición de filtro
                      @O_TotalRegistros: Cantidad de registros asociados a la definición de filtro
-----------------------------------------------
* Modificado por    : Miguel Ángel Barahona
* Fecha modificación: Viernes 4 diciembre de 2020
* Descripción       : Filtra y/o pagina los registros de la tabla Transaccional.Tramites, según los parámetros especificados. Asigna la cantidad de registros que cumplen con los filtros especificados a la variables de salida @O_TotalRegistros.
-----------------------------------------------
Test                :

    DECLARE
    @I_DefinicionFiltro VARCHAR(MAX),
    @O_TotalRegistros BIGINT;

    EXEC[Transaccional].[TramitesPendientes_Obtener] @I_DefinicionFiltro, @O_TotalRegistros OUTPUT;

        SELECT @O_TotalRegistros;

*/
ALTER PROCEDURE [Transaccional].[TramitesPendientes_Obtener]
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

            , TiposTramites.Nombre

            , Tramites.FechaCreacion

            , COUNT(Comparecientes.ComparecienteId) Comparecientes

        FROM Transaccional.Tramites
            INNER JOIN Parametricas.TiposTramites ON TiposTramites.TipoTramiteId = Tramites.TipoTramiteId

            INNER JOIN Transaccional.Comparecientes ON Comparecientes.TramiteId = Tramites.TramiteId
        WHERE (
        NOT EXISTS(SELECT FiltroValor FROM CTE_Filtro WHERE FiltroCampo = 'TramiteId') OR Tramites.TramiteId IN (SELECT VALUE FROM STRING_SPLIT((SELECT CAST(FiltroValor AS VARCHAR(8)) FROM CTE_Filtro WHERE FiltroCampo = 'TramiteId'), ',')))

        AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'Nombre')
        OR TiposTramites.Nombre LIKE '%' +  (SELECT CAST(FiltroValor AS VARCHAR(500)) FROM CTE_Filtro WHERE FiltroCampo = 'Nombre') + '%')

        AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'FechaCreacion')
        OR Tramites.FechaCreacion LIKE '%' +  (SELECT CAST(FiltroValor AS VARCHAR(8)) FROM CTE_Filtro WHERE FiltroCampo = 'FechaCreacion') + '%')
            --AND(Not EXISTS(Select FiltroValor From CTE_Filtro Where FiltroCampo = 'VerboHabilitado') OR Verbo.VerboHabilitado = (SELECT CAST(FiltroValor AS BIT) FROM CTE_Filtro WHERE FiltroCampo = 'VerboHabilitado')

        GROUP BY Tramites.TramiteId
			, TiposTramites.Nombre
			, Tramites.FechaCreacion
    )
    , CTE_TramitesPendientesPaginado AS
    (
        SELECT
              TramiteId
            , Nombre
            , FechaCreacion

            , Comparecientes
            , ROW_NUMBER() OVER (
        ORDER BY
            (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'TramiteId' AND OrdenacionDireccion = '1') THEN TramiteId END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'TramiteId' AND OrdenacionDireccion = '2') THEN TramiteId END) DESC
			, (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'Nombre' AND OrdenacionDireccion = '1') THEN Nombre END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'Nombre' AND OrdenacionDireccion = '2') THEN Nombre END) DESC            
			, (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'FechaCreacion' AND OrdenacionDireccion = '1') THEN FechaCreacion END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'FechaCreacion' AND OrdenacionDireccion = '2') THEN FechaCreacion END) DESC 
			, (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'Comparecientes' AND OrdenacionDireccion = '1') THEN Comparecientes END) ASC
            , (SELECT CASE WHEN EXISTS(SELECT 1 FROM CTE_Ordenacion WHERE OrdenacionCampo = 'Comparecientes' AND OrdenacionDireccion = '2') THEN Comparecientes END) DESC
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
				, Nombre AS TipoTramite
                , FechaCreacion AS Fecha
                , Comparecientes
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
                defaultValue: new DateTime(2020, 12, 2, 16, 30, 37, 559, DateTimeKind.Local).AddTicks(2765),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 4, 10, 33, 4, 822, DateTimeKind.Local).AddTicks(3761));

            migrationBuilder.Sql(@"DROP PROCEDURE [Transaccional].[TramitesPendientes_Obtener]");
        }
    }
}
