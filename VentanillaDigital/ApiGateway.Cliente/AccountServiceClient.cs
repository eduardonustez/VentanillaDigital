using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiGateway.Contratos;
using ApiGateway.Models;


namespace ApiGateway.Cliente
{
    public class AccountServiceClient : IAccountServiceClient
    {
        private readonly HttpClient _httpClient;
        public AccountServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthenticatedFuncionarioDTO> LoginFuncionario(LoginFuncionarioModel login)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/loginFuncionario", login);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<AuthenticatedFuncionarioDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<AuthenticatedUserDTO> Register(PersonaCreateDTO user)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/register", user);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<AuthenticatedUserDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<PersonaResponseDTO> UserRegister(AccountCreateDTO user)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/UserRegister", user);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<PersonaResponseDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<PersonaResponseDTO> UserUpdate(AccountUpdateDTO user)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/UserUpdate", user);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<PersonaResponseDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<List<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(FiltroListaUsuarioDTO notariaId)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/ListarUsuariosRegistrados", notariaId);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<List<AspNetUsersResponseDTO>>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<AspNetUsersResponseDTO> ObtenerUsuarioNotariaPorId(FiltroListaUsuarioDTO notariaId)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/ObtenerUsuarioNotariaPorId", notariaId);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<AspNetUsersResponseDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<AuthenticatedFuncionarioDTO> LoginOtp(LoginOtpModel loginOtp)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"OTP/Validar", loginOtp);

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return await httpResponse.Content.ReadFromJsonAsync<AuthenticatedFuncionarioDTO>();
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<string> RecoveryPassword(RecoveryPasswordRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/RecoverPassword", request).ConfigureAwait(false);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return string.Empty;
                case HttpStatusCode.BadRequest:
                    var errorValidation = await httpResponse.Content.ReadFromJsonAsync<InputErrorDTO>();
                    return errorValidation.errors.FirstOrDefault().Message;
                case HttpStatusCode.NotFound:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    return error.Errors[0];
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<string> ChangePassword(ChangePasswordRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/ResetPassword", request).ConfigureAwait(false);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return string.Empty;
                case HttpStatusCode.NotFound:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    return error.Errors[0];
                case HttpStatusCode.BadRequest:
                    Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
                    var errorValidation = await httpResponse.Content.ReadFromJsonAsync<InputErrorDTO>();
                    return errorValidation.errors.FirstOrDefault().Message;
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<bool> DeleteUsers(PersonaDeleteDTO request, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponse = await _httpClient.PostAsJsonAsync("Account/EliminarUsuarios", request).ConfigureAwait(false);
            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.NoContent:
                    return true;
                case HttpStatusCode.BadRequest:
                    var error = await httpResponse.Content.ReadFromJsonAsync<ErroresDTO>();
                    throw new ApplicationException(string.Join('\n', error.Errors));
                default:
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }
    }

}
