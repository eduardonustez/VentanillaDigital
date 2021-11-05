using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models
{
    public class ValidacionRequest
    {
        public string Ciudadano { get; set; }
        public string Oficina { get; set; }
        public string Asesor { get; set; }
        public long ProductoId { get; set; }
        public Guid? ConvenioId { get; set; }
        public string Grafo { get; set; }
        public long ClienteId { get; set; }
    }
}
