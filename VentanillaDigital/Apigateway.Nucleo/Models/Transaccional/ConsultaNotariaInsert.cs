using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class ConsultaNotariaInsert
    {
        public int idNotaria { get; set; }
        public bool Encontrado { get; set; }
        public string fechaTramite { get; set; }
        public int numeroDocumento { get; set; }
    }
}
