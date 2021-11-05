using PortalCliente.Services.Notaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Notaria
{
    public interface INotariaService
    {
        Task<ConfiguracionRNEC> ObtenerConfiguracionRNEC(long NotariaId);
        Task<DespachoNotaria> ObtenerNotariaPorId(long NotariaId);

    }
}
