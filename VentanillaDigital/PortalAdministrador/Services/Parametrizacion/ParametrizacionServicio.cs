using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Services.Biometria;

namespace PortalAdministrador.Services.Parametrizacion
{
    public class ParametrizacionServicio : IParametrizacionServicio
    {
        private readonly ICustomHttpClient _customHttpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IRNECService _rnecService;
        private readonly IMachineService _machineService;
        private readonly ISessionStorageService _sessionStorage;

        public ParametrizacionServicio(ICustomHttpClient customHttpClient, 
            AuthenticationStateProvider authenticationStateProvider,
            IRNECService rnecService,
            IMachineService machineService,
            ISessionStorageService sessionStorage)
        {
            _customHttpClient = customHttpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _rnecService = rnecService;
            _machineService = machineService;
            _sessionStorage = sessionStorage;
        }


        public async Task<Categoria[]> ObtenerCategorias()
        {
            var listaCategorias = await _customHttpClient.GetJsonAsync<Categoria[]>("Parametrizacion/ObtenerCategorias");
            listaCategorias = JsonConvert.DeserializeObject<Categoria[]>(listaCategorias.ToString());
            return listaCategorias;
        }

        public async Task RegistrarMaquina()
        {
            long notariaId;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity.IsAuthenticated)
            {
                var comprobacionAgente = await _sessionStorage.GetItemAsync<bool>("InstalacionAgenteComprobada");
                if (comprobacionAgente == false)
                {
                    var result = await _rnecService.ConsultarEstado();

                    if (result.Estado == "OK")
                    {
                        await _sessionStorage.SetItemAsync<bool>("InstalacionAgenteComprobada", true);
                        //var authuser = await ((PortalAdministrador.Data.CustomAuthenticationStateProvider)AuthenticationStateProvider).GetAuthenticatedUser();
                        var authuser = state.User;

                        if (long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                        {
                            await _machineService.Register(new ApiGateway.Models.NuevoMaquinaModel()
                            {
                                DireccionIP = result.Propiedades.FirstOrDefault(d => d.Key == "IPLocal").Value,
                                Nombre = result.Propiedades.FirstOrDefault(d => d.Key == "MacAddress").Value,
                                MAC = result.Propiedades.FirstOrDefault(d => d.Key == "MacAddress").Value,
                                NotariaId = notariaId,
                                TipoMaquina = 1,
                                UserName = state.User.Identity.Name
                            });
                        }
                    }
                    else
                    {
                        throw new ApplicationException("No fue posible verificar el estado del agente.");
                    }
                }

            }
            else
            {
                Console.WriteLine("RegistrarMaquina: Usuario No Autenticado");
            }
        }
    }
}
