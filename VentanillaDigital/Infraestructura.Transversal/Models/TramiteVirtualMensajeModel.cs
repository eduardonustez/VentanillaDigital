using System;

namespace Infraestructura.Transversal.Models
{
    public class TramiteVirtualMensajeModel
    {
        public long TramitePortalVirtualMensajeId { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Mensaje { get; set; }
        public bool EsNotario { get; set; }
    }
}
