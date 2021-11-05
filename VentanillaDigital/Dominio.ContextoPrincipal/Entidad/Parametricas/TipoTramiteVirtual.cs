using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class TipoTramiteVirtual : EntidadBase
    {
        public int TipoTramiteID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ActoNotarial> ActosNotariales { get; set; } = new HashSet<ActoNotarial>();
    }
}
