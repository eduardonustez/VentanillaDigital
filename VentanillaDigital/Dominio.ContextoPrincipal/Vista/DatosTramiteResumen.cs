using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Vista
{
    public class DatosTramiteResumen
    {
        public long TramiteId { get; set; }
        public DateTime TramiteFecha { get; set; }
        public string NotariaNombre { get; set; }
        public long CodigoTramite { get; set; }
        public string TipoTramiteNombre { get; set; }
        public string DatosAdicionales { get; set; }
        public string Estado { get; set; }
        public List<DatosComparecientesResumen> Comparecientes { get; set; }
    }
    public class DatosComparecientesResumen 
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public bool SinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public string Foto { get; set; }
        public string NombreDigitado { get; set; }
    }
}
