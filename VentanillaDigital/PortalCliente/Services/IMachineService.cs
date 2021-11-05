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

namespace PortalCliente.Services
{
    public interface IMachineService
    {
        
        Task Register(NuevoMaquinaModel nuevoMaquina);
        Task<MaquinaConfiguracionReturn> Consultar(string mac);

    }
}
