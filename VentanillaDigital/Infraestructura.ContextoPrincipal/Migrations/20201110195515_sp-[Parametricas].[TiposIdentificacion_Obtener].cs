using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class spParametricasTiposIdentificacion_Obtener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 11, 10, 14, 55, 15, 234, DateTimeKind.Local).AddTicks(4935),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 11, 10, 12, 47, 44, 554, DateTimeKind.Local).AddTicks(2144));

            var sp = @"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
-----------------------------------------------
* Elaborado por     : Miguel Barahona
* Descripción       : Filtra los registros de la tabla Parametros.TiposIdentificacion, según los parametros especificados.
* Fecha creación    : Martes, 10 de noviembre de 2020
* Parámetros        : @TipoIdentificacionId: Filtro por el identificador
                      @Nombre: Filtro por el nombre del tipo de documento
					  @isDeleted: Filtro por eliminado logico
					  @Abreviatura: Filtro por abreviación del documento
-----------------------------------------------
* Modificado por    : 
* Fecha modificación: 
* Descripción       :
-----------------------------------------------
Test                :

    DECLARE
    @TipoIdentificacionId INT = null,
	@Nombre NVARCHAR(120) = null,
	@isDeleted bit = null,
	@Abreviatura NVARCHAR(120) = null;

	--SET
		--@TipoIdentificacionId = 1
		--@Nombre = 0,
		--@isDeleted = 0,
		--@Abreviatura = 0;

    EXEC [Parametricas].[TiposIdentificacion_Obtener] @TipoIdentificacionId, @Nombre, @isDeleted, @Abreviatura;

*/
CREATE PROCEDURE [Parametricas].[TiposIdentificacion_Obtener]
	-- Add the parameters for the stored procedure here
	(
		@TipoIdentificacionId INT = null,
		@Nombre NVARCHAR(120) = null,
		@isDeleted bit = null,
		@Abreviatura NVARCHAR(120) = null
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
			[TiposIdentificacion].[TipoIdentificacionId]
			,[TiposIdentificacion].[FechaCreacion]
			,[TiposIdentificacion].[FechaModificacion]
			,[TiposIdentificacion].[UsuarioCreacion]
			,[TiposIdentificacion].[UsuarioModificacion]
			,[TiposIdentificacion].[IsDeleted]
			,[TiposIdentificacion].[Nombre]
			,[TiposIdentificacion].[Abreviatura]
       FROM [Parametricas].[TiposIdentificacion]
       WHERE ((@TipoIdentificacionId IS NULL OR ([TiposIdentificacion].[TipoIdentificacionId] =@TipoIdentificacionId))
		AND (@Nombre IS NULL OR ([TiposIdentificacion].[Nombre] = @Nombre))
		AND (@isDeleted IS NULL OR ([TiposIdentificacion].[IsDeleted] = @isDeleted))
		AND (@Abreviatura IS NULL OR ([TiposIdentificacion].[Abreviatura] = @Abreviatura)))
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
                defaultValue: new DateTime(2020, 11, 10, 12, 47, 44, 554, DateTimeKind.Local).AddTicks(2144),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 11, 10, 14, 55, 15, 234, DateTimeKind.Local).AddTicks(4935));
        }
    }
}
