using Microsoft.Extensions.Configuration;
using OTPClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace OTPClient
{
    public class OTPClient : IOTPClient
    {
        private readonly HttpClient _httpClient;
        private readonly Guid _codigoAplicacion;
        private readonly string _usuario;
        private readonly string _contrasena;

        public OTPClient (HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _codigoAplicacion = 
                new Guid(configuration.GetSection("ConfiguracionOTP:CodigoAplicacion").Value);
            _usuario = configuration.GetSection("ConfiguracionOTP:Usuario").Value;
            _contrasena = configuration.GetSection("ConfiguracionOTP:Contrasena").Value;
        }

        public async Task<OTPResponse> GenerarCodigoOTP(string correo, string celular)
        {
            var request = new OTPRequest()
            {
                CodigoAplicacion = _codigoAplicacion,
                Contrasena = _contrasena,
                Usuario = _usuario,
                Celular = celular,
                Correo = correo
            };
            var httpResponse = await _httpClient.PostAsJsonAsync("ConsultaGeneracionOTP", request);

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadFromJsonAsync<OTPResponse>();
            }
            else
            {
                throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<OTPValidationResponse> ValidacionOTP(Guid identificador, string textoOTP)
        {

            var request = new OTPValidationRequest()
            {
                CodigoAplicacion = _codigoAplicacion,
                Contrasena = _contrasena,
                Usuario = _usuario,
                Identificador = identificador,
                TextoOTP = textoOTP
            };
            var httpResponse = await _httpClient.PostAsJsonAsync("ConsultaValidacionOTP", request);

            if (httpResponse.IsSuccessStatusCode)
            {
                return await httpResponse.Content.ReadFromJsonAsync<OTPValidationResponse>();
            }
            else
            {
                throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }
    }
}
