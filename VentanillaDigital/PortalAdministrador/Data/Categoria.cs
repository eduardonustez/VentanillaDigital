using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data
{
    public class Categoria
    {
        public long CategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public IEnumerable<TipoTramite> TiposTramites { get; set; }

        public override string ToString ()
        {
            return Nombre;
        }
    }
}
