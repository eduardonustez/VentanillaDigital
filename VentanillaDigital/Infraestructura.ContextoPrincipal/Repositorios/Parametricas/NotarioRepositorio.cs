using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.StoredProcedures;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.ContratoRepositorio;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class NotarioRepositorio : RepositorioBase<Notario>, INotarioRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;


        #endregion

        #region Constructor
        public NotarioRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
        #endregion

        #region Contratos
        public async Task<bool> NotariaActiva(long NotariaId)
        {
            bool esNotariaActiva = await _unidadTrabajoContextoPrincipal.Notaria.Where(x => x.IsDeleted == false && x.NotariaId == NotariaId).AnyAsync().ConfigureAwait(false);

            return esNotariaActiva;
        }


        public async Task<int> ActualizarNotario(Notario Notario)
        {
            //Metodo para actualizar la informacion  de la tabla notario
            int respuesta = 0;
            try
            {
                _unidadTrabajoContextoPrincipal.SetModified(Notario);
                var actualizacionNotario = await _unidadTrabajoContextoPrincipal.CommitAsync().ConfigureAwait(false);
                if (actualizacionNotario > 0)
                {
                    respuesta = actualizacionNotario;
                }
                return respuesta;
            }
            catch (Exception)
            {
                return respuesta;
            }
        }

        public async Task<RespuestaProcedimientoViewModel> ObtenerNotariosPaginado(DefinicionFiltro definicionFiltro)
        {

            var Respuesta = new RespuestaProcedimientoViewModel();

            var oResultado = new SqlParameter("@O_Resultado", SqlDbType.NVarChar);
            oResultado.SqlDbType = SqlDbType.NVarChar;
            oResultado.Size = int.MaxValue;
            oResultado.Direction = ParameterDirection.Output;
            oResultado.DbType = DbType.String;

            var oTotalRegistros = new SqlParameter("@O_TotalRegistros", SqlDbType.BigInt);
            oTotalRegistros.Direction = ParameterDirection.Output;

            _unidadTrabajoContextoPrincipal.Database
                .ExecuteSqlRaw("EXEC [Transaccional].[Notarios_Obtener2] @I_DefinicionFiltro, @O_TotalRegistros OUTPUT, @O_Resultado OUTPUT",
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
            //throw new ApplicationException("petición sin datos");


            return Respuesta;
        }

        public async Task<Notario> ObtenerNotarioPrincipal(long notariaId, string[] includes = null)
        {
            int dos = 2;
            var query = (from n in _unidadTrabajoContextoPrincipal.Notario
                         join nu in _unidadTrabajoContextoPrincipal.NotariaUsuarios on n.NotariaUsuariosId equals nu.NotariaUsuariosId
                         where nu.NotariaId == notariaId && !n.IsDeleted && n.TipoNotario == dos
                         select n);

            if (includes != null)
            {
                includes.ToList().ForEach(i => query = query.Include(i));
            }

            return await query.FirstOrDefaultAsync();
        }
        #endregion
    }
}
