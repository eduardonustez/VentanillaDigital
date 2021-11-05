using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using ApiGateway.Models;
using ApiGatewayAdministrador.Helper;
using Infraestructura.Transversal.Models;
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

namespace ApiGatewayAdministrador.Controllers
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

        [HttpPost("ObtenerUsuariosPaginado")]
        public async Task<ActionResult<PaginableResponse<UsuarioModel>>> ObtenerUsuariosPaginado(DefinicionFiltro definicionFiltro)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ObtenerUsuariosPaginado",
                HttpMethod.Post, definicionFiltro);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<PaginableResponse<UsuarioModel>>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        //[HttpPost("ObtenerUsuariosPortalAdminPaginado")]
        //public async Task<ActionResult<PaginableResponse<UsuarioPortalAdminModel>>> ObtenerUsuariosPortalAdminPaginado(DefinicionFiltro definicionFiltro)
        //{
        //    var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ObtenerUsuariosPortalAdminPaginado",
        //        HttpMethod.Post, definicionFiltro);
        //    var res = await serviceResponse.Content.ReadAsStringAsync();
        //    if (serviceResponse.StatusCode == HttpStatusCode.OK)
        //    {
        //        var resul = JsonConvert.DeserializeObject<PaginableResponse<UsuarioPortalAdminModel>>(res);
        //        return Ok(resul);
        //    }

        //    var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
        //    return BadRequest(errors);
        //}

        [HttpPost]
        [Route("UserRegister")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaResponseDTO>> UserRegister(AccountCreateDTO model)
        {
            Console.WriteLine($"Notaria ID {model.NotariaId}");

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
        [Route("loginAdministracion")]
        public async Task<ActionResult<AuthenticatedFuncionarioDTO>>
            LoginAdministracion(LoginAdministracionModel login)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/loginAdministracion",
                HttpMethod.Post, login);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<AuthenticatedAdministracionDTO>(res);
                return Ok(resul);
            }

            var error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpPost]
        [Route("refresh")]
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
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ResetPasswordAdministracion",
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
            string hostUrl = _configuration.GetSection("EnlaceRecoveryPassword:HostUrl").Value;
            RecoveryPasswordDTO recoveryPassword = new RecoveryPasswordDTO
            {
                Email = recoveryPasswordRequest.Email,
                UrlRedirect = hostUrl
            };
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/RecoverPasswordAdministracion",
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

        [HttpPost]
        [Route("ObtenerUsuarioNotariaPorId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AspNetUsersResponseDTO>> ObtenerUsuarioNotariaPorId(FiltroListaUsuarioDTO model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/ObtenerUsuarioNotariaPorId",
                HttpMethod.Post, model);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                AspNetUsersResponseDTO resul =
                      JsonConvert.DeserializeObject<AspNetUsersResponseDTO>(res);

                return Ok(resul);
            }
            else
            {
                ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(error);
            }

        }

    }
}