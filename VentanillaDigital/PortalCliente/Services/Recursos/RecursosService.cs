using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace PortalCliente.Services.Recursos
{
    public class RecursosService: IRecursosService
    {
        public HttpClient HttpClient { get; set; }
        public RecursosService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<string> ObtenerRecurso (string url)
        {
            if(string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("El nombre del recurso no puede ser vacio",nameof(url));
            }
            var res = await HttpClient.GetAsync(url);
            if(res.IsSuccessStatusCode)
            {
                var bytes = await res.Content.ReadAsByteArrayAsync();
                return $"data:{res.Content.Headers.ContentType};base64,{Convert.ToBase64String(bytes)}";
            }
            else if(res.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new ArgumentException($"Recurso no encontrado {url}");
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new ApplicationException($"Permiso denegado para el recurso {url}");
            }
            else if (res.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            {
                throw new ApplicationException($"Tiempo de espera excedido");
            }
            else
            {
                throw new ApplicationException($"Error desconocido en el request: {res.StatusCode}");
            }
        }
    }
}
