using ApiGateway.Contratos;
using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using ApiGateway.Models;
using GenericExtensions;
using Infraestructura.Transversal.Models;
using PortalAdministrador.Data;
using PortalAdministrador.Data.Account;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ICustomHttpClient _customHttpClient;
        private readonly ILocalStorageService _localStorageService;


        public AccountService(IAccountServiceClient accountServiceClient,
            ICustomHttpClient customHttpClient,
            ILocalStorageService localStorageService)
        {
            _accountServiceClient = accountServiceClient;
            _customHttpClient = customHttpClient;
            _localStorageService = localStorageService;
        }

        public async Task<AuthenticatedUser> Login(UserLogin login)
        {
            var res = await _customHttpClient
                .PostJsonAsync<AuthenticatedAdministracionDTO>($"/api/Account/loginAdministracion", new LoginAdministracionModel
                {
                    Login = login.Usuario,
                    Password = login.Contrasena
                });

            return res.Adaptar<AuthenticatedUser>();
        }

        public async Task<AuthenticatedUser> Register(User user)
        {
            var req = user.Adaptar<PersonaCreateDTO>();

            var res = await _accountServiceClient.Register(req);

            return res.Adaptar<AuthenticatedUser>();
        }

        public async Task<PaginableResponse<UsuarioModel>> ObtenerUsuariosPaginado(DefinicionFiltro definicionFiltro)
        {
            var resultado = await _customHttpClient.PostJsonAsync<PaginableResponse<UsuarioModel>>($"/api/Account/ObtenerUsuariosPaginado", definicionFiltro);
            return resultado;
        }

        public async Task<PaginableResponse<UsuarioAdministracionModel>> ObtenerUsuariosPortalAdminPaginado(DefinicionFiltro definicionFiltro)
        {
            var resultado = await _customHttpClient.PostJsonAsync<PaginableResponse<UsuarioAdministracionModel>>($"/api/UsuarioAdministracion/ObtenerPaginado", definicionFiltro);
            return resultado;
        }

        public async Task<PersonaResponseDTO> UserRegister(UserAccount user)
        {
            var req = user.Adaptar<AccountCreateDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<PersonaResponseDTO>($"/api/Account/UserRegister", req);
            return resultado;
        }

        public async Task<PersonaResponseDTO> UserUpdate(UserAccount user)
        {
            var req = user.Adaptar<AccountCreateDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<PersonaResponseDTO>($"/api/Account/UserUpdate", req);
            return resultado;

        }
        public async Task<bool> UserDelete(UserDelete user)
        {
            var req = user.Adaptar<UserDeleteRequestDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<bool>($"/api/Account/UserDelete", req);
            return resultado;

        }

        public async Task<string> RecoveryPassword(string email)
        {
            var request = new RecoveryPasswordRequest
            {
                Email = email
            };
            return await _accountServiceClient.RecoveryPassword(request);
        }

        public async Task<string> ResetPassword(string code, string password, string confirmPassword, string email)
        {
            ChangePasswordRequest request = new ChangePasswordRequest
            {
                Code = code,
                Password = password,
                ConfirmPassword = confirmPassword,
                Email = email
            };
            return await _accountServiceClient.ChangePassword(request);
        }

        public async Task<AspNetUsersResponseDTO> ObtenerUsuarioNotariaPorId(Guid usuarioId) {
            FiltroListaUsuarioDTO request = new FiltroListaUsuarioDTO
            {
                UsuarioId = usuarioId
            };
            var resultado = await _accountServiceClient.ObtenerUsuarioNotariaPorId(request);
            return resultado;
        }

        public async Task<List<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(int notariaId)
        {
            FiltroListaUsuarioDTO request = new FiltroListaUsuarioDTO();
            request.NotariaId = notariaId;
            var resultado = await _accountServiceClient.ListarUsuariosRegistrados(request);
            return resultado;
        }

        public async Task<AuthenticatedUser> Login(UserLogin login, Guid identificador, string otp)
        {
            var req = login.Adaptar<LoginOtpModel>();
            req.Identificador = identificador;
            req.Otp = otp;

            var res = await _accountServiceClient.LoginOtp(req);

            return res.Adaptar<AuthenticatedUser>();
        }

        public async Task<bool> DeleteUsers(UserDeleteList request)
        {
            var token = await _localStorageService.GetItem<string>("token").ConfigureAwait(false);
            var req = request.Adaptar<PersonaDeleteDTO>();
            return await _accountServiceClient.DeleteUsers(req, token);
        }
    }
}
