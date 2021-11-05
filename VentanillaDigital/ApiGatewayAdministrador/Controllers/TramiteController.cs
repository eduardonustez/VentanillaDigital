using ApiGateway.Contratos.Models;
using ApiGateway.Models;
using ApiGatewayAdministrador.Helper;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Controllers
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

        [HttpGet]
        [Route("ObtenerTramite/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramiteDetalleModel>> ObtenerTramite(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/api/Administracion/ObtenerTramite/" + TramiteId,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteDetalleModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ObtenerTramitesPorNumeroIdentificacion/{numeroIdentificacion}/{fechaInicio}/{fechaFin}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TramiteInfoBasica>>> ObtenerTramitesPorNumeroIdentificacion(string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerTramitesPorNumeroIdentificacion/{numeroIdentificacion}/{fechaInicio.ToString("yyyy-MM-dd")}/{fechaFin.ToString("yyyy-MM-dd")}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<TramiteInfoBasica>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("ObtenerTramitesPorNumeroIdentificacionPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TramiteInfoBasica>>> ObtenerTramitesPorNumeroIdentificacionPaginado(DefinicionFiltro definicionFiltro)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerTramitesPorNumeroIdentificacionPaginado",
             HttpMethod.Post, definicionFiltro);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<PaginableResponse<TramiteInfoBasica>>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ObtenerHistorialTramite/{tramiteId}")]
        public async Task<ActionResult<IEnumerable<HistorialTramite>>> ObtenerHistorialTramite(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerHistorialTramite/{tramiteId}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<HistorialTramite>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}