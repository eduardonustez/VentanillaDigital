using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class OtpAuthenticator
    {
        [Required]
        public string usrDocument { get; set; }
        [Required]
        public string pin { get; set; }
    }
}
