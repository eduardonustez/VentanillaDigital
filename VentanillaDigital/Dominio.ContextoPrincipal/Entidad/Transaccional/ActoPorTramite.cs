using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class ActoPorTramite : EntidadBase
    {
        public long ActoPorTramiteId { get; set; }
        public int TramitePortalVirtualId { get; set; }
        public int ActoNotarialId { get; set; }
        public string Cuandi { get; set; }
        public short Orden { get; set; }

        public virtual ActoNotarial ActoNotarial { get; set; }
        public virtual TramitesPortalVirtual TramitesPortalVirtual { get; set; }
    }
}
