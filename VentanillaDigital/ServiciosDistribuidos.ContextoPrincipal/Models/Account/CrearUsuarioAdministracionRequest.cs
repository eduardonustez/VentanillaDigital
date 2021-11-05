using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Models.Account
{
    public class CrearUsuarioAdministracionRequest
    {
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class ActualizarUsuarioAdministracionRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class EliminarUsuarioAdministracionRequest
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class NotificarUsuarioAdminRequest
    {
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
