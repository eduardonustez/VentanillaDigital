using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalAdministrador.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalAdministrador.Data;
using PortalAdministrador.Components.Transversales;
using PortalAdministrador.Services;
using ApiGateway.Contratos.Models.Notario;
using Radzen;
using System.Threading;
using ApiGateway.Contratos.Enums;
using Newtonsoft.Json;
namespace PortalAdministrador.Pages.NotarioPages
{
    public partial class BandejaEntrada : ComponentBase
    {
        [Inject]
        IJSRuntime Js { get; set; }

        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }
        [Inject]
        NotificationService notificationService { get; set; }

        DataGrid child;

        TramitePendienteAutorizacionModel pendientesAutorizacion;
        FiltrosBandejaEntrada filtrosBandejaEntrada;
        DefinicionFiltro model = new DefinicionFiltro();
        object[,] registros;
        string[] columnas = { "Consec.", "Tipo de Trámite", "Comparecientes", " n.º Documento", "Fecha", "Estado" };
        int totalRegistros = 0;
        int totalPaginas = 0;
        string TituloBandejaEntrada = "Bandeja De Entrada";
        bool UsarFiltros = false;
        long notariaId;
        // Logica de modales
        public Guid Guid = Guid.NewGuid();
        public string ModalFirmaDisplay = "none;";
        public string ModalDisplay = "none;";
        public string ModalFirma = "";
        public string ModalRechazo = "";
        public bool ShowBackdrop = false;
        public string UserRol = "";
        string MsgActaNoEncontrada = "";
        bool MostrarAutorizar = true;
        string MsjAutorizacionResulOK = "";
        string MsjAutorizacionResulError = "";
        string MsjAutorizacionClass = "";
        bool MostrarErrorEnModal = false;
        int indice_actual = 1;
        bool mostrarCheckboxes = false;

        public List<string> paginasNoPendientes = (new List<string> { "3", "4" });

        //[Parameter]
        //public string PEstadoTramite { get; set; }
        string _value;

        [Parameter]
        public string PEstadoTramite
        {
            get { return _value; }
            set
            {
                _value = value;
                SetTitle();
                LimpiarCampos();
                ObtenerPendientes(1);
            }
        }

        FiltrosBandejaEntrada valoresPorDefectoFiltros()
        {
            return new FiltrosBandejaEntrada
            {
                FechaInicio = DateTime.Now.AddMonths(-1),
                FechaFin = DateTime.Now
            };
        }

