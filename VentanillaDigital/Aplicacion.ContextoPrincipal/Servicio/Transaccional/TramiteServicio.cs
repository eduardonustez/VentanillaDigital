using System;
using System.Collections.Generic;
using Aplicacion.Nucleo.Base;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Aplicacion.ContextoPrincipal.Contrato;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using GenericExtensions;
using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Infraestructura.Transversal.Models;
using Aplicacion.ContextoPrincipal.Enums;
using Microsoft.EntityFrameworkCore;
using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Aplicacion.TareasAutomaticas.Enums;
using System.Globalization;

namespace Aplicacion.ContextoPrincipal.Servicio
{
    public class TramiteServicio : BaseServicio, ITramiteServicio
    {
        #region Const - Mensajes
        private const string NOTARIAINCORRECTA = "El número de notaria es incorrecta";
        private const string TIPOTRAMITEINCORRECTO = "El tipo de tramite es incorrecto";
        private const string ERRORSISTEMA = "Error al validar por favor intente de nuevo, si el error persiste por favor comunicarse con el administrador";
        private const string ESTADOAUTORIZADO = "Autorizado";
        private const string ESTADOPENDIENTE = "Pendiente de Autorización";
        private const string ESTADOOTORGAMIENTO = "Otorgamiento";
        #endregion

        #region Propiedades
        private ITramiteRepositorio _tramiteRepositorio { get; }
        private IComparecienteRepositorio _comparecienteRepositorio { get; }
        public IProcedimientoAlmacenadoRepositorio _procedimientoAlmacenado { get; }

        private readonly IActaNotarialServicio _actaNotarialServicio;

        private readonly IPersonasServicio _personasServicio;

        private readonly IDocumentoPendienteAutorizarRepositorio _documentoPendienteAutorizarRepositorio;
        #endregion

        #region Constructor
        public TramiteServicio(
            ITramiteRepositorio tramiteRepositorio,
            IPersonasServicio personasServicio,
            IProcedimientoAlmacenadoRepositorio procedimientoAlmacenadoRepositorio,
            IComparecienteRepositorio comparecienteRepositorio,
            IActaNotarialServicio actaNotarialServicio,
            IDocumentoPendienteAutorizarRepositorio documentoPendienteAutorizarRepositorio
            ) : base(tramiteRepositorio, personasServicio, documentoPendienteAutorizarRepositorio)
        {
            _personasServicio = personasServicio;
            _tramiteRepositorio = tramiteRepositorio;
            _actaNotarialServicio = actaNotarialServicio;
            _procedimientoAlmacenado = procedimientoAlmacenadoRepositorio;
            _comparecienteRepositorio = comparecienteRepositorio;
            _documentoPendienteAutorizarRepositorio = documentoPendienteAutorizarRepositorio;
        }

        #endregion

        #region Contratos
        public async Task<TramiteReturnDTO> ObtenerTramite(long tramiteId)
        {
            Tramite tramite = (await _tramiteRepositorio.Obtener(t => t.TramiteId == tramiteId, t => t.TipoTramite)).FirstOrDefault();
            TramiteReturnDTO tramiteReturnDTO = tramite.Adaptar<TramiteReturnDTO>();
            List<Compareciente> comparecientes = (await _comparecienteRepositorio.Obtener(c => c.TramiteId == tramiteId,
                c => c.Persona.TipoIdentificacion, c => c.Foto.Archivo, c => c.Firma.Archivo))?.ToList();
            tramiteReturnDTO.ComparecientesCompletos = comparecientes != null && comparecientes.Count() == tramite.CantidadComparecientes;
            if (tramiteReturnDTO.ComparecientesCompletos)
                tramiteReturnDTO.ComparecienteActual = comparecientes.Count();
            else
                tramiteReturnDTO.ComparecienteActual = comparecientes == null ? 1 : comparecientes.Count() + 1;
            tramiteReturnDTO.NombreTramite = tramiteReturnDTO.TipoTramite.Nombre;
            tramiteReturnDTO.Comparecientes = comparecientes.Select(c => c.Adaptar<ComparecienteReturnDTO>()).ToList();
            return tramiteReturnDTO;
        }

