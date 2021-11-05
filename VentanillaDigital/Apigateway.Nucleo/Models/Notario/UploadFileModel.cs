using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Notario
{
    public class UploadFileModel
    {
        public int Index { get; set; }
        public string Nombre { get; set; }
        public string Formato { get; set; }
        public string Data { get; set; }
        public int? Type { get; set; }
    }
}
