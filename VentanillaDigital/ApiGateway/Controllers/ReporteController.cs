using ApiGateway.Contratos.Models.Reportes;
using ApiGateway.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : Controller
    {
        public string uriAPI;
        private IHttpClientHelper _httpClientHelper;
        private IConfiguration _configuration;

        public ReporteController(IHttpClientHelper httpClientHelper, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClientHelper = httpClientHelper;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
        }

        [HttpPost]
        [Route("ReporteOperacionalDiario")]
        [Authorize(Policy = "RequireNotario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonaResponseDTO>> ReporteOperacionalDiario(ReporteRequest reporte)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Reporte/ReporteOperacionalDiario",
                HttpMethod.Post, reporte);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
