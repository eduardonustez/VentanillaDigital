using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Ubicacion:EntidadBase
    {
        public long UbicacionId { get; set; }
        public string Nombre { get; set; }
        public long? UbicacionPadreId { get; set; }

        public virtual Ubicacion? UbicacionPadre { get; set; }
        public virtual IEnumerable<Ubicacion> UbicacionesHijo { get; set; }
    }
}
