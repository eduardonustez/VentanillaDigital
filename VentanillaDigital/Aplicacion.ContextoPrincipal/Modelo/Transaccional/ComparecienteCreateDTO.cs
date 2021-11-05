using Aplicacion.ContextoPrincipal.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class ComparecienteCreateDTO : NewRegisterDTO
    {
        public long TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string PeticionRNEC { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public int? ComparecienteActualPos { get; set; }
        public bool? HitDedo1 { get; set; }
        public bool?HitDedo2 { get; set; }
        public string Vigencia { get; set; }
        public string NombreDigitado { get; set; }
        public string NumeroCelular { set { Celular = value; } }
        [Newtonsoft.Json.JsonIgnore]
        public Documento Foto { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Documento Firma { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public Documento ImagenDocumento { get; set; }
        public ExcepcionHuellaModel Excepciones { get; set; }
    }

    public class Documento : NewRegisterDTO
    {
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public long Tamano { get; set; }
    }
    public class ExcepcionHuellaModel
    {
        public string Descripcion { get; set; }
        public EnumDedos[] Dedos { get; set; }
    }
}
