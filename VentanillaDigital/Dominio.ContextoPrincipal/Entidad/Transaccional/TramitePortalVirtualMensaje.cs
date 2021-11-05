using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class TramitePortalVirtualMensaje : EntidadBase
    {
        public long TramitePortalVirtualMensajeId { get; set; }
        public int TramitePortalVirtualId { get; set; }
        public string Mensaje { get; set; }
        public bool EsNotario { get; set; }

        public virtual TramitesPortalVirtual TramitesPortalVirtual { get; set; }
    }
}
