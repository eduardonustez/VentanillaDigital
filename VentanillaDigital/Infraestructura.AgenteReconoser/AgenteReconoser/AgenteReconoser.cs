using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using RestSharp;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Infraestructura.AgenteReconoser
{
    public class AgenteReconoser : IAgenteReconoser
    {
        IHttpClientFactory _clientFactory;
        IConfiguration _configuration;


        public AgenteReconoser(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }


        public async Task<response> registrarUsuarioRnec(InputUserRnec inputUser)
        {
            var ConfigReconoser = _configuration.GetSection("ConfigServiciosReconoser");
            
            var json = JsonConvert.SerializeObject(inputUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = ConfigReconoser["Url"] +  "/api/parametrizacion/crearUsuarioRnec";
            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {
                client.DefaultRequestHeaders.Add("Authorization", ConfigReconoser["PrefijoAuth"] + " " + await obtenerToken());
                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                response respuesta = JsonConvert.DeserializeObject<response>(result);
                return respuesta;
            }

        }

        public async Task<response> registrarUsuarioMovilesRnec(InputUserMovilRnec inputUser)
        {
            var ConfigReconoser = _configuration.GetSection("ConfigServiciosReconoser");

            var json = JsonConvert.SerializeObject(inputUser);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = ConfigReconoser["Url"] + "/api/parametrizacion/crearUsuarioMoviles";
            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {
                client.DefaultRequestHeaders.Add("Authorization", ConfigReconoser["PrefijoAuth"] + " " + await obtenerToken());
                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                response respuesta = JsonConvert.DeserializeObject<response>(result);
                return respuesta;
            }

        }

        public async Task<response> registrarMaquinaRnec(InputMachineRnec inputMachine)
        {
            var ConfigReconoser = _configuration.GetSection("ConfigServiciosReconoser");

            var json = JsonConvert.SerializeObject(inputMachine);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = ConfigReconoser["Url"] + "/api/parametrizacion/crearMaquinaRnec";
            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {
                client.DefaultRequestHeaders.Add("Authorization", ConfigReconoser["PrefijoAuth"] + " " + await obtenerToken());
                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                response respuesta = JsonConvert.DeserializeObject<response>(result);
                return respuesta;
            }

        }

        private async Task<string> obtenerToken()
        {
            var ConfigReconoser = _configuration.GetSection("ConfigServiciosReconoser");

            var loginAuthRnec = new LoginAuthRnec();
            loginAuthRnec.Username = ConfigReconoser["UserTokenAuth"];
            loginAuthRnec.Password = ConfigReconoser["PassTokenAuth"];

            var json = JsonConvert.SerializeObject(loginAuthRnec);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = ConfigReconoser["Url"] + "/api/login/authenticate";

            using (var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted"))
            {

                var response = await client.PostAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                string Token = JsonConvert.DeserializeObject<string>(result);
                return Token;
            }
        }
    }




}
