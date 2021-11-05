using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosTramiteActa
    {
        public long TramiteId { get; set; }
        public string TipoTramite { get; set; }
        public long TipoTramiteCodigo { get; set; }
        public DateTime FechaTramite { get; set; }
        public string DatosAdicionales { get; set; }
        public string Plantilla { get; set; }
        public string Plantilla2 { get; set; }
        public long TipoTramiteId { get; set; }
        public bool UsarSticker { get; set; }
        public bool FueraDeDespacho { get; set; }
        public string DireccionComparecencia { get; set; }
    }
}
