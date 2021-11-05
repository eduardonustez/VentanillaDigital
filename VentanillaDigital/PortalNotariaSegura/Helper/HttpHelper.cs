using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Notariado.Helper
{
    
    public class HttpHelper : IHttpHelper
    {
        public static IConfiguration _configuration;

        public HttpHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }




        public async Task<HttpResponseMessage> ConsumirServicioRest<T>(string serviceUrl, HttpMethod Metodo,
            T content)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var request = new HttpRequestMessage(Metodo, serviceUrl);

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(content.ToString()))
                    {
                        string json = JsonConvert.SerializeObject(content);
                        dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(json);

                        string json2 = JsonConvert.SerializeObject(obj);

                        HttpContent httpContent = new StringContent(json2, Encoding.UTF8, "application/json"); ;
                        request.Content = httpContent;
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    }

                    HttpResponseMessage asyncRes = await client.SendAsync(request);

                    return asyncRes;
                }
            }


        }

    }
}
