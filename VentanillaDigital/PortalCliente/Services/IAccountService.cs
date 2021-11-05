using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Models;
using PortalCliente.Data;
using PortalCliente.Data.Account;

namespace PortalCliente.Services
{
    public interface IAccountService
    {
        Task<AuthenticatedUser> Login(UserLogin login, bool movil = false);
        Task<AuthenticatedUser> Login(UserLogin login, Guid identificador, string otp);
        Task<PersonaResponseDTO> UserUpdate(UpdateUserAccount user);
        Task<AuthenticatedUser> Register(User user);
        Task<PersonaResponseDTO> UserRegister(UserAccount user);
        Task<bool> UserDelete(UserDelete user);
        Task<List<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(int notariaId);
        Task<string> RecoveryPassword(string email);
        Task<string> ResetPassword(string code, string password, string confirmPassword, string email);
        Task<bool> DeleteUsers(UserDeleteList usuarios);
    }
}
