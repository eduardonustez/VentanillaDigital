using System;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PortalAdministrador.Data;
using System.Diagnostics;

namespace PortalAdministrador.Components.RegistroTramite
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
        public EventCallback OnCancel { get; set; }

        string ComparecienteFirmaRuego { get; set; }

        
        protected override async Task OnParametersSetAsync()
        {
            if (TipoTramite.CodigoTramite==7 || TipoTramite.CodigoTramite == 8)
            {
                switch(ComparecienteActual){
                    case 1: ComparecienteFirmaRuego = "(Rogante)";
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