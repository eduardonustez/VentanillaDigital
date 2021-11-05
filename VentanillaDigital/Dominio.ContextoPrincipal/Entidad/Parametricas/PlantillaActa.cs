using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class PlantillaActa:EntidadBase
    {
        public long PlantillaActaId { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
        public virtual IEnumerable<TipoTramite> TiposTramites { get; set; }
 
    }
}
