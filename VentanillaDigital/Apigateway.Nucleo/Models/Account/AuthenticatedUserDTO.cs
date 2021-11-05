using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class AuthenticatedUserDTO
    {
        public string IsAuthenticated { get; set; }
        public string Token { get; set; }
        public RegisteredUserDTO RegisteredUser { get; set; }
    }

    public class AuthenticatedFuncionarioDTO
    {
        public string IsAuthenticated { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
        public string Notaria { get; set; }

        public Guid IdentificadorOTP { get; set; }
        public RegisteredFuncionarioDTO RegisteredUser { get; set; }
    }

    public class AuthenticatedAdministracionDTO
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
        public string Usuario { get; set; }
    }
}
