using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Infraestructura.Transversal.Log.Modelo;
using ApiGatewayAdministrador.Helper;

namespace ApiGatewayAdministrador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrazabilidadController : ControllerBase
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public TrazabilidadController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost]
        [Route("CrearTraza")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CrearTraza(InformationModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Trazabilidad/CrearTraza",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
