using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.LogTrazabilidad
{
    public class LogTrazabilidadModel
    {
        public string Usuario { get; set; }
        public long NotariaId { get; set; }
        public string Peticion { get; set; }
        public string FechaRegistro { get; set; }
        public string Tiempo { get; set; }
        public string Mensaje { get; set; }
    }
}
