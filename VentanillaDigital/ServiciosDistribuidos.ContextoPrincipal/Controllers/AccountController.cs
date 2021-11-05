using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Models;
using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Servicio;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo.Entidad;
using DotNetCore.Extensions;
using Infraestructura.ContextoPrincipal.Repositorios;
using Infraestructura.ContextoPrincipal.Repositorios.Parametricas;
using Infraestructura.Transversal;
using Infraestructura.Transversal.HandlingError;
using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Excepcion;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;
using ServiciosDistribuidos.ContextoPrincipal.AspNetIdentity;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using ServiciosDistribuidos.ContextoPrincipal.Models.Account;
using GenericExtensions;
using Microsoft.AspNetCore.Routing;
using Infraestructura.Transversal.Correo;
using System.Net.Mail;
using System.Globalization;
using Infraestructura.AgenteReconoser;
using Aplicacion.ContextoPrincipal.Modelo.Account;
using System.Web;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using System.Diagnostics;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using System.DirectoryServices.Protocols;
using System.Net;
using Infraestructura.Transversal.Models;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;

namespace ServiciosDistribuidos.ContextoPrincipal.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class AccountController : BaseController
    {
        #region Mensajes
        private const string USUARIOEXISTENTE = "El usuario ya se encuentra registrado en el sistema";
        private const string EMAILEXISTENTE = "Ya existe un usuario con ese email registrado en el sistema";
        private const string ROLNOVALIDO = "Rol invalido";
        private const string TIPOIDENTIFCACIONNOVALIDO = "Tipo de identificación inválida";
        private const string NOTARIANOVALIDA = "Notaria inválida";
        private const string ERRORCREACIONUSUARIO = "Error al crear el usuario";
        private const string ERRORACTUALIZACIONUSUARIO = "Error al crear el usuario";
        private const string ERRORELIMINACIONUSUARIO = "Error al eliminar el usuario";
        private const string TOKENINVALIDO = "Enlace de recuperación de contraseña inválido, por favor intente de nuevo de lo contrario por favor comunicarse con su administrador";
        private const string USUARIONOVALIDO = "Usuario no encontrado";
        private const string ERRORUSUARIORESETTOKEN = "Error del sistema, intente de nuevo si el error persiste por favor comunicarse con su administrador";
        private const string CORREOINVALIDO = "Correo inválido";
        private const string TOKENVENCIDO = "Enlace de recuperación vencido, por favor solicite un nuevo inicio de recuperación de contraseña";
        private const string CAMPOVACIO = "El campo {0} no puede estar vacío.";
        private const string ROLADMINISTRADORINVALIDO = "El campo {0} no puede ser modificado";
        #endregion

        private INotariaServicio _notariaServicio { get; }
        private ITipoIdentificacionServicio _tipoIdentificacionServicio { get; }
        IAgenteReconoser _AgenteReconoser;
        private ITemplateServicio _templateServicio { get; }
        private IPersonasServicio _personasServicio { get; }

        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly IConfiguration _configuration;
        readonly ILogger<AccountController> logger;
        private readonly AuthContext _authContext;
        private readonly IUsuarioAdministracionServicio _usuarioAdministracionServicio;
        private readonly INotariasUsuarioRepositorio _notariasUsuarioRepositorio;

        private IManejadorCorreos _manejadorCorreos { get; }

        public AccountController(UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IConfiguration configuration,
           ILogger<AccountController> logger, AuthContext authContext,
           INotariasUsuarioRepositorio notariasUsuarioRepositorio,
           INotariaServicio notariaServicio,
           IManejadorCorreos manejadorCorreos,
           ITipoIdentificacionServicio tipoIdentificacionServicio,
           ITemplateServicio templateServicio,
           IAgenteReconoser agenteReconoser,
           IPersonasServicio personasServicio,
           IUsuarioAdministracionServicio usuarioAdministracionServicio,
           IUsuarioAdministracionRepositorio usuarioAdministracionRepositorio
           ) :
            base(userManager, authContext, notariaServicio, personasServicio, usuarioAdministracionServicio, usuarioAdministracionRepositorio)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._configuration = configuration;
            this.logger = logger;
            this._authContext = authContext;
            _notariaServicio = notariaServicio;
            _manejadorCorreos = manejadorCorreos;
            _tipoIdentificacionServicio = tipoIdentificacionServicio;
            _AgenteReconoser = agenteReconoser;
            _templateServicio = templateServicio;
            _personasServicio = personasServicio;
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _usuarioAdministracionServicio = usuarioAdministracionServicio;
        }

        [HttpPost]
        [Route("loginAdministracion")]
        public async Task<IActionResult> LoginAdministracion(LoginAdministracionModel model)
        {
            try
            {
                var usuario = await _usuarioAdministracionServicio.ObtenerUsuarioAdministracion(model.Login);

                if (usuario == null) throw new Exception("Usuario no encontrado");

                string server = $"{_configuration["DirectorioActivo:Ip"]}:{_configuration["DirectorioActivo:Puerto"]}";

                //var newPassword = userManager.PasswordHasher.HashPassword(new ApplicationUser { }, model.Password);
                var resultCheck = await userManager.CheckPasswordAsync(new ApplicationUser { PasswordHash = usuario.Password }, model.Password);

                if (!resultCheck) throw new Exception("Usuario o contraseña incorrectos");

                //if (!IsAuthenticated(server, model.Login, model.Password, _configuration["DirectorioActivo:Dominio"]))
                //    return BadRequest(new ErrorDTO { Errors = new string[] { "Usuario incorrecto" } });

                var token = await GetTokenAdministracion(usuario.UsuarioAdministracionId, model.Login, usuario.Rol);

                // return Ok(new { Token = token, Rol = usuario.Rol, Usuario = @$"{_configuration["DirectorioActivo:Dominio"]}\{model.Login}", IsAuthenticated = true });
                return Ok(new { Token = token, Rol = usuario.Rol, Usuario = model.Login, IsAuthenticated = true });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    IsAuthenticated = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("loginFuncionario")]
        [AuditableFilter]
        public async Task<IActionResult> LoginFuncionario(LoginFuncionarioModel loginModel)
        {
            try
            {
                var loginResult = await signInManager.PasswordSignInAsync(loginModel.Usuario, loginModel.Contrasena, isPersistent: false, lockoutOnFailure: true);

                if (!loginResult.Succeeded)
                {
                    if (loginResult.IsLockedOut)
                    {
                        var LoginLink = Url.Action("LoginFuncionario", "Account", new { }, Request.Scheme);
                        var content = string.Format("Tu Cuenta esta Bloqueada por 5 minutos{0}", LoginLink);
                        ModelState.AddModelError("", "The account is locked out");
                        return Ok(new
                        {
                            IsAuthenticated = false,
                            Message = "Tu Cuenta esta Bloqueada por 5 minutos"
                        }); ;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Login Attempt");
                        return Ok(new
                        {
                            IsAuthenticated = false,
                            Message = "Intento Fallido de Autenticación"
                        });
                    }
                }

                var notaria = await _notariaServicio.ObtenerNotariaPorUsuario(loginModel.Usuario);

                var user = await userManager.FindByNameAsync(loginModel.Usuario);

                //await userManager.RemoveAuthenticationTokenAsync(user, "Default", "RefreshToken");

                var role = GetRolesByUser(user.Id).FirstOrDefault();
                //var valor = o.FirstOrDefault(x => x.Type == "NotariaId").Value;
                //var NotariaId = GetNotariaIdUser(userManager.cla).FirstOrDefault();

                //LogInformation(loginModel, "AspNetUsers", user.Id, loginModel.TransaccionGuid.ToString(),
                //    loginModel.Username, "Login");

                var resultado = new
                {
                    IsAuthenticated = true,
                    Token = loginModel.Movil ? "" : await GetTokenFuncionario(user, notaria == null ? 0 : notaria.NotariaId),
                    RegisteredUser = user,
                    Rol = role,
                    Notaria = notaria == null ? 0 : notaria.NotariaId,
                    Logo = notaria == null ? "" : notaria.Logo
                };

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en LoginFuncionario - Error: {ex}");
                throw;
            }
        }

        [HttpPost]
        [Route("validateToken")]
        public async Task<IActionResult> ValidateToken(ValidateTokenModel model)
        {
            if (await _authContext.UserTokens.AnyAsync(u => u.UserId == model.userId && u.Value == model.token && u.Name == "Token"))
                return NoContent();
            return BadRequest(new ErrorDTO() { Errors = new string[] { "Invalid Token" } });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = new ApplicationUser
            {
                UserName = registerModel.Email,
                Email = registerModel.Email
            };

            try
            {
                var identityResult = await this.userManager.CreateAsync(user, registerModel.Password);
                var userId = userManager.GetUserIdAsync(user);
                var notariausuario = new NotariaUsuarios
                {
                    UserEmail = registerModel.Email,
                    NotariaId = registerModel.NotariaId,
                    UsuariosId = userId.Result.ToString(),
                    UsuarioCreacion = registerModel.Email,
                    UsuarioModificacion = registerModel.Email,
                    IsDeleted = false,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now
                };
                _notariaServicio.AgregarNotariaUsuario(notariausuario);
                var rolResul = await this.userManager.AddToRoleAsync(user, registerModel.RolName);

                await signInManager.SignInAsync(user, isPersistent: false);
                return Ok(new { Token = await GetTokenFuncionario(user, registerModel.NotariaId) });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en LoginFuncionario - Error: {ex}");
                throw;
            }
        }

        [HttpPost]
        [Route("EliminarUsuario")]
        public async Task<IActionResult> EliminarUsuario(UsuarioDeleteRequestDTO requestUsuarios)
        {
            ErrorDTO errorDTO = new ErrorDTO();
            using var transaction = _authContext.Database.BeginTransaction();
            try
            {
                await EliminarUsuarioIdentityServer(requestUsuarios);
                await _notariaServicio.EliminarUsuario(requestUsuarios);

                //TODO : Eliminar usuarios en Reconoser
                await transaction.CommitAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ObtenerError(ERRORELIMINACIONUSUARIO, ref errorDTO);
                logger.LogError($"Error en EliminarUsuario - Error: {ex}");
                return BadRequest(errorDTO);
            }
            finally
            {
                transaction.Dispose();
            }
        }


        [HttpPost]
        [Route("EliminarUsuarios")]
        public async Task<IActionResult> EliminarUsuarios(PersonaDeleteRequestDTO RequestUsuarios)
        {
            int resultado = await _personasServicio.EliminarUsuarios(RequestUsuarios).ConfigureAwait(false);
            if (resultado > 0)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario(EditarUsuarioModel registroUsuarioModel)
        {
            ErrorDTO errorDTO = new ErrorDTO();
            using var transaction = _authContext.Database.BeginTransaction();
            try
            {
                string validacionRegistroUsuario = await ValidarUsuarioAActualizar(registroUsuarioModel);
                if (string.IsNullOrEmpty(validacionRegistroUsuario))
                {
                    IdentityResult identityResult = await ActualizarUsuarioIdentityServer(registroUsuarioModel).ConfigureAwait(false);

                    if (identityResult.Succeeded)
                    {
                        int usuarioRolCreado = await ActualizarUsuarioRol(registroUsuarioModel);

                        if (usuarioRolCreado > 0)
                        {
                            NotariaUsuarioCreateDTO notariaUsuarioCreate = new NotariaUsuarioCreateDTO();
                            if (registroUsuarioModel.esUsuarioNotaria)
                                notariaUsuarioCreate = ObtenerObjetoNotariaUsuario(registroUsuarioModel.Id, registroUsuarioModel);

                            PersonaCreateOrUpdateDTO personaCreateOrUpdate = ObtenerObjetoPersona(registroUsuarioModel);
                            List<PersonaDatosDTO> personaDatos = ObtenerObjetoPersonaDatos(registroUsuarioModel);


                            PersonasRequestDTO resultado = await _notariaServicio.ActualizacionPersonaYNotarioUsuario(personaCreateOrUpdate, notariaUsuarioCreate, personaDatos, registroUsuarioModel.Id, registroUsuarioModel.RolId).ConfigureAwait(false);

                            if (resultado != null)
                            {
                                await transaction.CommitAsync();

                                if (registroUsuarioModel.EmailAnterior != resultado.Email)
                                {
                                    var ConfigMovilesReconoser = _configuration.GetSection("ConfigHashMovilesRnce");
                                    var ConfigRecoveryPass = _configuration.GetSection("ConfigUrlReoceveryPass");

                                    InputUserMovilRnec registroUserMovilRnec = new InputUserMovilRnec();
                                    registroUserMovilRnec.Login = resultado.Email;
                                    registroUserMovilRnec.Correo = resultado.Email;
                                    registroUserMovilRnec.IdCliente = resultado.IdCliente;
                                    registroUserMovilRnec.IdRol = resultado.IdRol;
                                    registroUserMovilRnec.Nombre = resultado.Nombres;
                                    registroUserMovilRnec.TipoIdentificacion = resultado.TipoDocumentoAbreviatura;
                                    registroUserMovilRnec.NumeroIdentificacion = resultado.NumeroIdentificacion;
                                    registroUserMovilRnec.IdOficina = resultado.IdOficina;
                                    registroUserMovilRnec.PasswordHash = ConfigMovilesReconoser["Hash"];
                                    registroUserMovilRnec.PhoneNumber = resultado.Celular;
                                    registroUserMovilRnec.Habilitado = true;
                                    registroUserMovilRnec.Cargo = resultado.Cargo;
                                    registroUserMovilRnec.Area = resultado.Area;
                                    registroUserMovilRnec.IdConvenio = resultado.IdConvenio;


                                    // TODO: Revisar actualizacion en Reconocer
                                    var respuesta = await _AgenteReconoser.registrarUsuarioMovilesRnec(registroUserMovilRnec);
                                    if (respuesta.DescripcionError == null)
                                    {
                                        NotariaUsuarios listaNotariaUsuarios = await _notariaServicio.obtenerNotariaUsuarioxEmail(resultado.EmailNotaria);
                                        bool res = await _notariaServicio.ActualizarSincronizacionRNEC(listaNotariaUsuarios.NotariaUsuariosId);
                                    }

                                    RecoveryAccountDTO recoveryAccount = new RecoveryAccountDTO();

                                    recoveryAccount.Email = resultado.Email;
                                    recoveryAccount.UrlRedirect = ConfigRecoveryPass["Url"];

                                    await RecoverPassword(recoveryAccount);
                                }
                                return Ok(resultado);
                            }
                        }
                    }

                    transaction.Rollback();
                    ObtenerError(ERRORACTUALIZACIONUSUARIO, ref errorDTO);
                    return BadRequest(errorDTO);
                }
                else
                {
                    transaction.Rollback();
                    ObtenerError(validacionRegistroUsuario, ref errorDTO);
                    return BadRequest(errorDTO);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ObtenerError(ERRORACTUALIZACIONUSUARIO, ref errorDTO);
                logger.LogError($"Error en ActualizarUsuario - Error: {ex}");
                return BadRequest(errorDTO);
            }
            finally
            {
                transaction.Dispose();
            }
        }


        [HttpPost]
        [Route("RegistroUsuario")]
        public async Task<IActionResult> RegistroUsuario(RegistroUsuarioModel registroUsuarioModel)
        {
            //metodo para registro de usuarios
            ErrorDTO errorDTO = new ErrorDTO();

            using var transaction = _authContext.Database.BeginTransaction();
            try
            {
                string validacionRegistroUsuario = await ValidarUsuarioARegistrar(registroUsuarioModel);
                if (string.IsNullOrEmpty(validacionRegistroUsuario))
                {
                    IdentityResult identityResult = await CrearUsuarioIdentityServer(registroUsuarioModel).ConfigureAwait(false);
                    if (identityResult.Succeeded)
                    {
                        string prueba = await userManager.GetUserIdAsync(new ApplicationUser { UserName = registroUsuarioModel?.UserName, Email = registroUsuarioModel?.EmailNotaria }).ConfigureAwait(false);
                        var usuario = await userManager.FindByEmailAsync(registroUsuarioModel?.EmailNotaria).ConfigureAwait(false);

                        int usuarioRolCreado = await CrearUsuarioRol(usuario.Id, registroUsuarioModel);

                        if (usuarioRolCreado > 0)
                        {
                            NotariaUsuarioCreateDTO notariaUsuarioCreate = new NotariaUsuarioCreateDTO();
                            if (registroUsuarioModel.esUsuarioNotaria)
                            {
                                notariaUsuarioCreate = ObtenerObjetoNotariaUsuario(usuario.Id, registroUsuarioModel);
                            }
                            PersonaCreateOrUpdateDTO personaCreateOrUpdate = ObtenerObjetoPersona(registroUsuarioModel);
                            List<PersonaDatosDTO> personaDatos = ObtenerObjetoPersonaDatos(registroUsuarioModel);


                            PersonasRequestDTO resultado = await _notariaServicio.CreacionPersonaYNotarioUsuario(personaCreateOrUpdate, notariaUsuarioCreate, personaDatos, registroUsuarioModel.RolId).ConfigureAwait(false);

                            if (resultado != null)
                            {
                                await transaction.CommitAsync();
                                var ConfigMovilesReconoser = _configuration.GetSection("ConfigHashMovilesRnce");
                                var ConfigRecoveryPass = _configuration.GetSection("ConfigUrlReoceveryPass");

                                InputUserMovilRnec registroUserMovilRnec = new InputUserMovilRnec();
                                registroUserMovilRnec.Login = resultado.Email;
                                registroUserMovilRnec.Correo = resultado.Email;
                                registroUserMovilRnec.IdCliente = resultado.IdCliente;
                                registroUserMovilRnec.IdRol = resultado.IdRol;
                                registroUserMovilRnec.Nombre = resultado.Nombres;
                                registroUserMovilRnec.TipoIdentificacion = resultado.TipoDocumentoAbreviatura;
                                registroUserMovilRnec.NumeroIdentificacion = resultado.NumeroIdentificacion;
                                registroUserMovilRnec.IdOficina = resultado.IdOficina;
                                registroUserMovilRnec.PasswordHash = ConfigMovilesReconoser["Hash"];
                                registroUserMovilRnec.PhoneNumber = resultado.Celular;
                                registroUserMovilRnec.Habilitado = true;
                                registroUserMovilRnec.Cargo = resultado.Cargo;
                                registroUserMovilRnec.Area = resultado.Area;
                                registroUserMovilRnec.IdConvenio = resultado.IdConvenio;

                                var respuesta = await _AgenteReconoser.registrarUsuarioMovilesRnec(registroUserMovilRnec);
                                if (respuesta.DescripcionError == null)
                                {
                                    NotariaUsuarios listaNotariaUsuarios = await _notariaServicio.obtenerNotariaUsuarioxEmail(resultado.EmailNotaria);
                                    bool res = await _notariaServicio.ActualizarSincronizacionRNEC(listaNotariaUsuarios.NotariaUsuariosId);
                                }
                                RecoveryAccountDTO recoveryAccount = new RecoveryAccountDTO();

                                recoveryAccount.Email = resultado.Email;
                                recoveryAccount.UrlRedirect = ConfigRecoveryPass["Url"];

                                await RecoverPassword(recoveryAccount);
                                return Ok(resultado);
                            }
                        }
                    }

                    transaction.Rollback();
                    ObtenerError(ERRORCREACIONUSUARIO, ref errorDTO);
                    return BadRequest(errorDTO);
                }
                else
                {
                    transaction.Rollback();
                    ObtenerError(validacionRegistroUsuario, ref errorDTO);
                    return BadRequest(errorDTO);
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                logger.LogError($"Error en RegistroUsuario - Error: {ex}");
                ObtenerError(ERRORCREACIONUSUARIO, ref errorDTO);
                return BadRequest(errorDTO);
            }
            finally
            {
                transaction.Dispose();
            }
        }


        [HttpPost]
        [Route("ObntenerUsuarios")]
        public async Task<IActionResult> ObntenerUsuarios(FiltroUserNotariaDTO filtroUserNotaria)
        {
            List<AspNetUsersDTO> aspNetUsers = new List<AspNetUsersDTO>();

            try
            {
                var listaNotariaUsuarios = await _notariaServicio.obtenerListaUsuarios(filtroUserNotaria.NotariaId);
                foreach (NotariaUsuarios usuariosNotaria in listaNotariaUsuarios)
                {
                    var lis = await (from user in _authContext.Users
                                     join ur in _authContext.UserRoles on user.Id equals ur.UserId
                                     where user.Id == usuariosNotaria.UsuariosId
                                     select new
                                     {
                                         Id = user.Id,
                                         Email = user.Email,
                                         EmailConfirmed = user.EmailConfirmed,
                                         UserName = user.UserName,
                                         TwoFactorEnabled = user.TwoFactorEnabled,
                                         LockoutEnabled = user.LockoutEnabled,
                                         NormalizedEmail = user.NormalizedEmail,
                                         NormalizedUserName = user.NormalizedUserName,
                                         RoleNames = (from rol in _authContext.Roles
                                                      where rol.Id == ur.RoleId
                                                      select rol.Name).ToList()
                                     }).SingleOrDefaultAsync();

                    if (lis != null)
                    {
                        AspNetUsersDTO netUsersDTO = new AspNetUsersDTO();
                        netUsersDTO.Email = lis.Email;
                        netUsersDTO.EmailConfirmed = lis.EmailConfirmed;
                        netUsersDTO.Id = lis.Id;
                        netUsersDTO.LockoutEnabled = lis.LockoutEnabled;
                        netUsersDTO.NormalizedEmail = lis.NormalizedEmail;
                        netUsersDTO.NormalizedUserName = lis.NormalizedUserName;
                        netUsersDTO.NombreCompleto = usuariosNotaria.Persona.Nombres + " " + usuariosNotaria.Persona.Apellidos;
                        netUsersDTO.Nombres = usuariosNotaria.Persona.Nombres;
                        netUsersDTO.Apellidos = usuariosNotaria.Persona.Apellidos;
                        netUsersDTO.NumeroIdentificacion = usuariosNotaria.Persona.NumeroDocumento;
                        netUsersDTO.NumeroCelular = usuariosNotaria.Persona.NumeroCelular;
                        netUsersDTO.TipoDocumento = usuariosNotaria.Persona.TipoIdentificacion.Abreviatura;
                        netUsersDTO.TwoFactorEnabled = lis.TwoFactorEnabled;
                        netUsersDTO.UserName = lis.UserName;
                        netUsersDTO.RolName = lis.RoleNames[0];
                        netUsersDTO.Area = usuariosNotaria.Area;
                        netUsersDTO.Cargo = usuariosNotaria.Cargo;
                        netUsersDTO.Genero = usuariosNotaria.Persona.Genero;
                        aspNetUsers.Add(netUsersDTO);
                    }
                }


                return Ok(aspNetUsers);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en ObntenerUsuarios - Error: {ex}");
                throw;
            }
        }

        [HttpPost]
        [Route("ObtenerUsuarioNotariaPorId")]
        public async Task<IActionResult> ObtenerUsuarioNotariaPorId(FiltroUserNotariaDTO filtroUserNotaria)
        {
            List<AspNetUsersDTO> aspNetUsers = new List<AspNetUsersDTO>();

            try
            {
                var usuariosNotaria = await _notariaServicio.ObtenerUsuarioNotariaPorId(filtroUserNotaria.UsuarioId);
                var lis = await (from user in _authContext.Users
                                 join ur in _authContext.UserRoles on user.Id equals ur.UserId
                                 where user.Id == usuariosNotaria.UsuariosId
                                 select new
                                 {
                                     Id = user.Id,
                                     Email = user.Email,
                                     EmailConfirmed = user.EmailConfirmed,
                                     UserName = user.UserName,
                                     TwoFactorEnabled = user.TwoFactorEnabled,
                                     LockoutEnabled = user.LockoutEnabled,
                                     NormalizedEmail = user.NormalizedEmail,
                                     NormalizedUserName = user.NormalizedUserName,
                                     RoleNames = (from rol in _authContext.Roles
                                                  where rol.Id == ur.RoleId
                                                  select rol.Name).ToList()
                                 }).SingleOrDefaultAsync();

                if (lis != null)
                {
                    AspNetUsersDTO netUsersDTO = new AspNetUsersDTO();
                    netUsersDTO.Email = lis.Email;
                    netUsersDTO.EmailConfirmed = lis.EmailConfirmed;
                    netUsersDTO.Id = lis.Id;
                    netUsersDTO.LockoutEnabled = lis.LockoutEnabled;
                    netUsersDTO.NormalizedEmail = lis.NormalizedEmail;
                    netUsersDTO.NormalizedUserName = lis.NormalizedUserName;
                    netUsersDTO.NombreCompleto = usuariosNotaria.Persona.Nombres + " " + usuariosNotaria.Persona.Apellidos;
                    netUsersDTO.Nombres = usuariosNotaria.Persona.Nombres;
                    netUsersDTO.Apellidos = usuariosNotaria.Persona.Apellidos;
                    netUsersDTO.NumeroIdentificacion = usuariosNotaria.Persona.NumeroDocumento;
                    netUsersDTO.NumeroCelular = usuariosNotaria.Persona.NumeroCelular;
                    netUsersDTO.TipoDocumento = usuariosNotaria.Persona.TipoIdentificacion.Abreviatura;
                    netUsersDTO.TwoFactorEnabled = lis.TwoFactorEnabled;
                    netUsersDTO.UserName = lis.UserName;
                    netUsersDTO.RolName = lis.RoleNames[0];
                    netUsersDTO.Area = usuariosNotaria.Area;
                    netUsersDTO.Cargo = usuariosNotaria.Cargo;
                    netUsersDTO.Genero = usuariosNotaria.Persona.Genero;
                    netUsersDTO.NotariaId = usuariosNotaria.NotariaId;
                    aspNetUsers.Add(netUsersDTO);
                }
                return Ok(aspNetUsers.FirstOrDefault());
            }
            catch (Exception ex)
            {
                logger.LogError($"Error en ObtenerUsuarioNotariaPorId - Error: {ex}");
                throw;
            }
        }

        [HttpPost("ObtenerUsuariosPaginado")]
        public async Task<IActionResult> ObtenerUsuariosPaginado(DefinicionFiltro request)
        {
            try
            {
                var query = _notariasUsuarioRepositorio.ObtenerTodo()
                   .AsNoTracking();

                //var cantidad = await _notariasUsuarioRepositorio.ObtenerTodo().LongCountAsync();

                if (request != null && request.Filtros != null && request.Filtros.Any())
                {
                    foreach (var item in request.Filtros)
                    {
                        if (item.Columna == "NotariaId")
                            query = query.Where(m => m.NotariaId == long.Parse(item.Valor));
                        if (item.Columna == "Documento")
                            query = query.Where(m => m.Persona.NumeroDocumento.Contains(item.Valor));
                        if (item.Columna == "Correo")
                            query = query.Where(m => m.UserEmail.Contains(item.Valor));
                    }
                }

                var cantidad = await query.LongCountAsync();

                var data = await query.Skip((request.IndicePagina - 1) * request.RegistrosPagina)
                   .Take(request.RegistrosPagina)
                   .Select(m => new UsuarioDTO
                   {
                       Id = m.UsuariosId,
                       Email = m.UserEmail,
                       UserName = "",
                       RolId = "",
                       RolName = "",
                       NotariaId = m.NotariaId,
                       NotariaName = m.Notaria.Nombre,
                       PersonaId = m.PersonaId,
                       Documento = m.Persona.NumeroDocumento,
                       PersonaName = $"{m.Persona.Nombres} {m.Persona.Apellidos}"
                   }).ToListAsync();

                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var user = await userManager.Users.AsNoTracking()
                            .Include("UserRoles.Role")
                            .Where(m => m.Id == item.Id).FirstOrDefaultAsync();

                        if (user != null)
                        {
                            //item.Email = user.Email;
                            item.UserName = user.UserName;
                            item.RolId = user.UserRoles?.FirstOrDefault()?.RoleId;
                            item.RolName = user.UserRoles?.FirstOrDefault()?.Role?.Name;
                        }
                    }
                }

                return Ok(new PaginableResponse<UsuarioDTO>
                {
                    TotalRows = cantidad,
                    Pages = (int)Math.Ceiling(cantidad / (decimal)request.RegistrosPagina),
                    Data = data
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ChangePasswordModel changePassword)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var usuario = await userManager.FindByEmailAsync(changePassword.Email).ConfigureAwait(false);
                if (usuario != null)
                {
                    var usuarioToken = _authContext.UserTokens.Where(e => e.UserId == usuario.Id && e.LoginProvider == "Reset").FirstOrDefault();
                    if (usuarioToken != null)
                    {
                        if (usuarioToken.ExpirationDate > DateTime.Now)
                        {
                            var response = await userManager.ResetPasswordAsync(usuario, changePassword.Code, changePassword.Password).ConfigureAwait(false);
                            if (response.Succeeded)
                            {
                                await DeleteTokenResetPasword(usuarioToken).ConfigureAwait(false);
                                return Ok();
                            }
                            else
                            {
                                stopwatch.Stop();
                                throw new ArgumentException(response.Errors.FirstOrDefault().Description, $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                            }
                        }
                        else
                        {
                            stopwatch.Stop();
                            throw new ArgumentException(TOKENVENCIDO, $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                        }
                    }
                    else
                    {
                        stopwatch.Stop();
                        throw new ArgumentException(TOKENINVALIDO, $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                    }

                }
                else
                {
                    stopwatch.Stop();
                    throw new ArgumentException(USUARIONOVALIDO, $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError($"Error en ResetPassword - Error: {ex}");
                throw new ArgumentException(ex.Message + $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}", ex);
            }
        }

        [HttpPost]
        [Route("ResetPasswordAdministracion")]
        public async Task<IActionResult> ResetPasswordAdministracion(ChangePasswordModel changePassword)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var usuario = await _usuarioAdministracionServicio.ObtenerUsuarioAdministracion(changePassword.Email).ConfigureAwait(false);

                if (usuario != null)
                {
                    usuario.Password = userManager.PasswordHasher.HashPassword(new ApplicationUser { }, changePassword.Password);
                    await _usuarioAdministracionServicio.GuardarUsuarioAsync(usuario);
                    return Ok();
                }
                else
                {
                    stopwatch.Stop();
                    throw new ArgumentException(USUARIONOVALIDO, $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                logger.LogError($"Error en ResetPassword - Error: {ex}");
                throw new ArgumentException(ex.Message + $" {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}", ex);
            }
        }

        [HttpPost]
        [Route("RecoverPasswordAdministracion")]
        public async Task<IActionResult> RecoverPasswordAdministracion(RecoveryAccountDTO recoveryAccount)
        {
            try
            {
                var usuario = await _usuarioAdministracionServicio.ObtenerUsuarioAdministracion(recoveryAccount?.Email).ConfigureAwait(false);

                if (usuario != null)
                {
                    var token = await GetTokenAdministracion(usuario.UsuarioAdministracionId, usuario.Login, usuario.Rol);
                    //string token = await CrearTokenResetPassword(usuario).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(token))
                    {
                        await NotificarUsuario(recoveryAccount, token).ConfigureAwait(false);
                        return Ok();
                    }

                    ErrorDTO errorDTO = new ErrorDTO();
                    ObtenerError(ERRORUSUARIORESETTOKEN, ref errorDTO);
                    return NotFound(errorDTO);
                }
                else
                {
                    ErrorDTO errorDTO = new ErrorDTO();
                    ObtenerError(CORREOINVALIDO, ref errorDTO);
                    return NotFound(errorDTO);
                }
            }
            catch (Exception ex)
            {
                ErrorDTO errorDTO = new ErrorDTO();
                ObtenerError(ERRORUSUARIORESETTOKEN, ref errorDTO);
                logger.LogError($"Error en RecoverPassword - Error: {ex}");
                return NotFound(errorDTO);
            }
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(RecoveryAccountDTO recoveryAccount)
        {
            try
            {
                var usuario = await userManager.FindByEmailAsync(recoveryAccount?.Email).ConfigureAwait(false);
                if (usuario != null)
                {
                    var userTokenReset = await _authContext.UserTokens.Where(
                        x => x.UserId == usuario.Id
                        && x.LoginProvider == "Reset"
                        && x.Name == "ResetPasswordToken"
                        && x.IsDisabled == false).FirstOrDefaultAsync().ConfigureAwait(false);

                    if (userTokenReset != null)
                    {
                        bool esUserDeleteReset = await DeleteTokenResetPasword(userTokenReset).ConfigureAwait(false);
                        if (esUserDeleteReset)
                        {
                            string token = await CrearTokenResetPassword(usuario).ConfigureAwait(false);
                            if (!string.IsNullOrEmpty(token))
                            {
                                await NotificarUsuario(recoveryAccount, token).ConfigureAwait(false);
                                return Ok();
                            }
                        }
                        ErrorDTO errorDTO = new ErrorDTO();
                        ObtenerError(ERRORUSUARIORESETTOKEN, ref errorDTO);
                        return NotFound(errorDTO);
                    }
                    else
                    {
                        string token = await CrearTokenResetPassword(usuario).ConfigureAwait(false);
                        if (!string.IsNullOrEmpty(token))
                        {
                            await NotificarUsuario(recoveryAccount, token).ConfigureAwait(false);
                            return Ok();
                        }

                        ErrorDTO errorDTO = new ErrorDTO();
                        ObtenerError(ERRORUSUARIORESETTOKEN, ref errorDTO);
                        return NotFound(errorDTO);
                    }
                }
                else
                {
                    ErrorDTO errorDTO = new ErrorDTO();
                    ObtenerError(CORREOINVALIDO, ref errorDTO);
                    return NotFound(errorDTO);
                }
            }
            catch (Exception ex)
            {
                ErrorDTO errorDTO = new ErrorDTO();
                ObtenerError(ERRORUSUARIORESETTOKEN, ref errorDTO);
                logger.LogError($"Error en RecoverPassword - Error: {ex}");
                return NotFound(errorDTO);
            }
        }

        [HttpPost]
        [Route("AsignarPasswordAdministracion")]
        public async Task<IActionResult> AsignarPasswordAdministracion(ChangePasswordModel changePassword)
        {
            try
            {
                var usuario = await _usuarioAdministracionServicio.ObtenerUsuarioAdministracion(changePassword.Email).ConfigureAwait(false);

                if (usuario != null)
                {
                    bool esTokenValido = await _usuarioAdministracionServicio.ValidarTokenUsuarioAdministrador(changePassword.Code, usuario.UsuarioAdministracionId);
                    if (esTokenValido)
                    {
                        usuario.Password = userManager.PasswordHasher.HashPassword(new ApplicationUser { }, changePassword.Password);
                        await _usuarioAdministracionServicio.GuardarUsuarioAsync(usuario);

                        await _usuarioAdministracionServicio.EliminarTokenUsuarioAdministrador(changePassword.Code, usuario.UsuarioAdministracionId);

                        return Ok();
                    }
                    else
                    {
                        await _usuarioAdministracionServicio.EliminarTokenUsuarioAdministrador(changePassword.Code, usuario.UsuarioAdministracionId);
                        throw new ArgumentException("Enlace vencido, por favor dirigirse al inicio de sesión y dar clic en \"Olvidé mi contraseña\"");
                    }
                }
                else
                    throw new ArgumentException("Usuario no encontrado, por favor comuníquese con su administrador.");
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private async Task<string> GetToken(RegisteredUserDTO registeredUser)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new Claim[]
           {
                        new Claim(JwtRegisteredClaimNames.Sub, registeredUser.PersonaId.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, registeredUser.NumeroIdentificacion),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                        new Claim(JwtRegisteredClaimNames.FamilyName, registeredUser.Nombre_Completo),
                        //new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                        new Claim("NotariaId","2")
           };


            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime")),
                audience: this._configuration.GetValue<String>("Tokens:Audience"),
                issuer: this._configuration.GetValue<String>("Tokens:Issuer")
                );

            string newToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            //ApplicationUserToken userToken = _authContext.UserTokens.SingleOrDefault(u => u.UserId == persona.Id && u.Name=="Token");
            //if (userToken == null)
            //    _authContext.Add(new ApplicationUserToken()
            //    {
            //        UserId = persona.Id,
            //        LoginProvider = "Default",
            //        Name = "Token",
            //        Value = newToken,
            //        ExpirationDate = utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime"))
            //    });
            //else
            //{
            //    userToken.Value = newToken;
            //    userToken.ExpirationDate = utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime"));
            //}
            //_authContext.SaveChanges();

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private async Task<string> GetTokenFuncionario(ApplicationUser user, long idNotaria)
        {

            var utcNow = DateTime.UtcNow;
            var roles = GetRolesByUser(user.Id);


            var claims = new Claim[]
           {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                        new Claim(ClaimTypes.Role, roles.FirstOrDefault()),
                        new Claim("NotariaId", idNotaria.ToString())
           };
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime")),
                audience: this._configuration.GetValue<String>("Tokens:Audience"),
                issuer: this._configuration.GetValue<String>("Tokens:Issuer")
                );

            string newToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            ApplicationUserToken userToken = _authContext.UserTokens.SingleOrDefault(u => u.UserId == user.Id && u.Name == "Token");
            if (userToken == null)
                _authContext.Add(new ApplicationUserToken()
                {
                    UserId = user.Id,
                    LoginProvider = "Default",
                    Name = "Token",
                    Value = newToken,
                    ExpirationDate = utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime"))
                });
            else
            {
                userToken.Value = newToken;
                userToken.ExpirationDate = utcNow.AddSeconds(this._configuration.GetValue<int>("Tokens:Lifetime"));
            }
            _authContext.SaveChanges();

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<string> GetTokenAdministracion(long usuarioid, string login, string rol)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioid.ToString()),
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

        private List<string> GetRolesByUser(string userId)
        {
            var UserRole = _authContext.UserRoles.Where(UR => UR.UserId == userId).FirstOrDefault();
            return _authContext.Roles.Where(r => r.Id == UserRole.RoleId).Select(r => r.Name).ToList();
        }

        private async Task<string> ValidarUsuarioAActualizar(EditarUsuarioModel registroUsuarioModel)
        {
            string resultado = string.Empty;
            if (string.IsNullOrEmpty(registroUsuarioModel.NumeroCelular))
                resultado = string.Format(CAMPOVACIO, "Número Celular");
            if (string.IsNullOrEmpty(registroUsuarioModel.Area))
                resultado += string.Format(CAMPOVACIO, "Area");
            if (string.IsNullOrEmpty(registroUsuarioModel.Cargo))
                resultado += string.Format(CAMPOVACIO, "Cargo");
            if (registroUsuarioModel.Genero <= 0 || registroUsuarioModel.Genero > 2)
                resultado += string.Format(CAMPOVACIO, "Género");

            var usuarioExistente = await userManager.FindByEmailAsync(
                registroUsuarioModel.EmailNotaria.ToLower()).ConfigureAwait(false);
            if (usuarioExistente != null && usuarioExistente.Id != registroUsuarioModel.Id)
                resultado += EMAILEXISTENTE;
            else
            {
                var rolExistente = _authContext.Roles.Where(x => x.Id == registroUsuarioModel.RolId.ToString(CultureInfo.InvariantCulture));

                if (rolExistente.Any())
                {
                    var rolUsuario = (await userManager.GetRolesAsync(usuarioExistente)).FirstOrDefault();

                    var usuarioPeticion = await userManager.FindByIdAsync(registroUsuarioModel.UserId.ToLower()).ConfigureAwait(false);
                    var rolUsuarioPeticion = (await userManager.GetRolesAsync(usuarioPeticion)).FirstOrDefault();
                    if (rolUsuario == "Administrador" && rolUsuarioPeticion != "Administrador")
                    {
                        resultado += string.Format(ROLADMINISTRADORINVALIDO, "Rol");
                    }

                    var notariaExistente = await _notariaServicio.ObtenerNotariaPorId(registroUsuarioModel.NotariaId).ConfigureAwait(false);
                    if (notariaExistente == null)
                        return resultado += NOTARIANOVALIDA;

                    var tipoIdentificacion = await _tipoIdentificacionServicio.ObtenerTipoIdentificacionPorId(registroUsuarioModel.TipoIdentificacionId).ConfigureAwait(false);

                    if (tipoIdentificacion == null)
                        return resultado += TIPOIDENTIFCACIONNOVALIDO;

                }
                else
                    resultado += ROLNOVALIDO;
            }
            return resultado;
        }

        private async Task<string> ValidarUsuarioARegistrar(RegistroUsuarioModel registroUsuarioModel)
        {
            string resultado = string.Empty;
            var usuarioExistente = await userManager.FindByEmailAsync(registroUsuarioModel.EmailNotaria.ToLower()).ConfigureAwait(false);

            var rolExistente = _authContext.Roles.Where(x => x.Id == registroUsuarioModel.RolId.ToString(CultureInfo.InvariantCulture));

            if (registroUsuarioModel.Genero <= 0 || registroUsuarioModel.Genero > 2)
                resultado = string.Format(CAMPOVACIO, "Género");
            if (string.IsNullOrWhiteSpace(registroUsuarioModel.NumeroCelular))
                resultado = string.Format(CAMPOVACIO, "Celular");

            if (usuarioExistente == null && rolExistente.Any())
            {
                var notariaExistente = await _notariaServicio.ObtenerNotariaPorId(registroUsuarioModel.NotariaId).ConfigureAwait(false);
                if (notariaExistente == null)
                    return resultado = NOTARIANOVALIDA;

                var tipoIdentificacion = await _tipoIdentificacionServicio.ObtenerTipoIdentificacionPorId(registroUsuarioModel.TipoIdentificacionId).ConfigureAwait(false);

                if (tipoIdentificacion == null)
                    return resultado = TIPOIDENTIFCACIONNOVALIDO;
            }
            else
                resultado = !rolExistente.Any() ? ROLNOVALIDO : USUARIOEXISTENTE;

            return resultado;
        }

        private async Task<IdentityResult> ActualizarUsuarioIdentityServer(EditarUsuarioModel registroUsuarioModel)
        {
            var user = await userManager.FindByIdAsync(registroUsuarioModel.Id);

            user.UserName = registroUsuarioModel?.EmailNotaria;
            user.Email = registroUsuarioModel?.EmailNotaria;

            return await userManager.UpdateAsync(user).ConfigureAwait(false);
        }

        private async Task<IdentityResult> EliminarUsuarioIdentityServer(UsuarioDeleteRequestDTO registroUsuarioModel)
        {
            var user = await userManager.FindByIdAsync(registroUsuarioModel.Id);
            if (user != null)
                return await userManager.DeleteAsync(user);
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> CrearUsuarioIdentityServer(RegistroUsuarioModel registroUsuarioModel)
        {
            var user = new ApplicationUser
            {
                UserName = registroUsuarioModel?.EmailNotaria,
                Email = registroUsuarioModel?.EmailNotaria
            };
            return await userManager.CreateAsync(user, "Olimpia.01").ConfigureAwait(false);
        }

        private async Task<int> ActualizarUsuarioRol(EditarUsuarioModel registroUsuarioModel)
        {
            _authContext.UserRoles.Remove(_authContext.UserRoles.Single(p => p.UserId == registroUsuarioModel.Id));
            await _authContext.SaveChangesAsync().ConfigureAwait(false);
            return await CrearUsuarioRol(registroUsuarioModel.Id, registroUsuarioModel);
        }

        private async Task<int> CrearUsuarioRol(string userId, RegistroUsuarioModel registroUsuarioModel)
        {
            await _authContext.UserRoles.AddAsync(
                new ApplicationUserRole
                {
                    UserId = userId,
                    RoleId = registroUsuarioModel.RolId.ToString()
                });
            return await _authContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private void ObtenerError(string errorMensaje, ref ErrorDTO error)
        {
            string[] errores = new string[1];
            errores[0] = errorMensaje;
            error.Errors = errores;
        }
        private NotariaUsuarioCreateDTO ObtenerObjetoNotariaUsuario(string userId, RegistroUsuarioModel registroUsuarioModel)
        {
            NotariaUsuarioCreateDTO notaria = new NotariaUsuarioCreateDTO();
            notaria.NotariaId = registroUsuarioModel.NotariaId;
            notaria.UsuarioId = userId;
            notaria.UsuarioEmail = registroUsuarioModel?.EmailNotaria;
            notaria.UserId = registroUsuarioModel?.UserId;
            notaria.UserName = registroUsuarioModel?.UserName;
            notaria.Area = registroUsuarioModel?.Area;
            notaria.Cargo = registroUsuarioModel?.Cargo;
            notaria.Celular = registroUsuarioModel?.NumeroCelular;
            return notaria;
        }
        private PersonaCreateOrUpdateDTO ObtenerObjetoPersona(RegistroUsuarioModel registroUsuarioModel)
        {
            PersonaCreateOrUpdateDTO personaRequest = new PersonaCreateOrUpdateDTO();
            personaRequest.NotariaId = registroUsuarioModel.NotariaId;
            personaRequest.UserId = registroUsuarioModel?.UserId;
            personaRequest.UserName = registroUsuarioModel?.UserName;
            personaRequest.NumeroDocumento = registroUsuarioModel?.NumeroDocumento;
            personaRequest.Nombres = registroUsuarioModel?.Nombres;
            personaRequest.Apellidos = registroUsuarioModel?.Apellidos;
            personaRequest.Email = registroUsuarioModel?.EmailPersonal;
            personaRequest.NumeroCelular = registroUsuarioModel?.NumeroCelular;
            personaRequest.TipoIdentificacionId = registroUsuarioModel.TipoIdentificacionId;
            personaRequest.Genero = registroUsuarioModel.Genero;
            return personaRequest;
        }

        private List<PersonaDatosDTO> ObtenerObjetoPersonaDatos(RegistroUsuarioModel registroUsuarioModel)
        {
            //metodo que construye objeto de personadatos
            List<PersonaDatosDTO> personaDatoRequest = new List<PersonaDatosDTO>();

            foreach (PersonaDatosModel personaDatos in registroUsuarioModel.personaDatos)
            {
                PersonaDatosDTO DatoPersona = new PersonaDatosDTO();
                DatoPersona.TipoDatoId = personaDatos.TipoDatoId;
                DatoPersona.Valor = personaDatos.ValorDato;
                personaDatoRequest.Add(DatoPersona);
            }
            return personaDatoRequest;
        }

        private async Task<string> CrearTokenResetPassword(ApplicationUser userresult)
        {
            string respuesta = string.Empty;
            var token = await userManager.GeneratePasswordResetTokenAsync(userresult).ConfigureAwait(false);
            _authContext.UserTokens.Add(new ApplicationUserToken
            {
                UserId = userresult.Id,
                LoginProvider = "Reset",
                Name = "ResetPasswordToken",
                Value = token,
                ExpirationDate = DateTime.Now.AddHours(2),
                IsDisabled = false
            });
            int esCreado = await _authContext.SaveChangesAsync();
            if (esCreado > 0)
            {
                respuesta = token;
            }
            return respuesta;
        }

        private async Task NotificarUsuario(RecoveryAccountDTO recoveryAccount, string token)
        {
            List<string> destinatarios = new List<string>();
            destinatarios.Add(recoveryAccount.Email);
            var callbackUrl = new Uri($"{recoveryAccount.UrlRedirect}?token={HttpUtility.UrlEncode(token)}&email={recoveryAccount.Email}").ToString();

            string cuerpoHMTL = ObtenerCuerpoHTML(recoveryAccount.Email, callbackUrl);

            string htmldocument = cuerpoHMTL;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmldocument, null, "text/html");
            await _manejadorCorreos.EnviarCorreo(destinatarios
                    , "Recuperación de Contraseña"
                    , htmldocument, htmlView);
        }

        private string ObtenerCuerpoHTML(string Email, string enlace)
        {
            return _templateServicio.ObtenerTemplateRecuperacionPassword(Email, enlace);
        }

        private async Task<bool> DeleteTokenResetPasword(ApplicationUserToken userTokens)
        {
            bool respuesta = false;
            _authContext.UserTokens.Remove(userTokens);
            int esEliminado = await _authContext.SaveChangesAsync().ConfigureAwait(false);
            if (esEliminado > 0)
                respuesta = true;

            return respuesta;
        }

        /// <summary>
        /// Verifica si un usuario se encuentra autenticado
        /// </summary>
        /// <param name="server"></param>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private bool IsAuthenticated(string server, string username, string pwd, string domain)
        {
            const int ldapErrorInvalidCredentials = 0x31;

            using (var connection = new LdapConnection(server))
            {
                try
                {
                    NetworkCredential credential = new NetworkCredential(username, pwd, domain);
                    if (string.IsNullOrEmpty(domain))
                    {
                        connection.SessionOptions.SecureSocketLayer = true;
                        connection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);
                        connection.AuthType = AuthType.Negotiate;
                    }
                    //else
                    //    connection.Credential = credential;
                    connection.Bind(credential);
                    return true;
                }
                catch (LdapException ex)
                {
                    if (ex.ErrorCode.Equals(ldapErrorInvalidCredentials))
                    {
                        throw new LdapException("Error en la autenticación por directorio activo");
                    }

                    throw ex;
                }
            };
        }
    }
}