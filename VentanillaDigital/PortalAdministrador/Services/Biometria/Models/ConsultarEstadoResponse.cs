using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    public class ConsultarEstadoResponse
    {
        public string Estado { get; set; }
        public string Version { get; set; }
        public PropiedadConsulta[] Propiedades { get; set; }
    }

    public class PropiedadConsulta
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
  
}
