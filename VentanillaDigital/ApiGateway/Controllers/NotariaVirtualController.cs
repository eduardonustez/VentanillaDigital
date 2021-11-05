using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using ApiGateway.Contratos.Models.NotariaVirtual;
using ApiGateway.Contratos.Models.Notario;
using ApiGateway.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PortalCliente.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotariaVirtualController : ControllerBase
    {
        public string uriAPI;
        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        private readonly JsonSerializerOptions _options;
        public NotariaVirtualController(IConfiguration configuration, IHttpClientHelper httpClientHelper)
        {
            _configuration = configuration;
            uriAPI =
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [HttpGet]
        [Route("ConvenioNotariaVirtual/{NotariaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ConvenioNotariaVirtualResponse>> ConvenioNotariaVirtual(long NotariaId)
        {
            ConvenioNotariaVirtualModel notariaVirtualModel = new ConvenioNotariaVirtualModel { NotariaId = NotariaId };
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/NotariaVirtual/ValidarConvenioNotariaVirtual/",
             HttpMethod.Get, notariaVirtualModel);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                bool.TryParse(res, out bool esNotariaVirtualConvenio);
                ConvenioNotariaVirtualResponse result = new ConvenioNotariaVirtualResponse { esNotariaVirtual = esNotariaVirtualConvenio };
                return Ok(result);
            }
            return BadRequest(false);
        }

        [HttpGet]
        [Route("EstadosTramiteVirtual/{IsDeleted}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EstadoTramitesVirtualesResponse>>> EstadoTramiteVirtual(bool IsDeleted)
        {
            EstadoTramitesVirtualesRequest estadoTramitesVirtuales = new EstadoTramitesVirtualesRequest { IsDeleted = IsDeleted };
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/NotariaVirtual/ObtenerEstadosTramiteVirtual/",
             HttpMethod.Get, estadoTramitesVirtuales);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<List<EstadoTramitesVirtualesResponse>>(res);
                return Ok(resul);
            }
            return BadRequest(false);
        }

        [HttpPost]
        [Route("ObtenerMiConfiguracionMiFirma/{NotariaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MiFirmaResponse>> ObtenerMiConfiguracionMiFirma(long NotariaId)
        {
            ConvenioNotariaVirtualModel notariaVirtualModel = new ConvenioNotariaVirtualModel { NotariaId = NotariaId };
            var serviceResponse = await _httpClientHelper.ConsumirServicioRest($"{uriAPI}/NotariaVirtual/ObtenerMiConfiguracionMiFirma/",
             HttpMethod.Post, notariaVirtualModel);
            var res = await serviceResponse.Content.ReadAsStringAsync();
            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                List<string> list = new List<string>();
                ErroresDTO errorAuth = new ErroresDTO();
                ObtenerMiConfiguracionMiFirmaResponse result = JsonConvert.DeserializeObject<ObtenerMiConfiguracionMiFirmaResponse>(res);
                LoginAuthenticate login = new LoginAuthenticate() { userName = result.LoginConvenio, password = result.PasswordConvenio };

                #region authenticate
                DataHttpClient HttpClientAuth = new DataHttpClient()
                {
                    ServiceBaseAddress = $"{result.Gateway}v1_0/authenticate",
                    TipoTokenBasic = "Basic",
                    TokenBasic = result.ChannelAuthMiFirma,
                    NombreTokenBasic = "ChannelAuthorization",
                    TipoTokenBearer = "",
                    TokenBearer = "",
                    NombreTokenBearer = "",
                };

                HttpClientGeneric<HttpResponseMessage, LoginAuthenticate> Authenticate = new HttpClientGeneric<HttpResponseMessage, LoginAuthenticate>(HttpClientAuth);
                var response = await Authenticate.PostAsync(login);

                #endregion authenticate

                if ((int)response.StatusCode == (int)HttpStatusCode.OK)
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var serviceAuthResponse = JsonConvert.DeserializeObject<LoginAuthenticateResponse>(content);

                    #region Response

                    MiFirmaResponse MiFirma = new MiFirmaResponse()
                    {
                        MyFrame = result.MyFrame,
                        LoginConvenio = result.LoginConvenio,
                        Titulo = string.Empty,
                        Token = serviceAuthResponse.data.token,
                        ConfigurationGuid = result.ConfigurationGuid,
                        CUANDI = string.Empty
                    };
                    return Ok(MiFirma);

                    #endregion Response
                }
                else
                {
                    list.Add("No se logró autenticar mi firma (Convenio Notaria).");
                    errorAuth.Errors = list.ToArray();
                    return BadRequest(errorAuth);
                }
            }
            ErroresDTO error = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(error);
        }

    }
}
