using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Archivos
{
    public class Foto
    {
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamano { get; set; }
    }
}
