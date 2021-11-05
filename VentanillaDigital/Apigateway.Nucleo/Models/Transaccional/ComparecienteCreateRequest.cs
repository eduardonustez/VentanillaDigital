using ApiGateway.Contratos.Models.Archivos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models.Transaccional
{
    public class ComparecienteCreateRequest
    {
        public long TramiteId { get; set; }
        public int TipoDocumentoId { get; set; }
        public string NumeroDocumento { get; set; }
        public Foto Foto { get; set; }
        public Foto Firma { get; set; }
        public Foto ImagenDocumento { get; set; }
        public string PeticionRNEC { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroCelular { get; set; }
        public string Email { get; set; }
        public bool TramiteSinBiometria { get; set; }
        public string MotivoSinBiometria { get; set; }
        public int? ComparecienteActualPos { get; set; }
        public bool? HitDedo1 { get; set; }
        public bool? HitDedo2 { get; set; }
        public string Vigencia { get; set; }
        public string NombreDigitado { get; set; }
        public ExcepcionHuellaModel Excepciones { get; set; }
    }
}
