using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalAdministrador.Services;
using PortalAdministrador.Services.Biometria;
using PortalAdministrador.Services.Parametrizacion;
using PortalAdministrador.Services.RedireccionLogin;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages
{
    public partial class InstaladorAgente:ComponentBase
    {
        [Inject]
        protected IParametrizacionServicio ParametrizacionServicio { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRedireccionService RedireccionLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
           
        }
        private async Task Refresh()
        {
            try
            {
                await ParametrizacionServicio.RegistrarMaquina();
                await RedireccionLogin.IrAPaginaInicial();
            }
            catch (ApplicationException ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
        
    }
}
