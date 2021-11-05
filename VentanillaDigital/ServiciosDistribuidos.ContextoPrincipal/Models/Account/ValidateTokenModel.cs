using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models
{
    public class ValidateTokenModel
    {
        
        public string userId { get; set; }
        public string token { get; set; }
    }
}
