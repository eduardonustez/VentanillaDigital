using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Wacom;
using PortalCliente.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services;
using PortalCliente.Services.Recursos;
using System.Net.Http;
using PortalCliente.Data;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;
using PortalCliente.Services.DescriptorCliente;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class CapturaFirmaStep : IDisposable
    {
        bool _autorizado = false;
        bool _firmaRecibida = false;
        bool _capturando = false;

        bool _habilitarCambioFirma = true;
        bool _habilitarCambioVerbo = true;
        #region inject properties
        [Inject]
        IWacomServiceFactory wacomServiceFactory { get; set; }
        protected IWacomService WacomService { get; set; }

        [Inject]
        protected IRecursosService RecursosService { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }
        [Inject]
        protected IConfiguracionesService _configuraciones { get; set; }
        #endregion
        #region parameters properties
        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        #endregion
        private bool FirmaCapturada { get => !string.IsNullOrEmpty(Compareciente?.Firma); }

        private bool _sinFirma = false;
        private string _sinSaberFirma = "1";

        private bool _movil = false;
        private bool _sinTableta = false;
        private bool _autorizarAtdpEnPantalla = false;
        string channelId;
        SignatureSize signatureSize = new SignatureSize(720, 1200, 5F);
        protected override async Task OnInitializedAsync()
        {
            channelId = await _configuraciones.GetWacomChannelId();
            WacomService = await wacomServiceFactory.GetWacomServiceInstance(channelId);
            _movil = await DescriptorCliente.EsMovil;
            await LoadTabletConfigurations();
            if (FirmaCapturada)
                _autorizado = true;
            await base.OnInitializedAsync();
        }
        private async Task LoadTabletConfigurations()
        {
            if (_movil)
            {
                _sinTableta = true;
                _autorizarAtdpEnPantalla = false;
            }
            else
            {
                var configTablet = await _configuraciones.GetUseTablet();
                _sinTableta = !configTablet.Usetablet;
                _autorizarAtdpEnPantalla = configTablet.ShowAtdp;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (WacomService == null)
                {
                    string channelId = await _configuraciones.GetWacomChannelId();
                    WacomService = await wacomServiceFactory.GetWacomServiceInstance(channelId);
                }
                await LoadTabletConfigurations();
                if (!string.IsNullOrEmpty(Compareciente.Firma))
                {
                    await ReadyChanged.InvokeAsync(true);
                }
                else if (WacomService.EstadoServicio == EstadoServicio.DispositivoConectado)
                {
                    if (_sinFirma || _sinTableta)
                        await WacomService.TerminarCaptura();
                    else
                        await MostrarADTP();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Compareciente.Tramite.ComparecienteActualPos == 1)
            {
                if ((CodigoTipoTramite)Compareciente.Tramite.TipoTramite.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoInvidente)
                {
                    var datosAdicionales = JsonSerializer.Deserialize<DocumentoPrivadoInvidenteDTO>(Compareciente.Tramite.DatosAdicionales);
                    _sinFirma = !datosAdicionales.SabeFirmar;
                    _habilitarCambioFirma = datosAdicionales.SabeFirmar;
                    _habilitarCambioVerbo = true;
                }

                if ((CodigoTipoTramite)Compareciente.Tramite.TipoTramite.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoFirmaARuego)
                {
                    _sinFirma = true;
                    _habilitarCambioFirma = false;
                    _habilitarCambioVerbo = true;
                }
            }

            if (Compareciente.Tramite.ComparecienteActualPos == 2)
            {
                if ((CodigoTipoTramite)Compareciente.Tramite.TipoTramite.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoInvidente
                || (CodigoTipoTramite)Compareciente.Tramite.TipoTramite.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoFirmaARuego)
                {
                    _sinFirma = false;
                    _habilitarCambioFirma = _habilitarCambioVerbo = false;
                }
            }

            if ((CodigoTipoTramite)Compareciente.Tramite.TipoTramite.CodigoTramite == CodigoTipoTramite.AutenticacionFirma)
            {
                _sinFirma = false;
                _habilitarCambioFirma = _habilitarCambioVerbo = false;
            }
        }

        protected async Task MostrarADTP()
        {
            if (_sinTableta)
                return;
            _autorizado = false;
            await ReadyChanged.InvokeAsync(false);
            Compareciente.Firma = null;
            var botones = new Boton[]
            {
                new Boton
                {
                    Accion = Autorizar,
                    Posicion = new Rectangle(421,342,224,67)
                },
                new Boton
                {
                    Accion = Rechazar,
                    Posicion = new Rectangle(156,342,224,67)
                }
            };
            await WacomService.IniciarCaptura("images/Pad/ATDP.jpg", botones, false, signatureSize);
            if (channelId != "2")
            {
                await Task.Delay(1500).ContinueWith(async (t) =>
                {
                    if (!WacomService.Conectado)
                        await WacomService.IniciarCaptura("images/Pad/ATDP.jpg", botones, false,
                        signatureSize);
                });
            }

        }

        protected async Task ObtenerFirma()
        {
            Compareciente.Firma = await WacomService.ObtenerFirma(signatureSize.Height,
                                    signatureSize.Width, signatureSize.LineWidth);
            _firmaRecibida = true;
            StateHasChanged();
            await WacomService.TerminarCaptura();
            await ComparecienteChanged.InvokeAsync(Compareciente);
            await ReadyChanged.InvokeAsync(true);
            _capturando = false;
        }

        protected async Task AutorizaRec()
        {
            if (_autorizarAtdpEnPantalla && !_sinFirma && !_sinTableta && !_movil)
            {
                await Autorizar();
            }
            else
            {
                _autorizado = true;
                if (_sinFirma)
                {
                    string imagen = _sinSaberFirma == "1" ? "images/SinSaberFirma.png" : "images/SinPoderFirma.png";
                    Compareciente.Firma = await RecursosService.ObtenerRecurso(imagen);
                    //StateHasChanged();
                    await ReadyChanged.InvokeAsync(true);
                    await ComparecienteChanged.InvokeAsync(Compareciente);

                    _capturando = false;
                    if (!_movil)
                    {
                        try
                        {
                            await WacomService.TerminarCaptura();
                        }
                        catch { }
                    }
                    StateHasChanged();
                }
                else if (!_sinFirma && _sinTableta && !_movil)
                {
                    Compareciente.Firma = null;
                    await ReadyChanged.InvokeAsync(true);
                    await ComparecienteChanged.InvokeAsync(Compareciente);
                }
                else
                {
                    _capturando = true;
                    StateHasChanged();
                }

            }

        }

        protected async Task Autorizar()
        {
            _capturando = true;
            _autorizado = true;
            StateHasChanged();
            var botones = new Boton[]
            {
                new Boton
                {
                    Accion = ObtenerFirma,
                    Posicion = new Rectangle(423,360,224,67)
                },
                new Boton
                {
                    Accion = WacomService.LimpiarPantalla,
                    Posicion = new Rectangle(159,360,224,67)
                }
            };
            await WacomService.IniciarCaptura("images/Pad/Firma.jpg", botones, true);
            if (channelId != "2")
            {
                await Task.Delay(1500).ContinueWith(async (t) =>
            {
                if (!WacomService.Conectado)
                    await WacomService.IniciarCaptura("images/Pad/Firma.jpg", botones, true);
            });
            }
        }

        protected async Task Reiniciar()
        {
            await WacomService.VerificarServicio();
        }
        protected async Task Rechazar()
        {
            //await ComparecienteChanged.InvokeAsync(Compareciente.ObtenerNuevoCompareciente());
            await WacomService.TerminarCaptura();
            _capturando = false;
        }

        public async void Dispose()
        {
            if (_capturando)
                await WacomService.TerminarCaptura();
        }
        protected async Task ToggleSinFirmaAsync()
        {
            _sinFirma = !_sinFirma;
            if (_sinFirma || _sinTableta)
                await WacomService.TerminarCaptura();
            else
                await MostrarADTP();
        }

        void IDisposable.Dispose()
        {
            WacomService.TerminarCaptura();
        }
    }
}
