using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PortalCliente.Data;
using System.Diagnostics;
using PortalCliente.Services;
using ApiGateway.Models.Transaccional;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class TramiteBar : ComponentBase
    {
        [Parameter]
        public TipoTramite TipoTramite { get; set; }

        [Parameter]
        public long TramiteId { get; set; }

        [Parameter]
        public int Comparecientes { get; set; }

        [Parameter]
        public int ComparecienteActual { get; set; }

        [Parameter]
        public EventCallback<string> OnCancel { get; set; }
        private CancelarTramiteModal cancelarTramiteModal { get; set; }
        string ComparecienteFirmaRuego { get; set; }

        async Task CancelarTramite(string MotivoCancelacion)
        {
            if (!string.IsNullOrWhiteSpace(MotivoCancelacion))
            {
                await OnCancel.InvokeAsync(MotivoCancelacion);
            }
        }


        protected override async Task OnParametersSetAsync()
        {
            if (TipoTramite.CodigoTramite == 7 || TipoTramite.CodigoTramite == 8)
            {
                switch (ComparecienteActual)
                {
                    case 1:
                        ComparecienteFirmaRuego = "(Rogante)";
                        break;
                    case 2:
                        ComparecienteFirmaRuego = "(Rogado)";
                        break;
                    default:
                        ComparecienteFirmaRuego = "";
                        break;
                }
            }
            else
            {
                ComparecienteFirmaRuego = "";
            }

        }
    }
}