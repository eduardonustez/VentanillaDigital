using System;

namespace Infraestructura.Transversal.Models
{
    public class RecaudoTramiteModel
    {
        public long RecaudoTramiteVirtualId { get; set; }
        public int TramitePortalVirtualId { get; set; }
        public string Estado { get; set; }
        public string NombreCompleto { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroIdenficacion { get; set; }
        public string Correo { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacion { get; set; }
        public string CUS { get; set; }
        public DateTime? FechaPagado { get; set; }
        public decimal ValorPagado { get; set; }
    }
}
