using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Nucleo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios
{

    public class ComparecienteRepositorio : RepositorioBase<Compareciente>, IComparecienteRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public ComparecienteRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion

        #region Contratos
        public async Task<List<Compareciente>> ObtenerComparecientesPorTramiteID(long tramiteId)
        {
            FormattableString query = $"EXECUTE [Transaccional].[Comparecientes_Por_TramiteID_Obtener] {tramiteId}";
            var comparecientes = await _unidadTrabajoContextoPrincipal.Compareciente.FromSqlInterpolated(query).ToListAsync();
            return comparecientes;
        }

        public Persona ObtenerPersona(long? TipoIdentificacionId, string NumeroDocumento, bool? esEliminado)
        {
            var persona = _unidadTrabajoContextoPrincipal.Persona.Where(x => x.TipoIdentificacionId == TipoIdentificacionId && x.NumeroDocumento == NumeroDocumento).FirstOrDefault();
            return persona;
        }

        public int ActualizarPersona(Persona PersonaUpdate)
        {
            int respuesta = 0;
            try
            {
                PersonaUpdate.IsDeleted = false;
                _unidadTrabajoContextoPrincipal.SetModified(PersonaUpdate);
                var actualizado = _unidadTrabajoContextoPrincipal.Commit();
                if (actualizado > 0)
                {
                    respuesta = actualizado;
                }
                return respuesta;
            }
            catch (Exception)
            {
                return respuesta;
            }
        }

        public int ActualizarEstadoTramite(Tramite tramite)
        {
            int respuesta = 0;
            try
            {
                _unidadTrabajoContextoPrincipal.SetModified(tramite);
                var actualizado = _unidadTrabajoContextoPrincipal.Commit();
                if (actualizado > 0)
                {
                    respuesta = actualizado;
                }
                return respuesta;
            }
            catch (Exception)
            {
                return respuesta;
            }
        }

        public async Task<EstadoTramite> ObtenerEstadoTramite(string nombreEstado)
        {
            EstadoTramite respuestaEstado = new EstadoTramite();
            var obtenerEstadoTramite = await _unidadTrabajoContextoPrincipal.EstadoTramite.Where(x => x.Nombre == nombreEstado).AnyAsync().ConfigureAwait(false);
            if (obtenerEstadoTramite)
            {
                respuestaEstado = await _unidadTrabajoContextoPrincipal.EstadoTramite.Where(x => x.Nombre == nombreEstado).AsNoTracking().FirstOrDefaultAsync();
            }
            return respuestaEstado;
        }

        #endregion
    }
}
