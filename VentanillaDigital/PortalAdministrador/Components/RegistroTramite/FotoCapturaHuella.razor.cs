using Microsoft.AspNetCore.Components;
using PortalAdministrador.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.RegistroTramite
{
    public partial class FotoCapturaHuella
    {
        [Parameter]
        public Compareciente Compareciente { get; set; }

        [Parameter]
        public EventCallback<Compareciente> ComparecienteChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        [Parameter]
        public long CodigoTipoTramite { get; set; }

        private CapturaHuella CapturaHuella { get; set; }

        public Task Validar()
        {
            return CapturaHuella.Validar();
        }
    }
}
