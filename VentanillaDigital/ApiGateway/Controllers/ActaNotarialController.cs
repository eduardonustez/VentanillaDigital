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
using System.Reflection;
using GenericExtensions;
using ApiGateway.Contratos.Models.ActaNotarial;
using ApiGateway.Contratos.Models.Transaccional;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ActaNotarialController : Controller
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public ActaNotarialController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI = _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }


        [HttpGet]
        [Route("ObtenerActaNotarial/{tramiteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ActaNotarialModel>> ObtenerActaNotarial(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/ObtenerActaNotarial",
             HttpMethod.Post, new { TramiteId = tramiteId });
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ActaNotarialModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("RechazarTramiteNotarial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<TramiteRechazadoReturnModel>> RechazarTramiteNotarial(TramiteRechazadoModel pinFirma)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/RechazarTramiteNotarial",
             HttpMethod.Post, pinFirma);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteRechazadoReturnModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("CancelarTramiteNotarial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<TramiteRechazadoReturnModel>> CancelarTramiteNotarial(TramiteRechazadoModel pinFirma)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/CancelarTramiteNotarial",
             HttpMethod.Post, pinFirma);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<TramiteRechazadoReturnModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("FirmarActaNotarial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = "RequireNotario")]
        public async Task<ActionResult<FirmaActaNotarialModel>> FirmarActaNotarial(PinFirmaModel pinFirma)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/FirmarActaNotarial",
             HttpMethod.Post, pinFirma);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<FirmaActaNotarialModel>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("FirmaActaNotarialLote")]
        [Authorize(Policy = "RequireNotario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AutorizacionTramitesResponse>> FirmaActaNotarialLote(AutorizacionTramitesRequest request)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/ActaNotarial/FirmaActaNotarialLote/",
             HttpMethod.Post, request);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<List<AutorizacionTramitesResponse>>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }

        [HttpPost]
        [Route("CrearActaParaFirmaManual")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CrearActaParaFirmaManual(ActaCreate acta)
        {
            //if (acta.FirmarManual)
            //{
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest(uriAPI + "/ActaNotarial/CrearActaParaFirmaManual/",
             HttpMethod.Post, acta);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                return Ok(res);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
            //}
            //return Ok();
        }
        [HttpGet]
        [Route("ObtenerResumen/{tramiteId}")]
        public async Task<ActionResult<ActaResumen>> ObtenerResumen(long tramiteId)
        {
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/ActaNotarial/ObtenerResumen/{tramiteId}",
         HttpMethod.Get, "");
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<ActaResumen>(res);
                return Ok(resul);
            }

            ErroresDTO errors = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errors);
        }


        private async Task<ParametrosActaNotarial> PrepararParametrosActa(ActaCreate actaCreate)
        {
            var infoActa = actaCreate.Adaptar<ActaCreateToPlantilla>();

            ParametrosActaNotarial plantillaParametros = new ParametrosActaNotarial();
            var keyvalues = infoActa.GetType()
                            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                            .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(infoActa, null));

            var parametros = keyvalues.Select(kv => new Parametro()
            {
                NombreCampo = kv.Key,
                Valor = kv.Value
            }).ToList();

            if (!string.IsNullOrWhiteSpace(actaCreate.DataAdicional))
            {
                var datosAdicionales = JsonConvert.DeserializeObject<Dictionary<string, string>>(actaCreate.DataAdicional);

                if (datosAdicionales != null)
                {
                    foreach (var kv in datosAdicionales)
                    {
                        Parametro parametro = new Parametro() { NombreCampo = kv.Key, Valor = kv.Value };
                        parametros.Add(parametro);
                    }
                }
            }

            string TEXTOSINBIOMETRIA = "El compareciente no fue identificado mediante biometría en línea debido a:";
            string TEXTOCONBIOMETRIA = "Conforme al Artículo 18 del Decreto-Ley 019 de 2012, el compareciente fue identificado mediante cotejo biométrico de su huella dactilar con la información biográfica y biométrica de la base de datos de la Registraduría Nacional del Estado Civil.";
            plantillaParametros.Parametros = parametros;
            if (actaCreate.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoConFirmaARuego ||
                actaCreate.CodigoTramite == (int)EnumTipoTramite.DiligenciaDeReconocimientoDeFirmaYContenidoDeDocumentoPrivadoDePersonaInvidente
                )
            {
                //var comparecientesFirmaARuego = await ObtenerComparecientesFirmaARuego(tramite, TEXTOSINBIOMETRIA, TEXTOCONNBIOMETRIA);
                //plantillaParametros.Parametros = plantillaParametros.Parametros.Union(comparecientesFirmaARuego);
            }
            else
            {
                plantillaParametros.Comparecientes = await ObtenerParametrosComparecientes(actaCreate.ComparecientesCreate, TEXTOSINBIOMETRIA, TEXTOCONBIOMETRIA);
            }
            return plantillaParametros;
        }
        private async Task<List<IEnumerable<Parametro>>> ObtenerParametrosComparecientes(List<ComparecienteCreate> ListaComparecientes
          , string TEXTOSINBIOMETRIA, string TEXTOCONBIOMETRIA)
        {

            ListaComparecientes.ForEach(
                c => c.TextoBiometria = c.TramiteSinBiometria == "1" ? $"{TEXTOSINBIOMETRIA} {c.TextoBiometria}" : $"{TEXTOCONBIOMETRIA} {c.TextoBiometria}"
            );


            var parametrosTodosCompa =
                ListaComparecientes.Select(c =>
                {
                    var kvcomp = c.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                        .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(c, null));
                    return kvcomp.Select(kv => new Parametro()
                    {
                        NombreCampo = kv.Key,
                        Valor = kv.Value
                    });
                }).ToList();

            return parametrosTodosCompa;
        }


    }
}