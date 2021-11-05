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
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    //[ServiceFilter(typeof(TokenValidationFilterAttribute))]
    public class NotarioController : BaseController
    {
        private ITramiteServicio _tramiteServicio { get; }
        private INotarioServicio _notarioServicio { get; }

        public NotarioController(ITramiteServicio tramiteServicio, INotarioServicio notarioServicio) :
            base(tramiteServicio)
        {
            _tramiteServicio = tramiteServicio;
            _notarioServicio = notarioServicio;
        }

            
        [HttpPost]
        [Route("ObtenerPendientesAutorizacion")]
        public async Task<IActionResult> ObtenerPendientesAutorizacion(DefinicionFiltro request)
        {
             var tramite= await _tramiteServicio.ObtenerTramitesPendientes(request);
            return Ok(tramite);
        }


        [HttpPost]
        [Route("ObtenerTramitesPendientesAutPaginado")]
        public async Task<IActionResult> ObtenerTramitesPendientesAutPaginado(FiltroTramites request)
        {
            var tramite = await _tramiteServicio.ObtenerTramitesPendientesAutPaginado(request);
            return Ok(tramite);
        }


        [HttpPost]
        [Route("ObtenerTramitesAutorizadoPaginado")]
        public async Task<IActionResult> ObtenerTramitesAutorizadoPaginado(FiltroTramites request)
        {
            var tramite = await _tramiteServicio.ObtenerTramitesAutorizadoPaginado(request);
            return Ok(tramite);
        }

        [HttpPost]
        [Route("ObtenerTramitesEnProcesoPaginado")]
        public async Task<IActionResult> ObtenerTramitesEnProcesoPaginado(FiltroTramites request)
        {
            var tramite = await _tramiteServicio.ObtenerTramitesEnProcesoPaginado(request);
            return Ok(tramite);
        }

        [HttpPost]
        [Route("ObtenerTramitesRechazadosPaginado")]
        public async Task<IActionResult> ObtenerTramitesRechazadosPaginado(FiltroTramites request)
        {
            var tramite = await _tramiteServicio.ObtenerTramitesRechazadosPaginado(request);
            return Ok(tramite);
        }


        [HttpPost]
        [Route("ActualizarGrafoPinNotario")]
        public async Task<IActionResult> ActualizarGrafoPinNotario(NotarioCreateDTO request)
        {
            var notarioGrafoPin = await _notarioServicio.ActualizarGrafoPinNotario(request);
            return Ok(notarioGrafoPin);
        }

        //[HttpPost]
        //[Route("SeleccionarFormatoImpresion")]
        //public async Task<IActionResult> SeleccionarFormatoImpresion(SeleccionarFormatoImpresionDTO seleccionFormatoImpresionDTO)
        //{
        //    await _notarioServicio.SeleccionarFormatoImpresion(seleccionFormatoImpresionDTO);
        //    return Ok();
        //}

        [HttpGet]
        [Route("ObtenerEstadoPinFirma/{email}")]
        public async Task<IActionResult> ObtenerEstadoPinFirma(string email)
        {
            return Ok(await _notarioServicio.ObtenerEstadoPinFirma(email));
        } 
        //[HttpGet]
        //[Route("ObtenerOpcionesConfiguracion/{email}")]
        //public async Task<IActionResult> ObtenerOpcionesConfiguracion(string email)
        //{
        //    return Ok(await _notarioServicio.ObtenerOpcionesConfiguracion(email));
        //}

        [HttpGet]
        [Route("ObtenerGrafo/{email}")]
        public async Task<IActionResult> ObtenerGrafo(string email)
        {
            var grafo = await _notarioServicio.ObtenerGrafo(email);
            
            if (!string.IsNullOrEmpty(grafo))
            {
                return Ok(grafo);
            }
            else 
            {
                return NoContent();
            }
        }

        [HttpGet]
        [Route("ObtenerNotariosNotaria/{NotariaId}")]
        public async Task<IActionResult> ObtenerNotariosNotaria(long NotariaId)
        {
            return Ok(await _notarioServicio.ObtenerNotariosNotaria(NotariaId));
        }
        [HttpPost]
        [Route("SeleccionarNotarioNotaria")]
        public async Task<IActionResult> SeleccionarNotarioNotaria(NotarioNotariaDTO notarioNotariaDTO)
        {
            await _notarioServicio.SeleccionarNotarioNotaria(notarioNotariaDTO);
            return Ok(notarioNotariaDTO.NotarioId);
        }
        [HttpPost]
        [Route("ValidarSolicitudPin")]
        public async Task<IActionResult> ValidarSolicitudPin(ValSolicitudPinDTO valSolicitudPinDTO)
        {
            return Ok(await _notarioServicio.ValidarSolicitudPin(valSolicitudPinDTO));
        }
        [HttpPost]
        [Route("EsPinValido")]
        public async Task<IActionResult> EsPinValido(ValSolicitudPinDTO valSolicitudPinDTO)
        {
            return Ok(await _notarioServicio.EsPinValido(valSolicitudPinDTO));
        }

    }
}
