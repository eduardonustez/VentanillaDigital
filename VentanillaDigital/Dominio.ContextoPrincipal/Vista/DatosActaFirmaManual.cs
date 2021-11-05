using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosActaFirmaManual
    {
        public DatosTramiteActa datosTramiteActa { get; set; }
        public DatosNotaria datosNotaria { get; set; }
        public DatosNotario datosNotario { get; set; }
        public List<DatosComparecienteActa> comparecientesFirmaManual { get; set; }
        public List<DatosImagenActa> fotosFirmaManual { get; set; }
        public List<DatosImagenActa> grafosFirmaManual { get; set; }

        public DatosActaFirmaManual()
        {
            comparecientesFirmaManual = new List<DatosComparecienteActa>();
            fotosFirmaManual = new List<DatosImagenActa>();
            grafosFirmaManual = new List<DatosImagenActa>();
        }
    }
}
