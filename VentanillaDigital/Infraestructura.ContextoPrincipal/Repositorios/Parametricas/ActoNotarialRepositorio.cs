using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class ActoNotarialRepositorio : RepositorioBase<ActoNotarial>, IActoNotarialRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public ActoNotarialRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        public async Task<IEnumerable<ActoNotarial>> ObtenerTodosActosNotariales()
            => await _unidadTrabajoContextoPrincipal.ActosNotariales.Where(m => m.Activo).ToListAsync();

        public async Task<IEnumerable<ActoPorTramiteModel>> ObtenerActosPorTramite(long tramiteId)
        {
            return await (from t in _unidadTrabajoContextoPrincipal.TramitesPortalVirtual
                          join ap in _unidadTrabajoContextoPrincipal.ActosPorTramite on t.TramitesPortalVirtualId equals ap.TramitePortalVirtualId
                          join an in _unidadTrabajoContextoPrincipal.ActosNotariales on ap.ActoNotarialId equals an.ActoNotarialId
                          where t.TramitesPortalVirtualId == tramiteId
                          select new ActoPorTramiteModel
                          {
                              ActoNotarialId = ap.ActoNotarialId,
                              ActoNotarialNombre = $"{an.Codigo} - {an.Nombre}",
                              ActoPorTramiteId = ap.ActoPorTramiteId,
                              Cuandi = ap.Cuandi,
                              Fecha = ap.FechaCreacion
                          }).ToListAsync();
        }

        #endregion
        public async Task<int> ObtenerActoNotarialId(string codigo)
        {
            var actoNotarial= (await Obtener(x => x.Codigo == codigo)).FirstOrDefault();
            return (actoNotarial.ActoNotarialId);
        }
    }
}
