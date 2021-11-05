using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class functionsDefinicionFiltro_ObtenerDatosOrdenacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 24, 11, 48, 42, 31, DateTimeKind.Local).AddTicks(9039),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 28, 4, 749, DateTimeKind.Local).AddTicks(5330));

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

SELECT OrdenacionCampo
    , OrdenacionDireccion
FROM[dbo].[DefinicionFiltro_ObtenerDatosOrdenacion](@I_DefinicionFiltro);
            -----------------------------------------------
            */
            CREATE FUNCTION[dbo].[DefinicionFiltro_ObtenerDatosOrdenacion]
            (
                @I_DefinicionFiltro VARCHAR(MAX)
            )
RETURNS @RT TABLE
(
    OrdenacionCampo NVARCHAR(MAX),
    OrdenacionDireccion NVARCHAR(MAX)
)
AS
BEGIN


    INSERT INTO @RT
    SELECT OrdenacionCampo
        , OrdenacionDireccion
    FROM OPENJSON(@I_DefinicionFiltro)
    WITH
    (
        Ordenacion NVARCHAR(MAX) '$.Ordenacion' AS JSON
    )
    CROSS APPLY OPENJSON(Ordenacion, '$')
    WITH
    (
        OrdenacionCampo NVARCHAR(MAX) '$.Campo',
        OrdenacionDireccion NVARCHAR(MAX) '$.Direccion'
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
                defaultValue: new DateTime(2020, 11, 24, 11, 28, 4, 749, DateTimeKind.Local).AddTicks(5330),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 24, 11, 48, 42, 31, DateTimeKind.Local).AddTicks(9039));

            migrationBuilder.Sql(@"DROP FUNCTION [dbo].[DefinicionFiltro_ObtenerDatosOrdenacion]");
        }
    }
}
