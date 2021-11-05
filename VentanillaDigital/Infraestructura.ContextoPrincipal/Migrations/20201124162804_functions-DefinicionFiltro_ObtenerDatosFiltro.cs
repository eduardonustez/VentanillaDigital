using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class functionsDefinicionFiltro_ObtenerDatosFiltro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 28, 4, 749, DateTimeKind.Local).AddTicks(5330),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 13, 44, 417, DateTimeKind.Local).AddTicks(1220));

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



SELECT FiltroCampo
    , FiltroValor
FROM[dbo].[DefinicionFiltro_ObtenerDatosFiltro](@I_DefinicionFiltro);
            */
            CREATE FUNCTION[dbo].[DefinicionFiltro_ObtenerDatosFiltro]
            (
                @I_DefinicionFiltro VARCHAR(MAX)
            )
RETURNS @RT TABLE
(
    FiltroCampo NVARCHAR(MAX),
    FiltroValor NVARCHAR(MAX)
)
AS
BEGIN


    INSERT INTO @RT
    SELECT FiltroCampo
        , FiltroValor
    FROM OPENJSON(@I_DefinicionFiltro)
    WITH
    (
        Filtro NVARCHAR(MAX) '$.Filtro' AS JSON
    )
    CROSS APPLY OPENJSON(Filtro, '$')
    WITH
    (
        FiltroCampo NVARCHAR(MAX) '$.Campo',
        FiltroValor NVARCHAR(MAX) '$.Valor'
    );
            RETURN;

            END
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 13, 44, 417, DateTimeKind.Local).AddTicks(1220),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 28, 4, 749, DateTimeKind.Local).AddTicks(5330));

            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[DefinicionFiltro_ObtenerDatosFiltro]");
        }
    }
}
