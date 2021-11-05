using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Data
{
    public partial class User
    {
        public int TipoIdentificacionId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        //public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
    }
}
