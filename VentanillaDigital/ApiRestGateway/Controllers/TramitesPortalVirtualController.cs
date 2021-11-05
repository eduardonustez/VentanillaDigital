using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiRestGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TramitesPortalVirtualController : ControllerBase
    {
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public string uriAPI;

        public TramitesPortalVirtualController(IConfiguration configuration,
            IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpGet("ConsultarArchivoTramiteVirtualPorId/{archivoId}/{download}/{filename}")]
        public async Task<ActionResult<string>> ConsultarArchivoTramiteVirtualPorId1(long archivoId, bool download, string filename)
        {
            var token = HttpContext.Request.Query["token"];

            if (string.IsNullOrEmpty(token))
            {
                var err = new ErroresDTO { Errors = new[] { "Invalid Token" } };
                return BadRequest(err);
            }

            _httpClientHelper.ValidateToken(token);

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(
                $"{uriAPI}/RegistrarCiudadano/ConsultarArchivoTramiteVirtualPorId/{archivoId}?token={token}",
                HttpMethod.Get,
                ""
            );

            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var resul = JsonConvert.DeserializeObject<ArchivoTramiteVirtual>(res);

                    Stream stream = new MemoryStream(resul.FileBytes);
                    string mimeType = "application/pdf";

                    return File(resul.FileBytes, mimeType, download ? resul.Nombre.Contains(".pdf") ? resul.Nombre : $"{resul.Nombre}.pdf" : "");
                }
                catch
                {
                    return NotFound();
                }

            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
