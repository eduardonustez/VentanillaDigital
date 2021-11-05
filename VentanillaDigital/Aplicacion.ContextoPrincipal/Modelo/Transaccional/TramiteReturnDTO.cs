using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class TramiteReturnDTO
    {
        public long TramiteId { get; set; }
        public string NombreTramite { get; set; }
        public bool ComparecientesCompletos { get; set; }
        public long ComparecienteActual { get; set; }
        public int CantidadComparecientes { get; set; }
        public long TipoTramiteId { get; set; }
        public long NotariaId { get; set; }
        public long EstadoTramiteId { get; set; }
        public DateTime Fecha { get; set; }
        public string DatosAdicionales { get; set; }
        public bool UsarSticker { get; set; }
        public string DireccionComparecencia { get; set; }
        public bool FueraDeDespacho { get; set; }

        public TipoTramiteReturnDTO TipoTramite { get; set; }
        public List<ComparecienteReturnDTO> Comparecientes { get; set; }
    }
}
