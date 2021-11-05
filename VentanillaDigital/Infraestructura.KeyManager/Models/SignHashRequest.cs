using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class SignHashRequest
    {
        public int certificateId { get; set; }
        public int pin { get; set; }
        public string hash { get; set; }
    }
}
