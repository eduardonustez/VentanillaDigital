using Newtonsoft.Json;
using PortalAdministrador.Services.Notaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Notaria
{
    public class NotariaService : INotariaService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public NotariaService(ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }
        public async Task<ConfiguracionRNEC> ObtenerConfiguracionRNEC(long notariaId)
        {
            var resultado = await _customHttpClient.GetJsonAsync<ConfiguracionRNEC>($"notaria/ConfiguracionRNEC/{notariaId}");
            return resultado;
        }

        public async Task<IEnumerable<ApiGateway.Contratos.Models.NotariaClienteModel>> ObtenerNotariasCompleteAsync()
        {
            var resultado = await _customHttpClient.GetJsonAsync<IEnumerable<ApiGateway.Contratos.Models.NotariaClienteModel>>($"notaria/ObtenerNotarias");
            return resultado;
        }

        public async Task<IEnumerable<ApiGateway.Contratos.Models.NotariaBasicModel>> ObtenerNotariasAsync()
        {
            var resultado = await _customHttpClient.GetJsonAsync<IEnumerable<ApiGateway.Contratos.Models.NotariaBasicModel>>($"notaria");
            return resultado;
        }
    }
}
