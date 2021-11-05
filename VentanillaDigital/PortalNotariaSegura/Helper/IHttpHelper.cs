using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Notariado.Helper
{
    public interface IHttpHelper
    {
        Task<HttpResponseMessage> ConsumirServicioRest<T>(string serviceUrl, HttpMethod Metodo, T content);
    }
}
