using Dominio.ContextoPrincipal.ContratoRepositorio.Log;
using Dominio.ContextoPrincipal.Entidad.Log;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;

namespace Infraestructura.ContextoPrincipal.Repositorios.Log
{
    public class LogTramitePortalVirtualRepositorio : RepositorioBase<LogTramitePortalVirtual>, ILogTramitePortalVirtualRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        private IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        #endregion

        #region Constructor
        public LogTramitePortalVirtualRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext
            ) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));

        }
        #endregion
    }
}
