using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using PortalAdministrador.Services;
using System.Net.Http;
using PortalAdministrador.Data;
using ApiGateway.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.IO;
using System.Security.Claims;
using System;
using Radzen;
using Microsoft.JSInterop;
using PortalAdministrador.Services.Biometria.Models.Internal;
using Newtonsoft.Json;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class Resumen : ComponentBase
    {
        

        [Parameter] 
        public string Title { get; set; }

        [Parameter]
        public Compareciente Compareciente { get; set; }

        private string imgCompareciente;        
       


        protected override Task OnParametersSetAsync()
        {
            imgCompareciente = Compareciente.Foto;
            return base.OnParametersSetAsync();
        }



    }
}
