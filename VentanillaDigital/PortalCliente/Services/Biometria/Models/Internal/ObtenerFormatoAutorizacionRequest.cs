using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models.Internal
{
    internal class ObtenerFormatoAutorizacionRequest
    {
        public string Cedula { get; set; }
        public Guid? IdConvenio { get; set; }
        public long Ticks { get; set; }
    }
}
