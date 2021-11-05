using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class ValidarPersonaResponse
    {
        public bool TramiteExiste { get; set; }
        public DateTime? FechaCreacionTramite { get; set; }
    }
}
