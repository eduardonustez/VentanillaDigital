using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.Nucleo.Entidad;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Models;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class TramiteController : BaseController
    {
        private ITramiteServicio _tramiteServicio { get; }
        public TramiteController(ITramiteServicio tramiteServicio):
            base(tramiteServicio)
        {
            _tramiteServicio = tramiteServicio;
        }

            
        [HttpGet]
        [Route("ObtenerTramite/{tramiteId}")]
        public async Task<IActionResult> ObtenerTramite(long tramiteId)
        {
            var tramite= await _tramiteServicio.ObtenerTramite(tramiteId);
            return Ok(tramite);
        }

        [HttpGet]
        [Route("ObtenerTramite2/{tramiteId}")]
        public async Task<IActionResult> ObtenerTramite2(long tramiteId)
        {
            var tramite = await _tramiteServicio.ObtenerTramiteDetalle(tramiteId);
            return Ok(tramite);
        }

        [HttpPost]
        [Route("CrearTramite")]
        [AuditableFilter]
        public async Task<IActionResult> CrearTramite(TramiteCreateDTO tramite)
        {
            var resul = await _tramiteServicio.CrearTramite(tramite);
            return Ok(resul);
        }

        [HttpPost]
        [Route("ActualizarTramite")]
        [AuditableFilter]
        public async Task<IActionResult> ActualizarTramite(TramiteEditDTO tramite)
        {
            await _tramiteServicio.ActualizarTramite(tramite);
            return NoContent();
        }

        [HttpPost]
        [Route("ActualizarEstadoTramite/{tramiteId}")]
        [AuditableFilter]
        public async Task<IActionResult> ActualizarEstadoTramite(long tramiteId)
        {
            var resul = await _tramiteServicio.ActualizarEstadoTramite(tramiteId);
            return Ok(resul);
        }

        [HttpPost]
        [Route("AutorizacionTramite")]
        public async Task<IActionResult> AutorizacionTramite(TramiteAutorizacionCreateDTO tramiteAutorizacion)
        {
            await _tramiteServicio.AutorizarTramite(tramiteAutorizacion.TramiteId).ConfigureAwait(false);
            return Ok(true);
        }
    }
}
