using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface IPlantillaActaServicio:IDisposable
    {
        Task Crear(PlantillaActaCreateDTO plantillaActaCreateDTO);

        Task Actualizar(PlantillaActaEditDTO plantillaActaEditDTO);
        Task AsociarTiposTramites(PlantillaActaAssociateDTO plantillaActaAssociateDTO);
        Task<IEnumerable<PlantillaActaReturnDTO>> ObtenerPlantillas();
        Task<PlantillaActaFullReturnDTO> ObtenerPlantillaPorId(long Id);
    }
}
