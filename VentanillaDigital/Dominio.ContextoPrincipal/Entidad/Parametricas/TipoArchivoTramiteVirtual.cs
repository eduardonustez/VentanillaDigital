using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class TipoArchivoTramiteVirtual : EntidadBase
    {
        public short TipoArchivoTramiteVirtualId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
