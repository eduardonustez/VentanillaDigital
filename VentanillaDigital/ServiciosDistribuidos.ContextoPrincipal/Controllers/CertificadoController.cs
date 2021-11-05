using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CertificadoController : BaseController
    {
        private ICertificadoServicio _certificadoServicio { get; }

        public CertificadoController(ICertificadoServicio certificadoServicio) :
            base(certificadoServicio)
        {
            _certificadoServicio = certificadoServicio;
        }

        [HttpGet]
        [Route("ObtenerDatosSolicitud/{userId}")]
        public async Task<IActionResult> ObtenerDatosSolicitud(string userId)
        {

            return Ok(await _certificadoServicio.ObtenerDatosSolicitud(userId));
        }
        [HttpPost]
        [Route("RegistrarSolicitud")]
        public async Task<IActionResult> RegistrarSolicitud(CertificadoCreateDTO solicitud)
        {
            return Ok(await _certificadoServicio.RegistrarSolicitud(solicitud));
        }
        [HttpGet]
        [Route("ObtenerCertificadoNotario/{userId}")]
        public async Task<IActionResult> ObtenerCertificadoNotario(string userId)
        {

            return Ok(await _certificadoServicio.ObtenerCertificadoNotario(userId));
        }
        [HttpPost]
        [Route("ActualizarCertificadoNotario")]
        public async Task<IActionResult> ActualizarCertificadoNotario(CertificadoSelectedDTO certificadoDTO )
        {
            await _certificadoServicio.ActualizarCertificadoNotario(certificadoDTO);
            return Ok(true);
        }
        [HttpPost]
        [Route("ActualizarUsuarioNotario")]
        public async Task<IActionResult> ActualizarUsuarioNotario(CertificadoSelectedDTO certificadoDTO)
        {
            await _certificadoServicio.ActualizarUsuarioNotario(certificadoDTO);
            return Ok(true);
        }
    }
}