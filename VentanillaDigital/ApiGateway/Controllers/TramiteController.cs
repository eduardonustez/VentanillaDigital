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
    
    public class TramiteController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public TramiteController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpPost]
        [Route("CrearTramite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<TramiteReturn>> CrearTramite(TramiteModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Tramite/CrearTramite",
                HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                TramiteReturn resul = JsonConvert.DeserializeObject<TramiteReturn>(res);

                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }

        [HttpPost]
        [Route("ActualizarTramite")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult> ActualizarTramite(TramiteModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Tramite/ActualizarTramite",
                HttpMethod.Post, model);
            if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
            }
            var res = await serviceResponse.Content.ReadAsStringAsync();
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }

        [HttpGet]
        [Route("ObtenerTramite/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramiteReturn>> ObtenerTramite(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Tramite/ObtenerTramite/" + TramiteId,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteReturn>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
        [HttpPost]
        [Route("ValidarPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<PersonaInfoReturnDTO>> ValidarPersona(PersonaIdentificacionDTO persona)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Tramite/ValidarPersona",
                HttpMethod.Post, persona);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<PersonaInfoReturnDTO>(res);

                return Ok(resul);
            }
            if (serviceResponse.StatusCode == HttpStatusCode.NotFound)
                return NotFound();
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpGet]
        [Route("EscanearDocumento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<PersonaInfoReturnDTO>> EscanearDocumento()
        {
            var InfoEscaner = new PersonaInfoReturnDTO()
            {
                TipoIdentificacion = 1,
                NumeroIdentificacion = "1010209749",
                Nombres = "Jaime",
                Apellidos = "Silva"
            };

            return Ok(InfoEscaner);
        }

        [HttpGet]
        [Route("ValidarPersona/{tipoDocumento}/{documento}")]
        public async Task<ActionResult> ValidarPersona(int tipoDocumento, string documento)
        {
            var inputModel = new 
            {
                Documento = documento,
                TipoDocumento = tipoDocumento,
                CodigoAplicacion = "ba545bac-d281-4a93-b23c-28136bd970a5"
            };
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest("https://10.130.1.21:6311" + "/api/ValidacionAni",
              HttpMethod.Post, inputModel);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
               return Ok();
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest();
        }

        [HttpPost]
        [Route("AutorizarTramite")]
        [Authorize(Policy = "RequireNotario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult> AutorizacionTramite(TramiteAutorizacionRequest autorizacionRequest)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Tramite/AutorizacionTramite",
                HttpMethod.Post, autorizacionRequest);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok();
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }

    }
}