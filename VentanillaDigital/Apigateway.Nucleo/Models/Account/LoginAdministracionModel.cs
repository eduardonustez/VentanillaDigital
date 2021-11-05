namespace ApiGateway.Contratos.Models.Account
{
    public class LoginAdministracionModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class CrearUsuarioAdministracionModel
    {
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class ActualizarUsuarioAdministracionModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class EliminarUsuarioAdministracionModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }

    public class NotificacionPasswordUsuarioPortalAdminModel
    {
        public string Email { get; set; }
    }

    public class NotificacionPasswordUsuarioPortalAdminRequest
    {
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
