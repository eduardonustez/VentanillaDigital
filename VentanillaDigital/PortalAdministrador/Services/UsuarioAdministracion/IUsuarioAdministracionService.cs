using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using Infraestructura.Transversal.Models;
using PortalAdministrador.Data.Account;
using System.Threading.Tasks;

namespace PortalAdministracion.Services.UsuarioAdministracion
{
    public interface IUsuarioAdministracionService
    {
        Task<PaginableResponse<UsuarioAdministracionModel>> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro definicionFiltro);
        Task<UsuarioCreacionPortalAdminResponseDTO> UsuarioRegistroPortalAdmin(UsuarioPortalAdmin user);
        Task<UsuarioPortalAdminResponseDTO> ObtenerUsuarioPortalAdmin(long UsuarioAdminId);
        Task<UsuarioCreacionPortalAdminResponseDTO> ActualizarUsuarioPortalAdmin(UsuarioPortalAdmin user);
        Task<bool> EliminarUsuarioPortalAdmin(EliminarUsuarioPortalAdmin eliminarUsuarioPortal);

        Task<bool> NotificacionPwdUsuarioPortalAdmin(string email);

        Task<string> AsignarClaveUsuarioPortalAdmin(string token, string clave, string confirmacionClave, string email);
    }
}
