using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public interface ICustomHttpClient
    {
        Task<T> GetJsonAsync<T>(string requestUri);
        Task<string> GetStringAsync(string requestUri);
        Task<T> PostJsonAsync<T>(string requestUri, object data, System.Threading.CancellationToken cancellationToken =  default(System.Threading.CancellationToken));
        Task<T> PostFormDataAsync<T>(string requestUri, MultipartFormDataContent content, AuthenticationHeaderValue authenticationHeader);
        Task<HttpResponseMessage> SendJsonAsync(HttpMethod method, string requestUri, object data = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
