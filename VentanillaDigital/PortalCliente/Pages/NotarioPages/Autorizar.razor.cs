using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Models.Transaccional;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Data.Account;
using PortalCliente.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using ApiGateway.Contratos.Enums;
using Microsoft.JSInterop;
using PortalCliente.Components.Notario;
using Blazored.SessionStorage;
using PortalCliente.Services.Notario;
using Infraestructura.Transversal.Encriptacion;
using PortalCliente.Services.DescriptorCliente;

namespace PortalCliente.Pages.NotarioPages
{
    public partial class Autorizar : ComponentBase
    {

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        NotificationService notificationService { get; set; }

        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string PIdTramite { get; set; }

        [Parameter]
        public string PEstadoTramite { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }
        [Inject]
        private ISessionStorageService _sessionStorageService { get; set; }
        [Inject]
        public INotarioService notarioService { get; set; }
        ActaPrevia actaPreviaChild;

        string pdfFile;
        string MsgActaNoEncontrada = "";
        bool MostrarAutorizar = true;
        string MsjAutorizacionResul = "";
        string MsjAutorizacionClass = "";
        string titulo = "Revisión de Documento";
        bool MostrarErrorEnModal = false;
        string MotivoRechazo { get; set; }
        bool pinAsignadoNotario;
        bool esPdfGranTamanio = false;
        bool showSpinner = false;
        public List<string> paginasNoAutorizar = (new List<string> { "2", "4" });

        public long IdTramite
        {
            get => long.Parse(this.PIdTramite);
            set => this.PIdTramite = value.ToString();
        }

        // Logica de modales
        public Guid Guid = Guid.NewGuid();
        public string ModalFirmaDisplay = "none;";
        public string ModalDisplay = "none;";
        public string ModalFirma = "ModalFirma";
        public string ModalRechazo = "ModalRechazo";
        public bool ShowBackdrop = false;
        public string UserRol = "";
        string clave;
        private readonly string idModal = "modalRechazoTramite";
        private readonly string idInput = "InputMotivo";
        //private bool obteniendo = false;
        private bool _movil = false;

        protected override async Task OnInitializedAsync()
        {
            _movil = await DescriptorCliente.EsMovil;
            ModalFirmaDisplay = "none";
            await ConsultarRolUsuario();
            if (PEstadoTramite == "3")
                await ConsultarActa();
        }

        protected override async Task OnParametersSetAsync()
        {
            await ObtenerEstadoPinFirma();
        }

        async Task ObtenerEstadoPinFirma()
        {
            var usuarioAutenticado = await ((CustomAuthenticationStateProvider)_authenticationStateProvider)?.GetAuthenticatedUser();
            if (usuarioAutenticado.Rol == "Administrador" || usuarioAutenticado.Rol == "Notario Encargado")
            {
                var estadoPinFirma = await notarioService.ObtenerEstadoPinFirma(usuarioAutenticado.RegisteredUser.Email);
                pinAsignadoNotario = estadoPinFirma.PinAsignado && estadoPinFirma.FirmaRegistrada;
            }
        }

        private async Task ConsultarRolUsuario()
        {
            var userActual = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            AuthenticatedUser userName = await userActual.GetAuthenticatedUser();
            UserRol = userName.Rol;
        }

        public void IrAConfigurarPin()
        {
            NavigationManager.NavigateTo($"/Autorizaciones");
        }

        /// <summary>
        /// Consulta el acta
        /// </summary>
        /// <returns></returns>
        private async Task ConsultarActa()
        {
            esPdfGranTamanio = false;
            var acta = await actaNotarialService.ObtenerActaNotarial(IdTramite);
            MostrarAutorizar = !acta.Autorizada;
            titulo = acta.Autorizada ? "Documento Autorizado" : titulo;
            titulo = acta.Rechazada ? "Documento Rechazado" : titulo;

            if (acta != null)
            {
                if (_movil)
                {
                    pdfFile = "Previsualización";
                    await GenerarPrevisualizacionMovil(acta.Archivo);
                }
                else
                {
                    pdfFile = acta.Archivo;
                    if (acta.Archivo.Length > 2000000)
                    {
                        esPdfGranTamanio = true;
                    }
                }
            }
            else
            {
                MsgActaNoEncontrada = "¡Acta no encontrada!";
            }

            StateHasChanged();
        }

        async Task ObtenerPDF()
        {
            showSpinner = true;
            var currentMillis = DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            var fileName = $"Tramite_{IdTramite}_{DateTime.Now:yyyy-MM-dd}_{currentMillis}.pdf";
            await Js.InvokeVoidAsync("saveFile", pdfFile, "application/pdf", fileName);
            showSpinner = false;
        }

        async Task AutorizarSeleccionados()
        {
            clave = await _sessionStorageService.GetItemAsync<string>("__ucnc_p");

            if (string.IsNullOrWhiteSpace(clave) || !(await notarioService.ValidarSolicitudPin(clave).ConfigureAwait(true)))
            {
                await ToggleModal();
            }
            else
                await Firmar(clave);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("focusInputModal", idModal, idInput);
            }
        }

