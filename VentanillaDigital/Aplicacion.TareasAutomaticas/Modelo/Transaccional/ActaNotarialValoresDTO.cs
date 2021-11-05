using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.TareasAutomaticas.Modelo.Transaccional
{
    public class ActaNotarialValoresDTO
    {
        public string TramiteId { get; set; }
        public string FechaTramite { get; set; }
        public string TipoTramite { get; set; }
        public string NUT { get; set; }
        public string iddoc { get; set; }


        public string NumeroNotaria { get; set; }
        public string NumeroNotariaEnLetra { get; set; }
        public string CirculoNotaria { get; set; }
        public string Municipio { get; set; }
        public string Departamento { get; set; }
        public string NombreNotario { get; set; }
        public string TipoNotario { get; set; }
        public string GeneroNotario { get; set; }
        public string NumeroNotarioEnLetra { get; set; }

        public string UrlQr { get; set; }
        public string FirmaNotario { get; set; }
        public string SelloNotaria { get; set; }
        public string TramiteQR { get; set; }
        public string DireccionDiligencia { get; set; }
        public string DireccionComparecencia { get; set; }
    }
}
