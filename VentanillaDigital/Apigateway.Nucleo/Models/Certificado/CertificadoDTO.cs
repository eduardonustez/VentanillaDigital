using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Certificado
{
    public class CertificadoDTO
    {
        public int IdCertificado { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string ValidoDesde { get; set; }
        public string ValidoHasta { get; set; }
        public bool Seleccionado { get; set; }
    }
}
