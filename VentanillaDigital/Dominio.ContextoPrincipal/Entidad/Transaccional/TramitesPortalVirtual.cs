using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo.Entidad;
using System.Collections.Generic;

namespace Dominio.ContextoPrincipal.Entidad
{
    public class TramitesPortalVirtual : EntidadBase
    {
        public int TramitesPortalVirtualId { get; set; }
        public int NotariaId { get; set; }
        public int TipoTramiteVirtualId { get; set; }
        public int EstadoTramiteVirtualId { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int TramiteVirtualID { get; set; }
        public string CUANDI { get; set; }
        public string DatosAdicionales { get; set; }
        public string DetalleCambioEstado { get; set; }
        public string TramiteVirtualGuid { get; set; }
        public int? ActoPrincipalId { get; set; }
        public string DatosAdicionalesCierre { get; set; }

        public virtual TipoTramiteVirtual TipoTramiteVirtual { get; set; }
        public virtual EstadoTramiteVirtual EstadoTramiteVirtual { get; set; }
        public virtual ActoNotarial ActoPrincipal { get; set; }
        public virtual ICollection<ActoPorTramite> ActosPorTramite { get; set; } = new HashSet<ActoPorTramite>();
        public virtual ICollection<TramitePortalVirtualMensaje> TramitePortalVirtualMensajes { get; set; } = new HashSet<TramitePortalVirtualMensaje>();
        public virtual ICollection<RecaudoTramiteVirtual> RecaudosTramiteVirtual { get; set; } = new HashSet<RecaudoTramiteVirtual>();
    }
}
