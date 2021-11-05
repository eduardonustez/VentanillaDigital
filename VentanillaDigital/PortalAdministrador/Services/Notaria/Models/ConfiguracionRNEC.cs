using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Notaria.Models
{
    public class ConfiguracionRNEC
    {
        public long NotariaId { get; set; }
        public Guid ConvenioRNEC { get; set; }
        public string OficinaRNEC { get; set; }
        public long ClienteRNECId { get; set; }
    }
}
