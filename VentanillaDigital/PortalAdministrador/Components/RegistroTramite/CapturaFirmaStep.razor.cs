using Microsoft.AspNetCore.Components;
using PortalAdministrador.Data;
using PortalAdministrador.Data.DatosTramite;
using PortalAdministrador.Services.Recursos;
using PortalAdministrador.Services.Wacom;
using PortalAdministrador.Services.Wacom.Models;
using System;
using System.Drawing;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class CapturaFirmaStep
    {
        bool _autorizado = false;
        bool _firmaRecibida = false;
        bool _capturando = false;

        [Inject]
        public IWacomService WacomService { get; set; }

        [Inject]
        protected IRecursosService RecursosService { get; set; }

        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        private bool FirmaCapturada { get => !string.IsNullOrEmpty(Compareciente?.Firma); }

        private bool _sinFirma = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(Compareciente.Firma))
                {
                    await ReadyChanged.InvokeAsync(true);
                }
                else if (WacomService.EstadoServicio == EstadoServicio.DispositivoConectado)
                {
                    await MostrarADTP();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Compareciente.Tramite.TipoTramite.TipoTramiteId == 12 && Compareciente.Tramite.ComparecienteActualPos == 1)
            {
                var datosAdicionales = JsonSerializer.Deserialize<DocumentoPrivadoInvidenteDTO>(Compareciente.Tramite.DatosAdicionales);
                _sinFirma = !datosAdicionales.SabeFirmar;
            }
        }

        protected async Task MostrarADTP()
        {
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
            await WacomService.IniciarCaptura("images/Pad/ATDP.jpg", botones, false);
            await Task.Delay(1500).ContinueWith(async (t) =>
            {
                if (!WacomService.Conectado)
                    await WacomService.IniciarCaptura("images/Pad/ATDP.jpg", botones, false);
            });
        }

        protected async Task ObtenerFirma()
        {
            Compareciente.Firma = await WacomService.ObtenerFirma(720, 1200, new Rectangle(53, 284, 1095, 231));
            _firmaRecibida = true;
            StateHasChanged();
            await WacomService.TerminarCaptura();
            await ComparecienteChanged.InvokeAsync(Compareciente);
            await ReadyChanged.InvokeAsync(true);
            _capturando = false;
        }

        protected async Task AutorizaRec()
        {
            _autorizado = true;
            Compareciente.Firma = await RecursosService.ObtenerRecurso("images/SinFirma.png");
            StateHasChanged();
            await ReadyChanged.InvokeAsync(true);
            await ComparecienteChanged.InvokeAsync(Compareciente);

            _capturando = false;
            try
            {
                await WacomService.TerminarCaptura();
            }
            catch { }

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

            await Task.Delay(1500).ContinueWith(async (t) =>
            {
                if (!WacomService.Conectado)
                    await WacomService.IniciarCaptura("images/Pad/Firma.jpg", botones, true);
            });
        }

        protected async Task Reiniciar()
        {
            await WacomService.VerificarServicio();
        }
        protected async Task Rechazar()
        {
            await ComparecienteChanged.InvokeAsync(Compareciente.ObtenerNuevoCompareciente());
            await WacomService.TerminarCaptura();
            _capturando = false;
        }
    }
}
