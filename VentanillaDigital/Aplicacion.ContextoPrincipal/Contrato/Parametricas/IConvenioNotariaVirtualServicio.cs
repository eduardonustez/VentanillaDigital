using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Parametricas
{
    public interface IConvenioNotariaVirtualServicio : IDisposable
    {
        Task<bool> ObtenerNotariaVirtualConvenio(ConvenioNotariaVirtualDTO convenioNotaria);
        Task<List<EstadosTramitesVirtualResponseDTO>> ObtenerEstadosTramiteVirtual(EstadosTramiteVirtualDTO estadosTramite);
        Task<ConfiguracionMiFirmaDTO> ObtenerMiConfiguracionMiFirma(ConvenioNotariaVirtualDTO convenioNotaria);
    }
}
