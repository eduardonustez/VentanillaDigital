using Microsoft.AspNetCore.Components;
using PortalAdministrador.Components.Transversales;
using PortalAdministrador.Data;
using PortalAdministrador.Services;
using PortalAdministrador.Services.LoadingScreenService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class RegistrarCompareciente
    {
        [Inject]
        protected ITramiteService TramiteService { get; set; }

        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteTerminado { get; set; }

        [Parameter]
        public EventCallback TramiteTerminado { get; set; }

        [Parameter]
        public long CodigoTipoTramite { get; set; }

        private bool NextDisabled { get; set; }
        private bool PreviousDisabled { get; set; }

        private bool _ignorarClickSiguiente = false;

        private bool Terminado
        {
            get => TabSelector?.Current == "TResumen" &&
                Compareciente.IsValid(true);
        }
        private TabSelector TabSelector { get; set; }
        private FotoCapturaHuella CapturaHuella { get; set; }

        private PasoRegistrarCompareciente UltimoPasoCompleto { get; set; }

        private async Task ActualizarPaso(bool ready, PasoRegistrarCompareciente pasoActual)
        {
            if (ready)
            {
                UltimoPasoCompleto = (PasoRegistrarCompareciente)Math.Max((int)UltimoPasoCompleto, (int)pasoActual);
            }
            else if (pasoActual == UltimoPasoCompleto)
            {
                UltimoPasoCompleto--;
            }
        }

        private async Task SiguienteClick()
        {
            if (_ignorarClickSiguiente)
                return;

            _ignorarClickSiguiente = true;
            if (Terminado)
            {
                await ComparecienteTerminado.InvokeAsync(Compareciente);
            }
            else
                await TabSelector.SelectNext();

            await Task.Delay(200).ContinueWith((t) => _ignorarClickSiguiente = false);
        }

        private async Task OmitirCompareciente()
        {
            Compareciente.Tramite.CantidadComparecientes--;
            bool actualizado = await TramiteService.ActualizarTramite(Compareciente.Tramite);
            if (actualizado)
            {
                if (Compareciente.Tramite.CantidadComparecientes == Compareciente.Tramite.Comparecientes.Count)
                {
                    await TramiteTerminado.InvokeAsync(null);
                }
                else
                {
                    await ComparecienteChanged.InvokeAsync(Compareciente.ObtenerNuevoCompareciente());
                }
            }
        }
    }
    internal enum PasoRegistrarCompareciente
    {
        Ninguno,
        Datos,
        ATDP,
        Foto,
        Huellas,
        Resumen
    }
}
