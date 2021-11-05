using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class RecoveryPasswordDTO
    {
        public string Email { get; set; }
        public string UrlRedirect { get; set; }
    }
}
