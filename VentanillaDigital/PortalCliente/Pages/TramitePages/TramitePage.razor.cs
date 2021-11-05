using ApiGateway.Models.Transaccional;
using Microsoft.AspNetCore.Components;
using PortalCliente.Data;
using PortalCliente.Services;
using PortalCliente.Services.DescriptorCliente;
using Radzen;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PortalCliente.Services.Wacom;
using PortalCliente.Shared;
using Microsoft.JSInterop;
namespace PortalCliente.Pages.TramitePages
{
    public partial class TramitePage
    {
        [Parameter]
        public long TramiteId
        {
            get => Tramite.TramiteId;
            set => Tramite.TramiteId = value;
        }
        [Parameter]
        public long TramiteEnProcesoId { get; set; }
        [CascadingParameter]
        public MainLayout Layout { get; set; }

        #region Inject
        [Inject]
        protected ITramiteService TramiteService { get; set; }

        [Inject]
        protected IActaNotarialService ActaNotarialService { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        [Inject]
        IConfiguracionesService ConfiguracionesService { get; set; }

        [Inject]
        IDescriptorCliente DescriptorCliente { get; set; }
        [Inject]
        protected IWacomServiceFactory WacomServiceFactory { get; set; }
        protected IWacomService WacomService { get; set; }
        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        bool _esMovil = false;
        bool actualizarTramiteIdEnProcesoPadre = true;

        #endregion
        private Tramite _tramite;
        public Tramite Tramite
        {
            get { return _tramite; }
            set
            {
                _tramite = value;
                Layout.TramiteIdEnProceso = actualizarTramiteIdEnProcesoPadre ? value.TramiteId : 0;
            }
        }
        protected override async Task OnInitializedAsync()
        {
            if (Tramite == null)
                Tramite = Tramite.ObtenerNuevoTramite();
            if (Layout != null)
                Layout.tramitePage = this;
            _esMovil = await DescriptorCliente.EsMovil;
            string wacomChannelId= await ConfiguracionesService.GetWacomChannelId();
            WacomService = await WacomServiceFactory.GetWacomServiceInstance(wacomChannelId);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (TramiteEnProcesoId != 0)
            {
                await RetomarTramite(TramiteEnProcesoId);
            }
            await base.OnParametersSetAsync();
        }

        protected async Task TramiteTerminado()
        {
            await IniciarCreacionActaFirmaManual();
            Tramite = Tramite.ObtenerNuevoTramite();
            try
            {
                //await WacomService.TerminarCaptura();
            }
            catch { }
        }

        protected async Task ComparecienteTerminado(Compareciente compareciente)
        {
            LiberarDispositivos();
            Tramite.Comparecientes.Add(compareciente);
            compareciente.ComparecienteActualPos = Tramite.ComparecienteActualPos;
            bool resul = await TramiteService.CrearCompareciente(Tramite.TramiteId, compareciente);
            if (resul == true)
            {
                AddComparecienteDataActa(compareciente);
                ShowSuccessNotification();
                if (Tramite.ComparecienteActualPos == Tramite.CantidadComparecientes)
                {
                    await IniciarCreacionActaFirmaManual();
                    Tramite = Tramite.ObtenerNuevoTramite();
                    //Layout = null;
                    //TramiteEnProcesoId = 0;
                }
                else
                {
                    Tramite.ComparecienteActualPos++;
                    Tramite.ComparecienteActual = Compareciente.ObtenerNuevoCompareciente();
                }
            }
            else
            {
                ShowErrorNotification();
            }
        }
        async Task IniciarCreacionActaFirmaManual()
        {
            var configuraciones = await ConfiguracionesService.ObtenerOpcionesConfiguracion();
            Tramite.InfoActa.FirmarManual = configuraciones.FirmaManual;
            Tramite.InfoActa.UsarSticker = configuraciones.UsarSticker;
            Tramite.InfoActa.DireccionComparecencia = Tramite.DireccionComparecencia;
            Tramite.InfoActa.FueraDeDespacho = Tramite.FueraDeDespacho;

            if (Tramite.InfoActa.FirmarManual)
            {
                await ActaNotarialService.CrearActaParaFirmaManual(Tramite.InfoActa);
            }
        }
        void AddComparecienteDataActa(Compareciente compareciente)
        {
            var comparecienteCreate = new ComparecienteCreate()
            {
                Nombres = compareciente.Nombres,
                Apellidos = compareciente.Apellidos,
                NUIPCompareciente = compareciente.NumeroIdentificacion,
                FotoCompareciente = compareciente.Foto,
                FirmaCompareciente = compareciente.Firma,
                NUT = Tramite.TramiteId.ToString(),
                FechaCompletaNumeros = DateTime.Now.ToString("dd/MM/yyyy"),
                HoraCompletaNumeros = DateTime.Now.ToString("HH:mm:ss"),
                TextoBiometria = compareciente.MotivoSinBiometria,
                TramiteSinBiometria = compareciente.TramiteSinBiometria == true ? "1" : "0",
                NombreTipoDocumento = compareciente.TipoIdentificacion.Nombre,
                Posicion = Tramite.ComparecienteActualPos.ToString(),
                FechaCreacion = DateTime.Now
            };
            Tramite.InfoActa.ComparecientesCreate.Add(comparecienteCreate);
        }

        protected async Task RetomarTramite(long id)
        {
            Tramite = await TramiteService.ObtenerTramite(id);
            if (Tramite.ComparecientesCompletos)
            {
                Tramite = Tramite.ObtenerNuevoTramite();
            }
            else
            {
                Tramite.InfoActa = new ActaCreate()
                {
                    TramiteId = Tramite.TramiteId,
                    FechaTramite = DateTime.Now,
                    TipoTramite = Tramite.TipoTramite.TipoTramiteId.ToString(),
                    DataAdicional = Tramite.DatosAdicionales,
                    CodigoTramite = Tramite.TipoTramite.CodigoTramite
                };
                foreach (var compareciente in Tramite.Comparecientes)
                {
                    AddComparecienteDataActa(compareciente);
                }
                Tramite.ComparecienteActual = Compareciente.ObtenerNuevoCompareciente();
            }
        }

        public async Task CancelarTramite(string MotivoCancelacion)
        {
            await actaNotarialService.CancelarTramiteNotarial(new TramiteRechazadoModel
            {
                MotivoRechazo = MotivoCancelacion,
                TramiteId = TramiteId
            });
            LiberarDispositivos();
            if (TramiteEnProcesoId > 0)
                actualizarTramiteIdEnProcesoPadre = false;
            Tramite = Tramite.ObtenerNuevoTramite();
            await WacomService.TerminarCaptura();    

        }

        void ShowSuccessNotification()
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Compareciente Agregado", Detail = "Compareciente agregado satisfactoriamente!!", Duration = 4000 };
            NotificationService.Notify(message);
        }
        void ShowErrorNotification()
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Ocurrió un error intentado agregar el compareciente, por favor contacte al administrador!!", Duration = 4000 };
            NotificationService.Notify(message);
        }
        async void LiberarDispositivos()
        {
            await JsRuntime.InvokeAsync<string>("unlockDevice");
        }
    }
}
