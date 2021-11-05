using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models.Internal
{
    internal class ValidarIdentidadResponse
    {
        public string Detalle { get; set; }
        public string Documento { get; set; }
        //public DateTime FechaExpedicion { get; set; }
        public HuellaResponse[] Huellas { get; set; }
        public long NutValidacion { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string Respuesta { get; set; }
        public bool Validado { get; set; }
        public string Vigencia { get; set; }
        public Guid? IdPeticion { get; set; }
    }
    internal class HuellaResponse
    {
        public string Error { get; set; }
        public string NombreDedo { get; set; }
        public short NumeroDedo { get; set; }
        public string RespuestaAFI { get; set; }
        public int Score { get; set; }
    }
}
