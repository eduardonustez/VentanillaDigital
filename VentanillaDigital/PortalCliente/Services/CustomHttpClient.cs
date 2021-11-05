using ApiGateway.Models;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using PortalCliente.Services.Biometria;
using PortalCliente.Services.DescriptorCliente;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public class CustomHttpClient : ICustomHttpClient
    {
        private readonly HttpClient _httpClient;
        private ILocalStorageService _localStorageService;
        private AuthenticationStateProvider _authenticationStateProvider;
        private string token = "";
        private readonly IRNECService _rnecService;
        private readonly IDescriptorCliente _descriptorCliente;
        public CustomHttpClient(HttpClient httpClient, ILocalStorageService localStorageService
            , AuthenticationStateProvider AuthenticationStateProvider, IRNECService rnecService, IDescriptorCliente descriptorCliente)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _authenticationStateProvider = AuthenticationStateProvider;
            _rnecService = rnecService;
            _descriptorCliente = descriptorCliente;
        }
        public async Task<T> GetJsonAsync<T>(string requestUri)
        {
            try
            {
                var httpContent = await SendJsonAsync(HttpMethod.Get, requestUri);
                if (httpContent.StatusCode == HttpStatusCode.OK)
                {
                    string jsonContent = await httpContent.Content.ReadAsStringAsync();
                    T obj = JsonConvert.DeserializeObject<T>(jsonContent);
                    httpContent.Dispose();
                    return obj;
                }
                return default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetStringAsync(string requestUri)
        {
            try
            {
                var httpContent = await SendJsonAsync(HttpMethod.Get, requestUri);
                if (httpContent.StatusCode == HttpStatusCode.OK)
                {
                    return await httpContent.Content.ReadAsStringAsync();
                }
                return default;
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        public async Task<T> PostJsonAsync<T>(string requestUri, object data, CancellationToken cancellationToken =  default(System.Threading.CancellationToken))
        {
            var response = await SendJsonAsync(HttpMethod.Post, requestUri, data, cancellationToken);
            var res = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonObj = JsonConvert.DeserializeObject<T>(res);
                return jsonObj;
            }
            else
            {
                var jsonObj = JsonConvert.DeserializeObject<ErroresDTO>(res);
                if (jsonObj != null)
                    throw new Exception(String.Join('\n', jsonObj.Errors));
            }
            return default;
        }

        public async Task<T> PostFormDataAsync<T>(string requestUri, MultipartFormDataContent content, AuthenticationHeaderValue authenticationHeader)
        {
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeader;
            var response = await _httpClient.PostAsync(requestUri, content);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(res);
            }
            else
            {
                return default;
            }
        }

        public async Task<HttpResponseMessage> SendJsonAsync(HttpMethod method, string requestUri, object data = null, CancellationToken cancellationToken =  default(System.Threading.CancellationToken))
        {
            token = await ((CustomAuthenticationStateProvider)_authenticationStateProvider).GetTokenFromAuthenticatedUser();
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string registeredMac = await _localStorageService.GetItem<string>("REGISTERED-MACHINE-MAC");
            string registeredIp = await _localStorageService.GetItem<string>("REGISTERED-MACHINE-IP");

            if (string.IsNullOrWhiteSpace(registeredMac))
            {
                var datosEquipo = await _rnecService.ConsultarEstado();
                if (datosEquipo != null && datosEquipo.Estado == "OK")
                {
                    registeredMac = datosEquipo.Propiedades.FirstOrDefault(d => d.Key == "MacAddress").Value;
                    registeredIp = datosEquipo.Propiedades.FirstOrDefault(d => d.Key == "IPLocal").Value;
                    await _localStorageService.SetItem<string>("REGISTERED-MACHINE-MAC", registeredMac);
                    await _localStorageService.SetItem<string>("REGISTERED-MACHINE-IP", registeredIp); //siempre actualizamos la IP, porque esta puede cambiar
                }
            }


            _httpClient.DefaultRequestHeaders.Add("ipAddress", registeredIp);
            _httpClient.DefaultRequestHeaders.Add("macAddress", registeredMac);
            var message = new HttpRequestMessage(method, requestUri);

            if (data != null)
            {
                var json = JsonConvert.SerializeObject(data);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

                message.Content = stringContent;
            }

            var ret = await _httpClient.SendAsync(message, cancellationToken);


            if (ret.StatusCode == HttpStatusCode.BadRequest)
            {
                string jsonContent = await ret.Content.ReadAsStringAsync();
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(jsonContent);
                ret.Dispose();
                ret = null;
                if (error.Errors.Any(s => s.Equals("Invalid Token")))
                {
                    await ((CustomAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                }
                else
                {
                    throw new Exception(string.Join('\n', error.Errors));
                }
            }

            return ret;
        }
    }
}
