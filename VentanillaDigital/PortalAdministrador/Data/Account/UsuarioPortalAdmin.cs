using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data.Account
{
    public class UsuarioPortalAdmin
    {
        public long Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}",
        ErrorMessage = "No es un email permitido.")]
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class EliminarUsuarioPortalAdmin
    {
        public long Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}",
        ErrorMessage = "No es un email permitido.")]
        public string Email { get; set; }
        public string Rol { get; set; }
    }
}
