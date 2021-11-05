using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Wacom;
using PortalCliente.Services.Wacom.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;
using Radzen;
using PortalCliente.Services.Recursos;
using PortalCliente.Services.Notario;
using PortalCliente.Data.Account;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Services;
using PortalCliente.Components.Transversales;
using ApiGateway.Contratos.Models.Certificado;
using GenericExtensions;
using System.Collections.Generic;
using System.Linq;
namespace PortalCliente.Pages.NotarioPages
{
    public partial class ConfigurarFirma : ComponentBase, IDisposable
    {
        bool _capturando = false;
        bool _firmaRecibida = false;
        bool banPin = false;
        bool banFirma = false;
        string _valorPin;
        string grafo = "";
        string UserEmail = "";
        private byte codAscii;

        string CreacionPinResul = "";
        string CreacionPinClass = "";
        string CreacionFirmaResul = "";
        string CreacionFirmaClass = "";
        bool showSpinner = false;
        string pinNotario;
        int CertificateStep = 1;
        bool continueIsDisabled = true;
        bool tieneCertificadosActivos = true;

        [Inject]
        IWacomServiceFactory wacomServiceFactory { get; set; }
        protected IWacomService WacomService { get; set; }
        [Inject]
        protected IConfiguracionesService _configuraciones { get; set; }

        [Inject]
        protected IRecursosService RecursosService { get; set; }

        [Inject]
        protected INotarioService NotarioService { get; set; }
        [Inject]
        protected ICertificadoService CertificadoService { get; set; }

        [Inject]
        private AuthenticationStateProvider _sessionStorageService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        NotificationService notificationService { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }
        public ModalForm modalForm { get; set; }
        SolicitudCertificadoDto solicitudCertificado { get; set; }
        // SolicitudCertificadoDto solicitudCertificado
        // {
        //     get { return _solicitudCertificado; }
        //     set
        //     {
        //         if(_solicitudCertificado==value) return;
        //         _solicitudCertificado =solicitudCertificado;
        //         //ContinuarSolicitud();
        //     }
        // }
        bool pinValidado = false;
        bool solicitudCerEnProceso = false;
        string errorSolicitud = "";
        List<CertificadoDTO> certificados = new List<CertificadoDTO>();
        SignatureSize signatureSize = new SignatureSize(720, 1200, 4.5F);
        string channelId;
        protected override async Task OnInitializedAsync()
        {
            //solicitudCertificado = new SolicitudCertificado();
            channelId = await _configuraciones.GetWacomChannelId();
            WacomService = await wacomServiceFactory.GetWacomServiceInstance(channelId);
            AuthenticatedUser userName = await GetAuthenticatedUser();
            UserEmail = userName.RegisteredUser.UserName;
            await ObtenerEstadoPinFirma();
            if (!banFirma)
                await MostrarADTP();
            else
                grafo = await NotarioService.ObtenerGrafo(UserEmail);
            await ObtenerCertificados();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("skipToNextInput", "pin-autorizar");
            }
        }

        async Task ObtenerEstadoPinFirma()
        {
            var estadoPinFirma = await NotarioService.ObtenerEstadoPinFirma(UserEmail);
            if (estadoPinFirma.FirmaRegistrada)
            {
                banFirma = true;
            }
            if (estadoPinFirma.PinAsignado)
            {
                banPin = true;
            }
            solicitudCerEnProceso = estadoPinFirma.CertificadoSolicitado;
        }

        protected async Task MostrarADTP()
        {
            _capturando = true;
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
            await WacomService.IniciarCaptura("images/Pad/FirmaNotario.jpg", botones, true,signatureSize);
            if (channelId != "2")
            {
                await Task.Delay(1500).ContinueWith(async (t) =>
                {
                    if (!WacomService.Conectado)
                        await WacomService.IniciarCaptura("images/Pad/FirmaNotario.jpg", botones, true,signatureSize);
                });
            }
        }

        protected async Task ObtenerFirma()
        {
            //Notario.Firma = await WacomService.ObtenerFirma(720, 1200);
            grafo = await WacomService.ObtenerFirma(signatureSize.Height,
                                    signatureSize.Width, signatureSize.LineWidth);
            _firmaRecibida = true;
            _capturando = false;
            StateHasChanged();
            //await ComparecienteChanged.InvokeAsync(Compareciente);
            await ReadyChanged.InvokeAsync(true);
            await WacomService.TerminarCaptura();
            await GuardarFirma();
        }

        private async Task AdicionarPIN(string value)
        {
            _valorPin = value;
            await JsRuntime.InvokeVoidAsync("cerrarModal", "modalPIN");
            bool resultado = await NotarioService.ConfigurarFirmaPin(UserEmail, _valorPin, grafo);
            if (resultado)
            {
                CreacionPinClass = "alert-warning";
                CreacionPinResul = "¡Pin asignado correctamente!";
            }
            else
            {
                CreacionPinClass = "alert-danger";
                CreacionPinResul = "¡Ocurrió un error asignando el Pin!";
            }
            banPin = resultado;

        }

        protected async Task GuardarFirma()
        {
            bool resultado = await NotarioService.ConfigurarFirmaPin(UserEmail, _valorPin, grafo);
            if (resultado)
            {
                CreacionFirmaClass = "alert-warning";
                CreacionFirmaResul = "¡Firma registrada correctamente!";
            }
            else
            {
                CreacionFirmaClass = "alert-danger";
                CreacionFirmaResul = "¡Ocurrió un error registrando la Firma!";
            }
            banFirma = resultado;
            StateHasChanged();
        }