        void SetTitle()
        {
            switch (long.Parse(this.PEstadoTramite))
            {
                case 3:
                    TituloBandejaEntrada = "Trámites Autorizados";
                    break;
                case 4:
                    TituloBandejaEntrada = "Trámites Rechazados";
                    break;
                default:
                    TituloBandejaEntrada = "Trámites Pendientes";
                    break;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            model.RegistrosPagina = 20;
            filtrosBandejaEntrada = valoresPorDefectoFiltros();
            // if (PEstadoTramite == "3")
            // {
            //     StartCountdown();
            // };
            //ObtenerPendientes(1);
        }
        async void ObtenerPendientes(int indice)
        {
            mostrarCheckboxes = false;
            indice_actual = indice;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var authuser = state.User;
            await Js.InvokeVoidAsync("habilitarFilas");

            if (!long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                notariaId = -1;

            model.IndicePagina = indice;
            model.TotalRegistros = 0;
            List<Ordenacion> ordenacion = new List<Ordenacion>();
            ordenacion.Add(new Ordenacion("Tramites.FechaCreacion", "DESC"));
            model.Ordenacion = ordenacion;
            model.Filtros = new List<Filtro>();
            model.Filtros.Add(new Filtro("Tramites.NotariaId", notariaId.ToString()));
            switch (long.Parse(this.PEstadoTramite))
            {
                case 3:
                    model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Autorizado"));
                    model.Filtros.Add(new Filtro("DocumentosPendienteAutorizar.DocumentoPendienteAutorizarId", "", "NONULO", "", "OR"));
                    break;
                case 4:
                    model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Rechazado"));
                    break;
                default:
                    model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Pendiente de Autorización"));
                    model.Filtros.Add(new Filtro("DocumentosPendienteAutorizar.DocumentoPendienteAutorizarId", "", "IS NULL"));
                    mostrarCheckboxes = true;
                    break;
            }
            if (UsarFiltros)
            {
                UtilizarFiltros();
            }
            Console.WriteLine("filtros utilizados: " + JsonConvert.SerializeObject(model.Filtros));
            //pendientesAutorizacion = await tramiteService.ObtenerPendientesAutorizacion(model);
            //if (pendientesAutorizacion != null)
            //{
            //    registros = pendientesAutorizacion.Pendientes.To2DArray(p => p.Id, p => p.TipoTramite, p => p.Comparecientes,
            //    p => p.DocCompareciente, p => p.Fecha, p => p.Estado);
            //    totalRegistros = pendientesAutorizacion.TotalRegistros;
            //    totalPaginas = pendientesAutorizacion.TotalPaginas;
            //}
            StateHasChanged();

        }

        private void UtilizarFiltros()
        {
            if (filtrosBandejaEntrada.IdTramite != 0)
            {
                model.Filtros.Add(new Filtro("Tramites.TramiteId", filtrosBandejaEntrada.IdTramite.ToString()));
            }
            else
            {
                if (filtrosBandejaEntrada.FechaFin != null && filtrosBandejaEntrada.FechaInicio != null)
                {
                    model.Filtros.Add(new Filtro("Tramites.FechaCreacion", filtrosBandejaEntrada.FechaInicio.ToString("yyyy-MM-dd"), "between", filtrosBandejaEntrada.FechaFin.AddDays(1).ToString("yyyy-MM-dd")));
                }
                if (filtrosBandejaEntrada.NuipComparenciente != 0)
                {
                    model.Filtros.Add(new Filtro("Personas.NumeroDocumento", filtrosBandejaEntrada.NuipComparenciente.ToString()));
                }
                if (filtrosBandejaEntrada.NuipOperador != 0)
                {
                    model.Filtros.Add(new Filtro("OPERADORES.NumeroDocumento", filtrosBandejaEntrada.NuipOperador.ToString()));
                }

            }

        }

        protected async Task Autorizar(string idTramite)
        {
            //Console.WriteLine("Autorizar Documento: " + idTramite);
            /*
            [CAGH]TODO En pendientes, colocar el PDF borrador,
            En autorizado, el PDF firmado
            En rechazador, el PDF borrador
            */
            NavigationManager.NavigateTo($"/Autorizar/{PEstadoTramite}/{idTramite}");
        }

        async void MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }

        async void AutorizarSeleccionados()
        {
            AbrirModal("");
        }

        void Filtrar()
        {
            UsarFiltros = true;
            child.ResetIndex();
            ObtenerPendientes(1);
            MostrarFiltros(false);
        }

        async void LimpiarCampos()
        {
            filtrosBandejaEntrada = valoresPorDefectoFiltros();
            UsarFiltros = false;
            child.ResetIndex();
            ObtenerPendientes(1);
            await Js.InvokeVoidAsync("habilitarFilas");
        }

        public void AbrirModal(string modal, bool mostrarModal = true)
        {
            ModalFirmaDisplay = "block;";
            modal = "Show";
            ShowBackdrop = mostrarModal;
            StateHasChanged();
        }

        public void CerrarModal(string modal, bool mostrarModal = true)
        {
            ModalFirmaDisplay = "none";
            modal = "";
            ShowBackdrop = mostrarModal;
            StateHasChanged();
        }

        async void Firmar(string pinReceived)
        {
            ShowBackdrop = false;
            ModalFirmaDisplay = "none";
            MsgActaNoEncontrada = "Espere por favor...";
            MostrarErrorEnModal = false;
            MsjAutorizacionResulOK = "";
            MsjAutorizacionResulError = "";
            // [CAGH]TODO Quitar el loader de la peticion
            // hacer que la peticion responda OK y que el 
            // servidor procese la autorizacion en lote.

            await Js.InvokeVoidAsync("quitarFilas", child.consecMultiples);
            AutorizacionTramitesRequest request = new AutorizacionTramitesRequest
            {
                Pin = pinReceived,
                TramiteId = child.consecMultiples
            };
            await Js.InvokeVoidAsync("quitarLoadScreen");
            var signedFile = await actaNotarialService.FirmaActaNotarialLote(request);
            var autorizados = signedFile.FindAll(x => !x.EsError);
            var conError = signedFile.FindAll(x => x.EsError);
            if (autorizados.Count > 0)
            {
                ShowNotification(child.consecMultiples);
                string idTramitesAutorizados = string.Join(",", autorizados.Select(x => x.TramiteId).ToArray());
                //MsjAutorizacionResulOK = "El(los) trámite(s) " + idTramitesAutorizados + " fueron autorizados";
                //ShowSuccessNotification(idTramitesAutorizados);
            }
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
                            //CerrarModal(ModalFirma, false);
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
            child.consecMultiples = new List<long>();
            CerrarModal(ModalFirma, false);
            LimpiarCampos();
            ObtenerPendientes(indice_actual);
            child.DeSeleccionarTodos();
        }
        async void ShowSuccessNotification(string ids)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Autorización Exitosa", Detail = "Los siguientes tramites fueron autorizados: " + ids, Duration = 4000 };
            notificationService.Notify(message);
        }

        async void ShowNotification(List<long> ids)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Trámites autorizados correctamente", Detail = "Se han aprobado (" + ids.Count() + ") trámite(s) y se iniciará el proceso de firma. Una vez finalice el proceso de firma podrá consultar la actas en la página de autorizados.", Duration = 7000 };
            notificationService.Notify(message);
        }
        async void ShowErrorNotification(string msgError)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = msgError, Duration = 6000 };
            notificationService.Notify(message);
        }
        // void StartCountdown()
        // {
        //     var timer = new Timer(new TimerCallback(_ =>
        //     {
        //         LimpiarCampos();
        //         this.StateHasChanged();
        //     }), null, 30000, 30000);
        // }
        async void ActualizarEstado()
        {
            StateHasChanged();
        }

    }
}
