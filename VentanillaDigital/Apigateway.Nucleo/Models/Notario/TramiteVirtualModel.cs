using System;
using System.Collections.Generic;

namespace ApiGateway.Contratos.Models.Notario
{
    public class TramiteVirtualModel
    {
        public int NotariaId { get; set; }
        public int TipoTramiteVirtualId { get; set; }
        public int EstadoTramiteVirtualId { get; set; }
        public int TipoDocumento { get; set; }
        public string TipoDocumentoNombre { get; set; }
        public string NumeroDocumento { get; set; }
        public int TramiteVirtualID { get; set; }
        public string DatosAdicionales { get; set; } //= "{\"nombres\":\"Diego Andres\",\"Apellidos\":\"Roldan Lozano\",\"Edad\":\"31\"}";

        public string EstadoNombre { get; set; }
        public string CUANDI { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoTramiteVirtualNombre { get; set; }
        public int? ActoPrincipalId { get; set; }
        public string ActoPrincipalNombre { get; set; }

        public bool HasDatosAdicionales => !string.IsNullOrEmpty(DatosAdicionales) && 
            !DatosAdicionales.Equals("{}");

        public List<ArchivoTramiteVirtual> Archivos { get; set; } = new List<ArchivoTramiteVirtual>();
    }

    public class ArchivoTramiteVirtual
    {
        public long ArchivosPortalVirtualId { get; set; }
        public string Formato { get; set; }
        public string Base64 { get; set; }
        public string Nombre { get; set; }
        public bool IsBigFile { get; set; }
        public string TipoNombre { get; set; }
        public byte[] FileBytes { get; set; }
        public short TipoArchivo { get; set; }
    }
}
