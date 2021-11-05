using Aplicacion.ContextoPrincipal.Modelo;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiGateway.Models
{
    public class ChangePasswordModel
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }  
}
