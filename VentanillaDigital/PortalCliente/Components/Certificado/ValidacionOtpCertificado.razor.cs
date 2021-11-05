using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Notario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.Certificado;
using BlazorInputFile;
using System.IO;

namespace PortalCliente.Components.Certificado
{
    public partial class ValidacionOtpCertificado : ComponentBase
    {
        [Parameter]
        public EventCallback<string> otpChanged { get; set; }
        string otp;

        protected async void Validar()
        {
            await otpChanged.InvokeAsync(otp);
        }
      

    }
}