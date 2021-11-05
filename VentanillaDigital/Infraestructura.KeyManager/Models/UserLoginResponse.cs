using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class UserLoginResponse
    {
        public string PasswordExpiration { get; set; }
        public string JwtToken { get; set; }
        public bool Success { get; set; }
    }
}
