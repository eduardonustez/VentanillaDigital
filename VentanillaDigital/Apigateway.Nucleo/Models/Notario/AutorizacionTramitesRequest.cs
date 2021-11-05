using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class AutorizacionTramitesRequest
    {
        public string Pin { get; set; }
        public List<long> TramiteId { get; set; }
    }
}
