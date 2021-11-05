using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Correo
{
    public class ServidorCorreo
    {
        public string host { get; set; }
        public string port { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fromaddress { get; set; }
        public string fromname { get; set; }
    }
}
