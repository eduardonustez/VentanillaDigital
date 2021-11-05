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
namespace PortalCliente.Pages.TramitePages
{
    public partial class TramitesEnProcesoPage : ComponentBase
    {

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
        [Inject]
        ITramiteService tramiteService { get; set; }

        DataGrid child;

        TramitePendienteAutorizacionModel pendientesAutorizacion;
        FiltroTramites model = new FiltroTramites();
        object[,] registros;
        string[] columnas = { "Consec.", "Tipo de Trámite", "Comparecientes", " n.º Documento", "Fecha", "Creado Por" };
        int totalRegistros = 0;
        int totalPaginas = 0;
        string TituloBandejaEntrada = "Bandeja De Entrada";
        long notariaId;
        // Logica de modales
        public Guid Guid = Guid.NewGuid();
        public string ModalFirmaDisplay = "none";
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
        public List<string> paginasNoPendientes = (new List<string> { "3", "4" });

        string _value;
        string _username = "";
        List<Filtro> Filtros = new List<Filtro>();

        protected override async Task OnInitializedAsync()
        {
            TituloBandejaEntrada = "Trámites En Proceso";
            model.RegistrosPagina = 20;
            ModalFirmaDisplay = "none";
            var auth = (CustomAuthenticationStateProvider)_authenticationStateProvider;
            AuthenticatedUser authenticatedUser = await auth.GetAuthenticatedUser();
            _username = authenticatedUser.RegisteredUser.UserName;
            Filtros.Add(new Filtro("Tramites.UsuarioCreacion", _username));
            ObtenerPendientes(1);
        }
        async void ObtenerPendientes(int indice)
        {
            mostrarCheckboxes = false;
            indice_actual = indice;
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var authuser = state.User;
            await JsRuntime.InvokeVoidAsync("habilitarFilas");


            if (!long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                notariaId = -1;

            Filtrar();

            model.NumeroPagina = indice;
            model.RegistrosPagina = 20;
            //List<Ordenacion> ordenacion = new List<Ordenacion>();
            //ordenacion.Add(new Ordenacion("Tramites.FechaCreacion", "DESC"));

            pendientesAutorizacion = await tramiteService.ObtenerTramitesEnProceso(model);
            if (pendientesAutorizacion != null)
            {
                registros = pendientesAutorizacion.Pendientes.To2DArray(p => p.Id, p => p.TipoTramite, p => p.Comparecientes,
                p => p.DocCompareciente, p => p.Fecha, p => p.NombreOperador);
                totalRegistros = pendientesAutorizacion.TotalRegistros;
                totalPaginas = pendientesAutorizacion.TotalPaginas;
            }
            StateHasChanged();

        }

        private void Filtrar()
        {
            if (Filtros.Count == 1)
            {
                foreach (Filtro filtro in Filtros)
                {
                    model.NuipOperador = filtro.Valor;
                    model.NotariaId = notariaId;
                }
            }
            else
            {
                model.NuipOperador = null;
                model.NotariaId = notariaId;
            }

        }
        void ContinuarTramite(string idTramite)
        {
            NavigationManager.NavigateTo($"/tramite/{Convert.ToInt64(idTramite)}");
        }
        async Task FiltrosChangedHandler(List<Filtro> filtros)
        {
            Filtros = filtros;
            ObtenerPendientes(indice_actual);
        }

    }
}
