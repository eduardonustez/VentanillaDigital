using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Services.Biometria;

namespace PortalCliente.Services.Parametrizacion
{
    public class ParametrizacionServicio : IParametrizacionServicio
    {
        private readonly ICustomHttpClient _customHttpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IRNECService _rnecService;
        private readonly IMachineService _machineService;
        private readonly ISessionStorageService _sessionStorage;
        private readonly IConfiguracionesService _configuracionesService;

        public ParametrizacionServicio(ICustomHttpClient customHttpClient, 
            AuthenticationStateProvider authenticationStateProvider,
            IRNECService rnecService,
            IMachineService machineService,
            ISessionStorageService sessionStorage,
            IConfiguracionesService configuracionesService)
        {
            _customHttpClient = customHttpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _rnecService = rnecService;
            _machineService = machineService;
            _sessionStorage = sessionStorage;
            _configuracionesService = configuracionesService;
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
                    var result = await _rnecService.ConsultarEstado();

                    if (result!=null && result.Estado == "OK")
                    {
                        await _sessionStorage.SetItemAsync<bool>("InstalacionAgenteComprobada", true);
                        var authuser = state.User;
                        string canalWacom = await _configuracionesService.GetWacomChannelId();
                        if (long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                        {
                            await _machineService.Register(new ApiGateway.Models.NuevoMaquinaModel()
                            {
                                DireccionIP = result.Propiedades.FirstOrDefault(d => d.Key == "IPLocal")?.Value,
                                Nombre = result.Propiedades.FirstOrDefault(d => d.Key == "MacAddress")?.Value,
                                MAC = result.Propiedades.FirstOrDefault(d => d.Key == "MacAddress")?.Value,
                                CanalWacom = canalWacom,
                                EstadoWacomSigCaptX = result.Propiedades.FirstOrDefault(d => d.Key == "WacomSTUSigCaptX")?.Value,
                                EstadoDllWacom =result.Propiedades.FirstOrDefault(d => d.Key == "IsWacomDllRegistered")?.Value,
                                EstadoCaptor =result.Propiedades.FirstOrDefault(d => d.Key == "CaptorDetectado")?.Value,
                                NotariaId = notariaId,
                                TipoMaquina = 1,
                                UserName = state.User.Identity.Name
                            });
                        }
                    }
                    else
                    {
                       // throw new ApplicationException("No fue posible verificar el estado del agente.");
                    }
            }
        }
        public async Task ValidarEstadoCaptorHuella(bool primerIntento = true)
        {
            long notariaId;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity.IsAuthenticated)
            {
                var comprobacionAgente = await _sessionStorage.GetItemAsync<bool>("InstalacionAgenteComprobada");
                if (comprobacionAgente == false)
                {
                    var result = await _rnecService.ConsultarEstado();

                    if (result != null && result.Estado == "OK" && !(result.Propiedades?.Any(p => p.Key == "CaptorDetectado" && p.Value == false.ToString()) ?? false))
                    {
                        await _sessionStorage.SetItemAsync<bool>("InstalacionAgenteComprobada", true);
                    }
                    else if (primerIntento)
                    {
                        await _rnecService.ReiniciarCaptor();
                        await Task.Delay(10000);
                        await ValidarEstadoCaptorHuella(false);
                    }
                    else
                    {
                        throw new ApplicationException("No fue posible verificar el estado del agente.");
                    }
                }
            }
        }
    }
    
}
