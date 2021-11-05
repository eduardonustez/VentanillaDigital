using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models.Internal
{
    public class ConsultarEstadoMovilResponse
    {
        public bool Captor { get; set; }
        public string Imei { get; set; }
        public string Ip { get; set; }
        public string Version { get; set; }
    }
}
