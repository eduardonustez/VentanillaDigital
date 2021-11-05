using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class NotarioReturnDTO
    {
        public long NotarioId { get; set; }
        public string NotarioNombre { get; set; }
        public bool NotarioDeTurno { get; set; }
    }
    public class NotarioNotariaDTO:NewRegisterDTO
    {
        public long NotarioId { get; set; }
    }
}
