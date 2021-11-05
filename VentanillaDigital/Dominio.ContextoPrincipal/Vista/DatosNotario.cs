using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosNotario
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Pin { get; set; }
        public int TipoNotario { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Genero { get; set; }
    }

    public class DatosNotarioFirmaManual
    {
        public int NumeroNotaria { get; set; }
        public string NumeroNotariaEnLetras { get; set; }
        public string CirculoNotaria { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoNotario { get; set; }
        public string Genero { get; set; }
    }
}
