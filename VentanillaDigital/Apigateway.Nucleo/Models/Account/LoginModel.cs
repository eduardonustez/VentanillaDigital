using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class LoginModel
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

    }
    public class LoginFuncionarioModel
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        public bool Movil { get; set; }
    }
    public class LoginOtpModel
    {
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        public Guid Identificador { get; set; }
        public string Otp { get; set; }
    }
}
