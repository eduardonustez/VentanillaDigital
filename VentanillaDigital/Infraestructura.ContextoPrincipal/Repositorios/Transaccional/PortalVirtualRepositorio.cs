using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Transaccional
{
    public class PortalVirtualRepositorio : RepositorioBase<TramitesPortalVirtual>, IPortalVirtualRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        private IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        #endregion

        #region Constructor
        public PortalVirtualRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext
            ) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));

        }

        public async Task<RespuestaProcedimientoViewModel> ObtenerTramitePortalVirtual(DefinicionFiltro definicionFiltro)
        {
            try
            {
                var Respuesta = new RespuestaProcedimientoViewModel();

                var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
                oResultado.SqlDbType = SqlDbType.NVarChar;
                oResultado.Size = int.MaxValue;
                oResultado.Direction = ParameterDirection.Output;
                oResultado.DbType = DbType.String;

                var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
                oTotalRegistros.Direction = ParameterDirection.Output;

                await _unidadTrabajoContextoPrincipal.Database
                    .ExecuteSqlRawAsync("EXEC [Transaccional].[Tramites_ObtenerTPortalVirual] @I_DefinicionFiltro, @O_TotalRegistros OUTPUT, @O_Resultado OUTPUT",
                    new SqlParameter("@I_DefinicionFiltro", JsonConvert.SerializeObject(definicionFiltro)), oTotalRegistros, oResultado);


                if (!string.IsNullOrEmpty(oResultado.Value.ToString()))
                {
                    Respuesta.Resultado = oResultado.Value.ToString();
                    Respuesta.TotalRegistros = (long)oTotalRegistros.Value;
                    Respuesta.TotalPaginas = (long)Math.Ceiling((float)Respuesta.TotalRegistros / (float)definicionFiltro.RegistrosPagina);
                }
                else
                {
                    Respuesta.Resultado = "[]";
                    Respuesta.TotalRegistros = 0;
                }

                return Respuesta;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #endregion
    }
}
