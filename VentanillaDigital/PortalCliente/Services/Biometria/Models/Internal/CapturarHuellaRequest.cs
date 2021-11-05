using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models.Internal
{
    internal class CapturarHuellaRequest
    {
        public string Cedula { get; set; }
        public short Dedo { get; set; }
        public short Captura { get; set; }
        public long Ticks { get; set; }
    }
}
