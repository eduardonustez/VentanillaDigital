using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    public class CapturaMovilResponse
    {
        public string Respuesta { get; set; }
        public string Error { get; set; }
        public string DescripcionError { get; set; }
        public int CalidadHuella { get; set; }
    }
}
