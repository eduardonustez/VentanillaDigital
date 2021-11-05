using Dominio.Nucleo.Entidad;
using System.Collections.Generic;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class Categoria:EntidadBase
    {
        public long CategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual IEnumerable<TipoTramite> TiposTramites { get; set; }
    }
}
