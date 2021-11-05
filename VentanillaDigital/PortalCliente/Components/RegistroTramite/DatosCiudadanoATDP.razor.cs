using Microsoft.AspNetCore.Components;
using PortalCliente.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Components.RegistroTramite
{
    public partial class DatosCiudadanoATDP
    {
        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        private bool _datosReady = false;
        private bool _atdpReady = false;
        [Parameter]
        public EventCallback<bool> EmailCelular { get; set; }
    }
}