        public async Task<TramiteReturnDTO> CrearTramite(TramiteCreateDTO tramiteDto)
        {
            TramiteReturnDTO tramiteReturn;
            await ValidarRequestCreacionTramite(tramiteDto);
            var tramite = tramiteDto.Adaptar<Tramite>();
            var estadoOtorgamiento = await _tramiteRepositorio.ObtenerEstadoTramite(ESTADOOTORGAMIENTO).ConfigureAwait(false);
            if (estadoOtorgamiento != null)
            {
                tramite.EstadoTramiteId = estadoOtorgamiento.EstadoTramiteId;

                tramite.Fecha = DateTime.Now;
                _tramiteRepositorio.Agregar(tramite);
                await _tramiteRepositorio.UnidadDeTrabajo.CommitAsync().ConfigureAwait(false);
                long tramiteId = tramite.TramiteId;
                if (tramiteId > 0)
                {
                    tramiteReturn = await ObtenerRespuestaCreacionTramite(tramiteId).ConfigureAwait(false);
                }
                else
                    throw new ArgumentException(ERRORSISTEMA);
            }
            else
                throw new ArgumentException(ERRORSISTEMA);

            return tramiteReturn;
        }

        private async Task<TramiteReturnDTO> ObtenerRespuestaCreacionTramite(long tramiteId)
        {
            TramiteReturnDTO respuestaCreacion = new TramiteReturnDTO();
            Tramite tramiteBD = (await _tramiteRepositorio
                .Obtener(x => x.TramiteId == tramiteId,
                t => t.TipoTramite)
                .ConfigureAwait(false))
                .FirstOrDefault();
            respuestaCreacion.TramiteId = tramiteBD.TramiteId;
            respuestaCreacion.NombreTramite = tramiteBD.TipoTramite.Nombre;
            respuestaCreacion.CantidadComparecientes = tramiteBD.CantidadComparecientes;
            respuestaCreacion.ComparecienteActual = 0;
            respuestaCreacion.ComparecientesCompletos = false;
            return respuestaCreacion;
        }

        public async Task AutorizarTramite(long tramiteId)
        {
            var tramite = (await _tramiteRepositorio.Obtener(t => t.TramiteId == tramiteId)).FirstOrDefault();
            tramite.EstadoTramiteId = (long)EnumEstadoTramites.Autorizado;
            _tramiteRepositorio.Modificar(tramite);
            _tramiteRepositorio.UnidadDeTrabajo.Commit();
        }

        public async Task<bool> ActualizarEstadoTramite(long tramiteId)
        {
            int completos = await _comparecienteRepositorio.ObtenerTodo()
                                            .Where(c => c.TramiteId == tramiteId)
                                            .CountAsync();

            int cantidadComparecientes = await _tramiteRepositorio.ObtenerTodo()
                                            .Where(t => t.TramiteId == tramiteId)
                                            .Select(t => t.CantidadComparecientes)
                                            .FirstOrDefaultAsync();

            if (completos == cantidadComparecientes &&
                completos > 0)
            {
                var estadoTramite = await _tramiteRepositorio.ObtenerEstadoTramite(ESTADOPENDIENTE);

                var tramite = await _tramiteRepositorio.ObtenerAsync(tramiteId);
                tramite.EstadoTramiteId = estadoTramite.EstadoTramiteId;

                _tramiteRepositorio.Modificar(tramite);

                //TODO: crear documento previo
                try
                {
                    //_ = _actaNotarialServicio.GenerarArchivoPrevioAsync(tramite.TramiteId, insertDocument: true);
                }
                catch
                {

                }


                return (await _tramiteRepositorio.UnidadDeTrabajo.CommitAsync()) > 0;
            }
            return true;
        }


        #endregion

        #region Negocio

        private async Task ValidarRequestCreacionTramite(TramiteCreateDTO tramiteDto)
        {
            #region Validación Negocio

            bool esNotariaActiva = await _tramiteRepositorio.NotariaActiva(tramiteDto.NotariaId);
            if (!esNotariaActiva)
                throw new ArgumentException(NOTARIAINCORRECTA);

            bool esTipoTramiteActivo = await _tramiteRepositorio.TipoTramiteActivo(tramiteDto.TipoTramiteId);
            if (!esTipoTramiteActivo)
                throw new ArgumentException(TIPOTRAMITEINCORRECTO);
            #endregion
        }

        public async Task<ListaTramitePendienteReturnDTO> ObtenerTramitesPendientes(DefinicionFiltro definicionFiltro)
        {
            var tramitesObtenidos = await _tramiteRepositorio.ObtenerTramitesPaginado(definicionFiltro);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePendienteReturnDTO>();

            resul.Pendientes = JsonConvert.DeserializeObject<IEnumerable<TramitePendienteReturnDTO>>(tramitesObtenidos.Resultado);

            return resul;
        }

        public async Task<ListaTramitePendienteReturnDTO> ObtenerTramitesPendientesAutPaginado(FiltroTramites filtroTramites)
        {
            var tramitesObtenidos = await _tramiteRepositorio.ObtenerTramitesPendientesAutPaginado(filtroTramites);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePendienteReturnDTO>();

            resul.Pendientes = JsonConvert.DeserializeObject<IEnumerable<TramitePendienteReturnDTO>>(tramitesObtenidos.Resultado);

            return resul;
        }

