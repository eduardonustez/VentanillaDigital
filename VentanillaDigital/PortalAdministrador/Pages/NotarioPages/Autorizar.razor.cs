using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Models.Transaccional;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using ApiGateway.Contratos.Enums;
using Microsoft.JSInterop;

namespace PortalAdministrador.Pages.NotarioPages
{
    public partial class Autorizar : ComponentBase
    {
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

        string pdfFile;
        string MsgActaNoEncontrada = "";
        bool MostrarAutorizar = true;
        string MsjAutorizacionResul = "";
        string MsjAutorizacionClass = "";
        string titulo = "Revisión de Documento";
        bool MostrarErrorEnModal = false;
        string MotivoRechazo { get; set; }
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
        //private bool obteniendo = false;

        protected override async Task OnInitializedAsync()
        {
            await ConsultarRolUsuario();
            if(PEstadoTramite=="3")
                await ConsultarActa();
        }

        //protected override async Task OnParametersSetAsync()
        //{
        //    if(!obteniendo)
        //    {
        //        obteniendo = true;
        //        var acta = await actaNotarialService.ObtenerActaNotarial(IdTramite);
        //        MostrarAutorizar = (!acta.Autorizada && !acta.Rechazada);
        //        titulo = acta.Autorizada ? "Documento Autorizado":titulo;
        //        titulo = acta.Rechazada ? "Documento Rechazado" : titulo;

        //        if (acta != null)
        //        {
        //            pdfFile = acta.Archivo;
        //        }
        //        else
        //        {
        //            MsgActaNoEncontrada = "¡Acta no encontrada!";
        //        }
        //        StateHasChanged();
        //    }
        //}

        private async Task ConsultarRolUsuario()
        {
            var userActual = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            AuthenticatedUser userName = await userActual.GetAuthenticatedUser();
            UserRol = userName.Rol;
            Console.WriteLine("Consulta ROl en Autorizar:" + UserRol);
        }

        /// <summary>
        /// Consulta el acta
        /// </summary>
        /// <returns></returns>
        private async Task ConsultarActa()
        {
            Console.WriteLine("Consultando acta");
            var acta = await actaNotarialService.ObtenerActaNotarial(IdTramite);
            Console.WriteLine("Acta tiene datos " + acta.Autorizada);
            MostrarAutorizar = !acta.Autorizada;
            titulo = acta.Autorizada ? "Documento Autorizado" : titulo;
            titulo = acta.Rechazada ? "Documento Rechazado" : titulo;

            if (acta != null)
            {
                pdfFile = acta.Archivo;
            }
            else
            {
                MsgActaNoEncontrada = "¡Acta no encontrada!";
            }

            StateHasChanged();
        }

        async void Firmar(string pinReceived)
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
                //pdfFile = signedFile.Archivo;
                MsgActaNoEncontrada = "";
                MsjAutorizacionResul = "Autorización Exitosa";
                MsjAutorizacionClass = "mensaje-ok";
                MostrarAutorizar = false;
                CerrarModal();
                await Js.InvokeVoidAsync("cerrarModal", "#modalForPinFirma");
                NavigationManager.NavigateTo($"/bandejaEntrada/2");
            }
            //else
            //{
            //    MsgActaNoEncontrada = "Acta no encontrada!";
            //    CerrarModal();
            //}
            var conError = signedFile.FindAll(x => x.EsError);
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
                            await Js.InvokeVoidAsync("cerrarModal", "#modalForPinFirma");

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
                if (estadoTramite.EsError)
                {
                    MsjAutorizacionResul = estadoTramite.Estado;
                    MsjAutorizacionClass = "mensaje-error";
                }
                else
                {
                    MsjAutorizacionResul = "Rechazo Confirmado";
                    MsjAutorizacionClass = "mensaje-ok";
                    MostrarAutorizar = false;
                }
                await Js.InvokeVoidAsync("cerrarModal", "#modalRechazoTramite");
            }
            else
            {
                MsjAutorizacionResul = "Ingrese su PIN y/o motivo de rechazo";
                MostrarErrorEnModal = true;
                MsjAutorizacionClass = "mensaje-alerta";
            }
        }

        public void CerrarModal()
        {
            ModalFirmaDisplay = "none";
            ShowBackdrop = false;
            StateHasChanged();
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

    }
}
