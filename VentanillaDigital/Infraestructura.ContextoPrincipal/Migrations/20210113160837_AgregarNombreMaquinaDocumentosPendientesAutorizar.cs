using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructura.ContextoPrincipal.Migrations
{
    public partial class AgregarNombreMaquinaDocumentosPendientesAutorizar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MachineName",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar",
                unicode: false,
                maxLength: 150,
                nullable: true);

            migrationBuilder.Sql(@"-- =============================================
-- Author:		Diego Roldán
-- Create date: 2021-01-13
-- Description:	Actualiza el estado de los registros a recorrer devolviendo los ids
-- =============================================
CREATE PROCEDURE [Transaccional].[ObtenerDocumentosPendientesAutorizar]
	@Cantidad Int,
    @MachineName Varchar(150)
AS
BEGIN
	SET NOCOUNT ON;

    begin tran tt

	begin try

		update top (@Cantidad) dp set dp.Estado = 2, dp.Intentos += 1, dp.MachineName = @MachineName
		output inserted.DocumentoPendienteAutorizarId
		from [Transaccional].[DocumentosPendienteAutorizar] dp
		where dp.Estado = 1

		commit tran tt
	end try
	begin catch
		rollback tran tt
	end catch

end
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineName",
                schema: "Transaccional",
                table: "DocumentosPendienteAutorizar");
            migrationBuilder.Sql(@"DROP PROCEDURE [Transaccional].[ObtenerDocumentosPendientesAutorizar]");
        }
    }
}
