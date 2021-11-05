using Aplicacion.ContextoPrincipal.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models.Account
{
    public class EditarUsuarioModel : RegistroUsuarioModel
    {        
        public string Id { get; set; }
        public string EmailAnterior { get; set; }

    }
}
