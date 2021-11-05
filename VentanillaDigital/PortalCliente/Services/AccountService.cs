using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Contratos;
using ApiGateway.Models;
using GenericExtensions;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;
using System.Text.Json;
using Infraestructura.Transversal.Log.Modelo;

namespace PortalCliente.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountServiceClient _accountServiceClient;
        private readonly ICustomHttpClient _customHttpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly ITrazabilidadService _trazabilidadService;
        protected AuthenticationStateProvider _authenticationStateProvider;

        public AccountService(IAccountServiceClient accountServiceClient,
            ICustomHttpClient customHttpClient,
            ILocalStorageService localStorageService,
            ITrazabilidadService trazabilidadService,
            AuthenticationStateProvider authentication)
        {
            _accountServiceClient = accountServiceClient;
            _customHttpClient = customHttpClient;
            _localStorageService = localStorageService;
            _trazabilidadService = trazabilidadService;
            _authenticationStateProvider = authentication;
        }

        public async Task<AuthenticatedUser> Login(UserLogin login, bool movil = false)
        {
            var req = login.Adaptar<LoginFuncionarioModel>();
            req.Movil = movil;

            //var res = await _accountServiceClient.LoginFuncionario(req);
            var res = await _customHttpClient.PostJsonAsync<AuthenticatedFuncionarioDTO>($"/api/Account/loginFuncionario", req);
            return res.Adaptar<AuthenticatedUser>();
        }

        public async Task<AuthenticatedUser> Register(User user)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var req = user.Adaptar<PersonaCreateDTO>();

            var res = await _accountServiceClient.Register(req);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"{{\"Usuario\": \"{user?.Email}\"}}"
            };
            await AgregarLog("Register", "PersonaCreateDTO", informacionTraza);

            return res.Adaptar<AuthenticatedUser>();
        }

        public async Task<PersonaResponseDTO> UserRegister(UserAccount user)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var req = user.Adaptar<AccountCreateDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<PersonaResponseDTO>($"/api/Account/UserRegister", req);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"{{\"Usuario\": \"{user?.EmailNotaria}\", \"UserId\": \"{user?.UserId}\"}}"
            };
            await AgregarLog("UserRegister", "UserAccount", informacionTraza);
            return resultado;
        }

        public async Task<PersonaResponseDTO> UserUpdate(UpdateUserAccount user)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var req = user.Adaptar<AccountUpdateDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<PersonaResponseDTO>($"/api/Account/UserUpdate", req);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"{{\"Usuario\": \"{user?.EmailNotaria}\", \"UserId\": \"{user?.UserId}\"}}"
            };
            await AgregarLog("UserUpdate", "UpdateUserAccount", informacionTraza);
            return resultado;

        }
        public async Task<bool> UserDelete(UserDelete user)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var req = user.Adaptar<UserDeleteRequestDTO>();
            var resultado = await _customHttpClient.PostJsonAsync<bool>($"/api/Account/UserDelete", req);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"{{\"Usuario\": \"{user?.Email}\", \"UserId\": \"{user?.Id}\"}}"
            };
            await AgregarLog("UserDelete", "UserDelete", informacionTraza);
            return resultado;
        }

        public async Task<string> RecoveryPassword(string email)
        {
            RecoveryPasswordRequest request = new RecoveryPasswordRequest
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

        private async Task AgregarLog(string metodo, string entidad, InformacionTrazaModel informacionAdicional)
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            InformationModel objTraza = new InformationModel("1",
               "Portal Cliente",
               "AccountService",
               metodo,
               entidad,
               null,
               JsonSerializer.Serialize(informacionAdicional), state.User.Identity.Name);
            await _trazabilidadService.CrearTraza(objTraza);
        }
    }

}
