using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class TramitePortalVirtualModelResponse
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<ListaTramitesPortalVirtualModel> Tramites { get; set; }
    }
    public class ListaTramitesPortalVirtualModel {
		public int TramitesPortalVirtualId { get; set; }
	    public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public byte IsDeleted { get; set; }
        public int  NotariaId { get; set; }
        public string  TipoDocumento { get; set; }
        public string  NumeroDocumento { get; set; }
        public string CUANDI { get; set; }
        public string EstadoTramite { get; set; }
        public string TipoTramite { get; set; }
	}
}
