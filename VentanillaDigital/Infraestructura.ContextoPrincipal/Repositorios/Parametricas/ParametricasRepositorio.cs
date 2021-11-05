using Dominio.ContextoPrincipal.ContratoRepositorio;
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
    public class ParametricaRepositorio : RepositorioBase<Parametrica>, IParametricaRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public ParametricaRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion
    }
}
