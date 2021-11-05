using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Modelo
{
    public class SerilogConfig
    {
        public string ConnectionStrings { get; set; }
        public string NombreTabla { get; set; }
        public string NombreSchema { get; set; }
    }
}
