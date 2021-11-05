using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data.Account
{
    public class UserAccount
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int RolId { get; set; }
        [Required]
        public int TipoIdentificacionId { get; set; }
        //public string Password { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}",
        ErrorMessage = "No es un email permitido.")]
        public string EmailNotaria { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}",
        ErrorMessage = "Verifique que el email está escrito correctamente")]
        public string EmailPersonal { get; set; }
        [Required]
        public string NumeroDocumento { get; set; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        [MinLength(7, ErrorMessage = "El número de celular no puede contener menos de 7 digitos ")]
        [MaxLength(12, ErrorMessage = "El número de celular no puede contener más de 12 digitos ")]
        public string NumeroCelular { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Cargo { get; set; }
        public bool EsUsuarioNotaria { get; set; }
        public List<PersonaDatos> personaDatos { get; set; }
        [Required]
        //[Range(0, int.MaxValue, ErrorMessage = "Selecciona un elemento de la lista")]
        public long NotariaId { get; set; }
        public int Genero { get; set; }

        public string GetError()
        {
            if (!string.IsNullOrWhiteSpace(NumeroCelular))
            {
                return "Este campo no puede estar vacío";
            }

            return "";

        }
    }
}
