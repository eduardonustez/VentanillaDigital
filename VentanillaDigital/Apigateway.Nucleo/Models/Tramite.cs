using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class Tramite
    {
        public long TramiteId { get; set; }
        public long TipoTramiteId { get; set; }
        public EnumComponente Componente { get; set; }
    }
}
