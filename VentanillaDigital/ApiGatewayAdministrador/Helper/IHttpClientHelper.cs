using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Helper
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> ConsumirServicioRest<T>(string serviceUrl, HttpMethod Metodo, T content);
        ClaimModel GetClaims();
        string GetToken();
      
    }
}
