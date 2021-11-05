using System.Threading.Tasks;
using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Log.Modelo;
using System.Net.Http;
using System.Net.Http.Json;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace PortalCliente.Services
{
    public class TrazabilidadService : ITrazabilidadService
    {
        private readonly HttpClient _client;
        private ILocalStorageService _localStorageService;
        public TrazabilidadService(HttpClient client
            ,ILocalStorageService localStorageService)
        {
            _client = client;
            _localStorageService = localStorageService;
        }

        public async Task CrearTraza(InformationModel model)
        {
            await _client.PostAsJsonAsync("Trazabilidad/CrearTraza", model);
        }
        public async Task CrearExcepcion(ErrorModel model)
        {
            try
            {
                string token = await _localStorageService.GetItem<string>("token");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await _client.PostAsJsonAsync("Trazabilidad/CrearExcepcion", model);
            }
            catch (Exception ex) {
                Trace.WriteLine(ex.ToString());
            }

        }

    }

}
