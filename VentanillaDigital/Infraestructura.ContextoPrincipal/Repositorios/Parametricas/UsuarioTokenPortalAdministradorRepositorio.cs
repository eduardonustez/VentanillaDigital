using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class UsuarioTokenPortalAdministradorRepositorio : RepositorioBase<UsuarioTokenPortalAdministrador>, IUsuarioTokenPortalAdministradorRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public UsuarioTokenPortalAdministradorRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext)
            : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
        #endregion
    }
}
