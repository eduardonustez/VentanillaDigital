using Dominio.Nucleo.Entidad;
using System;

namespace Dominio.ContextoPrincipal.Entidad.Transaccional
{
    public class RecaudoTramiteVirtual : EntidadBase
    {
        public long RecaudoTramiteVirtualId { get; set; }
        public int TramitePortalVirtualId { get; set; }
        public eEstadoRecaudo Estado { get; set; }
        public string NombreCompleto { get; set; }
        public int TipoIdentificacion { get; set; }
        public string NumeroIdenficacion { get; set; }
        public string Correo { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacion { get; set; }
        public string CUS { get; set; }
        public DateTime? FechaPagado { get; set; }
        public decimal ValorPagado { get; set; }
        public decimal IVA { get; set; }

        public string RespuestaServicio { get; set; }

        public virtual TramitesPortalVirtual TramitesPortalVirtual { get; set; }
    }

    public enum eEstadoRecaudo
    {
        Generado = 1,
        Enviado,
        Pagado,
        Anulado,
        Rechazado
    }
}
