using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class LoginAuthenticate
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
}
