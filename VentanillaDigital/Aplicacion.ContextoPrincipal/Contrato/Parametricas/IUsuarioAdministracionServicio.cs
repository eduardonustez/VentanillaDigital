using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Infraestructura.Transversal.Models;
using System;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Parametricas
{
    public interface IUsuarioAdministracionServicio : IDisposable
    {
        Task<UsuarioAdministracion> ObtenerUsuarioAdministracion(string login);
        Task GuardarUsuarioAsync(UsuarioAdministracion usuario);
        Task<PaginableResponse<UsuarioAdministracionDTO>> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro request);
        Task CrearUsuario(string email, string rol);
        Task ActualizarUsuario(long Id, string email, string rol);
        Task EliminarUsuario(long Id, string email, string rol);
        Task <UsuarioAdministracion> ObtenerUsuarioAdmin(long UsuarioAdministracionId, string Email);
        Task<string> GetTokenAdministracion(long Id, string login, string rol);
        Task NotificarUsuario(string email, string url, string token);

        Task<string> InsertarToken(UsuarioAdministracion usuario, string token, DateTime FechaExpiracion);
        Task<bool> ValidarTokenUsuarioAdministrador(string token, long usuarioAdministradorId);
        Task EliminarTokenUsuarioAdministrador(string token, long usuarioAdministradorId);
    }
}
