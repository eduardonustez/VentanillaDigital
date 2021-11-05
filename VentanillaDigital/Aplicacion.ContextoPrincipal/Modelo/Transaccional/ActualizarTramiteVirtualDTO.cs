using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ActualizarTramiteVirtualDTO
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

    public class ActualizarTramiteVirtualResponseDTO
    {
        public bool EsTramiteActualizado { get; set; }
        public string MensajeError { get; set; }
    }
}
