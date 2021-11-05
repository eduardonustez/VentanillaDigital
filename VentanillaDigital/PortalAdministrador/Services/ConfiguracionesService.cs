using ApiGateway.Contratos.Models.Notario;
using Microsoft.AspNetCore.Components;
using PortalAdministrador.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public class ConfiguracionesService : IConfiguracionesService
    {

        public ILocalStorageService _localStorageService { get; set; }

        private readonly ICustomHttpClient _customHttpClient;


        public ConfiguracionesService(ICustomHttpClient customHttpClient, ILocalStorageService localStorageService)
        {
            _customHttpClient = customHttpClient;
            _localStorageService = localStorageService;
        }

        public async Task<OpcionesConfiguracion> ObtenerOpcionesConfiguracion()
        {
            return new OpcionesConfiguracion()
            {
                FirmaManual = await GetFirmaManual(),
                UsarSticker = await GetUsarSticker()
            };
        }

        public async Task GuardarOpcionesConfiguracion(OpcionesConfiguracion opcionesConfiguracion)
        {
            await SetFirmaManual(opcionesConfiguracion.FirmaManual);
            await SetUsarSticker(opcionesConfiguracion.UsarSticker);
        }

        private async Task SetFirmaManual(bool FirmaManual)
        {
            await _localStorageService.SetItem<bool>("FirmaManual", FirmaManual);
        }

        private async Task SetUsarSticker(bool UsarSticker)
        {
            await _localStorageService.SetItem<bool>("UsarSticker", UsarSticker);
        }

        private async Task<bool> GetFirmaManual()
        {
            return await _localStorageService.GetItem<bool>("FirmaManual");
        }

        private async Task<bool> GetUsarSticker()
        {
            return await _localStorageService.GetItem<bool>("UsarSticker");
        }

        public async Task<List<NotarioReturnDTO>> ObtenerNotariosNotaria()
        {
            var resultado = await _customHttpClient.PostJsonAsync<List<NotarioReturnDTO>>("Notario/ObtenerNotariosNotaria", "");
            return resultado;
        }
        public async Task<long> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO)
        {
            var resultado = await _customHttpClient.PostJsonAsync<long>("Notario/SeleccionarNotarioNotaria", notarioNotariaDTO);
            return resultado;
        }
    }
}
