using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayAdministrador.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net;
using ApiGateway.Models;
using Newtonsoft.Json;
using System.IO;
using ApiGateway.Contratos;

namespace ApiGatewayAdministrador.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public ConsultaController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpGet]
        [Route("{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ObtenerActaNotarialPublico(string codigo)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/ObtenerActaNotarialPublico/" + codigo,
             HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var bytePdf = Convert.FromBase64String(res);

                    Stream stream = new MemoryStream(bytePdf);
                    string mimeType = "application/pdf";
                    return new FileStreamResult(stream, mimeType)
                    {
                        FileDownloadName = "ActaNotarial.pdf"
                    };
                }
                catch
                {
                    return NotFound();
                }
                
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet]
        [Route("ObtenerActa/{notariaId}/{fechaTramite}/{tramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<string>> ObtenerActa(string notariaId, string fechaTramite, string tramiteId)
        {
            ObtenerActaNotarialSeguraRequest req = new ObtenerActaNotarialSeguraRequest()
            {
                NotariaId = Convert.ToInt32(notariaId),
                FechaTramite = Convert.ToDateTime(fechaTramite),
                TramiteId = tramiteId,
            };

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/ObtenerActaNotarialSegura/",
             HttpMethod.Get, req);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return res;
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
