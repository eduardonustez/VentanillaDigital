using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortalAdministrador.Data
{
    public class PersonaInfo
    {
        [Required]
        public TipoIdentificacion TipoIdentificacion { get; set; }

        [Required]
        public string NumeroIdentificacion { get; set; }

        [Required]
        public virtual string Nombres { get; set; }

        [Required]
        public virtual string Apellidos { get; set; }

        public string NumeroCelular { get; set; }

        public string Email { get; set; }

        public string NombreCompleto { get => $"{Nombres} {Apellidos}"; }

        public virtual bool IsValid(bool terminando=false)
        {
            bool valid = true;
            if (terminando)
            {
                valid &= !(string.IsNullOrEmpty(Nombres) || string.IsNullOrEmpty(Apellidos));
            }

            if (!string.IsNullOrEmpty(Email)) 
                valid&= Regex.IsMatch(Email, @"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}", RegexOptions.IgnoreCase);

            return valid &&
                TipoIdentificacion != null &&
                !string.IsNullOrWhiteSpace(NumeroIdentificacion) &&
                ( TipoIdentificacion?.Abreviatura == "P" || long.TryParse(NumeroIdentificacion, out long x) );
        }

        public string GetError ()
        {
            if (!string.IsNullOrWhiteSpace(NumeroIdentificacion) && (TipoIdentificacion?.Abreviatura != "P" && !long.TryParse(NumeroIdentificacion, out long x))) {
                return "El número de documento no puede contener letras";
            }

            return "";
            
        }
    }
}
