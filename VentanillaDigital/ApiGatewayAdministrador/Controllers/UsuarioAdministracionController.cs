using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using ApiGateway.Models;
using ApiGatewayAdministrador.Helper;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioAdministracionController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public UsuarioAdministracionController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpPost("ObtenerPaginado")]
        public async Task<ActionResult<PaginableResponse<UsuarioAdministracionModel>>> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro definicionFiltro)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerUsuariosAdministracionPaginado",
                HttpMethod.Post, definicionFiltro);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<PaginableResponse<UsuarioAdministracionModel>>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public async Task<ActionResult<UsuarioCreacionPortalAdminResponseDTO>> CrearUsuario(CrearUsuarioAdministracionModel request)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/CrearUsuario",
                HttpMethod.Post, request);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                UsuarioCreacionPortalAdminResponseDTO result = new UsuarioCreacionPortalAdminResponseDTO { Error = false };
                return Ok(result);
            }

            var error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpGet]
        [Route("ObtenerUsuarioPortalAdmin/{UsuarioAdminId}")]
        public async Task<ActionResult<UsuarioPortalAdminResponseDTO>> ObtenerUsuarioPortalAdmin(long UsuarioAdminId)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ObtenerUsuarioAdmin/{UsuarioAdminId}",
                HttpMethod.Get, string.Empty);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<UsuarioPortalAdminResponseDTO>(res);
                return Ok(result);
            }

            var error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpPost]
        [Route("ActualizarUsuarioAdmin")]
        public async Task<ActionResult<UsuarioCreacionPortalAdminResponseDTO>> ActualizarUsuarioAdmin(ActualizarUsuarioAdministracionModel request)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/ActualizarUsuario",
                HttpMethod.Post, request);

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                UsuarioCreacionPortalAdminResponseDTO result = new UsuarioCreacionPortalAdminResponseDTO { Error = false };
                return Ok(result);
            }

            var error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

        [HttpPost]
        [Route("EliminarUsuarioAdmin")]
        public async Task<ActionResult<bool>> EliminarUsuarioAdmin(EliminarUsuarioAdministracionModel model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/EliminarUsuarioAdmin",
                HttpMethod.Post, model);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(true);
            }
            else
            {                
                return BadRequest(false);
            }
        }

        [HttpPost]
        [Route("NotificacionPwdUsuarioPortalAdmin")]
        public async Task<ActionResult<bool>> NotificacionPwdUsuarioPortalAdmin(NotificacionPasswordUsuarioPortalAdminModel model)
        {
            NotificacionPasswordUsuarioPortalAdminRequest asignacionClave = new NotificacionPasswordUsuarioPortalAdminRequest
            {
                Email = model.Email
                , Url = _configuration.GetSection("EnlaceAsignacionClavePortalAdmin:HostUrl").Value
            };

            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/api/Administracion/NotificacionPwdUsuarioPortalAdmin",
                HttpMethod.Post, asignacionClave);

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(false);
            }
        }

        [HttpPost]
        [Route("AsignarPasswordAdministracion")]
        public async Task<ActionResult<string>> AsignarPasswordAdministracion(AsignarClaveUsuarioAdminRequest model)
        {
            var serviceResponse =
                await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Account/AsignarPasswordAdministracion",
                HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(string.Empty);
            }
            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
    }
}
