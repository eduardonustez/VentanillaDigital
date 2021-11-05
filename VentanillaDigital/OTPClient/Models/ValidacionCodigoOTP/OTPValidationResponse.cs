using System;
using System.Collections.Generic;
using System.Text;

namespace OTPClient.Models
{
    public class OTPValidationResponse
    {
        public int CodigoRespuesta { get; set; }
        public string DescripcionRespuesta { get; set; }
        public bool EsValidoOTP { get; set; }
        public int idConsulta { get; set; }
    }
}
