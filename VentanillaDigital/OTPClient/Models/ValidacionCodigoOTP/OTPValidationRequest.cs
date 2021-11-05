using System;
using System.Collections.Generic;
using System.Text;

namespace OTPClient.Models
{
    public class OTPValidationRequest
    {
        public Guid CodigoAplicacion { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public Guid Identificador { get; set; }
        public string TextoOTP { get; set; }
    }
}
