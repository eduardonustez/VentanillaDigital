using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models
{
    public class ConfiguracionRNEC
    {
        public long NotariaId { get; set; }
        public Guid ConvenioRNEC { get; set; }
        public string OficinaRNEC { get; set; }
        public long ClienteRNECId { get; set; }
    }
}
