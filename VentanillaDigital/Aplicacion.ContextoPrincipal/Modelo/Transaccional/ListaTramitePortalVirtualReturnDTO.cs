using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ListaTramitePortalVirtualReturnDTO: RespuestaServicioViewModel
    {
        public IEnumerable<TramitePortalVirtualReturnDTO> TramitesPortalVirtualReturn { get; set; }
    }
    public class TramitePortalVirtualReturnDTO {
        public int TramitesPortalVirtualId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool IsDeleted { get; set; }
        public int NotariaId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CUANDI { get; set; }
        public string EstadoTramite { get; set; }
        public string TipoTramite { get; set; }
    }
}
