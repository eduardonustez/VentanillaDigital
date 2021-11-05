using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGateway.Contratos
{
    public interface IAccountServiceClient
    {
        Task<AuthenticatedFuncionarioDTO> LoginFuncionario(LoginFuncionarioModel login);
        Task<AuthenticatedUserDTO> Register(PersonaCreateDTO model);
        Task<PersonaResponseDTO> UserRegister(AccountCreateDTO model);
        Task<PersonaResponseDTO> UserUpdate(AccountUpdateDTO user);
        Task<List<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(FiltroListaUsuarioDTO notariaId);
        Task<AspNetUsersResponseDTO> ObtenerUsuarioNotariaPorId(FiltroListaUsuarioDTO notariaId);
        Task<AuthenticatedFuncionarioDTO> LoginOtp(LoginOtpModel login);
        Task<string> RecoveryPassword(RecoveryPasswordRequest request);
        Task<string> ChangePassword(ChangePasswordRequest request);
        Task<bool> DeleteUsers(PersonaDeleteDTO request, string token);
    }
}