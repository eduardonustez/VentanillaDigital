using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class GetCertificateResponse
    {
        public string Pfx { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
