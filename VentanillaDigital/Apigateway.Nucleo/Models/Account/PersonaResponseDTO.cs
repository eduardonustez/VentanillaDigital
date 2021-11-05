using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Models
{
   public class PersonaResponseDTO
    {
        public Guid UserId { get; set; }
        public long PersonaId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Nombre_Completo { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Token { get; set; }
        public string IsAuthenticated { get; set; }
        public string rolName { get; set; }
    }
}
