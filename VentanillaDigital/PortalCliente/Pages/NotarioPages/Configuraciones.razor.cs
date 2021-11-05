using ApiGateway.Contratos.Models.Configuraciones;
using ApiGateway.Contratos.Models.Notario;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using PortalCliente.Services;
using PortalCliente.Services.DescriptorCliente;
using PortalCliente.Services.Notario;
using PortalCliente.Services.Parametrizacion;
using PortalCliente.Services.SignalR;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.NotarioPages
{
    public partial class Configuraciones : ComponentBase
    {
        [Inject]
        protected INotarioService NotarioService { get; set; }

        [Inject]
        protected ISignalRv2Service ScannerService { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        private IConfiguracionesService _configuracionesService { get; set; }

        [Inject]
        NotificationService notificationService { get; set; }
        [Inject]
        IParametrizacionServicio _parametrizacion{get;set;}

        string UsarSticker;
        long notarioId;
        string NoUsarTableta;
        string MostrarATDP;

        string UserEmail = "";
        List<NotarioReturnDTO> notarioReturnDTOs;
        bool UsarFirmaManual;
        bool noUsarTableta;
        bool requerirFirma;
        bool mostrarAtdpAplicacion;
        int tipoIdentificacion = 0;

        bool _movil;
        bool usarScanner;
        int SeleccionarDpi;
        string SeleccionarEscaner;
        bool EsSignalRConectado;
        List<string> Escaners;
        readonly int[] DPIOptions = new int[] { 300, 400, 600 };
        private DotNetObjectReference<Configuraciones> objRef;
        string channelSelected;

        protected override async Task OnInitializedAsync()
        {
            _movil = await DescriptorCliente.EsMovil;
            var opc_conf = await _configuracionesService.ObtenerOpcionesConfiguracion();
            notarioReturnDTOs = await _configuracionesService.ObtenerNotariosNotaria();
            var notarioReturn = notarioReturnDTOs.Where(n => n.NotarioDeTurno == true).FirstOrDefault();
            notarioId = notarioReturn != null ? notarioReturn.NotarioId : 0;
            if (opc_conf.UsarSticker)
                UsarSticker = "1";
            else
                UsarSticker = "0";
            UsarFirmaManual = opc_conf.FirmaManual;
            channelSelected = await _configuracionesService.GetWacomChannelId();
            if (!_movil)
            {
                await estadoConfiguracionTableta();
                await ObtenerConfiguracionScanner();
            }
        }

        public async Task Guardar()
        {
            await _configuracionesService.GuardarOpcionesConfiguracion(new OpcionesConfiguracion()
            {
                FirmaManual = UsarFirmaManual,
                UsarSticker = UsarSticker == "1"
            });
            await _configuracionesService.SetUseTablet(new ConfigTabletViewModel()
            {
                Usetablet = requerirFirma,
                ShowAtdp = mostrarAtdpAplicacion
            });

            await _configuracionesService.SetConfigScanner(new ScannerConfigModel()
            {
                UsarScanner = usarScanner,
                Opciones = new OpcionesScanner()
                {
                    NombreDispositivo = usarScanner ? SeleccionarEscaner : string.Empty,
                    Dpi = SeleccionarDpi
                }
            });
            await _configuracionesService.SetWacomChannel(channelSelected);
            await _parametrizacion.RegistrarMaquina();
            await _configuracionesService.SeleccionarNotarioNotaria(new NotarioNotariaDTO() { NotarioId = notarioId });
            ShowNotification();
        }

        async void ShowNotification()
        {
            var message = new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Summary = "Configuración guardada",
                Detail = "Se han almacenado sus preferencias.",
                Duration = 7000
            };
            notificationService.Notify(message);
        }

        void ActivarFirmaManualCheck(object checkedValue)
        {
            UsarFirmaManual = (bool)checkedValue;
        }

        async Task ObtenerConfiguracionScanner()
        {
            objRef = DotNetObjectReference.Create(this);
            EsSignalRConectado = await ScannerService.EstadoSignalv2R();
            var configTablet = await _configuracionesService.GetConfigScanner();
            if (EsSignalRConectado)
            {
                await ScannerService.AgregarFuncionesNativas(objRef);
                usarScanner = configTablet.UsarScanner;
                SeleccionarEscaner = configTablet.Opciones.NombreDispositivo;
                SeleccionarDpi = configTablet.Opciones.Dpi;
                await SolicitarListaEscaner();
            }
            else
            {
                var scannerConfig = new ScannerConfigModel()
                {
                    UsarScanner = false,
                    Opciones = new OpcionesScanner()
                    {
                        NombreDispositivo = configTablet.Opciones.NombreDispositivo,
                        Dpi = configTablet.Opciones.Dpi
                    }
                };
                await _configuracionesService.SetConfigScanner(scannerConfig);
            }
        }

        private async Task SolicitarListaEscaner()
        {
            await ScannerService.ObtenerListaScanners();
            Escaners = await ScannerService.ObtenerEscanerVariable();
            if (Escaners == null)
            {
                usarScanner = false;
            }
            if (Escaners?.Count > 0 && string.IsNullOrEmpty(SeleccionarEscaner))
            {
                SeleccionarEscaner = Escaners[0];
            }
            StateHasChanged();
        }

        async Task RefrescarDispositivos()
        {
            await SolicitarListaEscaner();
        }

        [JSInvokable]
        public void AsignarEscaner(string nombreEscaner)
        {
            SeleccionarEscaner = nombreEscaner;
        }

        [JSInvokable]
        public void RecuperarScanners(List<string> escaners)
        {
            Escaners = escaners;
            StateHasChanged();
        }
        
        async Task estadoConfiguracionTableta()
        {
            var configTablet = await _configuracionesService.GetUseTablet();
            noUsarTableta = !configTablet.Usetablet;
            requerirFirma = configTablet.Usetablet;
            mostrarAtdpAplicacion = configTablet.ShowAtdp;
        }
        
        void useTabletCheck(ChangeEventArgs args)
        {
            noUsarTableta = (string)args.Value == "NO";
            requerirFirma = !noUsarTableta;
            if (noUsarTableta)
                mostrarAtdpAplicacion = true;

        }

        async Task UsarScannerChange(ChangeEventArgs args)
        {
            usarScanner = (bool)args.Value;
            if (usarScanner)
            {
                await ScannerService.ObtenerListaScanners();
            }
        }
    }
}
