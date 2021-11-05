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
using ApiGateway.Helper;
using System.Net.Http;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.Transversal.Log.Implementacion;
using Microsoft.AspNetCore.Authorization;
using ApiGateway.Models.Transaccional;
using ApiGateway.Enums;
using ApiGateway.Contratos.Models.Notario;
using Infraestructura.Transversal.Models;
using Infraestructura.KeyManager;
using ApiGateway.Contratos.Models.Certificado;
using GenericExtensions;
using Infraestructura.KeyManager.Models;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class NotarioController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        private IKeyManagerClient _keyManagerClient;
        public NotarioController(IConfiguration configuration,
            IHttpClientHelper httpClientHelper
            , IKeyManagerClient keyManagerClient)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
            _keyManagerClient = keyManagerClient;
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
        [Route("ObtenerTramitesPendientesAutPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitePendienteAutorizacionModel>> ObtenerTramitesPendientesAutPaginado(FiltroTramites model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerTramitesPendientesAutPaginado",
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
        [Route("ObtenerTramitesAutorizadoPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitePendienteAutorizacionModel>> ObtenerTramitesAutorizadoPaginado(FiltroTramites model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerTramitesAutorizadoPaginado",
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
        [Route("ObtenerTramitesEnProcesoPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitePendienteAutorizacionModel>> ObtenerTramitesEnProcesoPaginado(FiltroTramites model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerTramitesEnProcesoPaginado",
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
        [Route("ObtenerTramitesRechazadosPaginado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TramitePendienteAutorizacionModel>> ObtenerTramitesRechazadosPaginado(FiltroTramites model)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ObtenerTramitesRechazadosPaginado",
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
            var claims = _httpClientHelper.GetClaims();
            var email = claims.UserName;
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/Notario/ActualizarGrafoPinNotario",
             HttpMethod.Post, model);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                //TODO CHANGE PIN

                //var resul = await _keyManagerClient.CertificateStatus("", email);
                //if (resul.Success)
                //{
                //    List<CertificadoDTO> certificadoDTOs = resul.Certificates.Select(c => c.Adaptar<CertificadoDTO>()).ToList();
                //    foreach (var cer in certificadoDTOs)
                //        await _keyManagerClient.ChangePin(new PinChangeRequest()
                //        {
                //            RequestId = cer.IdCertificado,
                //            PinOld = "", //To DO,
                //            PinNew = model.Pin
                //        });
                //}
                return Ok(true);
            }
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

        [HttpPost]
        [Route("ValidarSolicitudPin")]
        public async Task<IActionResult> ValidarSolicitudPin(ValidacionClaveDTO clave)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notario/ValidarSolicitudPin",
             HttpMethod.Post, clave);
            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(res);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }
        [HttpPost]
        [Route("EsPinValido")]
        public async Task<IActionResult> EsPinValido(ValidacionClaveDTO clave)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/Notario/EsPinValido",
             HttpMethod.Post, clave);
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