using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria.Models
{
    public class ValidacionResponse
    {
        public string Documento { get; set; }
        //public DateTime FechaExpedicion { get; set; }
        public Huella[] Huellas { get; set; }
        public string NutValidacion { get; set; }
        public string NombreRNEC { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public bool Validado { get; set; }
        public string Vigencia { get; set; }
    }
    public class Huella
    {
        public string Detalle { get; set; }
        public Dedo Dedo { get; set; }
        public bool Hit { get; set; }
        public int Score { get; set; }
    }
}
