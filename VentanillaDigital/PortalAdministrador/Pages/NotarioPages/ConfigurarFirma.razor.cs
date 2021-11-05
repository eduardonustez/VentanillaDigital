using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalAdministrador.Services.Wacom;
using PortalAdministrador.Services.Wacom.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using PortalAdministrador.Services.Recursos;
using System.Net.Http;
using PortalAdministrador.Services.Notario;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using Blazored.SessionStorage;
using PortalAdministrador.Data.Account;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Services;

namespace PortalAdministrador.Pages.NotarioPages
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

        [Inject]
        public IWacomService WacomService { get; set; }

        [Inject]
        protected IRecursosService RecursosService { get; set; }

        [Inject]
        protected INotarioService NotarioService { get; set; }

        [Inject]
        private AuthenticationStateProvider _sessionStorageService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AuthenticatedUser userName = await GetAuthenticatedUser();
            UserEmail = userName.Usuario;
            await ObtenerEstadoPinFirma();
            if (!banFirma)
                await MostrarADTP();
            else
                grafo = await NotarioService.ObtenerGrafo(UserEmail);
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("skipToNextInput");
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
            await WacomService.IniciarCaptura("images/Pad/FirmaNotario.jpg", botones, true);
            await Task.Delay(1500).ContinueWith(async (t) =>
            {
                if (!WacomService.Conectado)
                    await WacomService.IniciarCaptura("images/Pad/FirmaNotario.jpg", botones, true);
            });
        }

        protected async Task ObtenerFirma()
        {
            //Notario.Firma = await WacomService.ObtenerFirma(720, 1200);
            grafo = await WacomService.ObtenerFirma(720, 1200, new Rectangle(53, 315, 1095, 218));
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
            await JsRuntime.InvokeVoidAsync("cerrarModal", "#modalPIN");
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

        public async void Dispose()
        {
            if (_capturando)
                await WacomService.TerminarCaptura();
        }
    }
}
