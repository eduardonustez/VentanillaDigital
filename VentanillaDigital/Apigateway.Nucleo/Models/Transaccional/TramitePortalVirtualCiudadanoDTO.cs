using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class TramitePortalVirtualCiudadanoDTO
    {
        public int NotariaId { get; set; }
        public int TipoTramiteVirtualId { get; set; }
        public int EstadoTramiteVirtualId { get; set; }
        public int TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public int TramiteVirtualID { get; set; }
        public string TramiteVirtualGuid { get; set; }
        public string CUANDI { get; set; }
        public string DatosAdicional { get; set; }
        public int ActoPrincipalId { get; set; }
        public List<ActoTramite> ActosTramite { get; set; } 
        public List<ArchivosDTO> Archivos { get; set; }
    }

    public class ActoTramite
    {
        public int ActoNotarialId { get; set; }
        public string CUANDI { get; set; }
        public int Orden { get; set; }
    }

    public class ArchivosDTO
    {
        public int TipoArchivo { get; set; }
        public string Formato { get; set; }
        public string Base64 { get; set; }
        public string Nombre { get; set; }
    }
}
