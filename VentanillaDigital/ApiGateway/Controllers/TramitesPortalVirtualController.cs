using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Contratos.Models.Transaccional;
using ApiGateway.Helper;
using ApiGateway.Models;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TramitesPortalVirtualController : Controller
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

        [HttpPost]
        [Route("RegistrarCiudadano")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> RegistrarCiudadano(TramitePortalVirtualCiudadanoDTO model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/RegistrarCiudadano/RegistrarCiudadano",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                bool.TryParse(res, out bool resultado);
                return Ok(resultado);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("actualizarPago/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> ActualizarPago(long recaudoTramiteVirtualId, [FromBody] ActualizarPagoModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/actualizarPago/{recaudoTramiteVirtualId}",
                HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<bool>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("obtenerTotalPagadoTramite/{tramitePortalVirtualId}")]
        public async Task<IActionResult> ObtenerTotalPagadoTramite(int tramitePortalVirtualId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/obtenerTotalPagadoTramite/{tramitePortalVirtualId}",
              HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<decimal>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("ActualizarTramiteVirtual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramiteVirtualActualizacionResponse>> ActualizarTramiteVirtual(TramiteVirtualActualizacionRequest model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/RegistrarCiudadano/ActualizarTramiteVirtual",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteVirtualActualizacionResponse>(res);
                return Ok(resul);
            }
            return BadRequest(false);
        }

        [HttpPost]
        [Route("ObtenerTramitePortalVirtual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitesVirtualesModel>> ObtenerTramitePortalVirtual(DefinicionFiltro model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/RegistrarCiudadano/ObtenerTramitePortalVirtual",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramitesVirtualesModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("EnviarRecaudo/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> EnviarRecaudo(long recaudoTramiteVirtualId, [FromBody] EnviarRecaudoModel model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/EnviarRecaudo/{recaudoTramiteVirtualId}",
                HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<bool>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("EliminarRecaudo/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> EliminarRecaudo(long recaudoTramiteVirtualId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/EliminarRecaudo/{recaudoTramiteVirtualId}",
                HttpMethod.Delete, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<bool>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }


        [HttpGet("ConsultarMensajesTramiteVirtual/{tramiteId}")]
        public async Task<ActionResult<IEnumerable<TramiteVirtualMensajeModel>>> ConsultarMensajesTramiteVirtual(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/ConsultarMensajesTramiteVirtual/{tramiteId}",
                HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<TramiteVirtualMensajeModel>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("ConsultarPorId")]
        public async Task<ActionResult<TramiteVirtualModel>> ConsultarPorId(ConsultaTramiteRequest consulta)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(
                $"{uriAPI}/RegistrarCiudadano/ConsultarTramiteVirtualPorId/",
                HttpMethod.Post,
                consulta
            );
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteVirtualModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ConsultarArchivoTramiteVirtualPorId2/{archivoId}/{download}")]
        public async Task<ActionResult<string>> ConsultarArchivoTramiteVirtualPorId1(long archivoId, bool download)
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

                    if (download)
                    {
                        return new FileStreamResult(stream, mimeType)
                        {
                            FileDownloadName = resul.Nombre.Contains(".pdf") ? resul.Nombre : $"{resul.Nombre}.pdf"
                        };
                    }

                    return File(resul.FileBytes, mimeType);
                }
                catch
                {
                    return NotFound();
                }

            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ObtenerActosPorTramite/{tramiteId}")]
        public async Task<IActionResult> ObtenerActosPorTramite(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(
                $"{uriAPI}/RegistrarCiudadano/ObtenerActosPorTramite/{tramiteId}",
                HttpMethod.Get,
                ""
            );

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<ActoPorTramiteModel>>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ObtenerActosNotariales")]
        public async Task<IActionResult> ObtenerActosNotariales()
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(
                $"{uriAPI}/RegistrarCiudadano/ObtenerActosNotariales",
                HttpMethod.Get,
                ""
            );

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<ActoNotarialModel>>(res);
                return Ok(resul);
            }

            var errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ConsultarRecaudosTramite/{tramiteId}")]
        public async Task<IActionResult> ConsultarRecaudosTramite(int tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/ConsultarRecaudosTramite/{tramiteId}",
                HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<RecaudoTramiteModel>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("GuardarRecaudo")]
        public async Task<ActionResult<ResponseCrearRecaudo>> GuardarRecaudo([FromBody] CrearRecaudoTramiteVirtualModel body)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/GuardarRecaudo",
                HttpMethod.Post, body);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ResponseCrearRecaudo>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("CambiarEstadoCliente/{tramiteId}")]
        public async Task<ActionResult<TramiteVirtualModel>> CambiarEstadoCliente([FromBody] CambiarEstadoTramiteVirtualClienteModel body, long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/CambiarEstadoCliente/{tramiteId}",
                HttpMethod.Post, body);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteVirtualModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ConsultarUrlSubirArchivosMiFirma")]
        public async Task<ActionResult<UrlMiFirmaModel>> ConsultarUrlSubirArchivosMiFirma()
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/ConsultarUrlSubirArchivosMiFirma", HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<UrlMiFirmaModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost("CambiarEstado/{tramiteId}")]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<TramiteVirtualModel>> CambiarEstado([FromBody] CambiarEstadoTramiteVirtualModel body, long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/CambiarEstado/{tramiteId}", HttpMethod.Post, body);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteVirtualModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpGet("ObtenerTestamento/{tramiteId}/{claveTestamento}")]
        public async Task<ActionResult<TestamentoModel>> ObtenerTestamento(long tramiteId, string claveTestamento)
        {
            if (!HttpContext.User.IsInRole("Administrador")
                    && !HttpContext.User.IsInRole("Notario Encargado"))
                throw new System.Exception("Operación no permitida para el usuario");

            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/RegistrarCiudadano/ObtenerTestamento/{tramiteId}/{claveTestamento}", HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TestamentoModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("ValidarPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<ValidarPersonaResponse>> ValidarPersonaVirtual(ValidarPersonaRequest validarPersonaRequest)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/RegistrarCiudadano/ValidacionTramitePersona",
                HttpMethod.Post, validarPersonaRequest);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ValidarPersonaResponse>(res);
                return Ok(resul);
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);

            return BadRequest(error);
        }

        [HttpGet("ConsultarArchivoTramiteVirtualPorId/{archivoId}/")]
        public async Task<ActionResult<ArchivoTramiteVirtual>> ConsultarArchivoTramiteVirtualPorId1(long archivoId)
        {
            var token = HttpContext.Request.Query["token"];

            //if (string.IsNullOrEmpty(token))
            //{
            //    var err = new ErroresDTO { Errors = new[] { "Invalid Token" } };
            //    return BadRequest(err);
            //}

            //_httpClientHelper.ValidateToken(token);

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
                    resul.Nombre = resul.Nombre.Contains(".pdf") ? resul.Nombre : $"{resul.Nombre}.pdf";
                    return Ok(resul);
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
