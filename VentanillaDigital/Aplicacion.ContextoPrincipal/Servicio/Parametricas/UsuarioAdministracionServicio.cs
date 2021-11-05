using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Infraestructura.Transversal.Models;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using Infraestructura.Transversal.Correo;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.Entidad.Parametricas;

namespace Aplicacion.ContextoPrincipal.Servicio.Parametricas
{
    public class UsuarioAdministracionServicio : BaseServicio, IUsuarioAdministracionServicio
    {
        private readonly IUsuarioAdministracionRepositorio _usuarioAdministracionRepositorio;
        readonly IConfiguration _configuration;
        private IManejadorCorreos _manejadorCorreos { get; }
        private ITemplateServicio _templateServicio { get; }
        private IUsuarioTokenPortalAdministradorRepositorio _usuarioTokenPortalAdministrador { get; }

        public UsuarioAdministracionServicio(IUsuarioAdministracionRepositorio usuarioAdministracionRepositorio
            , IConfiguration configuration
            , IManejadorCorreos manejadorCorreos
            , ITemplateServicio templateServicio
            , IUsuarioTokenPortalAdministradorRepositorio usuarioTokenPortalAdministrador)
            : base(usuarioAdministracionRepositorio
                  , usuarioTokenPortalAdministrador)
        {
            _usuarioAdministracionRepositorio = usuarioAdministracionRepositorio;
            _configuration = configuration;
            _manejadorCorreos = manejadorCorreos;
            _templateServicio = templateServicio;
            _usuarioTokenPortalAdministrador = usuarioTokenPortalAdministrador;
        }

        public async Task CrearUsuario(string email, string rol)
        {
            ValidarPeticionUsuariosAdmin(Id: null, email, rol);
            var usuario = await _usuarioAdministracionRepositorio.GetOneAsync(m => m.Login.Equals(email), includeDeleted: true);

            if (usuario != null)
            {
                if (!usuario.IsDeleted)
                {
                    if (usuario != null) throw new Exception($"Ya existe un usuario con correo ({email})");
                }
                else
                {
                    usuario.Rol = rol;
                    usuario.IsDeleted = false;
                    usuario.Password = null;
                    _usuarioAdministracionRepositorio.Modificar(usuario);
                    await _usuarioAdministracionRepositorio.UnidadDeTrabajo.CommitAsync();
                }
            }
            else
            {
                _usuarioAdministracionRepositorio.Agregar(new Dominio.ContextoPrincipal.Entidad.Parametricas.UsuarioAdministracion { Login = email, Rol = rol, FechaCreacion = DateTime.Now, IsDeleted = false });
                await _usuarioAdministracionRepositorio.UnidadDeTrabajo.CommitAsync();
            }
        }

        public async Task GuardarUsuarioAsync(UsuarioAdministracion usuario)
        {
            _usuarioAdministracionRepositorio.Modificar(usuario, m => m.Password);
            await _usuarioAdministracionRepositorio.UnidadDeTrabajo.CommitAsync();
        }

        public Task<UsuarioAdministracion> ObtenerUsuarioAdministracion(string login)
            => _usuarioAdministracionRepositorio.ObtenerUsuarioAdministracion(login);

        public async Task<PaginableResponse<UsuarioAdministracionDTO>> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro request)
        {
            var query = _usuarioAdministracionRepositorio.ObtenerTodo().AsNoTracking();

            if (request != null && request.Filtros != null && request.Filtros.Any())
            {
                foreach (var item in request.Filtros)
                {
                    if (item.Columna == "Login")
                        query = query.Where(m => m.Login.Contains(item.Valor));
                }
            }

            var data = query
                .AsNoTracking()
                .Skip((request.IndicePagina - 1) * request.RegistrosPagina)
                .Take(request.RegistrosPagina)
                .Select(m => new UsuarioAdministracionDTO
                {
                    Id = m.UsuarioAdministracionId,
                    Login = m.Login,
                    Rol = m.Rol == "ADMIN" ? "Administrador" : "Consultor"
                });

            var cantidad = await _usuarioAdministracionRepositorio.ObtenerTodo().LongCountAsync();

            return new PaginableResponse<UsuarioAdministracionDTO>
            {
                TotalRows = cantidad,
                Pages = (int)Math.Ceiling(cantidad / (decimal)request.RegistrosPagina),
                Data = data
            };
        }

        public async Task<UsuarioAdministracion> ObtenerUsuarioAdmin(long usuarioAdministracionId, string email)
        {
            var usuarioAdmin = (await _usuarioAdministracionRepositorio.Obtener(x => !x.IsDeleted && x.UsuarioAdministracionId == usuarioAdministracionId && x.Login != email)).FirstOrDefault();
            if (usuarioAdmin != null)
            {
                Dominio.ContextoPrincipal.Entidad.Parametricas.UsuarioAdministracion usuarioAdministrador = new Dominio.ContextoPrincipal.Entidad.Parametricas.UsuarioAdministracion
                {
                    UsuarioAdministracionId = usuarioAdmin.UsuarioAdministracionId
                    ,
                    Rol = usuarioAdmin.Rol
                    ,
                    Login = usuarioAdmin.Login
                };
                return usuarioAdministrador;
            }
            else
            {
                return null;
            }
        }

