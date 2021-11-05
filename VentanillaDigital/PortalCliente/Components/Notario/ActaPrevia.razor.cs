
using Microsoft.AspNetCore.Components;
using PortalCliente.Services;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.ActaNotarial;
using System.Collections.Generic;
using System;
using System.Text.Json;
using PortalCliente.Data.DatosTramite;

namespace PortalCliente.Components.Notario
{
    public partial class ActaPrevia : ComponentBase
    {
        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }

        [Parameter]
        public long IdTramite { get; set; }

        ActaResumen actaResumen;
        string motivoRechazo;
        string rechazadoPor;
        int contadorDatosAdicionales = 0;

        T ConvertirJson<T>(string json)
        {
            var entidad = JsonSerializer.Deserialize<T>(json);
            return entidad;
        }

        public async Task RefrescarComponente()
        {
            await ObtenerDatosResumen();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await ObtenerDatosResumen();
        }

        private async Task ObtenerDatosResumen()
        {
            actaResumen = await actaNotarialService.ObtenerResumen(IdTramite);
            if (!string.IsNullOrWhiteSpace(actaResumen.DatosAdicionales))
            {
                JsonElement datosAdicionalesTramite = JsonSerializer.Deserialize<JsonElement>(actaResumen.DatosAdicionales);
                foreach (var datoAdicional in datosAdicionalesTramite.EnumerateObject())
                {
                    if (datoAdicional.Name == "MotivoRechazo")
                    {
                        motivoRechazo = datoAdicional.Value.ToString();
                    }
                    else if (datoAdicional.Name == "RechazadoPor")
                    {
                        rechazadoPor = datoAdicional.Value.ToString();
                    }
                    else
                    {
                        contadorDatosAdicionales++;
                    }
                }
            }
        }
    }
}