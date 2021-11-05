using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.Nucleo;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Repositorios;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructura.ContextoPrincipal.Repositorios
{
    public class NotariaRepositorio : RepositorioBase<Notaria>, INotariaRepositorio
    {
        #region Miembros
        private readonly UnidadTrabajo _unidadTrabajoContextoPrincipal;
        private IUnidadDeTrabajo UnidadTrabajoContextoPrincipal => _unidadTrabajoContextoPrincipal;

        private INotariasUsuarioRepositorio _notariasUsuariosRepositorio;
        #endregion

        #region Constructor
        public NotariaRepositorio(UnidadTrabajo unidadTrabajoContextoPrincipal, 
            IHttpContextAccessor httpContext, 
            INotariasUsuarioRepositorio notariasUsuariosRepositorio) : base(unidadTrabajoContextoPrincipal, httpContext)
        {
            _unidadTrabajoContextoPrincipal = unidadTrabajoContextoPrincipal ?? throw new ArgumentNullException(nameof(unidadTrabajoContextoPrincipal));
            _notariasUsuariosRepositorio = notariasUsuariosRepositorio;
        }

        #endregion

        #region Contratos
        public async Task<Guid> CrearPersonaYNotariaUsuario(Persona persona, NotariaUsuarios notariaUsuarios, List<PersonaDatos> personaDatos)
        {
            using var transaction = await _unidadTrabajoContextoPrincipal.Database.BeginTransactionAsync();

            try
            {
                Persona transaccionPersona = await CrearActualizarPersona(persona);

                if (transaccionPersona.PersonaId > 0)
                {
                    int transaccionPersonaDato = await CrearActualizarPersonaDatos(personaDatos, transaccionPersona.PersonaId);

                    notariaUsuarios.PersonaId = transaccionPersona.PersonaId;
                    if (notariaUsuarios != null)
                    {
                        int transaccionNotariaUsuario = await CrearNotariaUsuario(notariaUsuarios);
                    }

                    await transaction.CommitAsync().ConfigureAwait(false);
                    return transaction.TransactionId;
                }
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw new Exception("Error al crear Notaria usuario ó persona");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        } 

        public Task<ConvenioRNEC> ObtenerConvenioRNEC(long notariaID)
        {
            return _unidadTrabajoContextoPrincipal.ConveniosRNEC
                .Where(c => c.NotariaId == notariaID)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<NotariaUsuarios>> ObtenerListadoUsuariosNotaria(long NotariaId)
        {
            return await _notariasUsuariosRepositorio.Obtener(x => x.IsDeleted == false && x.NotariaId == NotariaId, un => un.Persona, un => un.Persona.TipoIdentificacion);            
        }

        public Persona obtenerPersonaId(long personaId)
        {
            Persona personaExistente = new Persona();
            personaExistente = _unidadTrabajoContextoPrincipal.Persona.Where(
                x => x.IsDeleted == false
                && x.PersonaId == personaId).FirstOrDefault();
            return personaExistente;
        }

        public NotariaUsuarios obtenerNotariaUsuarioxEmail(string Email)
        {
            NotariaUsuarios NotariaUsuariosExistente = new NotariaUsuarios();
            NotariaUsuariosExistente = _unidadTrabajoContextoPrincipal.NotariaUsuarios.Where(
                x => x.IsDeleted == false
                && x.UserEmail == Email).FirstOrDefault();
            return NotariaUsuariosExistente;
        }
        public NotariaUsuarios ObtenerUsuarioNotariaPorId(Guid UsuarioId)
        {
            NotariaUsuarios NotariaUsuariosExistente = new NotariaUsuarios();
            NotariaUsuariosExistente = _notariasUsuariosRepositorio.Obtener(
                x => x.IsDeleted == false
                && x.UsuariosId == UsuarioId.ToString(), un => un.Persona, un => un.Persona.TipoIdentificacion).Result.FirstOrDefault();
            return NotariaUsuariosExistente;
        }

        public async Task<bool> ActualizarSincronizacionRNEC(long notariaUsuarioId)
        {
            NotariaUsuarios notariaUsuariosExistente = new NotariaUsuarios();

            notariaUsuariosExistente = _unidadTrabajoContextoPrincipal.NotariaUsuarios.Where(
                x => x.IsDeleted == false
                && x.NotariaUsuariosId == notariaUsuarioId).FirstOrDefault();

            notariaUsuariosExistente.SincronizoRNEC = true;
            try
            {
                _contextoUnidadDeTrabajo.SetModified(notariaUsuariosExistente);
                await _unidadTrabajoContextoPrincipal.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<NotariaCliente>> Obtener()
        {
            FormattableString query = $"EXECUTE [Parametricas].[ObtenerNotarias]";
            var notarias = await _unidadTrabajoContextoPrincipal.NotariaCliente.FromSqlInterpolated(query).ToListAsync();
            return notarias;
        }

        #endregion

        #region Métodos Privados
        private async Task<Persona> CrearActualizarPersona(Persona persona)
        {
            Persona personaExistente = new Persona();
            var esPersonaExistente = _unidadTrabajoContextoPrincipal.Persona.Where(
               x => x.IsDeleted == false
            && x.NumeroDocumento == persona.NumeroDocumento
            && x.TipoIdentificacionId == persona.TipoIdentificacionId).Any();

            if (esPersonaExistente)
            {
                personaExistente = _unidadTrabajoContextoPrincipal.Persona.Where(
                    x => x.IsDeleted == false
                    && x.NumeroDocumento == persona.NumeroDocumento
                    && x.TipoIdentificacionId == persona.TipoIdentificacionId).FirstOrDefault();

                personaExistente.Nombres = persona.Nombres;
                personaExistente.Apellidos = persona.Apellidos;
                personaExistente.Email = persona.Email;
                personaExistente.NumeroCelular = persona.NumeroCelular;
                personaExistente.tokenAuth = persona.tokenAuth;
                personaExistente.Genero = persona.Genero;

                _contextoUnidadDeTrabajo.SetModified(personaExistente);
                await _unidadTrabajoContextoPrincipal.SaveChangesAsync();
                personaExistente = _unidadTrabajoContextoPrincipal.Persona.Where(
                    x => x.IsDeleted == false
                    && x.NumeroDocumento == persona.NumeroDocumento
                    && x.TipoIdentificacionId == persona.TipoIdentificacionId).FirstOrDefault();
            }
            else
            {
                await _unidadTrabajoContextoPrincipal.AddAsync(persona);
                await _unidadTrabajoContextoPrincipal.SaveChangesAsync();
                personaExistente = _unidadTrabajoContextoPrincipal.Persona.Where(
                   x => x.IsDeleted == false
                   && x.NumeroDocumento == persona.NumeroDocumento
                   && x.TipoIdentificacionId == persona.TipoIdentificacionId).FirstOrDefault();
            }

            return personaExistente;
        }



        private async Task<int> CrearActualizarPersonaDatos(List<PersonaDatos> personaDatos, long personaId)
        {
            int resultado = 0;
            foreach (PersonaDatos datoPersona in personaDatos)
            {
                var esDatoPersonaExistente = _unidadTrabajoContextoPrincipal.PersonaDatos.Where(
                    x => x.IsDeleted == false
                    && x.TipoDatoId == datoPersona.TipoDatoId
                    && x.PersonaId == datoPersona.PersonaId).Any();

                if (esDatoPersonaExistente)
                {
                    var personaDatosExistente = _unidadTrabajoContextoPrincipal.PersonaDatos.Where(
                         x => x.IsDeleted == false
                         && x.TipoDatoId == datoPersona.TipoDatoId
                         && x.PersonaId == datoPersona.PersonaId).FirstOrDefault();

                    personaDatosExistente.PersonaId = datoPersona.PersonaId;
                    personaDatosExistente.TipoDatoId = datoPersona.TipoDatoId;
                    personaDatosExistente.IsDeleted = datoPersona.IsDeleted;
                    personaDatosExistente.PersonaId = personaId;
                    personaDatosExistente.ValorDato = datoPersona.ValorDato;

                    _contextoUnidadDeTrabajo.SetModified(personaDatosExistente);
                    resultado = await _unidadTrabajoContextoPrincipal.SaveChangesAsync();

                }
                else
                {
                    datoPersona.PersonaId = personaId;
                    await _unidadTrabajoContextoPrincipal.AddAsync(datoPersona);
                    resultado = await _unidadTrabajoContextoPrincipal.SaveChangesAsync();

                }
            }

            return resultado; ;
        }


        private async Task<int> CrearNotariaUsuario(NotariaUsuarios notariaUsuarios)
        {            
            var usuarioExistente = _unidadTrabajoContextoPrincipal.NotariaUsuarios.Where(x => x.IsDeleted == false && x.UsuariosId == notariaUsuarios.UsuariosId).Any();
            if (!usuarioExistente)
                await _unidadTrabajoContextoPrincipal.AddAsync(notariaUsuarios).ConfigureAwait(false);
            else
            {
                var mNotaria = _unidadTrabajoContextoPrincipal.NotariaUsuarios.Single(x => x.IsDeleted == false && x.UsuariosId == notariaUsuarios.UsuariosId && x.NotariaId == notariaUsuarios.NotariaId);

                mNotaria.UserEmail= notariaUsuarios.UserEmail;
                mNotaria.UsuarioModificacion = notariaUsuarios.UsuarioModificacion;
                mNotaria.FechaModificacion = DateTime.Now;
                mNotaria.Area = mNotaria.Area;
                mNotaria.Cargo = mNotaria.Cargo;
                mNotaria.Celular = mNotaria.Celular;

                _unidadTrabajoContextoPrincipal.Update(mNotaria);
            }
            
            return await _unidadTrabajoContextoPrincipal.SaveChangesAsync();            
        }
        #endregion
    }
}