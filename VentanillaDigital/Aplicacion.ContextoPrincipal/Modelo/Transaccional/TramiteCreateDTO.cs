using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteCreateDTO : NewRegisterDTO
    {
        public int CantidadComparecientes { get; set; }
        public long TipoTramiteId { get; set; }
        public string DatosAdicionales { get; set; }
        public bool UsarSticker { get; set; }
        public bool FueraDeDespacho { get; set; }
        public string DireccionComparecencia { get; set; }
    }
}
