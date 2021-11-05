using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ComparecienteCreateReturnDTO
    {
        public long TramiteId { get; set; }
        public string NombreTramite { get; set; }
        public bool ComparecientesCompletos { get; set; }
        public long ComparecienteActual { get; set; }
        public int CantidadComparecientes { get; set; }
    }
}
