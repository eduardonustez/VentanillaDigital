using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal;
using Infraestructura.Transversal.ExtensionMethods;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotariaController : BaseController
    {
        private INotariaServicio _notariaServicio { get; }
        public NotariaController(INotariaServicio notariaServicio):
            base(notariaServicio)
        {
            _notariaServicio = notariaServicio;
        }
            
        [HttpGet]
        [Route("ConfiguracionRNEC/{notariaID}")]
        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        public async Task<IActionResult> ObtenerConfiguracionRNEC(long notariaID)
        {

            return Ok(await _notariaServicio.ObtenerConfiguracionRNEC(notariaID));
        }

        [HttpGet]
        [Route("ObtenerNotariaPorId/{notariaID}")]
        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        public async Task<IActionResult> ObtenerNotariaPorId(long notariaID)
        {
            return Ok(await _notariaServicio.ObtenerNotariaPorId(notariaID));
        }

        [HttpPost]
        [Route("ActualizarSelloNotaria/{notariaId}")]        
        public async Task<IActionResult> ActualizarSelloNotaria(long notariaId, IFormFile formFile)
        {
            await _notariaServicio.ActualizarSelloNotaria(new SelloNotariaEditDto() { 
                NotariaId=notariaId
                ,Sello=formFile.GetContentTypeAndBase64FromFormFile()});
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
            => Ok(await _notariaServicio.ObtenerTodasNotarias());

        [HttpGet]
        [Route("ObtenerNotarias")]
        public async Task<IActionResult> ObtenerNotarias()
        {
            return Ok(await this._notariaServicio.ObtenerNotarias());
        }
    }
}
