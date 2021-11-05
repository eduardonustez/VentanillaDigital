using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Log.Modelo;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Contratos.Models.ActaNotarial;

namespace PortalCliente.Services
{
    public interface ITrazabilidadService
    {
        Task CrearTraza(InformationModel model);
        Task CrearExcepcion(ErrorModel model);
    }
}
