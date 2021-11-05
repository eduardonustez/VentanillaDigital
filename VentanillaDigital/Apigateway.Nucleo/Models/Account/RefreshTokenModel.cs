using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class RefreshTokenModel
    {
        //[Required]
        //public String Token { get; set; }

        [Required]
     
        public String RefreshToken { get; set; }

    }
}
