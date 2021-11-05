using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PortalCliente.Data;
using PortalCliente.Data.DatosTramite;
using PortalCliente.Services;
using Radzen;
using PortalCliente.Services.DescriptorCliente;
using System.Collections.Generic;
using System;
using ApiGateway.Contratos.Models.Notario;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class CrearTramite : ComponentBase
    {
        string TextoRecibido = "";

        [Inject]
        protected ITramiteVirtualService TramitesVirtualService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IParametricasService ParametricasService { get; set; }


        [Inject]
        protected ITramiteService TramiteService { get; set; }

        [Inject]
        NotificationService notificationService { get; set; }
        [Inject]
        IConfiguracionesService configuracionesService { get; set; }

        [Inject]
        public IDescriptorCliente descriptorCliente { get; set; }

        [Parameter]
        public Tramite Tramite { get; set; }

        [Parameter]
        public EventCallback<Tramite> TramiteChanged { get; set; }

        bool showSpinner;

        private bool _bloquearNumeroComparecientes = false;
        private bool _esCrearTramite = false;
        private bool _esAndroid = false;
        private string lugarComparecencia = "hola";
        private string direccionComparecencia = "";
        bool _usarStickerConfigurado;
        bool _firmaManualConfigurado;
        string _notarioConfigurado;

        private async Task ChildChanged(string prop, object args)
        {
            var documentosPrivados = new List<CodigoTipoTramite>{
                CodigoTipoTramite.DocumentoPrivadoFirmaARuego,
                CodigoTipoTramite.DocumentoPrivadoInvidente,
                CodigoTipoTramite.EnrolamientoNotariaDigital
            };
            switch (prop)
            {
                case "TipoTramite":

                    Tramite.TipoTramite = (TipoTramite)args;
                    if (documentosPrivados.Contains((CodigoTipoTramite)Tramite.TipoTramite?.CodigoTramite))
                    {
                        _bloquearNumeroComparecientes = true;
                        if ((CodigoTipoTramite)Tramite.TipoTramite?.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoFirmaARuego)
                        {
                            Tramite.CantidadComparecientes = 2;
                        }
                        else
                        {
                            Tramite.CantidadComparecientes = 1;
                        }
                    }
                    else
                    {
                        _bloquearNumeroComparecientes = false;
                        Tramite.CantidadComparecientes = 1;
                    }
                    break;
                case "CantidadComparecientes":
                    Tramite.CantidadComparecientes = int.Parse((string)args);
                    break;
            }
            TextoRecibido = "";
            await TramiteChanged.InvokeAsync(Tramite);
        }

        private Categoria[] categorias;
        protected override async Task OnInitializedAsync()
        {

            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var authuser = state.User;
            bool esNotariaVirtual = false;
            if (long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out long notariaId))
            {
                try
                {
                    esNotariaVirtual = await TramitesVirtualService.ValidarConvenioNotariaVirtual(notariaId);
                }
                catch { }
            }
            categorias = await ParametricasService.ObtenerCategorias(esNotariaVirtual);
            _esAndroid = await descriptorCliente.EsMovil;
            if (_esAndroid)
            {
                UpdateLugar(2);
            }
            await VerificarOpcionesFirmaManual();
        }

        private async Task VerificarOpcionesFirmaManual()
        {
            var opc_conf = await configuracionesService.ObtenerOpcionesConfiguracion();
            _usarStickerConfigurado = opc_conf.UsarSticker;
            _firmaManualConfigurado = opc_conf.FirmaManual;
            if (_firmaManualConfigurado)
            {
                List<NotarioReturnDTO> notarioReturnDTOs = await configuracionesService.ObtenerNotariosNotaria();
                if(notarioReturnDTOs!=null && notarioReturnDTOs.Any(n => n.NotarioDeTurno == true))
                    _notarioConfigurado = notarioReturnDTOs.Where(n => n.NotarioDeTurno == true).FirstOrDefault().NotarioNombre; 
            }
        }

        protected async Task Crear()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity.IsAuthenticated) {
                Tramite.DatosAdicionales = TextoRecibido;
                Tramite.FueraDeDespacho = lugarComparecencia == "FueraDespacho";
                Tramite.DireccionComparecencia = direccionComparecencia;
                string resultadoValidacion = Tramite.IsValid(Tramite.TipoTramite.TipoTramiteId);

                if (string.IsNullOrEmpty(resultadoValidacion))
                {
                    long valorDefecto = Tramite.TramiteId;
                    showSpinner = true;
                    StateHasChanged();
                    var configuraciones = await configuracionesService.ObtenerOpcionesConfiguracion();
                    Tramite.UsarSticker = configuraciones.UsarSticker;
                    Tramite.TramiteId = await TramiteService.CrearTramite(Tramite);
                    if (Tramite.TramiteId > 0)
                    {
                        FillDataActa();
                        Tramite.ComparecienteActual = Compareciente.ObtenerNuevoCompareciente();
                        // Add JS function that adds current TramiteId and test
                        await Js.InvokeVoidAsync("agregarIdTramiteaURL", Tramite.TramiteId);
                    }
                    else
                    {
                        Tramite.TramiteId = valorDefecto;
                        showSpinner = false;
                        var message = new NotificationMessage() { Severity = NotificationSeverity.Warning, Summary = "Crear trámite", Detail = $"El trámite no fue creado. Intente nuevamente.", Duration = 4000 };
                        notificationService.Notify(message);
                    }
                    await TramiteChanged.InvokeAsync(Tramite);
                }
                else
                {
                    showSpinner = false;
                    var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"{resultadoValidacion}", Duration = 4000 };
                    notificationService.Notify(message);
                    await TramiteChanged.InvokeAsync(Tramite);
                }
                
            } else {
                await Js.InvokeVoidAsync("cerrarModal", "sesionClosedModalCenter");
                await Task.Delay(4000);
                await Js.InvokeVoidAsync("cerrarModal", "sesionClosedModalCenter");
                NavigationManager.NavigateTo("/login");
            }

        }

        private void FillDataActa()
        {
            Tramite.InfoActa.TramiteId = Tramite.TramiteId;
            Tramite.InfoActa.FechaTramite = DateTime.Now;
            Tramite.InfoActa.TipoTramite = Tramite.TipoTramite.TipoTramiteId.ToString();
            Tramite.InfoActa.DataAdicional = Tramite.DatosAdicionales;
            Tramite.InfoActa.CodigoTramite = Tramite.TipoTramite.CodigoTramite;
        }

        void SetCamposAdicionales(string camposAdicionales)
        {
            TextoRecibido = camposAdicionales;
        }

        void NoSabeFirmarChanged(bool SabeFirmar)
        {
            Tramite.CantidadComparecientes = !SabeFirmar ? 2 : 1;
        }

        void SumarCompareciente()
        {
            Tramite.CantidadComparecientes++;
        }

        void RestarCompareciente()
        {
            if (Tramite.CantidadComparecientes > 1)
            {
                Tramite.CantidadComparecientes--;
            }
        }
        void UpdateLugar(int l)
        {
            lugarComparecencia = l == 1 ? "EnNotaria" : "FueraDespacho";
            StateHasChanged();

        }
    }
}
