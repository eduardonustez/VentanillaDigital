using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.TramitePages
{
    public partial class Dashboard
    {
        public TramiteReturn TramiteActual { get; set; }
        private bool NuevoTramite { get; set; }

        public async Task OnStepChanged(int index)
        {
            TramiteActual = new TramiteReturn()
            {
                CantidadComparecientes = 3,
                ComparecienteActual = 1,
                TramiteId = 1234,
                NombreTramite = "Diligencia de reconocimiento de firma y contenido de documento privado"
            };
        }
    }
}
