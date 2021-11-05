using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
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
using ServiciosDistribuidos.ContextoPrincipal.Models;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticatorController : BaseController
    {
        private IAuthenticatorServicio _authenticatorServicio { get; }

        public AuthenticatorController(IAuthenticatorServicio authenticatorServicio):
            base(authenticatorServicio)
        {
            _authenticatorServicio = authenticatorServicio;
        }

        [HttpPost]
        [Route("VerificarUsuarioOTP")]
        public async Task<IActionResult> VerificarUsuarioOTP(TramiteCreateDTO tramite)//TODO: cambiar por objeto correspondiente
        {
            var resul = await _authenticatorServicio.VerificarUsuarioOTP(tramite);
            return Ok(resul);
        }

        [HttpPost]
        [Route("CreateUserToken")]
        public async Task<IActionResult> CreateUserToken(string numeroDocumento)
        {
            var resul = await _authenticatorServicio.CreateUserToken(numeroDocumento);
            return Ok(resul);
        }

        [HttpPost]
        [Route("AutenticarOtp")]
        public async Task<IActionResult> AutenticarOtp(OtpAuthenticator userLogin)
        {
            var resul = await _authenticatorServicio.AutenticarOtp(userLogin);
            return Ok(resul);
        }
    }
}
