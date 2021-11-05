using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using GenericExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class PersonasServicio : BaseServicio, IPersonasServicio
    {
        #region Const - Mensajes
        private const string NOTARIAINCORRECTA = "El número de notaria es incorrecta";
        private const string ERRORALELIMINARUSUARIOS = "Error al eliminar los usuarios seleccionados";
        private const string ERRORIDENTIFICACIONES = "Ingrese 1 o más números de indentificación";
        private const string SINUSUARIOS = "La notaria no contiene ningún usuario";
        private const string USUARIONOEXISTE = "El usuario identificado con el número ";
        #endregion
        public IPersonasRepositorio _personasRepositorio { get; }
        public INotariasUsuarioRepositorio _notariasUsuarioRepositorio { get; }
        public INotarioRepositorio _notarioRepositorio { get; }
        public PersonasServicio(
            IPersonasRepositorio personasRepositorio,
            INotariasUsuarioRepositorio notariasUsuarioRepositorio,
            INotarioRepositorio notarioRepositorio)
            : base(personasRepositorio,
                  notariasUsuarioRepositorio,
                  notarioRepositorio)
        {
            _personasRepositorio = personasRepositorio;
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _notarioRepositorio = notarioRepositorio;
        }
        public async Task<int> CrearActualizarPersona(PersonaCreateOrUpdateDTO persona)
        {
            int resultado = 0;
            var esPersonaExistente = (await _personasRepositorio.Obtener(
               x => x.IsDeleted == false
            && x.NumeroDocumento == persona.NumeroDocumento
            && x.TipoIdentificacionId == persona.TipoIdentificacionId).ConfigureAwait(false)).Any();

            if (esPersonaExistente)
            {
                var personaExistente = (await _personasRepositorio.Obtener(
               x => x.IsDeleted == false
            && x.NumeroDocumento == persona.NumeroDocumento
            && x.TipoIdentificacionId == persona.TipoIdentificacionId).ConfigureAwait(false)).FirstOrDefault();

                var personaRequest = persona.Adaptar<Persona>();
                personaRequest.PersonaId = personaExistente.PersonaId;
                _personasRepositorio.Modificar(personaRequest);
                resultado = await _personasRepositorio.UnidadDeTrabajo.CommitAsync();
            }
            else
            {
                var personaRequest = persona.Adaptar<Persona>();
                _personasRepositorio.Agregar(personaRequest);
                resultado = await _personasRepositorio.UnidadDeTrabajo.CommitAsync();
            }

            return resultado;
        }

        public async Task<int> EliminarUsuarios(PersonaDeleteRequestDTO persona)
        {
            try
            {
                //IdentityResult identityResult = await ActualizarUsuarioIdentityServer(registroUsuarioModel).ConfigureAwait(false);


                ValidarRequestEliminacionUsuarios(persona);
                await ValidarUsuariosANotaria(persona);
                List<NotariaUsuarios> usuariosNotaria = 
                    (await _notariasUsuarioRepositorio
                        .Obtener(x => !x.IsDeleted && x.NotariaId == persona.NotariaId, 
                        nu=>nu.Persona, 
                        nu=> nu.Notario))
                        .ToList();

                if (usuariosNotaria.Count > 0)
                {
                    foreach (var item in persona.Identificacion)
                    {
                        var usuario = usuariosNotaria.Where(i => i.Persona.NumeroDocumento == item).FirstOrDefault();
                        if (usuario != null)
                        {
                            _notariasUsuarioRepositorio.Eliminar(usuario);
                            if (usuario.Persona != null)
                            {
                                _personasRepositorio.Eliminar(usuario.Persona);
                                if (usuario.Notario != null)
                                {
                                    _notarioRepositorio.Eliminar(usuario.Notario);
                                }
                            }
                            else
                            {
                                _notariasUsuarioRepositorio.UnidadDeTrabajo.RollbackChanges();
                                throw new ArgumentException(ERRORALELIMINARUSUARIOS);
                            }
                        }
                        else
                        {
                            _notariasUsuarioRepositorio.UnidadDeTrabajo.RollbackChanges();
                            throw new ArgumentException(ERRORALELIMINARUSUARIOS);
                        }
                    }
                    return await _notariasUsuarioRepositorio.UnidadDeTrabajo.CommitAsync();
                }
                throw new ArgumentException(ERRORALELIMINARUSUARIOS);
            }
            catch (Exception)
            {
                _notariasUsuarioRepositorio.UnidadDeTrabajo.RollbackChanges();
                throw;
            }
        }

        private void ValidarRequestEliminacionUsuarios(PersonaDeleteRequestDTO persona)
        {
            if (persona != null)
            {
                if (persona.NotariaId <= 0)
                    throw new ArgumentException(NOTARIAINCORRECTA);
                if (persona.Identificacion == null)
                    throw new ArgumentException(ERRORALELIMINARUSUARIOS);
                if (persona.Identificacion.Count <= 0)
                    throw new ArgumentException(ERRORIDENTIFICACIONES);
            }
            else
                throw new ArgumentException(ERRORALELIMINARUSUARIOS);

        }

        private async Task ValidarUsuariosANotaria(PersonaDeleteRequestDTO persona)
        {
            IEnumerable<NotariaUsuarios> usuariosNotaria =
                await _notariasUsuarioRepositorio
                        .Obtener(x => !x.IsDeleted && x.NotariaId == persona.NotariaId, 
                        nu=>nu.Persona);

            if (usuariosNotaria.Any())
            {
                foreach (var item in persona.Identificacion)
                {
                    bool esUsuarioExistente = usuariosNotaria.Where(i => i.Persona.NumeroDocumento == item).Any();
                    if (!esUsuarioExistente)
                        throw new ArgumentException($"{USUARIONOEXISTE}{item} no se encuentra asociado a la notaria");
                }
            }
            else
                throw new ArgumentException(SINUSUARIOS);
        }

        public Task<Persona> ConsultarPersonaPorCorreo(string correo)
            => _personasRepositorio.ObtenerPorCorreo(correo);
    }
}
