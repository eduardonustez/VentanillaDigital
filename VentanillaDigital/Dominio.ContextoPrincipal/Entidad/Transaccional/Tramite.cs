using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class Tramite : EntidadBase
    {
        public long TramiteId { get; set; }
        public int CantidadComparecientes { get; set; }
        public long TipoTramiteId { get; set; }
        public long NotariaId { get; set; }
        public long EstadoTramiteId { get; set; }
        public DateTime Fecha { get; set; }
        public long? ActaNotarialId { get; set; }
        public string DatosAdicionales { get; set; }
        public bool UsarSticker { get; set; }
        public bool FueraDeDespacho { get; set; }
        public string DireccionComparecencia { get; set; }
        public virtual TipoTramite TipoTramite { get; set; }
        public virtual EstadoTramite EstadoTramite { get; set; }
        public virtual Notaria Notaria { get; set; }
        public virtual MetadataArchivo ActaNotarial { get; set; } 
        public virtual DocumentoPrevio DocumentoPrevio { get; set; }
        //public virtual ICollection<DocumentoPendienteAutorizar> DocumentosPendienteAutorizar { get; set; } = new HashSet<DocumentoPendienteAutorizar>();
    }
}
