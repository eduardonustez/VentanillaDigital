using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Pages.TramitePages
{
    public partial class Dashboard
    {
        public TramiteReturn TramiteActual { get; set; }
        private bool NuevoTramite { get; set; }
    }
}
