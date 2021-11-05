using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ApiGatewayAdministrador.Helper;
using System.Net.Http;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Log.Implementacion;
using Microsoft.AspNetCore.Authorization;
using ApiGateway.Models.Transaccional;
using ApiGatewayAdministrador.Enums;
using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;

namespace ApiGatewayAdministrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotarioController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public NotarioController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpPost]
        [Route("ObtenerPendientesAutorizacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitePendienteAutorizacionModel>> ObtenerPendientesAutorizacion(DefinicionFiltro model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerPendientesAutorizacion",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramitePendienteAutorizacionModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }


        [HttpPost]
        [Route("ActualizarGrafoPinNotario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<bool>> ActualizarGrafoPinNotario(NotarioDTOModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ActualizarGrafoPinNotario",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                //var resul = JsonConvert.DeserializeObject<NotarioDTOResponse>(res);
                return Ok(true);
            }

            //ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            //return BadRequest(errors);
            return Ok(false);
        }
        [HttpPost]
        [Route("SeleccionarFormatoImpresion/{UsarSticker}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<bool>> SeleccionarFormatoImpresion(bool UsarSticker)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/SeleccionarFormatoImpresion" ,
             HttpMethod.Post, new { UsarSticker=UsarSticker});
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {

                return Ok(true);
            }

            //ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            //return BadRequest(errors);
            return Ok(false);
        }

        [HttpGet]
        [Route("ObtenerEstadoPinFirma/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<EstadoPinFirmaModel>> ObtenerEstadoPinFirma(string email)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerEstadoPinFirma/" + email,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<EstadoPinFirmaModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
        [HttpGet]
        [Route("ObtenerOpcionesConfiguracion/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<OpcionesConfiguracioNotarioModel>> ObtenerOpcionesConfiguracion(string email)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerOpcionesConfiguracion/" + email,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<OpcionesConfiguracioNotarioModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerGrafo/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<string>> ObtenerGrafo(string email)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notario/ObtenerGrafo/{email}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(res);
            }
            else if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return NotFound();
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("ObtenerNotariosNotaria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ObtenerNotariosNotaria()
        {
            ClaimModel claimModel = _httpClientHelper.GetClaims();

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notario/ObtenerNotariosNotaria/{claimModel.NotariaId}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var notariosNotaria= JsonConvert.DeserializeObject<List<NotarioReturnDTO>>(res);
                return Ok(notariosNotaria);
            }
            
            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("SeleccionarNotarioNotaria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notario/SeleccionarNotarioNotaria",
             HttpMethod.Post, notarioNotariaDTO);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(res);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}