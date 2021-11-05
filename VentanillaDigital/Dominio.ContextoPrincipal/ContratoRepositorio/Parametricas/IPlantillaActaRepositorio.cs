using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;

namespace Dominio.ContextoPrincipal.ContratoRepositorio
{
    public interface IPlantillaActaRepositorio : IRepositorioBase<PlantillaActa>, IDisposable
    {
        Task<PlantillaActa> ObtenerPlantillaActaPorTipoTramite(long tipoTramiteId);
        Task<PlantillaActa> ObtenerPlantillaStickerPorTipoTramite(long tipoTramiteId);
        Task<PlantillaActa> ObtenerPlantillaDosStickerPorTipoTramite(long tipoTramiteId);
    }
}
