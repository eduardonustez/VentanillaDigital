using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using PortalCliente.Components.RegistroTramite;
using PortalCliente.Pages.NotarioPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.Configuraciones;

namespace PortalCliente.Services.SignalR
{
    public class SignalRv2Service: ISignalRv2Service, IDisposable
    {
        private readonly IConfiguration Configuration;
        private readonly IJSRuntime JSRuntime;

        public SignalRv2Service(IConfiguration configuration, IJSRuntime jSRuntime)
        {
            Configuration = configuration;
            JSRuntime = jSRuntime;
        }

        public async Task AgregarFuncionesNativas(DotNetObjectReference<DatosCiudadano> objRef)
        {
            await JSRuntime.InvokeVoidAsync("SCANNER_IMPL.SR_SetHelper", objRef);
        }

        public async Task Initialize()
        {
            Console.WriteLine("📇 Inicializar servicio");
            var hubDirection = Configuration["ConfiguracionServiciosAPI:AgenteSignalR2"];
            await JSRuntime.InvokeVoidAsync("setSignalR2URL", hubDirection);
            await JSRuntime.InvokeVoidAsync("signalRv2_IsConnected");
        }

        public async Task ObtenerListaScanners()
        {
            await JSRuntime.InvokeVoidAsync("obtenerDispositivosTwain");
        }

        public async Task<List<string>> ObtenerEscanerVariable()
        {
            return await JSRuntime.InvokeAsync<List<string>>("obtenerEscanerVariable");
        }

        public async Task EnviarAEscanear(OpcionesScanner opciones)
        {
            await JSRuntime.InvokeVoidAsync("enviarAEscanear", opciones);
        }

        public async Task AgregarFuncionesNativas(DotNetObjectReference<Configuraciones> objRef)
        {
            await JSRuntime.InvokeVoidAsync("SCANNER_IMPL.SetConfigScannerHelper", objRef);
        }
        public async void Dispose()
        {
            await JSRuntime.InvokeVoidAsync("SignalRv2_Disconnect");
        }

        public async Task<bool> EstadoSignalv2R()
        {
            return await JSRuntime.InvokeAsync<bool>("SR_IsConnected");
        }
    }
}
