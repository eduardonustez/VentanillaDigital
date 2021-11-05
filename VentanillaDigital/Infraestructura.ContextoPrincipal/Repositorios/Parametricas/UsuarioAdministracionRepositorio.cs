using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class UsuarioAdministracionRepositorio : RepositorioBase<UsuarioAdministracion>, IUsuarioAdministracionRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public UsuarioAdministracionRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal,
            IHttpContextAccessor httpContext)
            : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }
        #endregion

        public Task<UsuarioAdministracion> ObtenerUsuarioAdministracion(string login)
            => (from u in _unidadTrabajoContextoPrincipal.UsuariosAdministracion where u.Login.Equals(login) && !u.IsDeleted select u).FirstOrDefaultAsync();
    }
}
