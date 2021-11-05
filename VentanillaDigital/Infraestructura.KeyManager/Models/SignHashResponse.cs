using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class SignHashResponse
    {
        public string status { get; set; }
        public int status_code { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
