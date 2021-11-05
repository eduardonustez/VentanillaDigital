using Microsoft.AspNetCore.Components;
using PortalCliente.Components.Transversales;
using PortalCliente.Data;
using PortalCliente.Services;
using PortalCliente.Services.LoadingScreenService;
using System;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services.DescriptorCliente;
using Infraestructura.Transversal.Log.Modelo;
using ApiGateway.Models.Transaccional;
using System.Text.Json;
using Radzen;


using Microsoft.JSInterop;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class RegistrarCompareciente
    {
        [Inject]
        NotificationService notificationService { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        protected ITramiteService TramiteService { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        protected ITrazabilidadService _trazabilidadService { get; set; }

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

        [Parameter]
        public EventCallback<bool> EmailCelular { get; set; }

        private bool NextDisabled { get; set; }
        private bool PreviousDisabled { get; set; }

        private bool _ignorarClickSiguiente = false;

        private bool _esMovil = false;

        string MotivoOmision { get; set; }

        bool showSpinner;

        private bool Terminado
        {
            get => TabSelector?.Current == "TResumen" &&
                Compareciente.IsValid(true);
        }
        [Parameter]
        public bool Incluir { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _esMovil = await DescriptorCliente.EsMovil;
            await base.OnInitializedAsync();
        }

        private TabSelector TabSelector { get; set; }
        private CapturaHuella CapturaHuella { get; set; }

        private FotoCapturaHuella FotoCapturaHuella { get; set; }
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
            if (Compareciente.Tramite.CantidadComparecientes == Compareciente.Tramite.ComparecienteActualPos && NextDisabled && Terminado)
            {
                await Js.InvokeVoidAsync("removerTramiteDeURL");
            }
            if (Terminado)
            {
                showSpinner = true;
                await ComparecienteTerminado.InvokeAsync(Compareciente);
            }
            else
            {
                if (Incluir && string.IsNullOrEmpty(Compareciente.Email) && string.IsNullOrEmpty(Compareciente.NumeroCelular))
                {
                    var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"Por favor, diligenciar los campos adicionales", Duration = 4000 };
                    notificationService.Notify(message);
                }
                else
                {
                    showSpinner = false;
                    await TabSelector.SelectNext();
                }
            }

            await Task.Delay(200).ContinueWith((t) => _ignorarClickSiguiente = false);
        }

        private async Task OmitirCompareciente()
        {
            if (!string.IsNullOrWhiteSpace(MotivoOmision))
            {
                await AgregarLog();
                Compareciente.Tramite.CantidadComparecientes--;
                bool actualizado = await TramiteService.ActualizarTramite(Compareciente.Tramite);
                if (actualizado)
                {
                    if (Compareciente.Tramite.CantidadComparecientes == Compareciente.Tramite.Comparecientes.Count)
                    {
                        await TramiteTerminado.InvokeAsync(null);
                        await Js.InvokeVoidAsync("removerTramiteDeURL");
                    }
                    else
                    {
                        await ComparecienteChanged.InvokeAsync(Compareciente.ObtenerNuevoCompareciente());
                    }
                }
            }
        }

        private async Task AgregarLog()
        {
            ComparecienteOmitidoModel comparecienteOmitido = new ComparecienteOmitidoModel
            {
                MotivoOmision = MotivoOmision,
                TramiteId = Compareciente.Tramite.TramiteId
            };
            InformationModel objTraza = new InformationModel("1",
               "Portal Cliente",
               "OmitirCompareciente",
               "Omitir compareciente",
               "Compareciente",
               null,
               JsonSerializer.Serialize(comparecienteOmitido), "");
            await _trazabilidadService.CrearTraza(objTraza);
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
