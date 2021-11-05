using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class AutorizacionTramitesDTO : NewRegisterDTO
    {
        public string Pin { get; set; }
        public List<long> TramiteId { get; set; }
    }

    public class AutorizacionTramitesResponseDTO
    {
        public long TramiteId { get; set; }
        public bool Autorizada { get; set; }
        public bool EsError { get; set; }
        public int CodigoResultado { get; set; }
       
    }
}