        public async Task<ListaTramitePendienteReturnDTO> ObtenerTramitesAutorizadoPaginado(FiltroTramites filtroTramites)
        {
            var tramitesObtenidos = await _tramiteRepositorio.ObtenerTramitesAutorizadoPaginado(filtroTramites);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePendienteReturnDTO>();

            resul.Pendientes = JsonConvert.DeserializeObject<IEnumerable<TramitePendienteReturnDTO>>(tramitesObtenidos.Resultado);

            return resul;
        }

        public async Task<ListaTramitePendienteReturnDTO> ObtenerTramitesEnProcesoPaginado(FiltroTramites filtroTramites)
        {
            var tramitesObtenidos = await _tramiteRepositorio.ObtenerTramitesEnProcesoPaginado(filtroTramites);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePendienteReturnDTO>();
            resul.Pendientes = JsonConvert.DeserializeObject<IEnumerable<TramitePendienteReturnDTO>>(tramitesObtenidos.Resultado);
            return resul;
        }

        public async Task<ListaTramitePendienteReturnDTO> ObtenerTramitesRechazadosPaginado(FiltroTramites filtroTramites)
        {
            var tramitesObtenidos = await _tramiteRepositorio.ObtenerTramitesRechazadosPaginado(filtroTramites);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePendienteReturnDTO>();

            resul.Pendientes = JsonConvert.DeserializeObject<IEnumerable<TramitePendienteReturnDTO>>(tramitesObtenidos.Resultado);

            return resul;
        }



        public async Task<bool> ActualizarTramite(TramiteEditDTO tramite)
        {
            var entidad = tramite.Adaptar<Tramite>();

            int comparecientesCompletos = await _comparecienteRepositorio.ObtenerTodo()
                                            .Where(c => c.TramiteId == tramite.TramiteId)
                                            .CountAsync();

            _tramiteRepositorio.Modificar(entidad,
                x => x.CantidadComparecientes,
                x => x.DatosAdicionales,
                x => x.TipoTramiteId);

            if (comparecientesCompletos == entidad.CantidadComparecientes)
            {
                entidad.EstadoTramiteId = (int)EnumEstadoTramites.Pendiente;
                _tramiteRepositorio.Modificar(entidad,
                    x => x.EstadoTramiteId);

                //TODO: crear documento previo
                //_ = _actaNotarialServicio.GenerarArchivoPrevioAsync(tramite.TramiteId, insertDocument: true);
            }

            return await _tramiteRepositorio.UnidadDeTrabajo.CommitAsync() > 0;
        }

        public async Task<TramiteDetalleDTO> ObtenerTramiteDetalle(long tramiteId)
        {
            Tramite tramiteBD = await _tramiteRepositorio.GetOneAsync(x => x.TramiteId == tramiteId, t => t.TipoTramite, t => t.Notaria, t => t.EstadoTramite);

            if (tramiteBD == null) throw new ArgumentException("Trámite no encontrado, por favor verifique los datos ingresados e intente nuevamente.");

            return new TramiteDetalleDTO
            {
                TramiteId = tramiteBD.TramiteId,
                CantidadComparecientes = tramiteBD.CantidadComparecientes,
                ComparecienteActual = 0,
                Estado = tramiteBD.EstadoTramite.Nombre,
                Fecha = tramiteBD.FechaCreacion,
                NombreNotaria = tramiteBD.Notaria.Nombre,
                TipoTramite = tramiteBD.TipoTramite.Nombre,
                UsuarioCreacion = tramiteBD.UsuarioCreacion,
                ComparecientesCompletos = false,
                ActaGenerada = tramiteBD != null && tramiteBD.ActaNotarialId.HasValue
            };
        }

        public async Task<IEnumerable<TramiteInfoBasica>> ObtenerTramitesPorNumeroIdentificacion(string numeroIdentificacion, DateTime fechaInicio, DateTime fechaFin)
        { 
            var comparecientes = await _comparecienteRepositorio.Obtener(c => 
                c.Persona.NumeroDocumento == numeroIdentificacion && 
                c.FechaCreacion.Date >= fechaInicio.Date && 
                c.FechaCreacion.Date <= fechaFin.Date);
            return await _tramiteRepositorio.ObtenerTodo()
                .Where(t=> comparecientes.Select(c=>c.TramiteId).Contains(t.TramiteId))
                .Select(t => new TramiteInfoBasica
                {
                    TramiteId = t.TramiteId,
                    CantidadComparecientes = t.CantidadComparecientes,
                    TipoTramite = t.TipoTramite.Nombre,
                    NotariaId = t.NotariaId,
                    NotariaNombre = t.Notaria.Nombre,
                    TipoTramiteId = t.TipoTramiteId
                })
                .ToListAsync();
        }

