using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Parametricas
{
    public class ConfiguracionRNECDTO
    {
        public long NotariaId { get; set; }
        public string OficinaRNEC { get; set; }
        public Guid ConvenioRNEC { get; set; }
        public long ClienteRNECId { get; set; }
    }
}
