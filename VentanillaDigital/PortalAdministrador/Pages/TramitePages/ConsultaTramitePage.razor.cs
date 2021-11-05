using HashidsNet;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalAdministrador.Components.Shared;
using PortalAdministrador.Services;
using Radzen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace PortalAdministrador.Pages.TramitePages
{
    public partial class ConsultaTramitePage
    {
        #region Parámetros e Injects
        [Parameter]
        public long TramiteId { get; set; }
        [Inject]
        protected ITramiteService _tramiteService { get; set; }
        [Inject]
        protected IActaNotarialService ActaNotarialService { get; set; }
        [Inject]
        NotificationService NotificationService { get; set; }
        [Inject]
        IJSRuntime Js { get; set; }
        #endregion

        #region Propiedades y Variables
        IEnumerable<Data.DatosComparecienteModel> Comparecientes;
        bool ShowHistorial { get; set; } = false;
        Data.TramiteModel Tramite;
        List<ApiGateway.Contratos.Models.TramiteInfoBasica> Tramites;
        public int TotalPages { get; set; }
        public long TotalRows { get; set; }
        public bool consultando { get; set; }
        public string MensajeError { get; set; }
        private FilterModel filterModel = new FilterModel { SelectedFilter = 1, FechaInicio = DateTime.Now.AddDays(-29), FechaFin = DateTime.Now };
        public GridControl<ApiGateway.Contratos.Models.TramiteInfoBasica> Grid { get; set; }
        public List<(string, string)> Columns { get; set; } = new List<(string, string)>
        {
            ("Trámite", "TramiteId"),
            ("Notaría", "NotariaNombre"),
            ("Tipo de trámite", "TipoTramite"),
            ("Comparecientes", "CantidadComparecientes")
        };
        bool preventDefault = false;
        private EditContext editContext;
        ValidationMessageStore msgStore;
        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            //await ConsultarTramite(TramiteId);
            //await ConsultarComparecientes(TramiteId);
            editContext = new EditContext(filterModel);
            msgStore = new ValidationMessageStore(editContext);
        }
        #endregion

        #region Privates
        private void ChangeTipoConsulta(ChangeEventArgs e)
        {
            filterModel.SelectedFilter = int.Parse(e.Value.ToString());
            ShowHistorial = false;
            Tramites = null;
            MensajeError = string.Empty;
            Tramite = null;
            Comparecientes = null;
            msgStore.Clear();
            this.StateHasChanged();
        }

        private async Task InputNumberKeyboardEventHandler(KeyboardEventArgs args)
        {
            byte codAscii = Encoding.ASCII.GetBytes(args.Key.ToString())[0];

            if ((codAscii >= 48 && codAscii <= 57) ||
                 codAscii == 66 || codAscii == 68 ||
                 (args.Code == "Enter" || args.Code == "NumpadEnter") ||
                 codAscii == 65 || codAscii == 84)
            {

                this.preventDefault = false;
            }
            else
            {
                this.preventDefault = true;

            }
        }

        public async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await ConsultarTramitePorFiltro();
            }
        }

        async Task ConsultarTramitePorFiltro()
        {
            try
            {
                ShowHistorial = false;
                Tramites = null;
                MensajeError = string.Empty;
                Tramite = null;
                Comparecientes = null;

                Console.WriteLine(filterModel.NumeroTramite);

                if (filterModel.SelectedFilter == 1)
                {
                    await ConsultarPorNumeroTramite(filterModel.NumeroTramite);
                }
                else if (filterModel.SelectedFilter == 2)
                {
                    await ConsultarTramitesPorNumeroIdentificacion(1, filterModel.NumeroIdentificacion, filterModel.FechaInicio, filterModel.FechaFin);
                }
                else if (filterModel.SelectedFilter == 3)
                {
                    await ConsultarTramitesPorNUT(filterModel.NUT);
                }
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
            }
        }

        public async void OnChangePage(int indice)
        {
            await ConsultarTramitesPorNumeroIdentificacion(indice, filterModel.NumeroIdentificacion, filterModel.FechaInicio, filterModel.FechaFin);
        }

        public async void OnViewClick(ApiGateway.Contratos.Models.TramiteInfoBasica item)
        {
            await SeleccionarTramite(item.TramiteId);
        }

        private async Task ConsultarTramitesPorNUT(string nUT)
        {
            if (string.IsNullOrEmpty(nUT))
            {
                MensajeError = "Digite el NUT";
                return;
            }

            var hashids = new Hashids("NUTNOTARIA", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
            var tramiteId = hashids.DecodeLong(nUT).FirstOrDefault();
            await ConsultarPorNumeroTramite(tramiteId.ToString());
        }

        async Task SeleccionarTramite(long tramiteId)
        {
            Tramites = null;
            filterModel.NumeroTramite = tramiteId.ToString();
            await ConsultarPorNumeroTramite(filterModel.NumeroTramite);
        }

        private async Task ConsultarPorNumeroTramite(string numeroTramite)
        {
            ShowHistorial = false;
            if (!long.TryParse(numeroTramite, out long tramiteId)) throw new System.Exception("Número de trámite incorrecto");

            await ConsultarTramite(tramiteId);
            await ConsultarComparecientes(tramiteId);
            TramiteId = tramiteId;
            StateHasChanged();
        }

        private async Task ConsultarTramitesPorNumeroIdentificacion(int indice, string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin)
        {
            if (string.IsNullOrEmpty(numeroIdentificacion))
            {
                MensajeError = "Digite todos los campos";
                return;
            }

            if (fechaInicio.AddDays(30) < fechaFin)
            {
                MensajeError = "Rango máximo permitido para la consulta: 30 días";
                return;
            }

            consultando = true;
            var model = new DefinicionFiltro();
            model.IndicePagina = indice;
            model.TotalRegistros = 0;
            model.RegistrosPagina = 10;
            model.Filtros = new List<Filtro>();

            model.Filtros.Add(new Filtro("NumeroIdentificacion", numeroIdentificacion));
            model.Filtros.Add(new Filtro("FechaInicio", fechaInicio.ToString("yyyy-MM-dd")));
            model.Filtros.Add(new Filtro("FechaFin", fechaFin.ToString("yyyy-MM-dd")));

            var res = await _tramiteService.ConsultarTramitesPorNumeroIdentificacionPaginado(model);

            if (res != null)
            {
                if (res.TotalRows > 1)
                {
                    Tramites = res.Data.ToList();
                    TotalRows = res.TotalRows;
                    TotalPages = res.Pages;
                }
                else if (res.TotalRows == 1)
                {
                    filterModel.NumeroTramite = res.Data.FirstOrDefault().TramiteId.ToString();
                    await ConsultarPorNumeroTramite(filterModel.NumeroTramite);
                }
                else
                {
                    MensajeError = "No se encontraron datos para el filtro seleccionado";
                }
            }

            consultando = false;

            StateHasChanged();
        }

        private async Task ConsultarTramite(long tramiteId)
        {
            Tramite = await _tramiteService.ConsultarTramitePorId(tramiteId);
        }

        private async Task ConsultarComparecientes(long tramiteId)
        {
            Comparecientes = await _tramiteService.ConsultarComparecientesPorTramiteId(tramiteId);
        }

        async Task DescargarActa()
        {
            var res = await ActaNotarialService.ObtenerActaNotarial(TramiteId);

            var currentMillis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;

            var fileName = $"Tramite_{TramiteId}_{DateTime.Now.ToString("yyyy-MM-dd")}_{currentMillis}.pdf";

            if (res != null) await Js.InvokeVoidAsync("saveFile", res.Archivo, "application/pdf", fileName);
        }

        void ShowSuccessNotification()
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = "Compareciente Agregado", Detail = "Compareciente agregado satisfactoriamente!!", Duration = 4000 };
            NotificationService.Notify(message);
        }
        void ShowErrorNotification()
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Ocurrió un error intentado agregar el compareciente, por favor contacte al administrador!!", Duration = 4000 };
            NotificationService.Notify(message);
        }
        #endregion
    }

    public class FilterModel
    {
        public int SelectedFilter { get; set; }
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Número de identificación incorrecto")]
        public string NumeroIdentificacion { get; set; }
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Número de trámite incorrecto")]
        public string NumeroTramite { get; set; }
        [StringLength(20, MinimumLength = 4, ErrorMessage = "NUT incorrecto")]
        public string NUT { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
