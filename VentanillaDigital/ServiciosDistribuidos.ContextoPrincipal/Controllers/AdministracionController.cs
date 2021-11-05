using ApiGateway.Models;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Correo;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using ServiciosDistribuidos.ContextoPrincipal.Models.Account;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(TokenValidationAdministrationAttribute))]
    public class AdministracionController : BaseController
    {
        private readonly ITramiteServicio _tramiteServicio;
        private readonly IActaNotarialServicio _actaNotarialServicio;
        private readonly IComparecienteServicio _comparecienteServicio;
        private readonly IUsuarioAdministracionServicio _usuarioAdministracionService;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;

        public AdministracionController(
            ITramiteServicio tramiteServicio,
            UserManager<ApplicationUser> userManager,
            IActaNotarialServicio actaNotarialServicio,
            IComparecienteServicio comparecienteServicio,
            IUsuarioAdministracionServicio usuarioAdministracionService
            , ITemplateServicio templateServicio
            , IConfiguration configuration)
            : base(tramiteServicio
                  , comparecienteServicio
                  , actaNotarialServicio
                  , usuarioAdministracionService
                  , templateServicio)
        {
            this.userManager = userManager;
            _tramiteServicio = tramiteServicio;
            _actaNotarialServicio = actaNotarialServicio;
            _comparecienteServicio = comparecienteServicio;
            _usuarioAdministracionService = usuarioAdministracionService;
            _configuration = configuration;
        }

        #region ActaNotarial
        [HttpGet("ObtenerActaNotarial/{tramiteId}")]
        public async Task<IActionResult> ObtenerActaNotarial(long tramiteId)
        {
            var acta = await _actaNotarialServicio.ObtenerActaNotarial(new ActaNotarialGetDTO { TramiteId = tramiteId });
            return Ok(acta);
        }
        #endregion

        #region Tramite
        [HttpGet("ObtenerTramite/{tramiteId}")]
        public async Task<IActionResult> ObtenerTramite(long tramiteId)
        {
            var tramite = await _tramiteServicio.ObtenerTramiteDetalle(tramiteId);
            return Ok(tramite);
        }

        [HttpGet("ObtenerTramitesPorNumeroIdentificacion/{numeroIdentificacion}/{fechaInicio}/{fechaFin}")]
        public async Task<IActionResult> ObtenerTramitesPorNumeroIdentificacion(
            string numeroIdentificacion,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            try
            {
                if (string.IsNullOrEmpty(numeroIdentificacion))
                    throw new Exception("Digite todos los campos");

                if (fechaInicio.AddDays(30) < fechaFin)
                    throw new Exception("Rango máximo permitido para la consulta: 30 días");

                var tramite = await _tramiteServicio.ObtenerTramitesPorNumeroIdentificacion(numeroIdentificacion, fechaInicio, fechaFin);
                return Ok(tramite);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("ObtenerTramitesPorNumeroIdentificacionPaginado")]
        public async Task<IActionResult> ObtenerTramitesPorNumeroIdentificacionPaginado(
            DefinicionFiltro filtro
            )
        {
            var tramite = await _tramiteServicio.ObtenerTramitesPorNumeroIdentificacionPaginado(filtro);
            return Ok(tramite);
        }

        [HttpGet("ObtenerHistorialTramite/{tramiteId}")]
        public async Task<IActionResult> ObtenerHistorialTramite(long tramiteId)
        {
            var historial = await _tramiteServicio.ObtenerHistorialTramite(tramiteId);
            return Ok(historial);
        }
        #endregion

        #region Comparecientes
        [HttpGet("ObtenerComparecientesPorTramiteId/{tramiteId}")]
        public async Task<IActionResult> ObtenerPorTramiteId(long tramiteId)
        {
            var tramite = await _comparecienteServicio.ObtenerDatosComparecientesPorTramiteId(tramiteId);
            return Ok(tramite);
        }
        #endregion

        #region UsuariosAdministracion
        [HttpPost("ObtenerUsuariosAdministracionPaginado")]
        public async Task<IActionResult> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro request)
        {
            var usuarios = await _usuarioAdministracionService.ObtenerUsuariosAdministracionPaginado(request);
            return Ok(usuarios);
        }

        [HttpPost("CrearUsuario")]
        [ServiceFilter(typeof(TokenValidationAdministrationRoleAttribute))]
        public async Task<IActionResult> CrearUsuario(CrearUsuarioAdministracionRequest request)
        {

            //var newPassword = userManager.PasswordHasher.HashPassword(new ApplicationUser { }, request.Password);
            await _usuarioAdministracionService.CrearUsuario(request.Email, request.Rol);
            return Ok();
        }

        [HttpPost("ActualizarUsuario")]
        [ServiceFilter(typeof(TokenValidationAdministrationRoleAttribute))]
        public async Task<IActionResult> ActualizarUsuario(ActualizarUsuarioAdministracionRequest request)
        {
            await _usuarioAdministracionService.ActualizarUsuario(request.Id, request.Email, request.Rol);
            return Ok();
        }

        [HttpGet("ObtenerUsuarioAdmin/{UsuarioAdminId}")]
        [ServiceFilter(typeof(TokenValidationAdministrationRoleAttribute))]
        public async Task<IActionResult> ObtenerUsuarioAdmin(long UsuarioAdminId)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var queryString = HttpContext.Request.Query;
            StringValues token;
            JwtSecurityToken jwtSecurityToken;
            if (queryString.TryGetValue("token", out token))
            {
                jwtSecurityToken = jwtSecurityTokenHandler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
            }
            else
            {
                return BadRequest();
            }
            return Ok(await _usuarioAdministracionService.ObtenerUsuarioAdmin(UsuarioAdminId, jwtSecurityToken.Claims.Where(x => x.Type == JwtRegisteredClaimNames.UniqueName).Select(x => x.Value).FirstOrDefault()));
        }

        [HttpPost("EliminarUsuarioAdmin")]
        [ServiceFilter(typeof(TokenValidationAdministrationRoleAttribute))]
        public async Task<IActionResult> EliminarUsuarioAdmin(EliminarUsuarioAdministracionRequest request)
        {
            await _usuarioAdministracionService.EliminarUsuario(request.Id, request.Email, request.Rol);
            return Ok();
        }

        [HttpPost]
        [Route("NotificacionPwdUsuarioPortalAdmin")]
        public async Task<IActionResult> NotificacionPwdUsuarioPortalAdmin(NotificarUsuarioAdminRequest notificacionUsuario)
        {
            try
            {
                var usuario = await _usuarioAdministracionService.ObtenerUsuarioAdministracion(notificacionUsuario.Email).ConfigureAwait(false);

                if (usuario != null)
                {
                    var token = await _usuarioAdministracionService.GetTokenAdministracion(usuario.UsuarioAdministracionId, usuario.Login, usuario.Rol);
                    if (!string.IsNullOrEmpty(token))
                    {
                        //Insertar Token
                        DateTime fechaActual = DateTime.Now;
                        var fechaExpiracion = fechaActual.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime"));
                        string tokenInsercion = await _usuarioAdministracionService.InsertarToken(usuario, token, fechaExpiracion);
                        if (string.IsNullOrEmpty(tokenInsercion))
                        {
                            await _usuarioAdministracionService.NotificarUsuario(notificacionUsuario.Email, notificacionUsuario.Url, token).ConfigureAwait(false);
                            return Ok();
                        }
                        else
                        {
                            throw new ArgumentException(tokenInsercion);
                            //return BadRequest(tokenInsercion);
                        }
                    }
                    throw new ArgumentException("Error al obtener token");
                    //return BadRequest("Error al obtener token");
                }
                else
                {
                    //return BadRequest("Usuario no encontrado");
                    throw new ArgumentException("Usuario no encontrado");
                }
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                throw new ArgumentException(ex.Message);
            }
        }

        #endregion
    }
}
