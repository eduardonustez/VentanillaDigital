using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Archivos;
using ApiGateway.Models;
using ApiGateway.Models.Transaccional;
using ApiGatewayAdministrador.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComparecienteController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;

        public ComparecienteController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpGet]
        [Route("ObtenerTodoPorTramiteId/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DatosComparecientesModel>>> ObtenerPorTramiteId(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerComparecientesPorTramiteId/{TramiteId}",
                HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<DatosComparecientesModel>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerFotoId/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Foto>> ObtenerFotoId(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Compareciente/ObtenerFotoId/" + TramiteId,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ComparecienteCreateRequest>(res);
                return Ok(resul.Foto);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerDocumentoId/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Foto>> ObtenerDocumentoId(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Compareciente/ObtenerDocumentoId/" + TramiteId,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ComparecienteCreateRequest>(res);
                return Ok(resul.ImagenDocumento);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerFirmaId/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Foto>> ObtenerFirmaId(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Compareciente/ObteneFirmarId/" + TramiteId,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ComparecienteCreateRequest>(res);
                return Ok(resul.Firma);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
