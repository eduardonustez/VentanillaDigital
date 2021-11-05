using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using PortalCliente.Components.Transversales;
using PortalCliente.Data;
using PortalCliente.Services;
using Radzen;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public partial class RecaudoTramite
    {
        [Parameter] public long TramitePortalVirtualId { get; set; }
        [Parameter] public List<ArchivoTramiteVirtual> Archivos { get; set; }

        [Inject]
        public ITramiteVirtualService tramitesVirtualService { get; set; }
        [Inject]
        public IParametricasService parametricasService { get; set; }
        [Inject]
        private NotificationService notificationService { get; set; }

        public List<RecaudoTramiteModel> Recaudos { get; set; }
        public TipoIdentificacion[] TiposIdentificacion { get; set; }
        public bool IsLoading { get; set; }
        public string MensajeError { get; set; }
        public CrearRecaudoTramiteVirtualModel RecaudoFrm { get; set; } = new CrearRecaudoTramiteVirtualModel { };
        ModalQuestion ModalQuestion { get; set; }
        ModalForm ModalEnviar { get; set; }
        public RecaudoTramiteModel recaudoSeleccionado { get; set; }
        public CultureInfo Culture { get { return CultureInfo.CreateSpecificCulture("es-CO"); } }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            RecaudoFrm = NewInstance();
            TiposIdentificacion = await parametricasService.ObtenerTiposIdentificacion();
            await ConsultarRecaudosTramite();
        }

        async Task ConsultarRecaudosTramite()
        {
            IsLoading = true;
            var res = await tramitesVirtualService.ConsultarRecaudosTramite(TramitePortalVirtualId);
            Recaudos = res?.ToList();
            IsLoading = false;
        }

        private CrearRecaudoTramiteVirtualModel NewInstance()
        {
            var instance = new CrearRecaudoTramiteVirtualModel();
            instance.TramitePortalVirtualId = (int)TramitePortalVirtualId;
            return instance;
        }

        async Task OnSendRecaudo(bool estado)
        {
            if (estado)
            {
                ModalEnviar.Close();
                await ConsultarRecaudosTramite();
            }
        }
        async Task Guardar()
        {
            try
            {
                IsLoading = true;
                MensajeError = string.Empty;
                var res = await tramitesVirtualService.GuardarRecaudo(RecaudoFrm);

                if (res != null & res.Status)
                {
                    ShowSuccessNotification("Recaudo", "¡El recaudo se ha generado correctamente!");
                    RecaudoFrm = NewInstance();
                    await ConsultarRecaudosTramite();
                    StateHasChanged();
                }
            }
            catch (System.Exception ex)
            {
                MensajeError = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }

        void ShowSuccessNotification(string title, string text)
        {
            var message = new NotificationMessage() { Severity = NotificationSeverity.Success, Summary = title, Detail = text, Duration = 4000 };
            notificationService.Notify(message);
        }

        void MostrarModalEnviar(RecaudoTramiteModel recaudo)
        {
            recaudoSeleccionado = recaudo;
            ModalEnviar.Open();
        }

        void MostrarMensajeEliminar(RecaudoTramiteModel recaudo)
        {
            recaudoSeleccionado = recaudo;
            ModalQuestion.Open();
        }

        async Task Eliminar()
        {
            if (recaudoSeleccionado.Estado != "GENERADO")
            {
                MensajeError = "El registro no se puede eliminar";
                return;
            }

            try
            {
                IsLoading = true;
                var res = await tramitesVirtualService.EliminarRecaudo(recaudoSeleccionado.RecaudoTramiteVirtualId);

                if (res)
                {
                    ShowSuccessNotification("Recaudo", "¡El recaudo se ha eliminado correctamente!");
                    await ConsultarRecaudosTramite();
                    StateHasChanged();
                }
            }
            catch (System.Exception ex)
            {
                MensajeError = ex.Message;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
