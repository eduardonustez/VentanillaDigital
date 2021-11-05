using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class AsignarClaveUsuarioAdminRequest
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
