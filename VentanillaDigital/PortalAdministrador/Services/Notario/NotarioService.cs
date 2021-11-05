using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Notario
{
    public class NotarioService : INotarioService
    {
        private readonly ICustomHttpClient _customHttpClient;
        public NotarioService(ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }
        public async Task<bool> ConfigurarFirmaPin(string email, string pin, string grafo)
        {
            NotarioDTOModel model = new NotarioDTOModel { Email = email, Pin = pin, Grafo = grafo };
            var resultado = await _customHttpClient.PostJsonAsync<bool>("Notario/ActualizarGrafoPinNotario", model);
            return resultado;

        }
        public async Task<bool> SeleccionarFormatoImpresion(bool UsarSticker)
        {
            var resultado = await _customHttpClient.PostJsonAsync<bool>("Notario/SeleccionarFormatoImpresion/" + UsarSticker,"");
            return resultado;

        }

        public async Task<EstadoPinFirmaModel> ObtenerEstadoPinFirma(string email)
        {
            var resultado = await _customHttpClient.GetJsonAsync<EstadoPinFirmaModel>("Notario/ObtenerEstadoPinFirma/"+ email);
            return resultado;
        }
        public async Task<OpcionesConfiguracioNotarioModel> ObtenerOpcionesConfiguracion(string email)
        {
            var resultado = await _customHttpClient.GetJsonAsync<OpcionesConfiguracioNotarioModel>("Notario/ObtenerOpcionesConfiguracion/" + email);
            return resultado;
        }

        public Task<string> ObtenerGrafo(string email)
        {
            return _customHttpClient.GetStringAsync("Notario/ObtenerGrafo/" + email);
        }
    }
}
