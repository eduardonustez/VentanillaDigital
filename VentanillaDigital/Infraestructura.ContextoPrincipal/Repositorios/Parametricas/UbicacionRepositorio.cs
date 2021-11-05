using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Dominio.Nucleo.Entidad;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Nucleo;
using Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class UbicacionRepositorio : RepositorioBase<Ubicacion>, IUbicacionRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion

        #region Constructor
        public UbicacionRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal) : base(unidadTrabajoContextoPrincipal)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion
    }
}
