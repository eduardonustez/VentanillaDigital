using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteRechazadoDTO : NewRegisterDTO
    {
        public string Pin { get; set; }
        public string MotivoRechazo { get; set; }
        public long TramiteId { get; set; }
    }
}
