using Microsoft.AspNetCore.Components.Forms;
using PortalCliente.Data;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using PortalCliente.Helper;
using Microsoft.AspNetCore.Components.Web;
using PortalCliente.Services.DescriptorCliente;
using PortalCliente.Services;
using PortalCliente.Services.SignalR;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class DatosCiudadano : ComponentBase
    {
        [Inject]
        protected ISignalRv2Service ScannerService { get; set; }

        [Inject]
        private IConfiguracionesService _configuracionesService { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        [Inject]
        protected IParametricasService GeneralService { get; set; }

        [Inject]
        protected ITramiteService TramiteService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public PersonaInfo PersonaInfo { get; set; }

        [Parameter]
        public EventCallback<PersonaInfo> PersonaInfoChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        public string InputID = "input-id";
        private bool _movil = false;

        string CurrTipoDoc { get; set; }
        private DotNetObjectReference<DatosCiudadano> objRef;
        bool usarScanner;
        private static Action<string, string, string> action;

        private bool _incluirCelularEmail = false;
        private bool MostrarErrorEmail;
        private bool MostrarErrorCelular;
        private bool MostrarCheckDatosAdicionales = true;
        [Parameter] 
        public EventCallback<bool>  EmailCelular { get; set; }
        [Parameter]
        public Compareciente Compareciente { get; set; }
        private async Task ChildChanged(string property, object arg)
        {
            switch (property)
            {
                case "TipoIdentificacion":
                    CurrTipoDoc = (string)arg;
                    PersonaInfo.TipoIdentificacion = _tiposIdentificaciones.FirstOrDefault(t => t.Abreviatura == CurrTipoDoc);
                    break;
                case "NumeroIdentificacion":
                    PersonaInfo.NumeroIdentificacion = ((string)arg).ToUpper().Trim();
                    break;
                case "Nombres":
                    PersonaInfo.Nombres = ((string)arg).ToUpper().Trim();
                    break;
                case "Apellidos":
                    PersonaInfo.Apellidos = ((string)arg).ToUpper().Trim();
                    break;
                case "NumeroCelular":
                    PersonaInfo.NumeroCelular = (string)arg;
                    ValidarCelular(PersonaInfo.NumeroCelular);
                    break;
                case "Email":
                    PersonaInfo.Email = ((string)arg).ToUpper();
                    ValidarEmail(PersonaInfo.Email);
                    break;
            }
            await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
        }

        private TipoIdentificacion[] _tiposIdentificaciones;

        private async Task EstablecerTipoDocumentoDefecto()
        {
            if (_tiposIdentificaciones != null && _tiposIdentificaciones.Length > 0)
            {
                if (PersonaInfo != null && PersonaInfo.TipoIdentificacion == null)
                {
                    PersonaInfo.TipoIdentificacion = _tiposIdentificaciones[0];
                    await PersonaInfoChanged.InvokeAsync(PersonaInfo);
                    await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
                }
            }
        }

        protected async override Task OnInitializedAsync()
        {
            InputDatosAdicionales();
            action = UpdateMessage;
            _tiposIdentificaciones = await GeneralService.ObtenerTiposIdentificacion();
            _movil = await DescriptorCliente.EsMovil;
            await EstablecerTipoDocumentoDefecto();
            objRef = DotNetObjectReference.Create(this);
            await ScannerService.AgregarFuncionesNativas(objRef);
            if (!_movil)
            {
                var configuracionScanner = await _configuracionesService.GetConfigScanner();
                await ScannerService.ObtenerListaScanners();
                //var escaners = await ScannerService.ObtenerEscanerVariable();
                //if (escaners == null)
                //{
                //    usarScanner = false;
                //}
                //else
                //{
                    usarScanner = configuracionScanner.UsarScanner;
                //}
            }

            

            if (!string.IsNullOrEmpty(PersonaInfo.Email) && !string.IsNullOrEmpty(PersonaInfo.NumeroCelular))
            {
                _incluirCelularEmail = true;
            }

        }

        private async void UpdateMessage(string nombres, string apellidos, string documento)
        {
            PersonaInfo.NumeroIdentificacion = documento;
            PersonaInfo.Apellidos = apellidos;
            PersonaInfo.Nombres = nombres.Trim();
            StateHasChanged();
            await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
            StateHasChanged();
        }

        protected async Task Escanear()
        {
            if (!_movil)
            {
                var configuracionScanner = await _configuracionesService.GetConfigScanner();
                if (configuracionScanner.UsarScanner)
                {
                    await ScannerService.EnviarAEscanear(configuracionScanner.Opciones);
                }
            }

        }

        protected async override Task OnParametersSetAsync()
        {
            if (PersonaInfo == null)
            {
                PersonaInfo = new PersonaInfo();
                await PersonaInfoChanged.InvokeAsync(PersonaInfo);
            }

            await EstablecerTipoDocumentoDefecto();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && PersonaInfo != null)
            {
                await Focus();
                await ReadyChanged.InvokeAsync(PersonaInfo.IsValid());
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public void AgregarValoresACampos(string nombres, string apellidos, string documento)
        {
            action.Invoke(nombres, apellidos, documento);
        }

        public async Task Focus()
        {
            await JSRuntime.InvokeVoidAsync("focusInput", InputID);
        }
        protected void ToggleIncluirCelularEmail()
        {
            _incluirCelularEmail = !_incluirCelularEmail;

            if(!_incluirCelularEmail) 
                LimpiarEmailNumeroCelular();
            EmailCelular.InvokeAsync(_incluirCelularEmail);
            
        }
        private bool ValidarCelular(string celular)
        {
            MostrarErrorCelular = false;
            bool esValido = Regex.IsMatch(celular, @"^3\d{9}$", RegexOptions.IgnoreCase);
            if (!esValido) MostrarErrorCelular = true;
            return esValido;
        }
        private bool ValidarEmail(string email)
        {
            MostrarErrorEmail = false;
            bool esValido = Regex.IsMatch(email, @"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}", RegexOptions.IgnoreCase);
            if (!esValido) MostrarErrorEmail = true;
            return esValido;
        }
        private void LimpiarEmailNumeroCelular()
        {
            PersonaInfo.Email = string.Empty;
            PersonaInfo.NumeroCelular = string.Empty;
            MostrarErrorCelular = false;
            MostrarErrorEmail = false;
        }

        private void InputDatosAdicionales()
        {
            if (Compareciente.Tramite.TipoTramite.Nombre.ToUpper() == "Enrolamiento notaria digital".ToUpper())
            {
                MostrarCheckDatosAdicionales = false;
                var datosAdicionales = JsonSerializer.Deserialize<EnrolamientoNotariaDigitalDTO>(Compareciente.Tramite.DatosAdicionales);
                PersonaInfo.NumeroCelular = datosAdicionales.Telefono;
                PersonaInfo.Email = datosAdicionales.Correo;
                EmailCelular.InvokeAsync(true);
            }
        }
    }
}
