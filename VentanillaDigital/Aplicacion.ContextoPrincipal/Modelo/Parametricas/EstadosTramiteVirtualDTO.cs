using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Parametricas
{
    public class EstadosTramiteVirtualDTO
    {
        public bool IsDeleted { get; set; }
    }

    public class EstadosTramitesVirtualResponseDTO
    {
        public int EstadoTramiteVirtualId { get; set; }
        public string Descripcion { get; set; }
    }
}
