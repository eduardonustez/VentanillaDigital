using Newtonsoft.Json;
using PortalCliente.Services.Notaria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortalCliente.Services.Notaria
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

        public async Task<DespachoNotaria> ObtenerNotariaPorId(long NotariaId)
        {
            var resultado = await _customHttpClient.GetJsonAsync<DespachoNotaria>($"notaria/ObtenerNotariaPorId/{NotariaId}");
            return resultado;
        }
    }
}
