using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using PortalCliente.Services;
using System.Net.Http;
using PortalCliente.Data;
using ApiGateway.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.IO;
using System.Security.Claims;
using System;
using Radzen;
using Microsoft.JSInterop;
using PortalCliente.Services.Biometria.Models.Internal;
using Newtonsoft.Json;
using PortalCliente.Services.DescriptorCliente;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class Resumen : ComponentBase
    {
        

        [Parameter] 
        public string Title { get; set; }

        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Inject]
        protected IDescriptorCliente DescriptorCliente { get; set; }

        private string imgCompareciente;        

        private bool _movil = false;

        protected override Task OnParametersSetAsync()
        {
            imgCompareciente = Compareciente.Foto;
            return base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            _movil = await DescriptorCliente.EsMovil;
            await base.OnInitializedAsync();
        }
    }
}
