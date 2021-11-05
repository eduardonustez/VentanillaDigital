using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Transversal.Log.Modelo
{
    public class IdentificacionEquipo
    {
        public string NombreEquipo { get; set; }
        public string DireccionIp { get; set; }
        public string DireccionMac { get; set; }

        public IdentificacionEquipo(string nombreEquipo,string direccionIp,string direccionMac)
        {
            this.NombreEquipo = nombreEquipo;
            this.DireccionIp = direccionIp;
            this.DireccionMac = direccionMac;
        }
    }
}
