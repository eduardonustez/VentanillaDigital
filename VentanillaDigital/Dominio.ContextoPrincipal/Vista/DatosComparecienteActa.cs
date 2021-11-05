using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosComparecienteActa
    {
        public string NombreTipoDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NUIPCompareciente { get; set; }
        public string NUT { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public long FirmaId { get; set; }
        public long FotoId { get; set; }
        public int Posicion { get; set; }
    }
}
