using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Transaccional
{
    public class ArchivosPortalVirtualRepositorio : RepositorioBase<ArchivosPortalVirtual>, IArchivosPortalVirtualRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public ArchivosPortalVirtualRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }


        #endregion

        #region Contratos
        public async Task<string> ObtenerArchivoPortalVirtual(long ArchivoPortalVirtualId)
        {
            string resultado;
            try
            {
                var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
                oResultado.SqlDbType = SqlDbType.NVarChar;
                oResultado.Size = int.MaxValue;
                oResultado.Direction = ParameterDirection.Output;
                oResultado.DbType = DbType.String;

                await _unidadTrabajoContextoPrincipal.Database
                    .ExecuteSqlRawAsync("EXEC [Transaccional].[ObtenerArchivoPortalVirtual]  @ArchivoId, @O_Resultado OUTPUT",
                    new SqlParameter("@ArchivoId", ArchivoPortalVirtualId), oResultado);
                if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
                {
                    resultado = oResultado.Value.ToString();
                }
                else
                {
                    resultado = "[]";
                }
            }
            catch (Exception ex)
            {
                resultado = "[]";
                return resultado;
            }
            return resultado;
        }

        #endregion
    }
}
