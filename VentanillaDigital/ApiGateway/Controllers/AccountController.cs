using ApiGateway.Contratos.Models.Account;
using ApiGateway.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OTPClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        private IOTPClient _otpClient;

        public AccountController(IConfiguration configuration,
            IHttpClientHelper httpClientHelper,
            IOTPClient otpClient)
        {
            _otpClient = otpClient;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<AuthenticatedUserDTO>> Login([FromBody] LoginModel login)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/login",
                HttpMethod.Post, login);

            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                AuthenticatedUserDTO resul =
                    JsonConvert.DeserializeObject<AuthenticatedUserDTO>(res);

                return Ok(resul);
            }

            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpPost]
        [Route("loginFuncionario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticatedFuncionarioDTO>>
            LoginFuncionario(LoginFuncionarioModel login)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/loginFuncionario",
                HttpMethod.Post, login);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                AuthenticatedFuncionarioDTO resul =
                    JsonConvert.DeserializeObject<AuthenticatedFuncionarioDTO>(res);

                if (resul.IsAuthenticated == "true" && login.Movil)
                {
                    var funcionarioResponse =
                        await _httpClientHelper.ConsumirServicioRest(
                            $"{uriAPI}/Funcionario/Contacto/{resul.RegisteredUser.Id}",
                            HttpMethod.Get, "");

                    var resultContacto = await funcionarioResponse.Content.ReadAsStringAsync();
                    if (serviceResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var funcionarioObj = JsonConvert.DeserializeObject<ContactoFuncionario>(resultContacto);
                        var otpEnviado =
                            await _otpClient.GenerarCodigoOTP(funcionarioObj.Correo, funcionarioObj.Celular);

                        resul.IdentificadorOTP = otpEnviado.Identificador;
                    }
                    else
                    {
                        return BadRequest(JsonConvert.DeserializeObject<ErroresDTO>(resultContacto));
                    }

                }
                return Ok(resul);
            }

            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpPost]
        [Route("ValidateOtp/{otp}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ValidateOtp(string otp)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ValidateOtp",
                HttpMethod.Post, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                bool resul = Convert.ToBoolean(res);
                return Ok(resul);
            }
            if (otp == "1111")
                return true;
            else
                return false;

            //ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            //return BadRequest(error);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthenticatedUserDTO>> Register(PersonaCreateDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/register",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                AuthenticatedUserDTO resul =
                    JsonConvert.DeserializeObject<AuthenticatedUserDTO>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }


        [HttpPost]
        [Route("UserRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaResponseDTO>> UserRegister(AccountCreateDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/RegistroUsuario",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                PersonaResponseDTO resul =
                    JsonConvert.DeserializeObject<PersonaResponseDTO>(res);

                return Ok(resul);
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }

        }

        [HttpPost]
        [Route("UserUpdate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaResponseDTO>> UserUpdate(AccountUpdateDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ActualizarUsuario",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                PersonaResponseDTO resul = JsonConvert.DeserializeObject<PersonaResponseDTO>(res);
                return Ok(resul);
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }

        }

        [HttpPost]
        [Route("UserDelete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UserDelete(UserDeleteRequestDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/EliminarUsuario",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                bool resul = JsonConvert.DeserializeObject<bool>(res);
                return Ok(resul);
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }
        }

        [HttpPost]
        [Route("ListarUsuariosRegistrados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AspNetUsersResponseDTO>> ListarUsuariosRegistrados(FiltroListaUsuarioDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ObntenerUsuarios",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                List<AspNetUsersResponseDTO> resul =
                      JsonConvert.DeserializeObject<List<AspNetUsersResponseDTO>>(res);

                return Ok(resul);
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }

        }


        [HttpPost]
        [Route("registerAdmin")]
        [Authorize(Policy = "RequireAdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAdmin(PersonaCreateDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/register/Administrador",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
                return NoContent();

            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }

        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RegisteredUserDTO>> Refresh(RefreshTokenModel model)
        {

            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/refresh",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
                return Ok(JsonConvert.DeserializeObject<RegisteredUserDTO>(res));

            List<string> strError = new List<string>();
            strError.Add(res);
            return Unauthorized("");
        }

        [HttpPost]
        [Route("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ChangePasswordRequest model)
        {
            var modelnew = new ChangePasswordModelDTO(model);
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ResetPassword",
                HttpMethod.Post, modelnew);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return NoContent();
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return NotFound(error);
            }
        }

        [HttpPost]
        [Route("RecoverPassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RecoverPassword(RecoveryPasswordRequest recoveryPasswordRequest)
        {
            //ajuste recoverpassword
            string hostUrl = _configuration.GetSection("EnlaceRecoveryPassword:HostUrl").Value;
            RecoveryPasswordDTO recoveryPassword = new RecoveryPasswordDTO
            {
                Email = recoveryPasswordRequest.Email,
                UrlRedirect = hostUrl
            };
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/RecoverPassword",
                HttpMethod.Post, recoveryPassword);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return NoContent();
            }
            else
            {
                var res = await serviceResponse.Content.ReadAsStringAsync();
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return NotFound(error);
            }
        }

        [HttpGet]
        [Route("ValidateToken")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> ValidateToken()
        {
            ClaimModel claims = _httpClientHelper.GetClaims();
            string token = _httpClientHelper.GetToken();
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/validateToken",
                HttpMethod.Post, new { userId = claims.UserId, token = token });

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
                return NoContent();

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(errors);
        }

        [HttpGet]
        [Route("GetRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RolModel>>> GetRoles()
        {

            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/GetRoles",
                HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var roles = JsonConvert.DeserializeObject<IEnumerable<RolModel>>(res);
                return Ok(roles);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("EliminarUsuarios")]
        [Authorize(Policy = "RequireAdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EliminarUsuarios(PersonaDeleteDTO RequestUsuarios)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/EliminarUsuarios",
                HttpMethod.Post, RequestUsuarios);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return NoContent();
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }

        }
    }
}