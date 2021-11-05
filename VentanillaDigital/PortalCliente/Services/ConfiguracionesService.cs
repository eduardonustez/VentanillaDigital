using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Contratos.Models.Configuraciones;
using Microsoft.AspNetCore.Components;
using PortalCliente.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services.Biometria;

namespace PortalCliente.Services
{
    public class ConfiguracionesService : IConfiguracionesService
    {
        public ILocalStorageService _localStorageService { get; set; }

        private readonly ICustomHttpClient _customHttpClient;
        private readonly IRNECService _rnecService;
        private readonly IMachineService _machineService;

        public ConfiguracionesService(ICustomHttpClient customHttpClient
            ,ILocalStorageService localStorageService
            ,IRNECService rnecService
            ,IMachineService machineService)
        {
            _customHttpClient = customHttpClient;
            _localStorageService = localStorageService;
            _rnecService = rnecService;
            _machineService = machineService;
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

        public async Task SetUseTablet(ConfigTabletViewModel configTablet)
        {
            await _localStorageService.SetItem<ConfigTabletViewModel>("configTablet", configTablet);
        }

        public async Task<ConfigTabletViewModel> GetUseTablet()
        {
            var configTablet = await _localStorageService.GetItem<ConfigTabletViewModel>("configTablet");
            if (configTablet == null)
            {
                configTablet = new ConfigTabletViewModel()
                {
                    Usetablet = true,
                    ShowAtdp = false
                };
                await SetUseTablet(configTablet);
            }
            return configTablet;
        }

        public async Task SetConfigScanner(ScannerConfigModel scannerConfig)
        {
            await _localStorageService.SetItem("ScannerConfig", scannerConfig);
        }

        public async Task<ScannerConfigModel> GetConfigScanner()
        {
            var scannerConfig = await _localStorageService.GetItem<ScannerConfigModel>("ScannerConfig");
            if (scannerConfig == null)
            {
                scannerConfig = new ScannerConfigModel()
                {
                    UsarScanner = false,
                    Opciones = new OpcionesScanner()
                    {
                        NombreDispositivo = "",
                        Dpi = 400
                    }
                };
                await SetConfigScanner(scannerConfig);
            }
            return scannerConfig;
        }
        public async Task<string> GetWacomChannelId(){
            var wacomChannelId = await _localStorageService.GetItem<string>("wacom-channel");
            if (wacomChannelId == null)
            {
                var result = await _rnecService.ConsultarEstado();
                if (result != null && result.Estado == "OK")
                {
                    try
                    {
                        var machineConfig= await _machineService.Consultar(result.Propiedades.FirstOrDefault(d => d.Key == "MacAddress")?.Value);
                        wacomChannelId = machineConfig?.CanalWacom;
                        if (wacomChannelId != null)
                            await SetWacomChannel(wacomChannelId);
                        else
                            wacomChannelId = "1";
                    }
                    catch
                    {
                        wacomChannelId = "1";
                    }
                }
                else
                {
                    wacomChannelId="1";
                }
            }
            return wacomChannelId;
        }
        public async Task SetWacomChannel(string wacomChannelId)
        {
            await _localStorageService.SetItem("wacom-channel", wacomChannelId);
        }
    }
}
