using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Infraestructura.AgenteServicios.AgenteANI
{
    public class AgenteANI:IAgenteANI
    {
        private HttpClient Client { get; set; }
        private ANIParametersModel parameters { get; set; }
        public AgenteANI(ANIParametersModel parametersModel)
        {
            //this.Client = client;
            parameters = parametersModel;
            
            string unencodedUsernameAndPassword = string.Format("{0}:{1}", parameters.Username, parameters.Password);
            byte[] unencodedBytes = ASCIIEncoding.ASCII.GetBytes(unencodedUsernameAndPassword);
            string base64UsernameAndPassword = System.Convert.ToBase64String(unencodedBytes);

            string authorizationHeaderValue = string.Format("Basic {0}", base64UsernameAndPassword);
            var httpClientHandler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            };

            Client = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(parameters.Uri),
                Timeout = TimeSpan.FromMinutes(1),
            };
            Client.DefaultRequestHeaders.Add("Authorization", authorizationHeaderValue);
            
        }
           

        public async Task<ANIResponseModel> ValidarPersona(int tipoDocumento,string documento)
        {
            var inputModel = new ANIInputModel()
            {
                Documento = documento,
                TipoDocumento = tipoDocumento,
                CodigoAplicacion = parameters.Aplicacion
            };
            var json = JsonConvert.SerializeObject(inputModel);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("/api/ValidacionAni", stringContent);
            string jsonData = await response.Content.ReadAsStringAsync();
            ANIResponseModel data = JsonConvert.DeserializeObject<ANIResponseModel>(jsonData);
            return data;
        }

    }

   

}
