using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    public class CapturaMovilRequest
    {
        public string SerialDispositivo { get; set; }
        public int NumeroDedo { get; set; }
        public int TipoDedo { get; set; }
    }
}
