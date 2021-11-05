using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using PortalCliente.Components.RegistroTramite;
using PortalCliente.Pages.NotarioPages;
using ApiGateway.Contratos.Models.Configuraciones;

namespace PortalCliente.Services.SignalR
{
    /// <summary>
    /// Agregar compatibilidad con SignalR version 2, 
    /// para SignalR Server en NET Frameworks
    /// </summary>
    public interface ISignalRv2Service
    {
        Task Initialize();
        Task<bool> EstadoSignalv2R();
        Task AgregarFuncionesNativas(DotNetObjectReference<DatosCiudadano> objRef);
        Task AgregarFuncionesNativas(DotNetObjectReference<Configuraciones> objRef);
        Task<List<string>> ObtenerEscanerVariable();
        Task ObtenerListaScanners();
        Task EnviarAEscanear(OpcionesScanner opciones);
    }
}
