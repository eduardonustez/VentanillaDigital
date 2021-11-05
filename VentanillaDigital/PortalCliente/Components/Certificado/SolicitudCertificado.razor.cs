using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Notario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.Certificado;
using System.Text.Json;

namespace PortalCliente.Components.Certificado
{
    public partial class SolicitudCertificado : ComponentBase
    {

        [Parameter]
        public SolicitudCertificadoDto solicitud
        {
            get => _solicitud;
            set
            {
                if (JsonSerializer.Serialize(value) ==JsonSerializer.Serialize(_solicitud)) return;
                _solicitud =value;
                solicitudChanged.InvokeAsync(solicitud);
            }
        }
        [Parameter]
        public EventCallback<SolicitudCertificadoDto> solicitudChanged { get; set; }
        private SolicitudCertificadoDto _solicitud { get; set; }
        protected override void OnInitialized()
        {
            if (solicitud == null)
                solicitud = new SolicitudCertificadoDto();
            base.OnInitialized();
        }
        protected void onInputDir(string texto){
            solicitud.Direccion = texto;
            solicitudChanged.InvokeAsync(solicitud);
        }
        protected void onInputCel(ChangeEventArgs args){
            string texto = (string)args.Value;
            solicitud.Celular = texto;
            solicitudChanged.InvokeAsync(solicitud);
        }
    }
}