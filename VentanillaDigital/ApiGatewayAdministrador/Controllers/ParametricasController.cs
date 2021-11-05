using ApiGatewayAdministrador.Helper;
using ApiGateway.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Controllers;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGatewayAdministrador.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParametricasController : BaseController
    {

        private IConfiguration _configuration;
        private IHttpClientHelper _httpClientHelper;
        public string uriAPI;

        public ParametricasController(IConfiguration configuration,
            IHttpClientHelper httpClientHelper)
        {

            _configuration = configuration;
            uriAPI = 
                _configuration.GetSection("ConfiguracionServiciosAPI:ServiciosDistribuidos").Value;
            _httpClientHelper = httpClientHelper;
        }

        [HttpGet]
        [Route("ObtenerTiposIdentificacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<IEnumerable<TipoIdentificacionModel>>> 
            ObtenerTiposIdentificacion()
        {

            var serviceResponse = 
                await _httpClientHelper
                    .ConsumirServicioRest($"{uriAPI}/Parametricas/ObtenerTiposIdentificacion",
                    HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var resul = JsonConvert.DeserializeObject<IEnumerable<TipoIdentificacionModel>>(res);
                return Ok(resul);
            }

            ErroresDTO errorModel = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errorModel);
        }

        [HttpGet]
        [Route("ObtenerCategorias")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErroresDTO), 400)]
        public async Task<ActionResult<IEnumerable<CategoriaReturnDTO>>> ObtenerCategorias()
        {

            var serviceResponse = 
                await _httpClientHelper
                    .ConsumirServicioRest($"{uriAPI}/Parametricas/ObtenerCategorias",
                    HttpMethod.Get, "");

            var res = await serviceResponse.Content.ReadAsStringAsync();

            if (serviceResponse.StatusCode == HttpStatusCode.OK)
            {
                var categorias = 
                    JsonConvert.DeserializeObject<IEnumerable<CategoriaReturnDTO>>(res);
                return Ok(categorias);
            }

            ErroresDTO errorModel = JsonConvert.DeserializeObject<ErroresDTO>(res);
            return BadRequest(errorModel);
        }

        [HttpGet]
        [Route("GetTiposTramite/{comercioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErroresDTO), 404)]
        public async Task<ActionResult<IEnumerable<TipoTramiteResponse>>> 
            GetTiposTramite(long comercioId)
        {

            // var serviceResponse = _httpClientHelper.ConsumirServicioRest(uriAPI + "/Anuncio/ObtenerAnuncio/1",
            //    HttpMethod.Get, "");
            //var res = await serviceResponse.Content.ReadAsStringAsync();

            //if (serviceResponse.StatusCode == HttpStatusCode.OK)
            //{
            //    AnuncioForReturnDTO anuncio = JsonConvert.DeserializeObject<AnuncioForReturnDTO>(res);
            //    FileForReturn imageForReturn= _imageHelper.GetImage(anuncio.Imagen, anuncio.NombreImagen,anuncio.FormatoImagen, anuncio.FechaModificacion);
            //    return Ok(imageForReturn);

            //}
            //ErroresDTO errorModel = JsonConvert.DeserializeObject<ErroresDTO>(res);
            //return NotFound(errorModel);
            List<TipoTramiteResponse> response = new List<TipoTramiteResponse>();
            response.Add(new TipoTramiteResponse() 
            { 
                TipoTramiteId = 1, 
                Nombre = "Autenticación", 
                Descripcion = "Autenticación", 
                Tarifa = 1, 
                Logo = "tipo_tramite_1.png" 
            });
            response.Add(new TipoTramiteResponse() 
            {
                TipoTramiteId = 2, 
                Nombre = "Matrimonio", 
                Descripcion = "Matrimonio", 
                Tarifa = 1, 
                Logo = "tipo_tramite_2.png" 
            });
            response.Add(new TipoTramiteResponse() 
            {
                TipoTramiteId = 3, 
                Nombre = "Conciliación", 
                Descripcion = "Conciliación", 
                Tarifa = 1, 
                Logo = "tipo_tramite_3.png" 
            });
            response.Add(new TipoTramiteResponse() 
            { 
                TipoTramiteId = 4, 
                Nombre = "Remates", 
                Descripcion = "Remates", 
                Tarifa = 1, 
                Logo = "tipo_tramite_4.png" 
            });
            response.Add(new TipoTramiteResponse() 
            { 
                TipoTramiteId = 5, 
                Nombre = "Sucesiones",
                Descripcion = "Sucesiones",
                Tarifa = 1,
                Logo = "tipo_tramite_5.png" 
            });
            response.Add(new TipoTramiteResponse() 
            { 
                TipoTramiteId = 5, 
                Nombre = "Pruebas", 
                Descripcion = "Sucesiones",
                Tarifa = 1, 
                Logo = "tipo_tramite_5.png" 
            });
            return response;

        }
    }
}
