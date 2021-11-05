using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class TramitesVirtualesModel
    {
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<TramiteVirtuales> TramitesPortalVirtualReturn { get; set; }
    }

    public class TramiteVirtuales
    {
        public int TramitesPortalVirtualId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public bool IsDeleted { get; set; }
        public int NotariaId { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string CUANDI { get; set; }
        public string EstadoTramite { get; set; }
        public string TipoTramite { get; set; }
    }
}
