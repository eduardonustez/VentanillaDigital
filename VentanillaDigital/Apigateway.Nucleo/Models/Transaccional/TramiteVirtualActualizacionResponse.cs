using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class TramiteVirtualActualizacionResponse
    {
        public bool EsTramiteActualizado { get; set; }
        public string MensajeError { get; set; }
    }
}
