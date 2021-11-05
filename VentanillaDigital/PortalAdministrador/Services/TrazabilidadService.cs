using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Log.Modelo;

namespace PortalAdministrador.Services
{
    public class TrazabilidadService : ITrazabilidadService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public TrazabilidadService(ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }

        public async Task CrearTraza(InformationModel model)
        {
            await _customHttpClient.PostJsonAsync<string>("Trazabilidad/CrearTraza", model);
        }
    }
}
