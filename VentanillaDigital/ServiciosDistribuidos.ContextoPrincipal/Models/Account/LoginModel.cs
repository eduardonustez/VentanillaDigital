using Aplicacion.ContextoPrincipal.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models
{
    public class LoginModel:ModeloDTO
    {
        [Required]
        public string NumeroIdentificacion { get; set; }

    }
    public class LoginFuncionarioModel
    {
        public string Usuario { get; set; }
        [JsonIgnore]
        public string Contrasena { get; set; }
        public bool Movil { get; set; }

    }


}
