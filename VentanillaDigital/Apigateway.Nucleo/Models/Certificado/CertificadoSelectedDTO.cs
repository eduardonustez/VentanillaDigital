using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Certificado
{
    public class CertificadoSelectedDTO
    {
        public int CertificadoId { get; set; }
        public string UsuarioCertificado { get; set; }
        public string UsuarioId { get; set; }
    }
}
