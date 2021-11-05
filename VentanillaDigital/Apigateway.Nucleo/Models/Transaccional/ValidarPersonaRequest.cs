using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class ValidarPersonaRequest
    {
        public int NotariaId { get; set; }
        public string Email { get; set; }
        public int TipoIdentificacionId { get; set; }
        public string NumeroDocumento { get; set; }
    }
}
