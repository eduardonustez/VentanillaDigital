using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.NotariaVirtual
{
    public class EstadoTramitesVirtualesRequest
    {
        public bool IsDeleted { get; set; }
    }

    public class EstadoTramitesVirtualesResponse
    {
        public int EstadoTramiteVirtualId { get; set; }
        public string Descripcion { get; set; }
    }
}
