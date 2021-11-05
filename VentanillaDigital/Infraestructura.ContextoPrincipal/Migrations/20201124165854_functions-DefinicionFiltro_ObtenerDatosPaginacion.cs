using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class functionsDefinicionFiltro_ObtenerDatosPaginacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 58, 54, 635, DateTimeKind.Local).AddTicks(3603),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 48, 42, 31, DateTimeKind.Local).AddTicks(9039));

            migrationBuilder.Sql(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* 
-----------------------------------------------
* Elaborado por     : Miguel Angel Barahona
* Descripción       : Obtiene los datos de filtro de la definición de filtro
* Fecha creación    : Martes, 24 noviembre de 2020
* Parámetros        : @I_DefinicionFiltro: JSON con la definición de filtro
* Créditos			: Stefano Paredes Suárez
-----------------------------------------------
* Modificado por    : 
* Fecha modificación: 
* Descripción       :
-----------------------------------------------
Test JSON           :

DECLARE
@I_DefinicionFiltro VARCHAR(MAX);

SELECT SoloTotalRegistros
    , IndicePagina
    , CantidadFilasPagina
FROM [dbo].[DefinicionFiltro_ObtenerDatosPaginacion](@I_DefinicionFiltro);
-----------------------------------------------
*/
CREATE FUNCTION [dbo].[DefinicionFiltro_ObtenerDatosPaginacion] 
(
	@I_DefinicionFiltro VARCHAR(MAX)
)
RETURNS @RT TABLE
(
	SoloTotalRegistros BIT,
    IndicePagina INT,
    CantidadFilasPagina INT
)
AS
BEGIN
	
	INSERT INTO @RT
	SELECT ISNULL(SoloTotalRegistros, 0) AS SoloTotalRegistros
        , ISNULL(IndicePagina, 1) AS IndicePagina
        , ISNULL(CantidadFilasPagina, 10) AS CantidadFilasPagina
    FROM OPENJSON(@I_DefinicionFiltro)
    WITH 
    (
        SoloTotalRegistros BIT '$.Paginacion.SoloTotalRegistros',
        IndicePagina INT '$.Paginacion.IndicePagina',
        CantidadFilasPagina INT '$.Paginacion.CantidadFilasPagina'
    );
	RETURN;

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 48, 42, 31, DateTimeKind.Local).AddTicks(9039),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 58, 54, 635, DateTimeKind.Local).AddTicks(3603));

            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[DefinicionFiltro_ObtenerDatosPaginacion]");
        }
    }
}
