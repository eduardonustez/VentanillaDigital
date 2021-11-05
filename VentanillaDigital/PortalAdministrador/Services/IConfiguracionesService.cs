using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using ApiGateway.Contratos.Models.Notario;

namespace PortalAdministrador.Services
{
    public interface IConfiguracionesService
    {
        Task<OpcionesConfiguracion> ObtenerOpcionesConfiguracion();
        Task GuardarOpcionesConfiguracion(OpcionesConfiguracion opcionesConfiguracion);
        Task<List<NotarioReturnDTO>> ObtenerNotariosNotaria();
        Task<long> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO);

    }
}
