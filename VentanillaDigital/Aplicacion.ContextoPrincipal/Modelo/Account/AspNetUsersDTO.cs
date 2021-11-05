using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class AspNetUsersDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RolName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos{ get; set; }
        public string NombreCompleto{ get; set; }
        public string NumeroIdentificacion { get; set; }
        public string TipoDocumento { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string NumeroCelular { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Area { get; set; }
        public string Cargo { get; set; }
        public int? Genero { get; set; }
        public long NotariaId { get; set; }
    }
}
