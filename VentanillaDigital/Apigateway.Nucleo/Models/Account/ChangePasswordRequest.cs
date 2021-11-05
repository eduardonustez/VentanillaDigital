using System;
using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class ChangePasswordRequest
    {
        public string Code { get; set; }
        public string Password { get; set; } 
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

    }

    public class ChangePasswordModelDTO
    {
        public string Code { get; set; }   
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        public ChangePasswordModelDTO(ChangePasswordRequest model)
        {
            Code = model.Code;
            Password = model.Password;
            ConfirmPassword = model.ConfirmPassword;
            Email = model.Email;
        }
    }

}
