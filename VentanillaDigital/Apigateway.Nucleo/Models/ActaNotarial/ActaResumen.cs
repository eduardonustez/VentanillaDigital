using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.ActaNotarial
{
    public class ActaResumen
    {
        public long TramiteId { get; set; }
        public DateTime TramiteFecha { get; set; }
        public string NotariaNombre { get; set; }
        public long TipoTramiteId { get; set; }
        public string TipoTramiteNombre { get; set; }
        public string DatosAdicionales { get; set; }
        public string Estado { get; set; }
        public List<ComparecienteResumen> Comparecientes { get; set; }
    }
    public class ComparecienteResumen
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
