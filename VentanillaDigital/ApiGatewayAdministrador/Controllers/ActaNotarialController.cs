using ApiGateway.Contratos.Models.ActaNotarial;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Models;
using ApiGateway.Models.Transaccional;
using ApiGatewayAdministrador.Enums;
using ApiGatewayAdministrador.Helper;
using GenericExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActaNotarialController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public ActaNotarialController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpGet]
        [Route("ObtenerActaNotarial/{tramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ActaNotarialModel>> ObtenerActaNotarial(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerActaNotarial/{tramiteId}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ActaNotarialModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}