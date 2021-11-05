
using Microsoft.AspNetCore.Components;
using PortalAdministrador.Services;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.ActaNotarial;
using System.Collections.Generic;
using System;
using System.Text.Json;
using PortalAdministrador.Data.DatosTramite;

namespace PortalAdministrador.Components.Notario
{
    public partial class ActaPrevia : ComponentBase
    {
        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }

        [Parameter]
        public long IdTramite { get; set; }

        ActaResumen actaResumen;

        T ConvertirJson<T>(string json)
        {
            var entidad = JsonSerializer.Deserialize<T>(json);
            return entidad;
        }

        protected override async Task OnInitializedAsync()
        {
            actaResumen = await actaNotarialService.ObtenerResumen(IdTramite);
        }

    }

    class DatosRechazo {
        public string MotivoRechazo { get; set; }
    }
}