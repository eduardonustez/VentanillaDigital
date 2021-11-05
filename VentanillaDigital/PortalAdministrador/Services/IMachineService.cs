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
using ApiGateway.Contratos.Models;

namespace PortalAdministrador.Services
{
    public interface IMachineService
    {
        Task Register(NuevoMaquinaModel nuevoMaquina);
        Task<PaginableResponse<MaquinaConfiguracionReturn>> ObtenerConfiguracionesMaquina(ConfiguracionesNotariaRequest model);
    }
}
