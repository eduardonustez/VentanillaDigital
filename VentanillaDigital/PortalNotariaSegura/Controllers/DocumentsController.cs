using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notariado.Helper;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Notariado.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger<DocumentsController> _logger;
        private IHttpHelper _httpHelper;
        private IConfiguration _configuration;

        public DocumentsController(ILogger<DocumentsController> logger, IHttpHelper httpHelper, IConfiguration configuration)
        {
            _logger = logger;
            _httpHelper = httpHelper;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetActaNotarial/{notariaId}/{fechaTramite}/{email}/{tramiteId}")]
        public async Task<ActionResult<string>> GetActaNotarial(string notariaId,string fechaTramite,string email,string tramiteId)
        {
            var serviceResponse = await _httpHelper.ConsumirServicioRest(_configuration.GetValue<string>("urlApiVentanilla") + $"Consulta/ObtenerActa/{notariaId}/{fechaTramite}/{email}/{tramiteId}", HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return res;
            }

            return "Acta no encontrada";
        }

        [HttpGet]
        [Route("obtenernotarias")]
        public async Task<ActionResult<List<Notaria>>> ObtenerNotarias()
        {
            var serviceResponse = await _httpHelper.ConsumirServicioRest(_configuration.GetValue<string>("urlApiVentanilla") + $"consulta/obtenernotarias", HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notaria>>(res);
            }

            return null;
        }
    }
}