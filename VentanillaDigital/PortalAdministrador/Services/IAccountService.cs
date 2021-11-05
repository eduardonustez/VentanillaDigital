using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;

namespace PortalAdministrador.Services
{
    public interface IAccountService
    {
        Task<AuthenticatedUser> Login(UserLogin login);
        Task<AuthenticatedUser> Login(UserLogin login, Guid identificador, string otp);
        Task<PersonaResponseDTO> UserUpdate(UserAccount user);
        Task<AuthenticatedUser> Register(User user);
        Task<PersonaResponseDTO> UserRegister(UserAccount user);
        Task<bool> UserDelete(UserDelete user);
        Task<List<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(int notariaId);
        Task<AspNetUsersResponseDTO> ObtenerUsuarioNotariaPorId(Guid usuarioId);
        Task<string> RecoveryPassword(string email);
        Task<string> ResetPassword(string code, string password, string confirmPassword, string email);
        Task<bool> DeleteUsers(UserDeleteList usuarios);
        Task<PaginableResponse<UsuarioModel>> ObtenerUsuariosPaginado(DefinicionFiltro definicionFiltro);
    }
}
