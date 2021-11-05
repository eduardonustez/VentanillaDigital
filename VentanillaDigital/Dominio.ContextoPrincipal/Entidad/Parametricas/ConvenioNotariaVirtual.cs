using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad
{
    public class ConvenioNotariaVirtual : EntidadBase
    {
        public int ConvenioNotariaVirtualId { get; set; }
        public long NotariaId { get; set; }
        public decimal Latitud1 { get; set; }
        public decimal Latitud2 { get; set; }
        public decimal Longitud1 { get; set; }
        public decimal Longitud2 { get; set; }
        public string SerialCertificado { get; set; }

        public string IdNotariaSNR { get; set; }
        public string AutorizacionAutenticacionSNR { get; set; }
        public string ApiUserSNR { get; set; }
        public string ApiKeySNR { get; set; }

        public string UrlApiMiFirma { get; set; }
        public string UrlSubirDocumentosMiFirma { get; set; }
        public string SerialCertificadoNotarioEncargado { get; set; }
        public string ConfigurationGuid { get; set; }
        public string LoginConvenio { get; set; }
        public string PasswordConvenio { get; set; }

        public virtual Notaria Notaria { get; set; }
    }
}
