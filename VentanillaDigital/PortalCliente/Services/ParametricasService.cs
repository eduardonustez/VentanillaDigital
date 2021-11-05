using ApiGateway.Models;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PortalCliente.Services
{
    public class ParametricasService : IParametricasService
    {
        private readonly ICustomHttpClient _customHttpClient;
        private readonly ISessionStorageService _sessionStorageService;

        private Categoria[] _listaCategorias;

        public ParametricasService(ICustomHttpClient customHttpClient, ISessionStorageService sessionStorageService)
        {
            _customHttpClient = customHttpClient;
            _sessionStorageService = sessionStorageService;
        }

        public async Task<ApiGateway.Models.City[]> GetCities()
        {
            var resultado = await _customHttpClient.GetJsonAsync<ApiGateway.Models.City[]>("Parametricas/GetCities");
            return resultado;
        }

        public async Task<TipoIdentificacion[]> ObtenerTiposIdentificacion()
        {
            return await _customHttpClient.GetJsonAsync<TipoIdentificacion[]>("Parametricas/ObtenerTiposIdentificacion");
        }

        public async Task<Categoria[]> ObtenerCategorias(bool incluirTramitesDigitales=false)
        {
            var tramites = await _customHttpClient.GetJsonAsync<Categoria[]>("Parametricas/ObtenerCategorias");
            if (incluirTramitesDigitales)
                tramites = (await _customHttpClient.GetJsonAsync<Categoria[]>("Parametricas/ObtenerCategoriaNotariaVirtual"))
                    .Concat(tramites)
                    .ToArray();
            return tramites;
        }

        public async Task<AuthenticatedUser> GetAuthenticatedUser()
        {
            return await _sessionStorageService.GetItemAsync<AuthenticatedUser>("authenticatedUser");
        }
    }
    
}
