using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models
{
    public class RegisterModel:Aplicacion.ContextoPrincipal.Modelo.ModeloDTO
    {
        public int NotariaId { get; set; }
        public string Email { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string Password { get; set; }
        public string RolName { get; set; }
   
    }
}
