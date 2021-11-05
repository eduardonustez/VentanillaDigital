using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrarCiudadanoController : BaseController
    {
        private IPortalVirtualServicio _PortalVirtualServicio { get; }
        private readonly IActoNotarialServicio _actoNotarialServicio;

        public RegistrarCiudadanoController(
            IPortalVirtualServicio PortalVirtualServicio,
            IActoNotarialServicio actoNotarialServicio
            )
        {
            _actoNotarialServicio = actoNotarialServicio;
            _PortalVirtualServicio = PortalVirtualServicio;
        }

        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        [HttpGet("obtenerTotalPagadoTramite/{tramitePortalVirtualId}")]
        public async Task<IActionResult> ObtenerTotalPagadoTramite(int tramitePortalVirtualId)
            => Ok(await _PortalVirtualServicio.TotalPagadoTramite(tramitePortalVirtualId));

        [HttpPost]
        [Route("RegistrarCiudadano")]
        public async Task<IActionResult> RegistrarCiudadano(TramitePortalVirtualCiudadanoDTO request)
        {
            var portalVirtualServicio = await _PortalVirtualServicio.RegistrarCiudadano(request);
            return Ok(portalVirtualServicio);
        }

        [HttpPost("actualizarPago/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> ActualizarPago(long recaudoTramiteVirtualId, [FromBody] ActualizarPagoModel model)
            => Ok(await _PortalVirtualServicio.ActualizarPago(recaudoTramiteVirtualId, model));

        [HttpPost]
        [Route("ActualizarTramiteVirtual")]
        public async Task<IActionResult> AcutalizarTramiteVirtual(ActualizarTramiteVirtualDTO request)
        {
            var portalVirtualServicio = await _PortalVirtualServicio.ActualizarTramiteVirtual(request);
            return Ok(portalVirtualServicio);
        }

        [HttpPost]
        [Route("ObtenerTramitePortalVirtual")]
        public async Task<IActionResult> ObtenerTramitePortalVirtual(DefinicionFiltro request)
        {
            var portalVirtualServicio = await _PortalVirtualServicio.ObtenerTramitePortalVirtual(request);
            return Ok(portalVirtualServicio);
        }

        [HttpPost("ConsultarTramiteVirtualPorId")]
        public async Task<IActionResult> ConsultarTramiteVirtualPorId(ConsultarTramiteDTO consultarTramite)
            => Ok(await _PortalVirtualServicio.ConsultarTramiteVirtualPorId(consultarTramite));

        [HttpPost("EnviarRecaudo/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> EnviarRecaudo(long recaudoTramiteVirtualId, EnviarRecaudoModel model)
            => Ok(await _PortalVirtualServicio.EnviarRecaudo(recaudoTramiteVirtualId, model));

        [HttpGet("ConsultarMensajesTramiteVirtual/{archivoId}")]
        public async Task<IActionResult> ConsultarMensajesTramiteVirtual(long archivoId)
            => Ok(await _PortalVirtualServicio.ConsultarMensajesTramiteVirtual(archivoId));
        
        //[ServiceFilter(typeof(TokenValidationAdministrationAttribute))]
        [HttpGet("ConsultarArchivoTramiteVirtualPorId/{archivoId}")]
        public async Task<IActionResult> ConsultarArchivoTramiteVirtualPorId(long archivoId)
            => Ok(await _PortalVirtualServicio.ConsultarArchivoTramiteVirtualPorId(archivoId));

        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        [HttpDelete("EliminarRecaudo/{recaudoTramiteVirtualId}")]
        public async Task<IActionResult> EliminarRecaudo(long recaudoTramiteVirtualId)
            => Ok(await _PortalVirtualServicio.EliminarRecaudo(recaudoTramiteVirtualId));

        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        [HttpGet("ConsultarRecaudosTramite/{tramiteId}")]
        public async Task<IActionResult> ConsultarRecaudosTramite(long tramiteId)
            => Ok(await _PortalVirtualServicio.ConsultarRecaudosTramite(tramiteId));

        [ServiceFilter(typeof(TokenValidationFilterAttribute))]
        [HttpGet("ConsultarUrlSubirArchivosMiFirma")]
        public async Task<ActionResult<UrlMiFirmaModel>> ConsultarUrlSubirArchivosMiFirma()
        {
            var token = Request.Query["token"].ToString();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (!jwtSecurityTokenHandler.CanReadToken(token))
            {
                return BadRequest();
            }

            var jwtSecurityToken = jwtSecurityTokenHandler.ReadToken(token) as JwtSecurityToken;

            var notariaId = long.Parse(jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == "NotariaId").Value);
            var url = await _PortalVirtualServicio.ConsultarUrlSubirArchivosMiFirma(notariaId);
            return Ok(new UrlMiFirmaModel { UrlSubirArchivosMiFirma = url });
        }

        [HttpPost("CambiarEstado/{tramiteId}")]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoTramiteVirtualModel body, long tramiteId)
            => Ok(await _PortalVirtualServicio.CambiarEstado(tramiteId, body));

        [HttpPost("GuardarRecaudo")]
        public async Task<IActionResult> GuardarRecaudo([FromBody] CrearRecaudoTramiteVirtualModel body)
            => Ok(await _PortalVirtualServicio.GuardarRecaudo(body));

        [HttpPost("CambiarEstadoCliente/{tramiteId}")]
        public async Task<IActionResult> CambiarEstadoCliente([FromBody] CambiarEstadoTramiteVirtualClienteModel body, long tramiteId)
            => Ok(await _PortalVirtualServicio.CambiarEstadoCliente(tramiteId, body.Files, body.Estado, body.Usuario, body.Mensaje));

        [HttpGet("ObtenerTestamento/{tramiteId}/{claveTestamento}")]
        public async Task<IActionResult> ObtenerTestamento(long tramiteId, string claveTestamento)
            => Ok(await _PortalVirtualServicio.ObtenerTestamento(tramiteId, claveTestamento));

        [HttpPost]
        [Route("ValidacionTramitePersona")]
        public async Task<IActionResult> ValidacionTramitePersona(ValidarTramitePersonaDTO validarTramitePersonaDTO) => Ok(await _PortalVirtualServicio.ValidarTramitePersona(validarTramitePersonaDTO));

        [HttpGet("ObtenerActosNotariales")]
        public async Task<IActionResult> ObtenerActosNotariales()
            => Ok(await _actoNotarialServicio.ObtenerTodosActosNotariales());

        [HttpGet("ObtenerActosPorTramite/{tramiteId}")]
        public async Task<IActionResult> ObtenerActosPorTramite(long tramiteId)
            => Ok(await _actoNotarialServicio.ObtenerActosPorTramite(tramiteId));
    }
}
