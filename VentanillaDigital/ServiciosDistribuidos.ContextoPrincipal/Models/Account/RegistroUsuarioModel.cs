using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models.Account
{
    public class RegistroUsuarioModel : NewRegisterDTO
    {        
        public int RolId { get; set; }
        public int TipoIdentificacionId { get; set; }
        public string EmailNotaria { get; set; }
        public string EmailPersonal { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroCelular { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public bool esUsuarioNotaria { get; set; }
        public int Genero { get; set; }
        public List<PersonaDatosModel> personaDatos { get; set; }


    }
}
