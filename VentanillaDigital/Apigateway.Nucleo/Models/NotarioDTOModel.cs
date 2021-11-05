using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models
{
 public   class NotarioDTOModel
    {
        public string Email { get; set; }
        public string Grafo { get; set; }
        public string Pin { get; set; }
    }
    public class EstadoPinFirmaModel
    {
        public bool PinAsignado { get; set; }
        public bool FirmaRegistrada { get; set; }
        public bool CertificadoSolicitado { get; set; }
    }
    public class OpcionesConfiguracioNotarioModel
    {
        public bool UsarSticker { get; set; }
    }
}
