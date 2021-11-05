using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using PortalCliente.Services.Recursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services;

namespace PortalCliente.Services.Wacom
{
    public class WacomServiceFactory:IWacomServiceFactory
    {
        public WacomService _wacomService { get; set; }
        public WacomAgenteService _wacomAgenteService { get; set; }
        public WacomServiceFactory(WacomService wacomService,
            WacomAgenteService wacomAgenteService)
        {
            _wacomService= wacomService;
            _wacomAgenteService = wacomAgenteService;
        }
        public async Task<IWacomService> GetWacomServiceInstance(string wacomChannelId) 
        {
            if (wacomChannelId == "2")
               return _wacomAgenteService;
            else
                return _wacomService;
        }
    }
}
