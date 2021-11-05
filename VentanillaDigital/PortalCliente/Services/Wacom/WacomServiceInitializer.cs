using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services.Recursos;
using PortalCliente.Services.Biometria;
namespace PortalCliente.Services.Wacom
{
    public class WacomServiceInitializer : IAsyncInitializer
    {
        private WacomService _wacomService { get; set; }
        private WacomAgenteService _wacomAgenteService { get; set; }
        private IConfiguracionesService _configuracionesService { get; set; }
        private IRNECService _rnecService { get; set; }
        public WacomServiceInitializer(WacomService wacomService,WacomAgenteService wacomAgenteService,
            IConfiguracionesService configuracionesService, IRNECService rnecService)
        {
            _wacomService = wacomService;
            _wacomAgenteService = wacomAgenteService;
           _configuracionesService = configuracionesService;
            _rnecService = rnecService;
        }
        public async Task InitializeAsync()
        {
            string channelId =await _configuracionesService.GetWacomChannelId(); 
            _wacomService.FondoInactivo =_wacomAgenteService.FondoInactivo= "images/Pad/Bienvenida.jpg";
            var datosEquipo = await _rnecService.ConsultarEstado();
            string WacomSTUSigCaptX = "";
            string IsWacomDllRegistered = "0";
            if (datosEquipo != null && datosEquipo.Estado == "OK")
            {
                WacomSTUSigCaptX = datosEquipo.Propiedades.FirstOrDefault(d => d.Key == "WacomSTUSigCaptX").Value;
                IsWacomDllRegistered = datosEquipo.Propiedades.FirstOrDefault(d => d.Key == "IsWacomDllRegistered").Value;
            }
            Console.WriteLine($"🚨🚨🚨Inicializando servicio Wacom {WacomSTUSigCaptX}");

            if (WacomSTUSigCaptX == "NotInstalled" || WacomSTUSigCaptX == "Stopped")
            {
                await _configuracionesService.SetWacomChannel("2");
                await _wacomAgenteService.Initialize();
            }
            else if (channelId == "1")
            {
                await _wacomAgenteService.Initialize(false);
                await _wacomService.Initialize();
            }
            else if (channelId == "2")
            {
                await _wacomAgenteService.Initialize();
                await _wacomService.Initialize(false);
            }
        }
    }
}
