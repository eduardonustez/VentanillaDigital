using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class TipoTramiteReturnDTO
    {
        public long TipoTramiteId { get; set; }
        public long CodigoTramite { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public long ProductoReconoserId { get; set; }
    }
}
