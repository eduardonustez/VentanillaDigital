using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios.Parametricas
{
    public class PlantillaActaRepositorio : RepositorioBase<PlantillaActa>, IPlantillaActaRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        public IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;
        #endregion
        #region Constructor

        public PlantillaActaRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, IHttpContextAccessor httpContext) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
        }

        #endregion

        public Task<PlantillaActa> ObtenerPlantillaActaPorTipoTramite(long tipoTramiteId)
            => (from pa in _unidadTrabajoContextoPrincipal.PlantillaActa
                join tt in _unidadTrabajoContextoPrincipal.TipoTramite on pa.PlantillaActaId equals tt.PlantillaActaId
                select pa).FirstOrDefaultAsync();

        public Task<PlantillaActa> ObtenerPlantillaStickerPorTipoTramite(long tipoTramiteId)
            => (from pa in _unidadTrabajoContextoPrincipal.PlantillaActa
                join tt in _unidadTrabajoContextoPrincipal.TipoTramite on pa.PlantillaActaId equals tt.PlantillaStickerId
                select pa).FirstOrDefaultAsync();

        public Task<PlantillaActa> ObtenerPlantillaDosStickerPorTipoTramite(long tipoTramiteId)
            => (from pa in _unidadTrabajoContextoPrincipal.PlantillaActa
                join tt in _unidadTrabajoContextoPrincipal.TipoTramite on pa.PlantillaActaId equals tt.PlantillaDosStickerId
                select pa).FirstOrDefaultAsync();
    }
}
