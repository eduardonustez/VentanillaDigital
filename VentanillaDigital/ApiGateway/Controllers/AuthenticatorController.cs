using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ApiGateway.Helper;
using System.Net.Http;
using ApiGateway.Models.Transaccional;
using ApiGateway.Enums;
using GenericExtensions;
using System;


namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticatorController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;

        public AuthenticatorController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost]
        [Route("VerificarUsuarioOTP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<Boolean>> verificarUsuarioOTP(string usr, string psw)
        {

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Authenticator/VerificarUsuarioOTP",
                HttpMethod.Post, usr);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                Boolean resul = JsonConvert.DeserializeObject<Boolean>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);

        }

        [HttpPost]
        [Route("CrearUserToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<ResponseDTO>> createUserToken(string userLogin)
        {

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Authenticator/CreateUserToken",
                HttpMethod.Post, userLogin);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                ResponseDTO resul = JsonConvert.DeserializeObject<ResponseDTO>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);

        }

        [HttpPost]
        [Route("AutenticarOtp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<ResponseDTO>> autenticarOtp(OtpAuthenticator authModel)
        {

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Authenticator/AutenticarOtp",
                HttpMethod.Post, authModel);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                ResponseDTO resul = JsonConvert.DeserializeObject<ResponseDTO>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);

        }

    }
}
