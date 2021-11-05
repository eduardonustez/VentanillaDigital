using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class ArchivoActa
    {
        public long TramiteId { get; set; }
        public long EstadoTramiteId { get; set; }
        public long ActaNotarialId { get; set; }
        public string Contenido { get; set; }
        
    }

}
