using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.ContextoPrincipal.Utils;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Microsoft.AspNetCore.Mvc;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public class AuthenticatorServicio : BaseServicio, IAuthenticatorServicio
    {

        private IPersonasRepositorio _personasRepositorio { get; }


        public AuthenticatorServicio(
            IPersonasRepositorio personasRepositorio
            ) : base(personasRepositorio)
        {
            _personasRepositorio = personasRepositorio;
        }

        public Task<IActionResult> VerificarUsuarioOTP(TramiteCreateDTO tramite)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDTO> CreateUserToken(string numeroDocumento)
        {
            ResponseDTO response = new ResponseDTO();
            response.EsValido = false;

            Persona persona = await _personasRepositorio.Personas_Obtener(numeroDocumento);

            if (persona == null)
            {
                //la persona no existe
                response.Mensaje = "La persona aun no se ha creado en la base de datos";
            }
            else if(persona.tokenAuth != null)
            {
                response.Mensaje = "La persona ya tiene un token de autenticacion";
            }
            else
            {
                //se crea el registro con un token para el usuario y se
                //le envia un correo con el qr
                UtilAuthenticator u = new UtilAuthenticator();
                string token = u.createUserToken(numeroDocumento);

                //AuthenticatorConfig conf = new AuthenticatorConfig()
                //{
                //    IdUsuarioConvenioRNEC = usr.IdUsuarioConvenioRNEC,
                //    IdCliente = idCliente,
                //    Token = token,
                //    Habilitado = true,
                //    FechaCreacion = DateTime.Now,
                //    UsuarioCreacion = "Automatico",
                //};

                //var qr = u.GenerateImage(userLogin, token);

                //Olimpia.Reconoser.Dominio.ServiciosZeus.Correo correo = new Olimpia.Reconoser.Dominio.ServiciosZeus.Correo();
                //Contacto contacto = new Contacto();

                //MemoryStream memoryStream = new MemoryStream();
                //qr.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                //contacto.Asunto = "Mail de prueba";
                //contacto.Clave = "Olimpia.2016";
                //contacto.Email = userLogin;
                //contacto.Mensaje = "Use el qr adjunto para agregar su cuenta en la app del authenticator de reconoser.";
                //contacto.nombrePlantilla = "RestablecerContrasena.html";
                //contacto.rutaPlantilla = @"\PlantillasCorreo\";
                //contacto.qr = memoryStream;
                //response = correo.EnviarMensajeOlimpia(contacto);

                ////no guardar el usuario si no se envia el correo
                //if (!response)
                //    return response;

                persona.tokenAuth = token;
                _personasRepositorio.Modificar(persona);
                _personasRepositorio.UnidadDeTrabajo.Commit();
            }
            
            return response;
        }

        public async Task<ResponseDTO> AutenticarOtp(OtpAuthenticator authModel)
        {
            ResponseDTO response = new ResponseDTO();
            response.EsValido = false;

            Persona persona = await _personasRepositorio.Personas_Obtener(authModel.usrDocument);

            if (persona == null)
            {
                //la persona no existe
                response.Mensaje = "La persona aun no se ha creado en la base de datos";
            }
            else 
            {
                string token = persona.tokenAuth;
                UtilAuthenticator u = new UtilAuthenticator();
                response.EsValido = u.AutenticarOtp(authModel.pin, token);
            }

            return response;
        }
    }
}
