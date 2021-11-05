using ApiGateway.Helper;
using ApiGateway.Models;
using GenericExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OTPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OTPController: ControllerBase
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        private readonly IOTPClient _otpClient;
        public OTPController(IConfiguration configuration, 
            IHttpClientHelper httpClientHelper,
            IOTPClient otpClient)
        {
            _otpClient = otpClient;
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpPost]
        [Route("Validar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthenticatedFuncionarioDTO>> ValidarOtp(LoginOtpModel login)
        {
            var otpValidado = await _otpClient.ValidacionOTP(login.Identificador, login.Otp);

            if(otpValidado.EsValidoOTP)
            {
                var loginFuncionario = login.Adaptar<LoginFuncionarioModel>();
                var serviceResponse =
                    await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/loginFuncionario",
                    HttpMethod.Post, loginFuncionario);

                var res = await serviceResponse.Content.ReadAsStringAsync();

                if (serviceResponse.StatusCode == HttpStatusCode.OK)
                {
                    AuthenticatedFuncionarioDTO resul =
                        JsonConvert.DeserializeObject<AuthenticatedFuncionarioDTO>(res);

                    return Ok(resul);
                }
                ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
                return BadRequest(errors);
            }
            return Unauthorized();
        }
    }
}
