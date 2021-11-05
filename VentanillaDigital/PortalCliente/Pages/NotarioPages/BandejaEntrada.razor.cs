using ApiGateway.Models.Transaccional;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Data;
using PortalCliente.Components.Transversales;
using PortalCliente.Services;
using ApiGateway.Contratos.Models.Notario;
using Radzen;
using System.Threading;
using ApiGateway.Contratos.Enums;
using Newtonsoft.Json;
using PortalCliente.Data.Account;
using Microsoft.AspNetCore.Http;
using Infraestructura.Transversal.Encriptacion;
using PortalCliente.Services.Notario;
using Blazored.SessionStorage;
using System.Diagnostics;

namespace PortalCliente.Pages.NotarioPages
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
        [Inject]
        public INotarioService notarioService { get; set; }
        [Inject]
        ILocalStorageService _localStorageService { get; set; }
        [Inject]
        private ISessionStorageService _sessionStorageService { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        DataGrid child;
        FiltroTramites model = new FiltroTramites();
        TramitePendienteAutorizacionModel pendientesAutorizacion;
        FiltrosBandejaEntrada filtrosBandejaEntrada;
        //DefinicionFiltro model = new DefinicionFiltro();
        object[,] registros;
        string[] columnas = { "Consec.", "Tipo de Trámite", "Comparecientes", " n.º Documento", "Fecha", "Estado" };
        int totalRegistros = 0;
        int totalPaginas = 0;
        string TituloBandejaEntrada = "Bandeja De Entrada";
        long notariaId;
        // Logica de modales
        public Guid Guid = Guid.NewGuid();
        public string ModalDisplay = "none";
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
        string clave;
        bool pinAsignadoNotario;
        bool showSpinner;
        TimeSpan ti = new TimeSpan(00, 00, 0);
        TimeSpan tf = new TimeSpan(23, 59, 0);

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
            }
        }

        FiltrosBandejaEntrada valoresPorDefectoFiltros()
        {
            return new FiltrosBandejaEntrada
            {
                FechaInicio = DateTime.Now.AddDays(-7),
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
        }

        protected override async Task OnParametersSetAsync()
        {
            await ObtenerEstadoPinFirma();
            await ObtenerPendientes(1);
            await base.OnParametersSetAsync();
        }

        async Task ObtenerEstadoPinFirma()
        {
            var usuarioAutenticado = await ((CustomAuthenticationStateProvider)_authenticationStateProvider)?.GetAuthenticatedUser();
            if (usuarioAutenticado.Rol == "Administrador" || usuarioAutenticado.Rol == "Notario Encargado")
            {
                var estadoPinFirma = await notarioService.ObtenerEstadoPinFirma(usuarioAutenticado.RegisteredUser.Email);
                pinAsignadoNotario = estadoPinFirma == null ? false : estadoPinFirma.PinAsignado && estadoPinFirma.FirmaRegistrada;
                if (!pinAsignadoNotario)
                {
                    mostrarCheckboxes = false;
                }
            }
        }

        Task Actualizar()
        {
            return ObtenerPendientes(indice_actual);
        }

        async Task ObtenerPendientes(int indice)
        {
            mostrarCheckboxes = false;
            indice_actual = indice;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var authuser = state.User;
            await Js.InvokeVoidAsync("habilitarFilas");

            if (!long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                notariaId = -1;

            model.NotariaId = notariaId;
            model.NumeroPagina = indice;
            model.RegistrosPagina = 20;


            //model.IndicePagina = indice;
            //model.TotalRegistros = 0;
            //List<Ordenacion> ordenacion = new List<Ordenacion>();
            //ordenacion.Add(new Ordenacion("Tramites.FechaCreacion", "DESC"));
            //model.Ordenacion = ordenacion;
            //model.Filtros = new List<Filtro>();
            //model.Filtros.Add(new Filtro("Tramites.NotariaId", notariaId.ToString()));
            switch (long.Parse(this.PEstadoTramite))
            {
                case 3:
                //model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Autorizado"));
                //model.Filtros.Add(new Filtro("DocumentosPendienteAutorizar.DocumentoPendienteAutorizarId", "", "NONULO", "", "OR"));
                //break;
                case 4:
                    //model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Rechazado"));
                    break;
                default:
                    //model.Filtros.Add(new Filtro("EstadosTramites.Nombre", "Pendiente de Autorización"));
                    //model.Filtros.Add(new Filtro("DocumentosPendienteAutorizar.DocumentoPendienteAutorizarId", "", "IS NULL"));
                    mostrarCheckboxes = true;
                    break;
            }
            UtilizarFiltros();
            ObtenerTramites();



        }

        protected async Task ObtenerTramites()
        {
            var cancellationToken = new CancellationTokenSource();
            var token = cancellationToken.Token;
            showSpinner = true;
            StateHasChanged();

            var delay = Task.Delay(5000, token)
                .ContinueWith((x) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        showSpinner = false;
                        StateHasChanged();
                        cancellationToken.Cancel();
                    }
                }, token);

            //var send = tramiteService.ObtenerPendientesAutorizacion(model, token);

            switch (long.Parse(this.PEstadoTramite))
            {
                case 3:
                    var send = tramiteService.ObtenerTramitesAutorizados(model, token);
                    var finish = send.ContinueWith(async (x) =>
                    {
                        pendientesAutorizacion = await x;
                        cancellationToken.Cancel();
                    });
                    try
                    {
                        await Task.WhenAll(delay, finish);
                    }
                    catch { }
                    finally
                    {
                        showSpinner = false;
                        StateHasChanged();
                    }
                    break;
                case 4:
                    var sendRec = tramiteService.ObtenerTramitesRechazados(model, token);
                    finish = sendRec.ContinueWith(async (x) =>
                    {
                        pendientesAutorizacion = await x;
                        cancellationToken.Cancel();
                    });
                    try
                    {
                        await Task.WhenAll(delay, finish);
                    }
                    catch { }
                    finally
                    {
                        showSpinner = false;
                        StateHasChanged();
                    }
                    break;
                default:
                    send = tramiteService.ObtenerTramitesPendientes(model, token);
                    finish = send.ContinueWith(async (x) =>
                    {
                        pendientesAutorizacion = await x;
                        cancellationToken.Cancel();
                    });
                    try
                    {
                        await Task.WhenAll(delay, finish);
                    }
                    catch { }
                    finally
                    {
                        showSpinner = false;
                        StateHasChanged();
                    }
                    break;
            }

            if (pendientesAutorizacion != null)
            {
                registros = pendientesAutorizacion.Pendientes.To2DArray(p => p.Id, p => p.TipoTramite, p => p.Comparecientes,
                p => p.DocCompareciente, p => p.Fecha, p => p.Estado);
                totalRegistros = pendientesAutorizacion.TotalRegistros;
                totalPaginas = pendientesAutorizacion.TotalPaginas;
            }
            StateHasChanged();
        }

        private void UtilizarFiltros()
        {

            if (filtrosBandejaEntrada.FechaFin != null && filtrosBandejaEntrada.FechaInicio != null)
            {
                model.fechaInicial = filtrosBandejaEntrada.FechaInicio;
                model.fechaFinal = filtrosBandejaEntrada.FechaFin;
                model.fechaInicial = model.fechaInicial.Value.Date + ti;
                model.fechaFinal = model.fechaFinal.Value.Date + tf;
            }
            else
            {
                model.fechaFinal = DateTime.Now;
                model.fechaInicial = DateTime.Now.AddDays(-7);
                model.fechaInicial = model.fechaInicial.Value.Date + ti;
                model.fechaFinal = model.fechaFinal.Value.Date + tf;

            }
            if (filtrosBandejaEntrada.NuipComparenciente != 0)
            {
                model.NuipCompareciente = filtrosBandejaEntrada.NuipComparenciente.ToString();

                //model.Filtros.Add(new Filtro("Personas.NumeroDocumento", filtrosBandejaEntrada.NuipComparenciente.ToString()));
            }
            else
            {
                model.NuipCompareciente = null;
            }
            if (filtrosBandejaEntrada.IdTramite != 0)
            {
                model.TramiteId = filtrosBandejaEntrada.IdTramite;
            }
            else
            {
                model.TramiteId = 0;
            }
            if (filtrosBandejaEntrada.NuipOperador != 0)
            {
                model.NuipOperador = filtrosBandejaEntrada.NuipOperador.ToString();
                //model.Filtros.Add(new Filtro("OPERADORES.NumeroDocumento", filtrosBandejaEntrada.NuipOperador.ToString()));
            }
            else
            {
                model.NuipOperador = null;
            }
            //if (filtrosBandejaEntrada.IdTramite != 0)
            //{
            //    model.Filtros.Add(new Filtro("Tramites.TramiteId", filtrosBandejaEntrada.IdTramite.ToString()));
            //}
            //else
            //{
            //    if (filtrosBandejaEntrada.FechaFin != null && filtrosBandejaEntrada.FechaInicio != null)
            //    {
            //        model.Filtros.Add(new Filtro("Tramites.FechaCreacion", filtrosBandejaEntrada.FechaInicio.ToString("yyyy-MM-dd"), "between", filtrosBandejaEntrada.FechaFin.AddDays(1).ToString("yyyy-MM-dd")));
            //    }
            //    if (filtrosBandejaEntrada.NuipComparenciente != 0)
            //    {
            //        model.Filtros.Add(new Filtro("Personas.NumeroDocumento", filtrosBandejaEntrada.NuipComparenciente.ToString()));
            //    }
            //    if (filtrosBandejaEntrada.NuipOperador != 0)
            //    {
            //        model.Filtros.Add(new Filtro("OPERADORES.NumeroDocumento", filtrosBandejaEntrada.NuipOperador.ToString()));
            //    }

            //}

        }

        public void IrAConfigurarPin()
        {
            NavigationManager.NavigateTo($"/Autorizaciones");
        }

        protected async Task Autorizar(string idTramite)
        {
            NavigationManager.NavigateTo($"/Autorizar/{PEstadoTramite}/{idTramite}");
        }

        async void MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }

        async Task AutorizarSeleccionados()
        {
            clave = await _sessionStorageService.GetItemAsync<string>("__ucnc_p");

            if (string.IsNullOrWhiteSpace(clave) || !(await notarioService.ValidarSolicitudPin(clave).ConfigureAwait(true)))
            {
                await ToggleModal();
            }
            else
                Firmar(clave);
        }

        void Filtrar()
        {
            child.ResetIndex();
            MostrarFiltros(false);
        }

        Task LimpiarYRefrescar()
        {
            LimpiarCampos();

            child.ResetIndex();
            return Js.InvokeVoidAsync("habilitarFilas").AsTask();
        }

        void LimpiarCampos()
        {
            filtrosBandejaEntrada = valoresPorDefectoFiltros();
        }

        public async Task ToggleModal()
        {
            await JsRuntime.InvokeVoidAsync("cerrarModal", "modalForPinFirma");
        }

        async void Firmar(string pinReceived)
        {
            ShowBackdrop = false;
            MsgActaNoEncontrada = "Espere por favor...";
            MostrarErrorEnModal = false;
            MsjAutorizacionResulOK = "";
            MsjAutorizacionResulError = "";

            AutorizacionTramitesRequest request = new AutorizacionTramitesRequest
            {
                Pin = pinReceived,
                TramiteId = child.consecMultiples
            };
            var signedFile = await actaNotarialService.FirmaActaNotarialLote(request);
            var autorizados = signedFile.FindAll(x => !x.EsError);
            var conError = signedFile.FindAll(x => x.EsError && x.CodigoResultado != 6);
            var estabanAutorizados = signedFile.FindAll(x => x.CodigoResultado == 6);
            if (autorizados.Count > 0)
            {
                ShowNotification(child.consecMultiples);
                if (pinReceived.Length == 4)
                    await ToggleModal();
                child.consecMultiples = new List<long>();
                LimpiarCampos();
                ObtenerPendientes(indice_actual);
                child.DeSeleccionarTodos();
                if (pinReceived.Length == 4)
                    await _sessionStorageService.SetItemAsync("__ucnc_p", CifradoSHA512.Cifrar($"{pinReceived}{DateTime.Now.ToString("yyyyMMdd")}")).ConfigureAwait(true);
                return;
            }
            if (conError.Count > 0)
            {
                //await Js.InvokeVoidAsync("cerrarModalPin");
                string msgError = "Ocurrió un error intentando firmar los documentos";

                switch ((EnumResultadoFirma)signedFile[0].CodigoResultado)
                {
                    case EnumResultadoFirma.FirmaNoConfigurada:
                    case EnumResultadoFirma.PinNoAsignado:
                        msgError = "Por favor configure su firma y su pin en el módulo de autorizaciones";
                        NavigationManager.NavigateTo($"/Autorizaciones");
                        break;
                    case EnumResultadoFirma.PinNoValido:
                        msgError = "El pin ingresado no es válido";
                        await ToggleModal();
                        break;
                    case EnumResultadoFirma.ErrorServicioEstampa:
                        msgError = "Ocurrió un error con el servicio de estampa del documento";
                        break;
                }
                ShowErrorNotification(msgError);
            }
            if (estabanAutorizados.Count > 0)
            {
                ShowInfoNotification(string.Join(", ", estabanAutorizados.Select(x => x.TramiteId).ToArray()));
                if (pinReceived.Length == 4)
                    await ToggleModal();
                child.consecMultiples = new List<long>();
                LimpiarCampos();
                ObtenerPendientes(indice_actual);
                child.DeSeleccionarTodos();
                if (pinReceived.Length == 4)
                    await _sessionStorageService.SetItemAsync("__ucnc_p", CifradoSHA512.Cifrar($"{pinReceived}{DateTime.Now.ToString("yyyyMMdd")}")).ConfigureAwait(true);
                return;
            }
        }

        async void ShowInfoNotification(string ids)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Info, Summary = "Trámites autorizados", Detail = "Los siguientes tramites se encontraban autorizados: " + ids, Duration = 5000 };
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

    }
}
