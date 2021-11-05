using ApiGateway.Contratos.Models.Reportes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Notario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.NotarioPages
{
    public partial class Reportes : ComponentBase
    {
        [Inject]
        protected INotarioService NotarioService { get; set; }

        [Inject]
        protected IJSRuntime Js { get; set; }

        private string IdentificadorJS = "VisualizarReporte";

        protected override async Task OnInitializedAsync()
        {
            var tipoReporte = TipoReporte.ReporteOperacionalDiario.ToString();
            var reporte = await NotarioService.ObtenerReportes(tipoReporte);
            await Js.InvokeVoidAsync(IdentificadorJS, reporte);
        }
    }
}
