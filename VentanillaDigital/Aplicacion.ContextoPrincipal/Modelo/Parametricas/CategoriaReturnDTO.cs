using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class CategoriaReturnDTO
    {
        public long CategoriaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public virtual IEnumerable<TipoTramiteReturnDTO> TiposTramites { get; set; }
    }
}
