using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    internal class CapturarHuellaResponse
    {
        public int Calidad { get; set; }
        public string Detalle { get; set; }
        public string Respuesta { get; set; }
    }
}
