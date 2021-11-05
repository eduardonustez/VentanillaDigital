using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
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
        public string TramiteVirtualGuid { get; set; }
        public string DatosAdicionales { get; set; }
        public string CUANDI { get; set; }
        public string EstadoNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoTramiteVirtualNombre { get; set; }
        public int? ActoPrincipalId { get; set; }
        public string ActoPrincipalNombre { get; set; }

        public List<ArchivoTramiteVirtual> Archivos { get; set; } = new List<ArchivoTramiteVirtual>();
    }

    public class ArchivoTramiteVirtual
    {
        public long ArchivosPortalVirtualId { get; set; }
        public string Formato { get; set; }
        public string Base64 { get; set; }
        public string Nombre { get; set; }
        public short TipoArchivo { get; set; }
        public byte[] FileBytes { get; set; }
        public string TipoNombre { get; set; }
    }
}
