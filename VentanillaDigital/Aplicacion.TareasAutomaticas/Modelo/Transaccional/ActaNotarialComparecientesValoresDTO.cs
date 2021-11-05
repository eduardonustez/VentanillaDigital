using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.TareasAutomaticas.Modelo.Transaccional
{

    public class ActaNotarialComparecientesValoresDTO
    {
        public string NombreTipoDocumento { get; set; }
        public string NombreCompareciente { get; set; }
        public string NUIPCompareciente { get; set; }
        public string FotoCompareciente { get; set; }
        public string FirmaCompareciente { get; set; }
        public string NUT { get; set; }
        public string FechaCompletaNumeros { get; set; }
        public string HoraCompletaNumeros { get; set; }
        public string TramiteSinBiometria { get; set; }
        public string TextoBiometria { get; set; }
    }
}
