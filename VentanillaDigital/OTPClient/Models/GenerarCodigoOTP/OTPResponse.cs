using System;
using System.Collections.Generic;
using System.Text;

namespace OTPClient.Models
{
    public class OTPResponse
    {
        public int CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
        public int IdConsulta { get; set; }
        public Guid Identificador { get; set; }
        public string TextoOTP { get; set; }
    }
}
