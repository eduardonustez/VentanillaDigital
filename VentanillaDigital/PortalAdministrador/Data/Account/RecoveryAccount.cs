using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Data.Account
{
    public class RecoveryAccount
    {
        //[Required]
        //[EmailAddress(ErrorMessage = "Email invalido")]
        public string Email { get; set; }
    }
}
