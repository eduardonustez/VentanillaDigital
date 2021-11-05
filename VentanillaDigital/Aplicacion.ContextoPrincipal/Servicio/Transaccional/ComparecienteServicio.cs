using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using GenericExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public class ComparecienteServicio : BaseServicio, IComparecienteServicio
    {
        #region Const - Mensajes
        private const string COMPARECIENTEMAXIMO = "El número de compareciente excede la cantidad solicitada";
        private const string NOTARIAINCORRECTO = "El número de notaria es incorrecta";
        private const string TRAMITEINCORRECTO = "El número de tramite es incorrecto";
        private const string TIPODOCUMENTOINCORRECTO = "El tipo de documento incorrecto";
        private const string TIPOYNUMERODOCUMENTOEXISTENTE = "El tipo de documento y número de documento no puede estar en un mismo tramite";
        private const string ESTADOPENDIENTE = "Pendiente de Autorización";
        private const string ESTADOOTORGAMIENTO = "Otorgamiento";
        #endregion

        #region Propiedades
        private IComparecienteRepositorio _comparecienteRepositorio { get; }
        private ITramiteRepositorio _tramiteRepositorio { get; }
        private ITipoIdentificacionRepositorio _tipoIdentificacionRepositorio { get; }
        private IPersonasRepositorio _personasRepositorio { get; }
        #endregion

        #region Constructor
        public ComparecienteServicio(IComparecienteRepositorio comparecienteRepositorio
            , ITramiteRepositorio tramiteRepositorio
            , ITipoIdentificacionRepositorio tipoIdentificacionRepositorio
            , IPersonasRepositorio personasRepositorio)
            : base(comparecienteRepositorio
                  , tramiteRepositorio
                  , tipoIdentificacionRepositorio
                  , personasRepositorio)
        {
            _comparecienteRepositorio = comparecienteRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _tipoIdentificacionRepositorio = tipoIdentificacionRepositorio;
            _personasRepositorio = personasRepositorio;
        }
        #endregion

        #region Contratos
        public async Task<IEnumerable<ComparecienteDatosDTO>> ObtenerDatosComparecientesPorTramiteId(long tramiteId)
        {
            var comparecientes = await _comparecienteRepositorio.ObtenerTodo()
                .Where(m => m.TramiteId == tramiteId)
                .Select(m => new ComparecienteDatosDTO
                {
                    TramiteId = m.TramiteId,
                    TipoDocumentoId = m.Persona.TipoIdentificacionId,
                    NumeroDocumento = m.Persona.NumeroDocumento,
                    Nombres = m.Persona.Nombres,
                    Apellidos = m.Persona.Apellidos,
                    Email = m.Persona.Email,
                    TipoDocumentoNombre = m.Persona.TipoIdentificacion.Nombre,
                    HitDedo1 = m.HitDedo1.HasValue && m.HitDedo1.Value,
                    HitDedo2 = m.HitDedo2.HasValue && m.HitDedo2.Value
                })
                .AsNoTracking()
                .ToListAsync();

            return comparecientes;
        }

        public async Task<ComparecienteCreateReturnDTO> AgregarCompareciente(ComparecienteCreateDTO compareciente)
        {
            ComparecienteCreateReturnDTO comparecienteReturn = new ComparecienteCreateReturnDTO();
            await ValidarRequestCompareciente(compareciente);

            var entidadCompareciente = new Compareciente();
            ObtenerEntidadCompareciente(compareciente, ref entidadCompareciente);

            if (compareciente.Excepciones != null)
            {
                ExcepcionHuella excepcionHuella = new ExcepcionHuella()
                {
                    DedosExceptuados = compareciente.Excepciones.Dedos.Select(d => (int)d).Sum(),
                    Descripcion = compareciente.Excepciones.Descripcion ?? "",
                    UsuarioCreacion = entidadCompareciente.UsuarioCreacion,
                    UsuarioModificacion = entidadCompareciente.UsuarioModificacion,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                };
                entidadCompareciente.ExcepcionHuella = excepcionHuella;
            }

            var step = 0;
            try
            {
                _comparecienteRepositorio.Agregar(entidadCompareciente);
                step = 1;
                int numeroCompareciente = await _comparecienteRepositorio.UnidadDeTrabajo.CommitAsync();

                if (numeroCompareciente > 0)
                {
                    comparecienteReturn = ObtenerRespuestaComparecienteCreado(compareciente);
                }

                return comparecienteReturn;
            }
            catch (Exception ex)
            {
                if (step == 0)
                    throw new Exception($"{ex.Message}, Ocurrio un error [AgregarCompareciente] al agregar entidad Comparecientes.", ex);
                else
                    throw new Exception($"{ex.Message}, Ocurrio un error [AgregarCompareciente] al realizar Commit a la tabla comparecientes.", ex);
            }
        }

        public async Task<IEnumerable<TramiteComparecienteReturnDTO>> ObtenerComparecientesPorTramiteID(long tramiteId)
        {
            var comparecientes = await _comparecienteRepositorio.ObtenerComparecientesPorTramiteID(tramiteId);
            return comparecientes.Select(c => c.Adaptar<TramiteComparecienteReturnDTO>());
        }
        #endregion

        #region Métodos Privados
        private async Task ValidarRequestCompareciente(ComparecienteCreateDTO compareciente)
        {
            try
            {
                ComparecienteCreateReturnDTO validacionCompareciente = new ComparecienteCreateReturnDTO();

                //Tramite obtenerTramite = (await _tramiteRepositorio.Obtener(x => !x.IsDeleted && x.TramiteId == compareciente.TramiteId, "Comparecientes.Persona")).FirstOrDefault();

                bool esTipoIdentificacionValido = (await _tipoIdentificacionRepositorio.Obtener(x => !x.IsDeleted && x.TipoIdentificacionId == compareciente.TipoDocumentoId)).Any();

                /*if (obtenerTramite == null)
                    throw new ArgumentException($"{TRAMITEINCORRECTO}", $"{compareciente.TramiteId} {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                if (!esTipoIdentificacionValido)
                    throw new ArgumentException($"{TIPODOCUMENTOINCORRECTO}", $"{compareciente.TipoDocumentoId} {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                if (obtenerTramite.Comparecientes.Count() >= obtenerTramite.CantidadComparecientes)
                    throw new ArgumentException($"{COMPARECIENTEMAXIMO}", $"{compareciente.TramiteId} {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                if (obtenerTramite.Comparecientes.Any(c => c.Persona.NumeroDocumento == compareciente.NumeroDocumento && c.Persona.TipoIdentificacionId == compareciente.TipoDocumentoId))
                    throw new ArgumentException($"{TIPOYNUMERODOCUMENTOEXISTENTE}", $"{compareciente.NumeroDocumento} {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                if (obtenerTramite.NotariaId != compareciente.NotariaId)
                    throw new ArgumentException($"{NOTARIAINCORRECTO}", $"{compareciente.NotariaId} {stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}");
                stopwatch.Stop();*/
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, Ocurrio un error [ValidarRequestCompareciente] al obtener el tipo de identificación.");
            }
        }

        private async Task<bool> ValidarPersonaNoDuplicadaPorTramite(ComparecienteCreateDTO compareciente, IEnumerable<Compareciente> listaDeComparecientes)
        {
            bool respuesta = false;
            List<Persona> persona = (await _personasRepositorio.Obtener(x => !x.IsDeleted
            && x.TipoIdentificacionId == compareciente.TipoDocumentoId
            && x.NumeroDocumento == compareciente.NumeroDocumento)).ToList();
            if (persona.Count > 0)
            {
                respuesta = listaDeComparecientes.Where(x => x.PersonaId == persona.FirstOrDefault()?.PersonaId).Any();
            }
            return respuesta;
        }


        private ComparecienteCreateReturnDTO ObtenerRespuestaComparecienteCreado(ComparecienteCreateDTO compareciente)
        {
            ComparecienteCreateReturnDTO respuesta = new ComparecienteCreateReturnDTO()
            {
                TramiteId = compareciente.TramiteId,


            };

            return respuesta;
        }

        private void ObtenerEntidadCompareciente(ComparecienteCreateDTO compareciente,
            ref Compareciente entidadCompareciente)
        {
            entidadCompareciente = compareciente.Adaptar<Compareciente>();

            entidadCompareciente.Foto.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.Foto.UsuarioModificacion = compareciente.UserName;
            entidadCompareciente.Foto.Archivo = compareciente.Foto.Adaptar<Archivo>();
            entidadCompareciente.Foto.Archivo.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.Foto.Archivo.UsuarioModificacion = compareciente.UserName;

            entidadCompareciente.Firma.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.Firma.UsuarioModificacion = compareciente.UserName;
            entidadCompareciente.Firma.Archivo = compareciente.Firma.Adaptar<Archivo>();
            entidadCompareciente.Firma.Archivo.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.Firma.Archivo.UsuarioModificacion = compareciente.UserName;

            entidadCompareciente.ImagenDocumento.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.ImagenDocumento.UsuarioModificacion = compareciente.UserName;
            entidadCompareciente.ImagenDocumento.Archivo = compareciente.ImagenDocumento.Adaptar<Archivo>();
            entidadCompareciente.ImagenDocumento.Archivo.UsuarioCreacion = compareciente.UserName;
            entidadCompareciente.ImagenDocumento.Archivo.UsuarioModificacion = compareciente.UserName;
            entidadCompareciente.TramiteSinBiometria = compareciente.TramiteSinBiometria;
            entidadCompareciente.MotivoSinBiometria = compareciente.MotivoSinBiometria;
            entidadCompareciente.Posicion = compareciente.ComparecienteActualPos ?? 0;
            entidadCompareciente.HitDedo1 = compareciente.HitDedo1;
            entidadCompareciente.HitDedo2 = compareciente.HitDedo2;
            entidadCompareciente.Vigencia = compareciente.Vigencia;
            entidadCompareciente.NombreDigitado = compareciente.NombreDigitado;
            ObtenerInformacionPersona(compareciente, ref entidadCompareciente);
        }

        private void ObtenerInformacionPersona(ComparecienteCreateDTO compareciente,
            ref Compareciente entidadCompareciente)
        {
            try
            {
                Persona persona = _comparecienteRepositorio.ObtenerPersona(compareciente.TipoDocumentoId, compareciente.NumeroDocumento, false);

                if (persona != null)
                {
                    var personaUpdate = compareciente.Adaptar<Persona>();
                    personaUpdate.PersonaId = persona.PersonaId;
                    _comparecienteRepositorio.ActualizarPersona(personaUpdate);
                    entidadCompareciente.PersonaId = persona.PersonaId;
                }
                else
                {
                    entidadCompareciente.Persona = compareciente.Adaptar<Persona>();
                    entidadCompareciente.Persona.UsuarioCreacion = entidadCompareciente.UsuarioCreacion;
                    entidadCompareciente.Persona.UsuarioModificacion = entidadCompareciente.UsuarioModificacion;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}, Ocurrio un error [ObtenerInformacionPersona] al obtener la información de la persona.");
            }
        }
        #endregion
    }
}