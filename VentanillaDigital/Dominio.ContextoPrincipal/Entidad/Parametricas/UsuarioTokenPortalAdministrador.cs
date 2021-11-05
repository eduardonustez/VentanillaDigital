using Dominio.Nucleo.Entidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class UsuarioTokenPortalAdministrador : EntidadBase
    {
        public long UsuarioTokenPortalAdministradorId { get; set; }
        public long UsuarioAdministracionId { get; set; }
        public string LoginProvider { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}
