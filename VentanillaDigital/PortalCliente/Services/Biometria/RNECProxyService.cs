using Microsoft.JSInterop;
using PortalCliente.Services.Biometria.Models;
using PortalCliente.Services.Biometria.Models.Internal;
using PortalCliente.Services.DescriptorCliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria
{
    public class RNECProxyService : IRNECService
    {
        private IDescriptorCliente DescriptorCliente { get; set; }
        private IRNECService RNECService { get; set; }
        private RNECService ServicioFijas { get; set; }
        private RNECMovilService ServicioMoviles { get; set; }

        private async Task SelectRNECService()
        {
            if (RNECService != null) return;
            bool esMovil = false;
            try
            {
                esMovil = await DescriptorCliente.EsMovil;
            }
            finally
            {
                if (esMovil)
                {
                    RNECService = ServicioMoviles;
                }
                else
                {
                    RNECService = ServicioFijas;
                }
            }
        }

        public RNECProxyService(RNECService servicioFijas,
            RNECMovilService servicioMoviles,
            IDescriptorCliente descriptorCliente)
        {
            ServicioFijas = servicioFijas;
            ServicioMoviles = servicioMoviles;
            DescriptorCliente = descriptorCliente;
        }

        public Task<int> Captura1(Dedo dedo)
        {
            return SelectRNECService()
                .ContinueWith(t => RNECService.Captura1(dedo))
                .Unwrap();
        }

        public Task<int> Captura2(Dedo dedo)
        {
            return SelectRNECService()
                .ContinueWith(t => RNECService.Captura2(dedo))
                .Unwrap();
        }

        public Task<ConsultarEstadoResponse> ConsultarEstado()
        {
            return SelectRNECService()
                .ContinueWith(t => RNECService.ConsultarEstado())
                .Unwrap();
        }

        public Task<ValidacionResponse> ValidarIdentidad(ValidacionRequest request)
        {
            return SelectRNECService()
                .ContinueWith(t => RNECService.ValidarIdentidad(request))
                .Unwrap();
        }

        public Task ReiniciarCaptor()
        {
            return SelectRNECService()
                .ContinueWith(t => RNECService.ReiniciarCaptor())
                .Unwrap();
        }
    }
}