        public async Task<PaginableResponse<TramiteInfoBasica>> ObtenerTramitesPorNumeroIdentificacionPaginado(DefinicionFiltro filtro)
        {
            var numeroDocumento = filtro.Filtros.Find(m => m.Columna.Equals("NumeroIdentificacion")).Valor;
            var fechaInicio = DateTime.ParseExact(filtro.Filtros.Find(m => m.Columna.Equals("FechaInicio")).Valor, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var fechaFin = DateTime.ParseExact(filtro.Filtros.Find(m => m.Columna.Equals("FechaFin")).Valor, "yyyy-MM-dd", CultureInfo.InvariantCulture);


            var idsTramite = await _comparecienteRepositorio
               .ObtenerTodo()
               .Where(m => m.Persona.NumeroDocumento == numeroDocumento && 
               m.FechaCreacion.Date >= fechaInicio.Date && 
               m.FechaCreacion.Date <= fechaFin.Date &&
               m.TramiteId > -1)
               .AsNoTracking()
               .Skip((filtro.IndicePagina - 1) * filtro.RegistrosPagina)
               .Take(filtro.RegistrosPagina)
               .Select(m => m.TramiteId)
               .ToListAsync();

            var query = await _tramiteRepositorio.ObtenerTodo()
                .Where(t => idsTramite.Contains(t.TramiteId))
                .AsNoTracking()
                .Select(t => new TramiteInfoBasica
                {
                    TramiteId = t.TramiteId,
                    CantidadComparecientes = t.CantidadComparecientes,
                    TipoTramite = t.TipoTramite.Nombre,
                    NotariaId = t.NotariaId,
                    NotariaNombre = t.Notaria.Nombre,
                    TipoTramiteId = t.TipoTramiteId
                }).ToListAsync();


            var cantidad = await _comparecienteRepositorio.ObtenerTodo()
                .Where(m => m.Persona.NumeroDocumento == numeroDocumento && m.FechaCreacion.Date >= fechaInicio.Date && m.FechaCreacion.Date <= fechaFin.Date)
                .LongCountAsync();

            return new PaginableResponse<TramiteInfoBasica>
            {
                TotalRows = cantidad,
                Pages = (int)Math.Ceiling(cantidad / (decimal)filtro.RegistrosPagina),
                Data = query
            };
        }

        public async Task<IEnumerable<HistorialTramite>> ObtenerHistorialTramite(long tramiteId)
        {
            var tramite = await _tramiteRepositorio.GetOneAsync(m => m.TramiteId == tramiteId);

            if (tramite == null) throw new ArgumentException("Trámite no encontrado.");

            Dominio.ContextoPrincipal.Entidad.Parametricas.Persona personaCreacion = null;

            if (!string.IsNullOrEmpty(tramite.UsuarioCreacion))
            {
                personaCreacion = await _personasServicio.ConsultarPersonaPorCorreo(tramite.UsuarioCreacion);
            }

            var historial = new List<HistorialTramite>
            {
               new HistorialTramite
               {
                    Fecha = tramite.FechaCreacion,
                    Detalle = "Creación de trámite",
                    Documento = personaCreacion?.NumeroDocumento,
                    Persona = $"{personaCreacion?.Nombres} {personaCreacion?.Apellidos}"
                }
            };

            var comparecientes = await _comparecienteRepositorio
                .ObtenerTodo()
                .Include(m => m.Persona)
                .Where(m => m.TramiteId == tramiteId)
                .ToListAsync();

            if (comparecientes != null)
            {
                foreach (var compareciente in comparecientes)
                {
                    historial.Add(new HistorialTramite
                    {
                        Fecha = compareciente.FechaCreacion,
                        Detalle = $"Compareciente {comparecientes.IndexOf(compareciente) + 1} creado con {(compareciente.HitDedo1.HasValue && compareciente.HitDedo1.Value ? "éxito" : "fallo dactilar")}",
                        Documento = compareciente.Persona?.NumeroDocumento,
                        Persona = $"{compareciente.Persona?.Nombres} {compareciente.Persona?.Apellidos}"
                    });
                }
            }

            var documento = await _documentoPendienteAutorizarRepositorio.GetOneAsync(m => m.TramiteId == tramiteId);

            if (documento != null)
            {
                personaCreacion = await _personasServicio.ConsultarPersonaPorCorreo(documento.UsuarioCreacion);

                historial.Add(new HistorialTramite
                {
                    Fecha = documento.FechaCreacion,
                    Detalle = $"Notario autorizó el trámite",
                    Documento = personaCreacion?.NumeroDocumento,
                    Persona = $"{personaCreacion?.Nombres} {personaCreacion?.Apellidos}"
                });
            }

            return historial;
        }


        #endregion
    }
}
