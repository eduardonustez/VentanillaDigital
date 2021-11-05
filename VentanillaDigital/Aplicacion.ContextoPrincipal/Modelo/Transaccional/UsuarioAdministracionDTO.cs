using System;
using System.Collections.Generic;
using System.Text;

namespace Aplicacion.ContextoPrincipal.Modelo.Transaccional
{
    public class UsuarioAdministracionDTO
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Rol { get; set; }
    }
}
