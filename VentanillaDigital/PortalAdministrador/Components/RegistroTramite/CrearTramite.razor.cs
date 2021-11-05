using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PortalAdministrador.Data;
using PortalAdministrador.Data.DatosTramite;
using PortalAdministrador.Services;
using Radzen;
using PortalAdministrador.Services.DescriptorCliente;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class CrearTramite : ComponentBase
    {
        string TextoRecibido = "";

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

        private bool _bloquearNumeroComparecientes = false;
        private bool _esCrearTramite = false;
        private bool _esAndroid = false;
        private string lugarComparecencia = "hola";
        private string direccionComparecencia = "";

        private async Task ChildChanged(string prop, object args)
        {
            var documentosPrivados = new List<CodigoTipoTramite>{
                CodigoTipoTramite.DocumentoPrivadoFirmaARuego,
                CodigoTipoTramite.DocumentoPrivadoInvidente
            };
            switch (prop)
            {
                case "TipoTramite":
                    Tramite.TipoTramite = (TipoTramite)args;
                    if (documentosPrivados.Contains((CodigoTipoTramite)Tramite.TipoTramite?.CodigoTramite))
                    {
                        _bloquearNumeroComparecientes = true;
                        if((CodigoTipoTramite)Tramite.TipoTramite?.CodigoTramite == CodigoTipoTramite.DocumentoPrivadoFirmaARuego)
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
            categorias = await ParametricasService.ObtenerCategorias();
            _esAndroid = await descriptorCliente.EsMovil;
            Console.WriteLine("Es movil: " + _esAndroid);
        }

        protected async Task Crear()
        {
            Tramite.DatosAdicionales = TextoRecibido;
            Tramite.FueraDeDespacho = lugarComparecencia == "FueraDespacho" ? true : false;
            Tramite.DireccionComparecencia = direccionComparecencia;
            string resultadoValidacion = Tramite.IsValid(Tramite.TipoTramite.TipoTramiteId);
            Console.WriteLine("Resultado validacion: " + resultadoValidacion);
            if (string.IsNullOrEmpty(resultadoValidacion))
            {
                var configuraciones = await configuracionesService.ObtenerOpcionesConfiguracion();
                Tramite.UsarSticker = configuraciones.UsarSticker;
                Tramite.TramiteId = await TramiteService.CrearTramite(Tramite);
                FillDataActa();
                Tramite.ComparecienteActual = Compareciente.ObtenerNuevoCompareciente();
                await TramiteChanged.InvokeAsync(Tramite);
            }
            else
            {
                var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = $"{resultadoValidacion}", Duration = 4000 };
                notificationService.Notify(message);
                await TramiteChanged.InvokeAsync(Tramite);
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
