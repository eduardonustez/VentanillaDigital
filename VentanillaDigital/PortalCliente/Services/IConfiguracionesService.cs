using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Contratos.Models.Configuraciones;

namespace PortalCliente.Services
{
    public interface IConfiguracionesService
    {
        Task<OpcionesConfiguracion> ObtenerOpcionesConfiguracion();
        Task GuardarOpcionesConfiguracion(OpcionesConfiguracion opcionesConfiguracion);
        Task<List<NotarioReturnDTO>> ObtenerNotariosNotaria();
        Task<long> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO);
        Task SetUseTablet(ConfigTabletViewModel useTablet);
        Task<ConfigTabletViewModel> GetUseTablet();
        Task SetConfigScanner(ScannerConfigModel scannerConfig);
        Task<ScannerConfigModel> GetConfigScanner();
        Task<string> GetWacomChannelId();
        Task SetWacomChannel(string wacomChannelId);
    }
}
