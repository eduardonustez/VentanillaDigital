using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class UploadFileModel
    {
        public string Nombre { get; set; }
        public string Formato { get; set; }
        public string Data { get; set; }
        public int? Type { get; set; }
    }
}
