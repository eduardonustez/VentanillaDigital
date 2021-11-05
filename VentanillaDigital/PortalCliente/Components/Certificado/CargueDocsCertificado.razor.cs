using Microsoft.AspNetCore.Components;
using PortalCliente.Services.Notario;
using System;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.Certificado;
using Microsoft.JSInterop;

namespace PortalCliente.Components.Certificado
{
    public partial class CargueDocsCertificado : ComponentBase
    {
        [Inject]
        private ICertificadoService certificadoService { get; set; }
        [Parameter]
        public SolicitudCertificadoDto solicitud { get; set; }
        [Parameter]
        public EventCallback<SolicitudCertificadoDto> solicitudChanged { get; set; }
        [Inject]
        private IJSRuntime js { get; set; }
        string accept;

        protected override async Task OnInitializedAsync()
        {
            //await solicitudChanged.InvokeAsync(solicitud);
            solicitud.Autorizacion = Convert.FromBase64String(await certificadoService.ObtenerAutorizacion(solicitud));
            solicitud.Contrato = Convert.FromBase64String(await certificadoService.ObtenerContrato(solicitud));
        }

        protected async void Download_Authorization()
        {
            if (solicitud.Autorizacion == null)
            {
                var templateText = await certificadoService.ObtenerAutorizacion(solicitud);
                solicitud.Autorizacion = Convert.FromBase64String(templateText);
            }
            await saveAsFile("autorizacion.pdf", Convert.ToBase64String(solicitud.Autorizacion));
        }
        protected async void Download_Contract()
        {
            if (solicitud.Contrato == null)
            {
                var templateText = await certificadoService.ObtenerContrato(solicitud);
                solicitud.Contrato = Convert.FromBase64String(templateText);
            }
            await saveAsFile("contrato.pdf", Convert.ToBase64String(solicitud.Contrato));
        }
        async Task saveAsFile(string filename, string base64String)
        {
            await js.InvokeAsync<object>("saveAsFile", filename, base64String);
        }

        protected async Task onLoadCedula(byte[] byteArray){
            solicitud.Cedula=byteArray;
            await solicitudChanged.InvokeAsync(solicitud);
        }
        protected async Task onLoadCComercio(byte[] byteArray){
            solicitud.CComercio=byteArray;
            await solicitudChanged.InvokeAsync(solicitud);
        }
        protected async Task onLoadRut(byte[] byteArray)
        {
            solicitud.Rut = byteArray;
            await solicitudChanged.InvokeAsync(solicitud);
        }
        protected async Task onLoadCedulaPrincipal(byte[] byteArray)
        {
            solicitud.CedulaPrincipal = byteArray;
            await solicitudChanged.InvokeAsync(solicitud);
        }
    }
}