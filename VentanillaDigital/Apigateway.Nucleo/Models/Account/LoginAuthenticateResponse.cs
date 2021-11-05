using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Contratos.Models.Account
{
    public class LoginAuthenticateResponse
    {
        public string status { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public LoginAuthenticateData data { get; set; }
    }
    public class LoginAuthenticateData
    {
        public bool isValid { get; set; }
        public string token { get; set; }
    }
}
