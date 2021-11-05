using System.Collections.Generic;
using Aplicacion.Nucleo.Base;
using System.Threading.Tasks;
using System.Linq;
using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using GenericExtensions;
using System;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Infraestructura.Transversal.Excepciones;
using Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas.Archivos;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class NotariaServicio : BaseServicio, INotariaServicio
    {
        private INotariaRepositorio _notariaRepositorio { get; }
        private INotarioRepositorio _notarioRepositorio { get; }
        private INotariasUsuarioRepositorio _notariaUsuariosRepositorio;
        private IConvenioRNECRepositorio _ConvenioRNECRepositorio { get; }
        private ITipoIdentificacionRepositorio _TipoIdentificacionRepositorio { get; }
        private IGrafoNotarioRepositorio _grafoRepositorio { get; }

        public NotariaServicio(INotariaRepositorio notariaRepositorio,
            INotariasUsuarioRepositorio notariasUsuarioRepositorio,
            IConvenioRNECRepositorio convenioRNECRepositorio,
            ITipoIdentificacionRepositorio tipoIdentificacionRepositorio,
            INotarioRepositorio notarioRepositorio,
            IGrafoNotarioRepositorio grafoRepositorio) :
            base(notariaRepositorio,
                 notariasUsuarioRepositorio,
                 convenioRNECRepositorio,
                 tipoIdentificacionRepositorio,
                 notarioRepositorio,
                 grafoRepositorio)
        {

            _notariaRepositorio = notariaRepositorio;
            _notariaUsuariosRepositorio = notariasUsuarioRepositorio;
            _ConvenioRNECRepositorio = convenioRNECRepositorio;
            _TipoIdentificacionRepositorio = tipoIdentificacionRepositorio;
            _notarioRepositorio = notarioRepositorio;
            _grafoRepositorio = grafoRepositorio;
        }

        public async Task<IEnumerable<NotariaForReturn>> Obtener()
        {
            var notarias = await _notariaRepositorio.Obtener(n => n.IsDeleted == false);
            return notarias.Select(p => p.Adaptar<NotariaForReturn>()).ToList();
        }

        public async Task<NotariaForReturn> ObtenerNotariaPorUsuario(string userEmail)
        {
            var notaria_usuario = (await _notariaUsuariosRepositorio
                .Obtener(e => e.UserEmail == userEmail,
                e => e.Notaria))
                .FirstOrDefault();

            return notaria_usuario?.Notaria?.Adaptar<NotariaForReturn>();
        }

        public async Task<NotariaForReturn> ObtenerNotariaPorId(long NotariaId)
        {
            Notaria notaria = (await _notariaRepositorio.Obtener(e => e.IsDeleted == false && e.NotariaId == NotariaId).ConfigureAwait(false)).FirstOrDefault();

            if (notaria != null)
            {
                return notaria?.Adaptar<NotariaForReturn>();
            }
            return null;
        }

        public async Task<PersonasRequestDTO> ActualizacionPersonaYNotarioUsuario(PersonaCreateOrUpdateDTO persona, NotariaUsuarioCreateDTO notaria, List<PersonaDatosDTO> personaDatos, string userId, int RolId)
        {
            var personaRequest = persona.Adaptar<Persona>();
            var notariaUsuarioRequest = notaria.Adaptar<NotariaUsuarios>();

            Guid IdUser = await _notariaRepositorio.CrearPersonaYNotariaUsuario(personaRequest, notariaUsuarioRequest, creaObjetoPersonaDatos(personaDatos)).ConfigureAwait(false);
            var notariaUsuario = (await _notariaUsuariosRepositorio.Obtener(n => n.UsuariosId == userId && n.NotariaId == notaria.NotariaId, n => n.Notario.GrafoArchivo)).FirstOrDefault();

            if (IdUser != null && RolId == 3)
            {
                if (notariaUsuario.Notario == null)
                {
                    _notarioRepositorio.Agregar(CrearNotario(persona.UserId, "", 2, notariaUsuario.NotariaUsuariosId));
                    _notariaRepositorio.UnidadDeTrabajo.Commit();
                }
            }
            else if (RolId == 2 && notariaUsuario.Notario != null)
            {
                if (notariaUsuario.Notario.GrafoArchivo != null)
                {
                    _grafoRepositorio.Eliminar(notariaUsuario.Notario.GrafoArchivo, true);
                }

                _notarioRepositorio.Eliminar(notariaUsuario.Notario, true);
                _notariaRepositorio.UnidadDeTrabajo.Commit();
            }

            notariaUsuario.UserEmail = notaria.UsuarioEmail;
            notariaUsuario.Area = notaria.Area;
            notariaUsuario.Cargo = notaria.Cargo;
            notariaUsuario.Celular = notaria.Celular;
            notariaUsuario.FechaModificacion = DateTime.Now;
            notariaUsuario.UsuarioModificacion = notaria.UserId;
            _notariaUsuariosRepositorio.Modificar(notariaUsuario);
            _notariaUsuariosRepositorio.UnidadDeTrabajo.Commit();

            PersonasRequestDTO personasRequest = new PersonasRequestDTO();
            personasRequest.UserId = IdUser;
            personasRequest.Nombres = persona.Nombres + " " + persona.Apellidos;
            personasRequest.Celular = persona.NumeroCelular;
            personasRequest.Email = persona.Email;
            personasRequest.EmailNotaria = notaria.UsuarioEmail;
            personasRequest.NumeroIdentificacion = persona.NumeroDocumento;
            ConvenioRNEC convenioRNEC = (await _ConvenioRNECRepositorio.Obtener(x => x.NotariaId == persona.NotariaId).ConfigureAwait(false)).FirstOrDefault();
            TipoIdentificacion tipoIdentificacion = (await _TipoIdentificacionRepositorio.Obtener(x => x.TipoIdentificacionId == persona.TipoIdentificacionId).ConfigureAwait(false)).FirstOrDefault();
            personasRequest.TipoDocumentoAbreviatura = tipoIdentificacion.Abreviatura;
            personasRequest.IdCliente = convenioRNEC.IdCliente;
            personasRequest.IdConvenio = convenioRNEC.Convenio;
            personasRequest.IdOficina = convenioRNEC.IdOficina;
            personasRequest.IdRol = convenioRNEC.IdRol;
            personasRequest.Cargo = notaria.Cargo;
            personasRequest.Area = notaria.Area;
            personasRequest.Habilitado = true;
            personasRequest.Login = persona.UserName;


            return personasRequest;
        }

        public async Task<PersonasRequestDTO> CreacionPersonaYNotarioUsuario(PersonaCreateOrUpdateDTO persona, NotariaUsuarioCreateDTO notaria, List<PersonaDatosDTO> personaDatos, int RolId)
        {
            var personaRequest = persona.Adaptar<Persona>();
            var notariaUsuarioRequest = notaria.Adaptar<NotariaUsuarios>();

            Guid IdUser = await _notariaRepositorio.CrearPersonaYNotariaUsuario(personaRequest, notariaUsuarioRequest, creaObjetoPersonaDatos(personaDatos)).ConfigureAwait(false);

            if (IdUser != null && RolId == 3)
            {
                var notariaUsuario = (await _notariaUsuariosRepositorio.Obtener(n => n.UserEmail == notaria.UsuarioEmail && n.NotariaId == notaria.NotariaId)).FirstOrDefault();

                _notarioRepositorio.Agregar(CrearNotario(persona.UserId, "", 2, notariaUsuario.NotariaUsuariosId));
                _notariaRepositorio.UnidadDeTrabajo.Commit();
            }

            PersonasRequestDTO personasRequest = new PersonasRequestDTO();
            personasRequest.UserId = IdUser;
            personasRequest.Nombres = persona.Nombres + " " + persona.Apellidos;
            personasRequest.Celular = persona.NumeroCelular;
            personasRequest.Email = persona.Email;
            personasRequest.EmailNotaria = notaria.UsuarioEmail;
            personasRequest.NumeroIdentificacion = persona.NumeroDocumento;
            ConvenioRNEC convenioRNEC = (await _ConvenioRNECRepositorio.Obtener(x => x.NotariaId == persona.NotariaId).ConfigureAwait(false)).FirstOrDefault();
            TipoIdentificacion tipoIdentificacion = (await _TipoIdentificacionRepositorio.Obtener(x => x.TipoIdentificacionId == persona.TipoIdentificacionId).ConfigureAwait(false)).FirstOrDefault();
            personasRequest.TipoDocumentoAbreviatura = tipoIdentificacion.Abreviatura;
            personasRequest.IdCliente = convenioRNEC.IdCliente;
            personasRequest.IdConvenio = convenioRNEC.Convenio;
            personasRequest.IdOficina = convenioRNEC.IdOficina;
            personasRequest.IdRol = convenioRNEC.IdRol;
            personasRequest.Cargo = notaria.Cargo;
            personasRequest.Area = notaria.Area;
            personasRequest.Habilitado = true;
            personasRequest.Login = persona.UserName;
            personaRequest.Genero = persona.Genero;

            return personasRequest;
        }

        private Notario CrearNotario(string user, string nit, int tipoNotario, long notariaUsuarioId)
        {
            return new Notario()
            {
                FechaCreacion = DateTime.Now,
                FechaModificacion = DateTime.Now,
                UsuarioCreacion = user,
                UsuarioModificacion = user,
                Nit = nit,
                Pin = null,
                TipoNotario = tipoNotario,
                NotariaUsuariosId = notariaUsuarioId
            };

        }

        public async Task<IEnumerable<NotariaUsuarios>> obtenerListaUsuarios(long NotariaId)
        {
            var notariaUsuarios = await _notariaRepositorio.ObtenerListadoUsuariosNotaria(NotariaId);
            return notariaUsuarios;
        }

        public async Task<Persona> obtenerPersonaId(long personaId)
        {
            var Persona = _notariaRepositorio.obtenerPersonaId(personaId);
            return Persona;
        }

        public async Task<bool> ActualizarSincronizacionRNEC(long notariaUsuarioId)
        {
            bool res = await _notariaRepositorio.ActualizarSincronizacionRNEC(notariaUsuarioId);
            return res;
        }

        public async Task<NotariaUsuarios> obtenerNotariaUsuarioxEmail(string Email)
        {
            NotariaUsuarios res = _notariaRepositorio.obtenerNotariaUsuarioxEmail(Email);
            return res;
        }

        public async Task<NotariaUsuarios> ObtenerUsuarioNotariaPorId(Guid UsuarioId)
        {
            NotariaUsuarios res = _notariaRepositorio.ObtenerUsuarioNotariaPorId(UsuarioId);
            return res;
        }

        public async Task<ConfiguracionRNECDTO> ObtenerConfiguracionRNEC(long notariaID)
        {
            return (await _notariaRepositorio.ObtenerConvenioRNEC(notariaID))
                .Adaptar<ConfiguracionRNECDTO>()
                .Anadir(await _notariaRepositorio.ObtenerAsync(notariaID));
        }

        public async Task ActualizarSelloNotaria(SelloNotariaEditDto selloNotariaEditDto)
        {
            var notaria = await _notariaRepositorio
                .GetOneAsync(n => n.NotariaId == selloNotariaEditDto.NotariaId,
                nta => nta.SelloArchivo,
                nta => nta.Ubicacion.UbicacionPadre);
            if (notaria == null)
                throw new NotFoundException("Notaria no encontrada");
            if (notaria.SelloArchivo == null)
            {
                notaria.SelloArchivo = SelloNotaria.FromDataUrl(
                    selloNotariaEditDto.Sello,
                    $"{notaria.NotariaId}_{notaria.Nombre}",
                    $"sellos/{notaria.Ubicacion.UbicacionPadre?.Nombre ?? notaria.Ubicacion.Nombre}");
            }
            else
            {
                notaria.SelloArchivo.MapDataUrl(selloNotariaEditDto.Sello);
            }
            _notariaRepositorio.Modificar(notaria);
            _notariaRepositorio.UnidadDeTrabajo.Commit();
        }

        private List<PersonaDatos> creaObjetoPersonaDatos(List<PersonaDatosDTO> personaDatos)
        {
            List<PersonaDatos> requestDatoPersona = new List<PersonaDatos>();

            foreach (PersonaDatosDTO itemDatoPersona in personaDatos)
            {
                PersonaDatos personaDato = new PersonaDatos();
                TipoDatoId tipoDatoId = (TipoDatoId)itemDatoPersona.TipoDatoId;

                personaDato.IsDeleted = false;
                personaDato.TipoDatoId = tipoDatoId;
                personaDato.ValorDato = itemDatoPersona.Valor;
                requestDatoPersona.Add(personaDato);
            }
            return requestDatoPersona;
        }

        public void AgregarNotariaUsuario(NotariaUsuarios item)
        {
            _notariaUsuariosRepositorio.Agregar(item);
        }

        public async Task<bool> EliminarUsuario(UsuarioDeleteRequestDTO usuarioDeleteDto)
        {
            var notariaUsuario = (
                await _notariaUsuariosRepositorio.Obtener(n => n.UsuariosId == usuarioDeleteDto.Id && n.NotariaId == usuarioDeleteDto.NotariaId,
                n => n.Notario,
                n => n.Notario.GrafoArchivo)).FirstOrDefault();

            if (notariaUsuario.Notario != null)
            {
                if (notariaUsuario.Notario.GrafoArchivo != null)
                    _grafoRepositorio.Eliminar(notariaUsuario.Notario.GrafoArchivo, true);

                _notarioRepositorio.Eliminar(notariaUsuario.Notario, true);
            }

            _notariaUsuariosRepositorio.Eliminar(notariaUsuario);
            _notariaUsuariosRepositorio.UnidadDeTrabajo.Commit();

            return true;
        }

        public async Task<IEnumerable<NotariaBasicDTO>> ObtenerTodasNotarias()
            => await _notariaRepositorio.ObtenerTodo().AsNoTracking()
            .Select(m => new NotariaBasicDTO
            {
                NotariaId = m.NotariaId,
                NotariaNombre = m.Nombre
            }).ToListAsync();

        public async Task<IEnumerable<NotariaClienteDTO>> ObtenerNotarias()
        {
            var notarias = await this._notariaRepositorio.Obtener();
            return notarias.Select(c => c.Adaptar<NotariaClienteDTO>());
        }

        public async Task<Guid> ObtenerConvenioRNEC(long notariaId)
        {
            var convenioRNEC = await _notariaRepositorio.ObtenerConvenioRNEC(notariaId);
            Guid.TryParse(convenioRNEC.Convenio, out Guid convenio);
            return convenio;
        }
    }
}
