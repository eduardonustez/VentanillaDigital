using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Data.Account
{
    public class AuthenticatedUser
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public string Rol { get; set; }
        public string Notaria { get; set; }
        public RegisteredUser RegisteredUser { get; set; }
        public Guid IdentificadorOTP { get; set; }
    }
}
