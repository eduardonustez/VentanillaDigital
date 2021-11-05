using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    internal class ValidarIdentidadRequest
    {
        public string Cedula { get; set; }
        public string Asesor { get; set; }
        public string Oficina { get; set; }
        public long Producto { get; set; }
        public Guid? IdConvenio { get; set; }
        public long Ticks { get; set; }
        public string Grafo { get; set; }
    }
}
