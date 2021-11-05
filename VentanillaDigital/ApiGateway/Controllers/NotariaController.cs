﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models;
using ApiGateway.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotariaController : ControllerBase
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public NotariaController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpGet]
        [Route("ConfiguracionRNEC/{NotariaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ConfiguracionRNEC>> ObtenerConfiguracionRNEC(long notariaId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notaria/ConfiguracionRNEC/{notariaId}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ConfiguracionRNEC>(res);
                return Ok(resul);
            }
            else if(serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return NotFound();
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerNotariaPorId/{NotariaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<DespachoNotaria>> ObtenerNotariaPorId(long notariaId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notaria/ObtenerNotariaPorId/{notariaId}",
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<DespachoNotaria>(res);
                return Ok(resul);
            }
            else if (serviceResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return NotFound();
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
