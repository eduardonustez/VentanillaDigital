using PortalCliente.Data;
using PortalCliente.Data.Account;
using System;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using System.Collections.Generic;
using ApiGateway.Contratos.Models.Archivos;
using Infraestructura.Transversal.Models;
using GenericExtensions;

namespace PortalCliente.Services
{
    public class MachineService : IMachineService
    {
        private readonly ICustomHttpClient _customHttpClient;
        private ILocalStorageService _localStorageService;

        public MachineService(ICustomHttpClient customHttpClient, ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
            _customHttpClient = customHttpClient;
           
        }

        public async Task Register(NuevoMaquinaModel nuevoMaquina)
        {
            // string registered = await _localStorageService.GetItem<string>("REGISTERED-MACHINE-MAC");
            // if (registered == null)
            // {
               var result= await _customHttpClient.PostJsonAsync<MaquinaReturn>("/api/Maquina/CrearMaquina", nuevoMaquina);
                // if(result!=null)
                //     await _localStorageService.SetItem<string>("REGISTERED-MACHINE-MAC", nuevoMaquina.MAC);

            // }
            // await _localStorageService.SetItem<string>("REGISTERED-MACHINE-IP", nuevoMaquina.DireccionIP); //siempre actualizamos la IP, porque esta puede cambiar

        }
        public async Task<MaquinaConfiguracionReturn> Consultar(string mac)
        {
            return await _customHttpClient.GetJsonAsync<MaquinaConfiguracionReturn>($"/api/Maquina/ConsultarMaquina/{mac}");
        }
    }

}
