using Dominio.ContextoPrincipal.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo
{
    public class AuthenticatedUserDTO 
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
        public RegisteredUserDTO RegisteredUser { get; set; }

    }



}