using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services;
using PortalCliente.Services.Biometria;
using PortalCliente.Services.Parametrizacion;
using PortalCliente.Services.RedireccionLogin;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages
{
    public partial class InstaladorAgente:ComponentBase
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected IParametrizacionServicio ParametrizacionServicio { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IRedireccionService RedireccionLogin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await JSRuntime.InvokeVoidAsync("ocultarMenuNav");
        }
        private async Task Refresh()
        {
            try
            {
                await ParametrizacionServicio.RegistrarMaquina();
                await RedireccionLogin.IrAPaginaInicial();
                await JSRuntime.InvokeVoidAsync("mostrarMenuNav");
            }
            catch (ApplicationException ex)
            {
                Console.Error.WriteLine(ex);
            }
        }
        
    }
}