        async Task Firmar(string pinReceived)
        {
            string documentoSinFirmar = pdfFile;
            pdfFile = "";
            MsgActaNoEncontrada = "Espere por favor... ";
            MostrarErrorEnModal = false;

            var signedFile = await actaNotarialService.FirmaActaNotarialLote(new AutorizacionTramitesRequest() { Pin = pinReceived, TramiteId = new List<long>(1) { IdTramite } });

            var autorizados = signedFile.FindAll(x => !x.EsError);
            if (autorizados.Count > 0)
            {
                ShowNotification(IdTramite.ToString());
                MsgActaNoEncontrada = "";
                MsjAutorizacionResul = "Autorización Exitosa";
                MsjAutorizacionClass = "mensaje-ok";
                MostrarAutorizar = false;

                if (pinReceived.Length == 4)
                {
                    await _sessionStorageService.SetItemAsync("__ucnc_p", CifradoSHA512.Cifrar($"{pinReceived}{DateTime.Now.ToString("yyyyMMdd")}")).ConfigureAwait(true);
                    await ToggleModal();
                }
                NavigationManager.NavigateTo($"/bandejaEntrada/2");
            }
            var conError = signedFile.FindAll(x => x.EsError && x.CodigoResultado != 6);
            var estabanAutorizados = signedFile.FindAll(x => x.CodigoResultado == 6);
            if (conError.Count > 0)
            {
                string msgError = "Ocurrió un error intentando firmar los documentos";
                try
                {
                    switch ((EnumResultadoFirma)signedFile[0].CodigoResultado)
                    {
                        case EnumResultadoFirma.FirmaNoConfigurada:
                        case EnumResultadoFirma.PinNoAsignado:
                            msgError = "Por favor configure su firma y su pin en el módulo de autorizaciones";
                            await ToggleModal();

                            NavigationManager.NavigateTo($"/Autorizaciones");
                            break;
                        case EnumResultadoFirma.PinNoValido:
                            msgError = "El pin ingresado no es válido";
                            break;
                        case EnumResultadoFirma.ErrorServicioEstampa:
                            msgError = "Ocurrió un error con el servicio de estampa del documento";
                            break;
                    }
                }
                catch
                {

                }
                ShowErrorNotification(msgError);
            }
            ShowBackdrop = false;
            if (estabanAutorizados.Count > 0)
            {
                ShowInfoNotification(string.Join(", ", estabanAutorizados.Select(x => x.TramiteId).ToArray()));
                MsgActaNoEncontrada = "";
                MsjAutorizacionResul = "Autorización Exitosa";
                MsjAutorizacionClass = "mensaje-ok";
                MostrarAutorizar = false;

                if (pinReceived.Length == 4)
                {
                    await _sessionStorageService.SetItemAsync("__ucnc_p", CifradoSHA512.Cifrar($"{pinReceived}{DateTime.Now.ToString("yyyyMMdd")}")).ConfigureAwait(true);
                    await ToggleModal();
                }
                NavigationManager.NavigateTo($"/bandejaEntrada/2");
            }
        }

        async Task RechazarTramite(string pinDesdePinFirma)
        {
            MostrarErrorEnModal = false;
            if (!string.IsNullOrWhiteSpace(pinDesdePinFirma) && !string.IsNullOrWhiteSpace(MotivoRechazo))
            {
                TramiteRechazadoModel tramiteRechazadoReturn = new TramiteRechazadoModel
                {
                    MotivoRechazo = MotivoRechazo,
                    TramiteId = IdTramite,
                    Pin = pinDesdePinFirma
                };
                var estadoTramite = await actaNotarialService.RechazarTramiteNotarial(tramiteRechazadoReturn);
                await Js.InvokeVoidAsync("cerrarModal", idModal);
                if (estadoTramite.EsError)
                {
                    ShowErrorNotification(estadoTramite.Estado);
                }
                else
                {
                    var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Trámite rechazado", Detail = "Se ha rechazado el trámite con éxito", Duration = 6000 };
                    notificationService.Notify(message);
                    MostrarAutorizar = false;
                    NavigationManager.NavigateTo($"/Autorizar/4/{IdTramite}");
                    await actaPreviaChild.RefrescarComponente();
                }
            }
            else
            {
                MsjAutorizacionResul = "Ingrese su PIN y/o motivo de rechazo";
                MostrarErrorEnModal = true;
                MsjAutorizacionClass = "mensaje-alerta";
            }
        }
        public async Task ToggleModal()
        {
            await Js.InvokeVoidAsync("cerrarModal", "modalForPinFirma");
        }

        public async Task GenerarPrevisualizacionMovil(string archivo)
        {
            await Js.InvokeVoidAsync("previsualizacionMovil", archivo);
        }

        async void ShowNotification(string id)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Trámite autorizado correctamente", Detail = "Se han aprobado el trámite (" + id + ") y se iniciará el proceso de firma. Una vez finalice el proceso de firma podrá consultar la actas en la página de autorizados.", Duration = 7000 };
            notificationService.Notify(message);
        }
        async void ShowErrorNotification(string msgError)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = msgError, Duration = 6000 };
            notificationService.Notify(message);
        }
        async void ShowInfoNotification(string ids)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Info, Summary = "Trámites autorizados", Detail = "Los siguientes tramites se encontraban autorizados: " + ids, Duration = 5000 };
            notificationService.Notify(message);
        }

    }
}
