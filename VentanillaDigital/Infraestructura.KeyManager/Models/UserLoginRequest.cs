using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.KeyManager.Models
{
    public class UserLoginRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
        public UserLoginRequest(string user,string password)
        {
            User = user;
            Password = password;
        }
    }
}
