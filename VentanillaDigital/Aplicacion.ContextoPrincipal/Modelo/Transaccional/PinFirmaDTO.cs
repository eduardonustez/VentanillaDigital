using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class PinFirmaDTO:NewRegisterDTO
    {
        public string Pin { get; set; }
        public long TramiteId { get; set; }
    }
    
}
