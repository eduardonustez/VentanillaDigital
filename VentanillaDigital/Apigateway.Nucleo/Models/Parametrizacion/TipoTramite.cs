using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models
{
    public class TipoTramiteResponse
    {
        public long TipoTramiteId { get; set; }
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public decimal Tarifa { get; set; }
        public string Logo { get; set; }

    }

}