        public async Task ActualizarUsuario(long Id, string email, string rol)
        {
            ValidarPeticionUsuariosAdmin(Id, email, rol);
            var usuario = await _usuarioAdministracionRepositorio.GetOneAsync(m => m.Login.Equals(email) && !m.IsDeleted && m.UsuarioAdministracionId.Equals(Id));
            if (usuario != null)
            {
                usuario.Rol = rol;
                _usuarioAdministracionRepositorio.Modificar(usuario);
                await _usuarioAdministracionRepositorio.UnidadDeTrabajo.CommitAsync();
            }
            else
                throw new Exception($"El usuario que intenta actualizar no existe o se encuentra eliminado ({email})");
        }

        public async Task EliminarUsuario(long Id, string email, string rol)
        {
            ValidarPeticionUsuariosAdmin(Id, email, rol);
            var usuario = await _usuarioAdministracionRepositorio.GetOneAsync(m => m.Login.Equals(email) && !m.IsDeleted && m.UsuarioAdministracionId.Equals(Id));
            if (usuario != null)
            {
                var usuarioToken = (await _usuarioTokenPortalAdministrador.Obtener(x => x.UsuarioAdministracionId == usuario.UsuarioAdministracionId)).FirstOrDefault();
                if (usuarioToken != null)
                {
                    _usuarioTokenPortalAdministrador.Eliminar(usuarioToken, fisico: true);
                }
                _usuarioAdministracionRepositorio.Eliminar(usuario);
                await _usuarioAdministracionRepositorio.UnidadDeTrabajo.CommitAsync();
            }
            else
                throw new Exception("Error");
        }

        private void ValidarPeticionUsuariosAdmin(long? Id, string email, string rol)
        {
            if (Id != null)
            {
                if (Id.Value <= 0)
                    throw new Exception($"Por favor ingrese un Id valido.");
            }

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception($"Por favor ingrese un email.");
            else
            {
                bool resultIsMatch = Regex.IsMatch(email, @"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}");
                if (!resultIsMatch)
                {
                    throw new Exception($"El email ingresado {email} es incorrecto");
                }
            }

            if (string.IsNullOrWhiteSpace(rol))
                throw new Exception($"Por favor ingrese un rol.");
            else
            {
                if (!rol.Equals("ADMIN") && !rol.Equals("USER"))
                    throw new Exception($"El rol ingresado es incorrecto.");
            }
        }

        public async Task<string> GetTokenAdministracion(long Id, string login, string rol)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                new Claim(ClaimTypes.Role, rol)
            };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken
            (
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime")),
                audience: this._configuration.GetValue<String>("Tokens:Audience"),
                issuer: this._configuration.GetValue<String>("Tokens:Issuer")
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task NotificarUsuario(string email, string url, string token)
        {
            List<string> destinatarios = new List<string>();
            destinatarios.Add(email);
            var callbackUrl = new Uri($"{url}?token={HttpUtility.UrlEncode(token)}&email={email}").ToString();

            string cuerpoHMTL = ObtenerCuerpoHTML(email, callbackUrl);

            string htmldocument = cuerpoHMTL;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmldocument, null, "text/html");
            await _manejadorCorreos.EnviarCorreo(destinatarios
                    , "Asignación de Contraseña"
                    , htmldocument, htmlView);
        }
        private string ObtenerCuerpoHTML(string Email, string enlace)
        {
            return _templateServicio.ObtenerTemplateAsignacionClave(Email, enlace);
        }

        public async Task<string> InsertarToken(UsuarioAdministracion usuario, string token, DateTime FechaExpiracion)
        {
            string respuesta = string.Empty;
            try
            {
                UsuarioTokenPortalAdministrador usuarioTokenPortal = new UsuarioTokenPortalAdministrador
                {
                    UsuarioAdministracionId = usuario.UsuarioAdministracionId,
                    Nombre = "AsignacionPwdToken",
                    LoginProvider = "Asingacion",
                    Token = token,
                    FechaExpiracion = FechaExpiracion
                };
                _usuarioTokenPortalAdministrador.Agregar(usuarioTokenPortal);
                var usuarioTokenId = await _usuarioTokenPortalAdministrador.UnidadDeTrabajo.CommitAsync();
                if (usuarioTokenId > 0)
                {
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return respuesta;
        }

        public async Task<bool> ValidarTokenUsuarioAdministrador(string token, long usuarioAdministradorId)
        {
            var usuarioToken = await _usuarioTokenPortalAdministrador.GetOneAsync(u => u.Token == token && u.UsuarioAdministracionId == usuarioAdministradorId);

            if (usuarioToken != null)
            {
                var fechaActual = DateTime.Now;
                if (fechaActual < usuarioToken.FechaExpiracion)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public async Task EliminarTokenUsuarioAdministrador(string token, long usuarioAdministradorId)
        {
            var usuarioToken = await _usuarioTokenPortalAdministrador.GetOneAsync(u => u.Token == token && u.UsuarioAdministracionId == usuarioAdministradorId);
            if (usuarioToken != null)
            {
                _usuarioTokenPortalAdministrador.Eliminar(usuarioToken, fisico: true);
                await _usuarioTokenPortalAdministrador.UnidadDeTrabajo.CommitAsync();
            }
        }
    }
}
