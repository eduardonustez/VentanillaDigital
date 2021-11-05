using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class AutorizacionTramitesResponse
    {
        public long TramiteId { get; set; }
        public bool EsError { get; set; }
        public int CodigoResultado { get; set; }
        
    }
}
