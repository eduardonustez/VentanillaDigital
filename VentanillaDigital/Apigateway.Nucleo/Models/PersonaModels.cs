using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class PersonaInfoReturnDTO
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
    }
    public class PersonaIdentificacionDTO
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
    }

}
