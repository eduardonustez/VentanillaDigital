using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiGateway.Helper;
using ApiGateway.Models;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ApiGateway.Models.Transaccional;
using System.Net;
using ApiGateway.Contratos.Models.Archivos;

namespace ApiGateway.Controllers
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

        [HttpPost]
        [Route("CrearCompareciente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        
        public async Task<ActionResult<TramiteReturn>> CrearCompareciente(ComparecienteCreateRequest comparecienteCreate)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Compareciente/CrearCompareciente", HttpMethod.Post, comparecienteCreate);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                TramiteReturn result = JsonConvert.DeserializeObject<TramiteReturn>(res);
                serviceResponse = 
                    await _httpClientHelper
                        .ConsumirServicioRest($"{uriAPI}/Tramite/ActualizarEstadoTramite/{comparecienteCreate.TramiteId}", 
                        HttpMethod.Post, "");
                res = await serviceResponse.Content.ReadAsStringAsync();
                if(serviceResponse.StatusCode == HttpStatusCode.OK)
                    return Ok(result);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }   
        
     
        [HttpGet]
        [Route("ObtenerPorTramiteId/{TramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramiteReturn>> ObtenerPorTramiteId(long TramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Compareciente/ObtenerPorTramiteId/" + TramiteId,
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
