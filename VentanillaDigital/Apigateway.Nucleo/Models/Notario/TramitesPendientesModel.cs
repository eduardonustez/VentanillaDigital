using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class TramitesPendientesModel
    {
        public int NumeroFilas { get; set; }
        public int IndicePagina { get; set; }
        public bool Conteo { get; set; }
    }
}
