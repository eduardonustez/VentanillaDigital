using Dominio.Nucleo.Entidad;

namespace Dominio.ContextoPrincipal.Entidad.Parametricas
{
    public class UsuarioAdministracion : EntidadBase
    {
        public long UsuarioAdministracionId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}
