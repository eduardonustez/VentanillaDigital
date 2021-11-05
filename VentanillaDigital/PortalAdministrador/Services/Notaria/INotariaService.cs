using PortalAdministrador.Services.Notaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Notaria
{
    public interface INotariaService
    {
        Task<ConfiguracionRNEC> ObtenerConfiguracionRNEC(long NotariaId);
        Task<IEnumerable<ApiGateway.Contratos.Models.NotariaClienteModel>> ObtenerNotariasCompleteAsync();
        Task<IEnumerable<ApiGateway.Contratos.Models.NotariaBasicModel>> ObtenerNotariasAsync();
    }
}
