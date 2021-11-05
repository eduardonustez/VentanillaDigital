using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ExcepcionHuellaCreateDTO : NewRegisterDTO
    {
        public int TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
     
    }

}
