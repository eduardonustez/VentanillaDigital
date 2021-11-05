using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Components.Transversales;
using PortalCliente.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public partial class TramitesPortalVirtualPage : ComponentBase
    {
        [Parameter]
        public long TramiteId { get; set; }

        string TituloBandejaTramitesVirtuales = "Trámites Virtuales";
        DefinicionFiltro model = new DefinicionFiltro();
        long notariaId;
        bool UsarFiltros = false;
        public bool FiltrosVisibles { get; set; } = true;
        private FilterModel filterModel = new FilterModel
        {
            FechaInicio = DateTime.Now.AddMonths(-1),
            FechaFin = DateTime.Now
        };
        string MsjAutorizacionResulError = string.Empty;
        public List<(string, string)> Columns { get; set; } = new List<(string, string)>
        {
            ("Consec.", "TramitesPortalVirtualId"),
            ("F. Creación", "FechaCreacion"),
            ("T. Documento", "TipoDocumento"),
            ("n.º Documento", "NumeroDocumento"),
            ("CUANDI", "CUANDI"),
            ("Estado Tramite", "EstadoTramite"),
            ("Tipo Tramite", "TipoTramite")
        };
        List<TramiteVirtuales> TramitesVirtual;
        public int TotalPages { get; set; }
        public long TotalRows { get; set; }
        public async void OnChangePage(int indice)
        {
            await ObtenerTramitesVirtuales(indice);
        }
        public GridControl<TramiteVirtuales> Grid { get; set; }
        private Dictionary<int, string> opcEstadosTramite = new Dictionary<int, string>();

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Inject]
        public ITramiteVirtualService tramitesVirtualService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IJSRuntime Js { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Js.InvokeVoidAsync("cambiarThemeAAzul");
            model.RegistrosPagina = 20;
            await ObtenerTramitesVirtuales(1);
            await valoresPorDefectoFiltros();
            await ObtenerEstadoTramitesVirtuales(false);
        }

        private async Task ObtenerEstadoTramitesVirtuales(bool isDeleted)
        {
            var EstadosTramites = await tramitesVirtualService.ObtenerEstadosTramite(isDeleted);
            Console.WriteLine($"total: {EstadosTramites} 👍👍👍");
            if (EstadosTramites != null)
            {
                foreach (var item in EstadosTramites)
                {
                    opcEstadosTramite.Add(item.EstadoTramiteVirtualId, item.Descripcion);
                }
            }
        }
        async Task VolverPortalNotario()
        {
            await Js.InvokeVoidAsync("cambiarThemeARojo");
            NavigationManager.NavigateTo("bandejaEntrada/3");
        }
        async Task MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }

        async void LimpiarCampos()
        {
            filterModel = await valoresPorDefectoFiltros();
            UsarFiltros = false;
            Grid.ResetIndex();
            await Js.InvokeVoidAsync("habilitarFilas");
        }

        async Task<FilterModel> valoresPorDefectoFiltros()
        {
            return new FilterModel
            {
                FechaInicio = DateTime.Now.AddMonths(-1),
                FechaFin = DateTime.Now
            };
        }

        async Task Filtrar()
        {
            UsarFiltros = true;
            Grid.ResetIndex();
            await MostrarFiltros(false);
        }

        private async Task ObtenerTramitesVirtuales(int indice)
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var authuser = state.User;

            if (!long.TryParse(authuser.Claims.FirstOrDefault(c => c.Type == "NotariaId")?.Value, out notariaId))
                notariaId = -1;

            model.IndicePagina = indice;
            model.TotalRegistros = 0;
            List<Ordenacion> ordenacion = new List<Ordenacion>();
            ordenacion.Add(new Ordenacion("TPV.FechaCreacion", "DESC"));
            model.Ordenacion = ordenacion;
            model.Filtros = new List<Filtro>();
            model.Filtros.Add(new Filtro("TPV.NotariaId", notariaId.ToString()));

            if (UsarFiltros)
            {
                UtilizarFiltros();
            }
            var obtenerTramitesVirtuales = await tramitesVirtualService.ObtenerTramitesVirtuales(model);

            if (obtenerTramitesVirtuales != null)
            {
                Console.WriteLine($"🤞🤞🤞 Ingreso ", obtenerTramitesVirtuales.TotalRegistros);
                TotalPages = obtenerTramitesVirtuales.TotalPaginas;
                TotalRows = obtenerTramitesVirtuales.TotalRegistros;
                TramitesVirtual = obtenerTramitesVirtuales.TramitesPortalVirtualReturn.ToList();
            }
            else
            {
                MsjAutorizacionResulError = "Error al obtener la lista de tramites virtuales";
            }
            StateHasChanged();
        }
        private void UtilizarFiltros()
        {
            if (!string.IsNullOrWhiteSpace(filterModel.CUANDI))
            {
                model.Filtros.Add(new Filtro("TPV.CUANDI", filterModel.CUANDI));
            }
            else
            {
                if (filterModel.Estado != 0)
                {
                    model.Filtros.Add(new Filtro("ETV.EstadoTramiteID", filterModel.Estado.ToString()));
                }
                if (filterModel.FechaFin != null && filterModel.FechaInicio != null)
                {
                    model.Filtros.Add(new Filtro("TPV.FechaCreacion", filterModel.FechaInicio.ToString("yyyy-MM-dd"), "between", filterModel.FechaFin.AddDays(1).ToString("yyyy-MM-dd")));
                }
            }
        }

        public async void OnViewClick(TramiteVirtuales item)
        {
            await SeleccionarTramite(item.TramitesPortalVirtualId);
        }

        async Task SeleccionarTramite(int tramiteId)
        {
            //Tramites = null;
            filterModel.NumeroTramite = tramiteId.ToString();
            await ConsultarPorNumeroTramite(filterModel.NumeroTramite);
        }

        private async Task ConsultarPorNumeroTramite(string numeroTramite)
        {
            //ShowHistorial = false;
            if (!long.TryParse(numeroTramite, out long tramiteId)) throw new Exception("Número de trámite virtual incorrecto");

            await ConsultarTramite(tramiteId);
            TramiteId = tramiteId;
            StateHasChanged();
        }

        private async Task ConsultarTramite(long tramiteId)
        {
            NavigationManager.NavigateTo($"/tramitesportalvirtual/autorizar/{tramiteId}");
        }
    }

    public class FilterModel
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Tramite Virtual Incorrecto")]
        public string NumeroTramite { get; set; }

        public string CUANDI { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int Estado { get; set; }
    }
}
