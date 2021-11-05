using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PortalCliente.Data
{
    public class Imagen
    {
        public byte[] Archivo { get; set; }
        public string Nombre { get; set; }
        public string Formato { get; set; }
        public string Descripcion { get; set; }
        public Size Dimensiones { get; set; }
        public long Tamano { get; set; }
    }
}
