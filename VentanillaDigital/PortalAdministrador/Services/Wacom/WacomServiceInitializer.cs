using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalAdministrador.Services.Recursos;

namespace PortalAdministrador.Services.Wacom
{
    public class WacomServiceInitializer : IAsyncInitializer
    {
        private WacomService WacomService { get; set; }
        public WacomServiceInitializer(WacomService wacomService,
            IRecursosService recursosService)
        {
            WacomService = wacomService;
        }
        public async Task InitializeAsync()
        {
            WacomService.FondoInactivo = "images/Pad/Bienvenida.jpg";
            await WacomService.Initialize();
        }
    }
}
