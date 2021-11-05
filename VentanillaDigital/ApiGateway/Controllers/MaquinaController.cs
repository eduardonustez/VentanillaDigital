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
using ApiGateway.Contratos.Models.Transaccional;
using Microsoft.AspNetCore.Authorization;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Policy = "RequireAdminOnly")]
    public class MaquinaController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public MaquinaController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpPost]
        [Route("CrearMaquina")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<MaquinaReturn>> CrearMaquina(NuevoMaquinaModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Maquina/CrearMaquina",
                HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                MaquinaReturn resul = JsonConvert.DeserializeObject<MaquinaReturn>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }
        [HttpGet]
        [Route("ConsultarMaquina/{mac}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        [Authorize]
        public async Task<ActionResult<MaquinaConfiguracionReturn>> ConsultarMaquina(string mac)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Maquina/ConsultarMaquina/{mac}",
                HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                MaquinaConfiguracionReturn resul = JsonConvert.DeserializeObject<MaquinaConfiguracionReturn>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }


    }
}