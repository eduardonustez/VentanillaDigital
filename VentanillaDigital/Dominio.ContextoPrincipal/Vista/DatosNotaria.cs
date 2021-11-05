using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosNotaria
    {
        public long NotariaId { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public int NumeroNotaria { get; set; }
        public string NumeroNotariaEnLetras { get; set; }
        public string CirculoNotaria { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
