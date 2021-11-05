using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad
{
    public class EstadoTramiteVirtual: EntidadBase
    {
        public int EstadoTramiteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
