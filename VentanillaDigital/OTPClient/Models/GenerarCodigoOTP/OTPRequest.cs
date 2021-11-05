using System;
using System.Collections.Generic;
using System.Text;

namespace OTPClient.Models
{
    public class OTPRequest
    {
        public Guid CodigoAplicacion { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
    }
}
