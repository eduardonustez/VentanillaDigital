using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class UpdatespParametricasPersonas_Obtener : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                schema: "Transaccional",
                table: "Tramites",
                nullable: false,
                defaultValue: new DateTime(2020, 12, 10, 10, 47, 27, 116, DateTimeKind.Local).AddTicks(4457),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2020, 12, 7, 15, 41, 47, 150, DateTimeKind.Local).AddTicks(8327));

            migrationBuilder.Sql(@"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
-----------------------------------------------
* Elaborado por     : Miguel Barahona
* Descripción       : Filtra los registros de la tabla Parametricas.Personas, según los parametros especificados.
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
	@NumeroDocumento NVARCHAR(40) = null,
	@isDeleted bit = null;

	--SET
		--@TipoIdentificacionId = 1,
		--@NumeroDocumento = N'101245254',
		--@isDeleted = 1,

    EXEC [Parametricas].[Personas_Obtener] @TipoIdentificacionId, @NumeroDocumento, @isDeleted;

*/
ALTER PROCEDURE [Parametricas].[Personas_Obtener]
	-- Add the parameters for the stored procedure here
	(
		@TipoIdentificacionId INT = null,
		@NumeroDocumento NVARCHAR(40) = null,
		@isDeleted bit = null
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
			Personas.PersonaId
			, Personas.FechaCreacion
			, Personas.FechaModificacion
			, Personas.UsuarioCreacion
			, Personas.UsuarioModificacion
			, Personas.IsDeleted
			, Personas.AspNetUserId
			, Personas.UserName
			, Personas.NumeroDocumento
			, Personas.Nombres
			, Personas.Apellidos
			, Personas.Email
			, Personas.NumeroCelular
			, Personas.TipoIdentificacionId
			, Personas.tokenAuth
       FROM [Parametricas].[Personas]
       WHERE ((@TipoIdentificacionId IS NULL OR ([Personas].[TipoIdentificacionId] = @TipoIdentificacionId))
		AND (@NumeroDocumento IS NULL OR ([Personas].[NumeroDocumento] = @NumeroDocumento))
		AND (@isDeleted IS NULL OR ([Personas].[IsDeleted] = @isDeleted)))

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
                defaultValue: new DateTime(2020, 12, 7, 15, 41, 47, 150, DateTimeKind.Local).AddTicks(8327),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2020, 12, 10, 10, 47, 27, 116, DateTimeKind.Local).AddTicks(4457));
            migrationBuilder.Sql(@"DROP PROCEDURE [Parametricas].[Personas_Obtener]");
        }
    }
}
