using ApiGateway.Contratos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Helper
{
    public class HttpClientGeneric<T, TIn> : IDisposable where T : class
    {
        protected readonly string serviceBaseAddress;
        private bool disposed = false;
        private HttpClient httpClient;
        private readonly string jsonMediaType = "application/json";
        public HttpClientGeneric(DataHttpClient dataHttp)
        {
            serviceBaseAddress = dataHttp.ServiceBaseAddress;
            httpClient = CrearHttpClient(dataHttp);
        }
        public async Task<HttpResponseMessage> PostAsync(TIn model)
        {
            var objectContent = CreateJsonObjectContent(model);
            HttpResponseMessage responseMessage = await httpClient.PostAsync(serviceBaseAddress, objectContent);
            return responseMessage;
        }
        public async Task<HttpResponseMessage> GetAsync()
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync(serviceBaseAddress);
           return responseMessage;
        }

        protected virtual HttpClient CrearHttpClient(
               DataHttpClient dataHttp
            )
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(dataHttp.ServiceBaseAddress);
            httpClient.DefaultRequestHeaders.Clear();

            if (dataHttp.TipoTokenBasic == "Basic")
                httpClient.DefaultRequestHeaders.Add(dataHttp.NombreTokenBasic, dataHttp.TipoTokenBasic + " " + dataHttp.TokenBasic);

            if (dataHttp.TipoTokenBearer == "Bearer")
                httpClient.DefaultRequestHeaders.Add(dataHttp.NombreTokenBearer, dataHttp.TipoTokenBearer + " " + dataHttp.TokenBearer);

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private StringContent CreateJsonObjectContent(TIn model)
        {
            return new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                if (httpClient != null)
                {
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }
                disposed = true;
            }
        }
        #endregion IDisposable
    }
}
