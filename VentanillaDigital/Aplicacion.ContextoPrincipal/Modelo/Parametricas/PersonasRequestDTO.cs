using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class PersonasRequestDTO
    {
        public Guid UserId { get; set; }
        public long PersonaId { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Nombre_Completo { get; set; }
        public string Email { get; set; }
        public string EmailNotaria { get; set; }
        public string Celular { get; set; }
        public string Token { get; set; }
        public string IsAuthenticated { get; set; }
        public long IdCliente { get; set; }
        public string IdConvenio { get; set; }
        public long IdOficina { get; set; }
        public long IdRol { get; set; }
        public string Cargo { get; set; }
        public string Area { get; set; }
        public string TipoDocumentoAbreviatura { get; set; }
        public string Login { get; set; }
        public bool Habilitado { get; set; }

    }
}
