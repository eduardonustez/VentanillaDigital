using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class NotarioReturnDTO
    {
        public long NotarioId { get; set; }
        public string NotarioNombre { get; set; }
        public bool NotarioDeTurno { get; set; }
    }
    public class NotarioNotariaDTO
    {
        public long NotarioId { get; set; }
    }
}