        public async Task<AuthenticatedUser> GetAuthenticatedUser()
        {
            var userActual = (CustomAuthenticationStateProvider)_sessionStorageService;
            return await userActual.GetAuthenticatedUser();
        }
        public async Task SolicitarCertificado()
        {
            if(banFirma && banPin)
            {
                solicitudCertificado = await CertificadoService.ObtenerDatosSolicitud();
                errorSolicitud = "";
                modalForm.Open();
            }
            else
            {
                var message = new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Key Manager",
                    Detail = $"Por favor asegurese de configurar su Pin y su Firma antes de continuar!",
                    Duration = 5000
                };
                notificationService.Notify(message);
            }
        }

        public async void Dispose()
        {
            if (_capturando)
                await WacomService.TerminarCaptura();
        }
        protected async Task ContinuarSolicitud()
        {
            errorSolicitud = "";
            continueIsDisabled = true;
            switch (CertificateStep)
            {
                case 1:
                    CertificateStep = 2;
                    continueIsDisabled=false;
                    break;
                case 2:
                    solicitudCertificado.AceptarTyc = true;
                    CertificateStep = 3;
                    break;
                case 3:
                    CertificateStep = 4;
                    continueIsDisabled = false;
                    break;
                case 4:
                    continueIsDisabled = false;
                    showSpinner = true;
                    pinValidado = await NotarioService.EsPinValido(solicitudCertificado.PinFirma);
                    if (!pinValidado)
                    {
                        errorSolicitud = "¡El pin ingresado no es válido!";
                    }
                    else
                    {
                        await SolicitarCer(solicitudCertificado);
                        modalForm.Close();
                    }
                    showSpinner = false;
                    break;
            }
        }
        protected async Task IrAtras()
        {
            continueIsDisabled = false;
            if (CertificateStep > 1)
            {
                CertificateStep -= 1;
                errorSolicitud = "";
            }
        }
        protected async Task SolicitarCer(SolicitudCertificadoDto solicitud)
        {
            solicitudCertificado = solicitud;
            showSpinner = true;
            bool isOk = await CertificadoService.RegistrarSolicitud(solicitud);
            if (isOk==true)
            {
                var message = new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Key Manager",
                    Detail = $"Su solicitud ha sido registrada con éxito!",
                    Duration = 5000
                };
                CertificateStep=0;
                notificationService.Notify(message);
                modalForm.Close();
                await ObtenerCertificados();
            }
            else
            {
                var message = new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Key Manager",
                    Detail = "Su solicitud no se ha podido completar, por favor vuelva a intentarlo!",
                    Duration = 5000
                };
                notificationService.Notify(message);
            }
            showSpinner = false;
        }
        public void HandleSolicitudChanged(SolicitudCertificadoDto solicitud)
        {
            solicitudCertificado = solicitud;
            errorSolicitud = "";
            switch (CertificateStep)
            {
                case 1:
                    if (string.IsNullOrWhiteSpace(solicitudCertificado.Celular) ||
                               string.IsNullOrWhiteSpace(solicitudCertificado.Direccion))
                    {
                        //errorSolicitud = "¡Por favor complete toda la información!";
                        continueIsDisabled = true;
                    }
                    else
                        continueIsDisabled = false;
                    break;
                case 3:
                    if (solicitudCertificado.Cedula == null ||
                          solicitudCertificado.CComercio == null || solicitudCertificado.Rut == null ||
                          solicitudCertificado.Autorizacion == null ||
                          solicitudCertificado.Contrato == null ||
                          (solicitudCertificado.Cargo =="Notario Encargado" && solicitudCertificado.CedulaPrincipal == null))
                    {
                        //errorSolicitud = "¡Por favor adjunte todos los documentos!";
                        continueIsDisabled = true;
                    }
                    else
                    {
                        continueIsDisabled = false;
                    }
                    break;
                default:
                    continueIsDisabled = false;
                    break;
            }

        }
        public void HandlePinChanged(string pin)
        {
            solicitudCertificado.PinFirma = pin;
            HandleSolicitudChanged(solicitudCertificado);
        }
        public async Task SeleccionarCertificado(int id)
        {
            bool resul = await CertificadoService.ActualizarCertificadoNotario(id);
            NotificationMessage message = new NotificationMessage()
            {
                Summary = "Key Manager",
                Duration = 5000
            };
            if (resul)
            {
                certificados.ForEach(c => {
                    if (c.IdCertificado == id)
                        c.Seleccionado = true;
                    else
                        c.Seleccionado = false;
                });
                message.Severity = NotificationSeverity.Success;
                message.Detail = $"¡Cambio de certificado realizado con éxito!";
            }
            else
            {
                message.Severity = NotificationSeverity.Error;
                message.Detail = "¡Ocurrió un error intentando cambiar el certificado!";
            }
            notificationService.Notify(message);
        }
        private async Task ObtenerCertificados()
        {
            tieneCertificadosActivos = true;
            certificados = (await CertificadoService.ObtenerCertificados()).ToList();
            if (certificados != null && certificados.Count>0)
            {
                tieneCertificadosActivos = certificados.Any(c => c.Estado == "Activo" && c.IdCertificado != 0);
            }
            else
            {
                tieneCertificadosActivos = false;
            }
        }
    }
}
