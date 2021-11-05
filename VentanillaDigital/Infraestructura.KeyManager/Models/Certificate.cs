using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
    }
}
