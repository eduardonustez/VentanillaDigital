using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class PersonaCreateOrUpdateDTO: NewRegisterDTO
    {
        public string AspNetUserId { get; set; }
        public string NombreUsuario { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string NumeroCelular { get; set; }
        public int TipoIdentificacionId { get; set; }
        public int Genero { get; set; }
    }
}
