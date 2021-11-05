using PortalCliente.Services.Biometria.Models;
using PortalCliente.Services.Biometria.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria
{
    public interface IRNECService
    {
        Task<int> Captura1(Dedo dedo);
        Task<int> Captura2(Dedo dedo);
        Task<ValidacionResponse> ValidarIdentidad(ValidacionRequest request);
        Task<ConsultarEstadoResponse> ConsultarEstado();
        Task ReiniciarCaptor();
    }
}
