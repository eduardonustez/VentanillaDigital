using System.Collections.Generic;

namespace ApiGateway.Contratos.Models.Transaccional
{
    public class TramiteVirtualActualizacionRequest
    {
        public int NotariaId { get; set; }
        public int EstadoTramiteVirtualId { get; set; }
        public int TramiteVirtualID { get; set; }
        public string TramiteVirtualGuid { get; set; }
        public string CUANDI { get; set; }
        public string DatosAdicional { get; set; }
        public bool BorrarArchivo { get; set; }
        public List<ArchivoRequest> Archivos { get; set; }
    }
    public class ArchivoRequest
    {
        public int TipoArchivo { get; set; }
        public string Formato { get; set; }
        public string Base64 { get; set; }
        public string Nombre { get; set; }
    }
}
