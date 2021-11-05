using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models.Transaccional
{
    public class ComparecienteCreateResponse
    {
        public int TramiteId { get; set; }
        public bool ComparecienteCompleto { get; set; }
        public int TotalComparecientes { get; set; }
        public int ComparecienteActual { get; set; }
    }
}
