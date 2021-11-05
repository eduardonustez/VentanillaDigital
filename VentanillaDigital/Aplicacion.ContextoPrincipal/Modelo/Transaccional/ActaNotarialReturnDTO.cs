using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ActaNotarialReturnDTO
    {
        public string Archivo { get; set; }
        public bool Autorizada { get; set; }
        public bool Rechazada { get; set; }
    }
    public class FirmaActaNotarialReturnDTO
    {
        public string Archivo { get; set; }
        public bool Autorizada { get; set; }
        public bool EsError { get; set; }
        public int CodigoResultado { get; set; }
    }

    public class TramiteRechazadoReturnDTO
    {
        public int CodigoResultado { get; private set; }
        public string Estado { get; set; }
        public bool EsError { get; set; }
    }
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
