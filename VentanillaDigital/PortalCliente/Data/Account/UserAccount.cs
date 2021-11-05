using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Data.Account
{
    public class UserAccount
    {
        public string UserId { get; set; }
        public int RolId { get; set; }
        public int TipoIdentificacionId { get; set; }
        //public string Password { get; set; }
        [MaxLength(51)]
        public string EmailNotaria { get; set; }
        [MaxLength(51)]
        public string EmailPersonal { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroCelular { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public bool EsUsuarioNotaria { get; set; }
        public List<PersonaDatos> personaDatos { get; set; }
        public long NotariaId { get; set; }
        public int Genero { get; set; }
    }
}
