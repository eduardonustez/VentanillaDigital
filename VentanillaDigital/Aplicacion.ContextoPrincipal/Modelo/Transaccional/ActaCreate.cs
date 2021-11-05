using System;
using System.Collections.Generic;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ActaCreateDTO:NewRegisterDTO
    {
        public long TramiteId { get; set; }
        public DateTime FechaTramite { get; set; }
        public string TipoTramite { get; set; }
        public long CodigoTramite { get; set; }
        public string DataAdicional { get; set; }
        public bool UsarSticker { get; set; }
        public bool FirmarManual { get; set; }
        public long NotarioId { get; set; }
        public bool FueraDeDespacho { get; set; }
        public string DireccionComparecencia { get; set; }
        public List<ComparecienteCreate> ComparecientesCreate { get; set; } = new List<ComparecienteCreate>();

    }
    public class ComparecienteCreate
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NUIPCompareciente { get; set; }
        public string FotoCompareciente { get; set; }
        public string FirmaCompareciente { get; set; }
        public string NUT { get; set; }
        public string FechaCompletaNumeros { get; set; }
        public string HoraCompletaNumeros { get; set; }
        public string TextoBiometria { get; set; }
        public string TramiteSinBiometria { get; set; }
        public string NombreTipoDocumento { get; set; }
        public string Posicion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
    public class ActaCreateToPlantilla
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
        public string DireccionComparecencia { get; set; }
        public string DireccionDiligencia { get; set; }


    }


}
