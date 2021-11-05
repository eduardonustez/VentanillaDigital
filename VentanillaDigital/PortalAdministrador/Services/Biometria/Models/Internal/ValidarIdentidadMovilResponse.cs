using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria.Models.Internal
{
    public class ValidarIdentidadMovilResponse
    {
        public string IpDispositivo { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Oficina { get; set; }
        public Biometria[] Biometrias { get; set; }

        public Candidato Candidato { get; set; }


        public class Biometria
        {
            public int Consecutivo { get; set; }
            public string Error { get; set; }
            public string Formato { get; set; }
            public int IdSubtipo { get; set; }
            public int IdTipo { get; set; }
            public string Resultado { get; set; }
            public int Score { get; set; }
        }

        public string DescripcionError { get; set; }
        public int Estado { get; set; }
        public string EstadoPeticion { get; set; }
        public string Fecharespuesta { get; set; }
        public string IdPeticion { get; set; }
        public int IdRespuesta { get; set; }
        public string IdTransaccion { get; set; }
        public string Resultado { get; set; }

       
    }
    public class Candidato
    {
        public string ExpFecha { get; set; }
        public string ExpLugar { get; set; }
        public int IdTipoDocumento { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string PrimerApellido { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoApellido { get; set; }
        public string SegundoNombre { get; set; }
        public string Vigencia { get; set; }

    }
}
