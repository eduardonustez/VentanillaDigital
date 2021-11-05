using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.Nucleo.Entidad;
using System.Collections.Generic;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class ActoNotarial : EntidadBase
    {
        public int ActoNotarialId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int? TipoTramiteVirtualId { get; set; }
        public bool Activo { get; set; }

        public virtual TipoTramiteVirtual TipoTramiteVirtual { get; set; }
        public virtual ICollection<ActoPorTramite> ActosPorTramite { get; set; } = new HashSet<ActoPorTramite>();
        public virtual ICollection<TramitesPortalVirtual> TramitesPortalVirtual { get; set; } = new HashSet<TramitesPortalVirtual>();
    }
}
