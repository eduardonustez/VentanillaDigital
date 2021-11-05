using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Enums;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.ContextoPrincipal.Utils;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using Dominio.ContextoPrincipal.Vista;
using GenericExtensions;
using HashidsNet;
using Infraestructura.Cosmos.Interfaces;
using Infraestructura.Storage.Interfaces;
using HtmlToPdf;
using HtmlToPdf.Entidades;
using Infraestructura.Transversal.Cache;
using Infraestructura.Transversal.Encriptacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TSAIntegracion.Entities;
using Aplicacion.TareasAutomaticas.Modelo.Transaccional;
using Aplicacion.TareasAutomaticas.Enums;
using Aplicacion.TareasAutomaticas.Contrato.Transaccional;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public class ActaNotarialServicio : BaseServicio, IActaNotarialServicio
    {

        #region Const - Mensajes
        private const string SINTRAMITES = "Numero de tramites invalido";
        private const string CACHE_INFO_ACTA_NOTARIAL = "InfoActaNotarial_{0}_{1}";
        private const string CACHE_COMPARECIENTES = "COMPARECIENTES_{0}";
        private const string CACHE_TEXTOSINBIOMETRIA = "TEXTOSINBIOMETRIA";
        private const string CACHE_TEXTOCONBIOMETRIA = "TEXTOCONBIOMETRIA";
        private const string CACHE_NOTARIA_UBICACION = "NOTARIA_UBICACION_{0}";
        private const string CACHE_URLQR = "URLQR";
        private const string CACHE_NOTARIA_USUARIO = "NOTARIA_USUARIO_{0}";
        private const string CACHE_NOMBRETIPODOCUMENTO = "NOMBRETIPODOCUMENTO";
        private const string CACHE_IDPLANTILLASTICKERINVIDENTESABEFIRMAR = "PLANTILLASTICKERINVIDENTESABEFIRMAR";
        private const string CACHE_IDPLANTILLACARTAINVIDENTESABEFIRMAR = "PLANTILLACARTAINVIDENTESABEFIRMAR";
        private const string CACHE_PLANTILLA = "PLANTILLA_{0}";
        private const string CACHE_STICKER = "STICKER_{0}";
        private const string CACHE_STICKER_DOS = "STICKER_DOS_{0}";
        #endregion

        #region Propiedades
        private IConsultaNotariaSeguraRepositorio _consultaNotariaSegura { get; } 
        private IActaNotarialRepositorio _actaNotarialRepositorio { get; }
        private ITramiteRepositorio _tramiteRepositorio { get; }
        private INotariasUsuarioRepositorio _notariasUsuarioRepositorio { get; }
        private IDocumentoPendienteAutorizarRepositorio _documentoPendienteAutorizarRepositorio { get; }
        private IHistoricosStorageInfraestructura _historicosStorageInfraestructura { get; }
        private IHistoricosCosmosInfraestructura _historicosCosmosInfraestructura { get; }
        private IGenerarActaServicio _generarActaServicio { get; }
        #endregion

        #region Constructor
        public ActaNotarialServicio(ITramiteRepositorio tramiteRepositorio,
            IActaNotarialRepositorio actaNotarialRepositorio,
            IConsultaNotariaSeguraRepositorio consultaNotariaSegura,
            INotariasUsuarioRepositorio notariasUsuarioRepositorio,
            IDocumentoPendienteAutorizarRepositorio documentoPendienteAutorizarRepositorio,
            IHistoricosStorageInfraestructura historicosStorageInfraestructura,
            IHistoricosCosmosInfraestructura historicosCosmosInfraestructura,
            IGenerarActaServicio generarActaServicio) : base(actaNotarialRepositorio, tramiteRepositorio
                , notariasUsuarioRepositorio, generarActaServicio)
        {
            _consultaNotariaSegura = consultaNotariaSegura;
            _tramiteRepositorio = tramiteRepositorio;
            _actaNotarialRepositorio = actaNotarialRepositorio;
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _documentoPendienteAutorizarRepositorio = documentoPendienteAutorizarRepositorio;
            _historicosStorageInfraestructura = historicosStorageInfraestructura;
            _historicosCosmosInfraestructura = historicosCosmosInfraestructura;
            _generarActaServicio = generarActaServicio;
        }

        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }

            return num2Text.ToLower();
        }

        public async Task<ActaNotarialReturnDTO> ObtenerActaNotarial(ActaNotarialGetDTO actaNotarialGetDTO)
        {
            ActaNotarialReturnDTO resul = new ActaNotarialReturnDTO();

            //var tramite = (await _tramiteRepositorio.Obtener(t => t.TramiteId == actaNotarialGetDTO.TramiteId, t => t.TipoTramite)).FirstOrDefault();
            ArchivoActa archivoActa = await _actaNotarialRepositorio.ObtenerActa(actaNotarialGetDTO.TramiteId);
            if (archivoActa.EstadoTramiteId == (int)EnumEstadoTramites.Autorizado)
            {
                //var archivo = (await _actaNotarialRepositorio
                //    .Obtener(t => t.MetadataArchivoId == tramite.ActaNotarialId,
                //    nu => nu.Archivo))
                //    .FirstOrDefault();

                resul.Archivo = archivoActa.Contenido;
                resul.Autorizada = true;
                return resul;
            }

            if (archivoActa.EstadoTramiteId == (int)EnumEstadoTramites.Rechazado)
            {
                resul.Rechazada = true;
            }
            if (archivoActa.EstadoTramiteId == (int)EnumEstadoTramites.Otorgamiento || 
                archivoActa.EstadoTramiteId == (int)EnumEstadoTramites.Pendiente)
            {
                resul.Autorizada = false;
                resul.Rechazada = false;
            }

            return resul;
        }
                
        public async Task<string> ObtenerActaNotarialPublico(string codigo)
        {
            var hashids = new Hashids("NUTNOTARIA", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
            var idTramite = hashids.DecodeLong(codigo);

            ArchivoActa archivoActa = await _actaNotarialRepositorio.ObtenerActa(idTramite[0]);

            if (archivoActa?.Contenido != null)
            {
                var Archivo = archivoActa.Contenido;
                return Archivo;
            }

            return "No se encuentra el acta notarial";
        }

        public async Task<string> ObtenerActaNotarialSegura(ActaNotarialSeguraRequest request)
        {
            bool encontrado = false;
            long idTramite=0;
            try {
                var hashids = new Hashids("NUTNOTARIA", 12, "abcdefghijklmnopqrstuvwxyz0123456789");

                try
                {
                    idTramite = hashids.DecodeLong(request.Tramiteid)[0];
                }
                catch
                {
                    return "No se encuentra el acta notarial";
                }

                ArchivoActa archivoActa =
                        await _actaNotarialRepositorio.ObtenerActa(idTramite, request.NotariaId, request.FechaTramite);

                if (archivoActa?.Contenido != null)
                {
                    encontrado = true;
                    return archivoActa.Contenido;
                }

                return "No se encuentra el acta notarial";
            }
            finally
            {
                await LogConsultaNotariaSegura(request, idTramite.ToString(), encontrado);
            }
        }

        public async Task<string> ObtenerActaNotarialSeguraHistorico(ActaNotarialSeguraRequest request)
        {
            bool encontrado = false;
            try
            {
                var rutaArchivo = await _historicosCosmosInfraestructura.ObtenerRutaArchivo(new Dominio.ContextoPrincipal.Entidad.CosmosDB.ConsultaHistoricoNotariaSegura
                {
                    Nut = request.Tramiteid,
                    Fecha = request.FechaTramite.ToString("yyyy-MM-dd"),
                    NotariaId = request.NotariaId.ToString()
                });

                if (!string.IsNullOrEmpty(rutaArchivo))
                {
                    var acta = await _historicosStorageInfraestructura.ObtenerActa(rutaArchivo);
                    if (!string.IsNullOrEmpty(acta) && acta != "No se encuentra el acta notarial")
                    {
                        encontrado = true;
                        return acta;
                    }
                }

                
                return "No se encuentra el acta notarial";
            }
            finally
            {
                await LogConsultaNotariaSegura(request, $"historico-{request.Tramiteid}", encontrado);
            }
        }


        public bool ValidarconfiguracionFirma(Notario notario, string pin, out EnumResultadoFirma res)
        {
            res = 0;
            if (notario?.GrafoId == null)
                res = EnumResultadoFirma.FirmaNoConfigurada;
            else if (string.IsNullOrWhiteSpace(notario?.Pin))
                res = EnumResultadoFirma.PinNoAsignado;
            else if (notario.Pin != pin && !CifradoSHA512.CompararTexto($"{notario.Pin}{DateTime.Now.ToString("yyyyMMdd")}", pin))
                res = EnumResultadoFirma.PinNoValido;
            else
                return true;
            return false;
        }

        #endregion
                
        public async Task<TramiteRechazadoReturnDTO> RechazarTramiteNotarial(TramiteRechazadoDTO pinFirmaDTO)
        {
            TramiteRechazadoReturnDTO result = new TramiteRechazadoReturnDTO
            {
                Estado = "",
                EsError = false
            };
            var tramite = _tramiteRepositorio.Obtener(pinFirmaDTO.TramiteId);
            if (tramite == null)
            {
                result.EsError = true;
                result.Estado = "Tramite no existe";
                return result;
            }
            var notariausuario =
                _notariasUsuarioRepositorio
                    .ObtenerSync(n => n.IsDeleted == false &&
                                n.UsuariosId == pinFirmaDTO.UserId &&
                                n.NotariaId == pinFirmaDTO.NotariaId,
                    n => n.Notario,
                    n=>n.Persona).FirstOrDefault();
            if (notariausuario == null || notariausuario.Notario == null)
            {
                result.EsError = true;
                result.Estado = "Usuario no autorizado";
                return result;
            }
            if (notariausuario.Notario.Pin == null || notariausuario.Notario.Pin != pinFirmaDTO.Pin)
            {
                result.EsError = true;
                result.Estado = "El pin no es válido";
                return result;
            }
            tramite.EstadoTramiteId = (long)EnumEstadoTramites.Rechazado;
            string notarioNombre = $"{notariausuario.Persona.Nombres} {notariausuario.Persona.Apellidos}";
            dynamic auxDatosAdicionales = JsonConvert.DeserializeObject<object>(tramite.DatosAdicionales);
            if (auxDatosAdicionales == null)
                auxDatosAdicionales = new { MotivoRechazo = pinFirmaDTO.MotivoRechazo, RechazadoPor=notarioNombre };
            else
            {
                auxDatosAdicionales.MotivoRechazo = pinFirmaDTO.MotivoRechazo;
                auxDatosAdicionales.RechazadoPor = notarioNombre;
            }
                
            tramite.DatosAdicionales = JsonConvert.SerializeObject(auxDatosAdicionales);
            _tramiteRepositorio.Modificar(tramite);
            _tramiteRepositorio.UnidadDeTrabajo.Commit();
            return result;
        }

        async Task<AutorizacionTramitesResponseDTO[]> IActaNotarialServicio.FirmaActaNotarialLote(AutorizacionTramitesDTO autorizacionTramites)
        {
            List<AutorizacionTramitesResponseDTO> response = new List<AutorizacionTramitesResponseDTO>(autorizacionTramites.TramiteId.Count());

            var notariaUsuario =
                (await _notariasUsuarioRepositorio.Obtener(
                       t => t.IsDeleted == false &&
                       t.UserEmail == autorizacionTramites.UserName,
                       nu => nu.Notaria.Ubicacion.UbicacionPadre,
                       nu => nu.Notario,
                       nu => nu.Persona
                    )).FirstOrDefault();

            EnumResultadoFirma stat;

            if (!ValidarconfiguracionFirma(notariaUsuario.Notario, autorizacionTramites.Pin, out stat))
            {
                //response = autorizacionTramites?.TramiteId?.Select(
                //    id => new AutorizacionTramitesResponseDTO()
                //    {
                //        CodigoResultado = (int)stat,
                //        EsError = true,
                //    }).ToArray();
                int j = 0;
                foreach (long id in autorizacionTramites.TramiteId)
                {
                    int codResul = (int)stat;
                    response.Add(new AutorizacionTramitesResponseDTO()
                    {
                        TramiteId = id,
                        Autorizada =false,
                        CodigoResultado = codResul,
                        EsError = true
                    });

                    j++;
                }
                return response.ToArray();
            }


            int i = 0;
            List<Tramite> tramites = (await _tramiteRepositorio.Obtener(t => autorizacionTramites.TramiteId.Contains(t.TramiteId))).ToList();
            foreach (long id in autorizacionTramites.TramiteId)
            {
                var tramiteParaAutorizar = tramites.FirstOrDefault(t => t.TramiteId == id);
                bool EsPendienteAutorizar = (await _documentoPendienteAutorizarRepositorio.Obtener(d => d.TramiteId == id)).FirstOrDefault() == null;
                if (tramiteParaAutorizar.EstadoTramiteId == (int)EnumEstadoTramites.Pendiente &&
                    EsPendienteAutorizar)
                {
                    var documentoPendiente = new DocumentoPendienteAutorizar
                    {
                        FechaCreacion = DateTime.Now,
                        Estado = EstadoDocumento.PENDIENTE,
                        Generado = false,
                        IsDeleted = false,
                        NotariaId = autorizacionTramites.NotariaId,
                        NotarioUsuarioId = notariaUsuario.NotariaUsuariosId,
                        TramiteId = id,
                        UsarSticker = tramiteParaAutorizar.UsarSticker,
                        UsuarioCreacion = autorizacionTramites.UserName,
                        UsuarioModificacion = autorizacionTramites.UserName
                    };
                    _documentoPendienteAutorizarRepositorio.Agregar(documentoPendiente);
                    response.Add(new AutorizacionTramitesResponseDTO()
                    {
                        TramiteId = id,
                        Autorizada = true,
                        CodigoResultado = (int)EnumResultadoFirma.Firmado,
                        EsError = false
                    });
                }
                else if ((tramiteParaAutorizar.EstadoTramiteId == (int)EnumEstadoTramites.Pendiente &&
                   !EsPendienteAutorizar) || tramiteParaAutorizar.EstadoTramiteId == (int)EnumEstadoTramites.Autorizado)
                {
                    response.Add(new AutorizacionTramitesResponseDTO()
                    {
                        TramiteId = id,
                        Autorizada = false,
                        CodigoResultado = (int)EnumResultadoFirma.TramiteEstaAutorizado,
                        EsError = true
                    });
                }
                else
                {
                    response.Add(new AutorizacionTramitesResponseDTO()
                    {
                        TramiteId = id,
                        Autorizada = false,
                        CodigoResultado = (int)EnumResultadoFirma.TramiteEstadoDifentePendiente,
                        EsError = true
                    });
                }
                i++;
            }
            _documentoPendienteAutorizarRepositorio.UnidadDeTrabajo.Commit();
            return response.ToArray();
        }

        public async Task<ActaResumen> ObtenerResumen(long tramiteId)
        {
            DatosTramiteResumen datosTramiteResumen = await _tramiteRepositorio.ObtenerDatosTramiteResumen(tramiteId);

            if (datosTramiteResumen == null)
                throw new ArgumentException("Tramite no existe");

            var resumen = datosTramiteResumen.Adaptar<ActaResumen>();
            return resumen;

        }

        public async Task CrearActaParaFirmaManual(ActaCreateDTO actaCreate)
        {
            NotariaUsuarios notariaUsuarios = (await _notariasUsuarioRepositorio.Obtener(nu => nu.NotariaId == actaCreate.NotariaId &&
                             nu.Notaria.NotarioEnTurno == nu.Notario.NotarioId, nu => nu.Notario)).FirstOrDefault();
            if (notariaUsuarios == null)
                notariaUsuarios = (await _notariasUsuarioRepositorio.Obtener(nu => nu.NotariaId == actaCreate.NotariaId && nu.Notario.TipoNotario == 1)).FirstOrDefault();

            string notarioUserEmail = notariaUsuarios?.UserEmail;
            DatosActaFirmaManual datos = await ObtenerDatosActaFirmaManual(actaCreate);

            var archivo = await _generarActaServicio.GenerarArchivoPrevioAsync(actaCreate.TramiteId,
                usarSticker: actaCreate.UsarSticker, 
                notarioUser: notarioUserEmail, 
                esFirmaManual: true, 
                datosActaFirmaManual: datos);
            Tramite tramite = await _tramiteRepositorio.GetOneAsync(m => m.TramiteId == actaCreate.TramiteId);
            tramite.EstadoTramiteId = (int)EnumEstadoTramites.Autorizado;
            tramite.ActaNotarial = new MetadataArchivo()
            {
                Nombre = $"Acta Notarial Tramite {tramite.TramiteId}",
                Extension = ".pdf",
                Tamanio = 0,
                Ruta = "",
                Archivo = new Archivo()
                {
                    Contenido = archivo.archivo
                }
            };

            _tramiteRepositorio.Modificar(tramite);
            _tramiteRepositorio.UnidadDeTrabajo.Commit();
        }

        private async Task<DatosActaFirmaManual> ObtenerDatosActaFirmaManual(ActaCreateDTO actaCreate)
        {
            DatosActaFirmaManual datos = new DatosActaFirmaManual();
            var datosNotaria = await _actaNotarialRepositorio.ObtenerDatosNotarioFirmaManual(actaCreate.NotariaId);

            datos.datosNotaria = new DatosNotaria()
            {
                NotariaId = actaCreate.NotariaId,
                Municipio = datosNotaria.Municipio,
                Departamento = datosNotaria.Departamento,
                NumeroNotaria = datosNotaria.NumeroNotaria,
                NumeroNotariaEnLetras = datosNotaria.NumeroNotariaEnLetras,
                CirculoNotaria = datosNotaria.CirculoNotaria,
                FechaModificacion = DateTime.Now
            };

            datos.datosNotario = new DatosNotario()
            {
                Nombres = datosNotaria.Nombres,
                Apellidos = datosNotaria.Apellidos,
                Pin = "",
                TipoNotario = Convert.ToInt32(datosNotaria.TipoNotario),
                FechaModificacion = DateTime.Now,
                Genero = datosNotaria.Genero
            };

            datos.datosTramiteActa = new DatosTramiteActa()
            {
                TramiteId = actaCreate.TramiteId,
                TipoTramite = actaCreate.TipoTramite,
                TipoTramiteCodigo = actaCreate.CodigoTramite,
                FechaTramite = actaCreate.FechaTramite,
                DatosAdicionales = actaCreate.DataAdicional,
                Plantilla = "",
                Plantilla2 = "",
                UsarSticker = actaCreate.UsarSticker,
                FueraDeDespacho = actaCreate.FueraDeDespacho,
                DireccionComparecencia = actaCreate.DireccionComparecencia
            };

            int idCompareciente = 1;
            foreach (var c in actaCreate.ComparecientesCreate)
            {
                datos.comparecientesFirmaManual.Add(new DatosComparecienteActa()
                {
                    NombreTipoDocumento = c.NombreTipoDocumento,
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    NUIPCompareciente = c.NUIPCompareciente,
                    NUT = c.NUT,
                    FechaCreacion = c.FechaCreacion,
                    TramiteSinBiometria = c.TramiteSinBiometria == "1" ? true : false,
                    MotivoSinBiometria = c.TextoBiometria,
                    FirmaId = idCompareciente,
                    FotoId = idCompareciente,
                    Posicion = Convert.ToInt32(c.Posicion)
                });
                datos.fotosFirmaManual.Add(new DatosImagenActa()
                {
                    MetadataArchivoId = idCompareciente,
                    Contenido = c.FotoCompareciente
                });
                datos.grafosFirmaManual.Add(new DatosImagenActa()
                {
                    MetadataArchivoId = idCompareciente,
                    Contenido = c.FirmaCompareciente
                });
                idCompareciente++;
            }
            return datos;
        }


        public async Task<TramiteRechazadoReturnDTO> CancelarTramiteNotarial(TramiteRechazadoDTO pinFirmaDTO)
        {
            TramiteRechazadoReturnDTO result = new TramiteRechazadoReturnDTO
            {
                Estado = "",
                EsError = false
            };
            var tramite = _tramiteRepositorio.Obtener(pinFirmaDTO.TramiteId);
            if (tramite == null)
            {
                result.EsError = true;
                result.Estado = "Tramite no existe";
                return result;
            }
            var notariausuario =
                _notariasUsuarioRepositorio
                    .ObtenerSync(n => n.IsDeleted == false &&
                                n.UsuariosId == pinFirmaDTO.UserId &&
                                n.NotariaId == pinFirmaDTO.NotariaId,
                    n => n.Notario,
                    n => n.Persona).FirstOrDefault();
            if (notariausuario == null)
            {
                result.EsError = true;
                result.Estado = "Usuario no autorizado";
                return result;
            }
            
            tramite.EstadoTramiteId = (long)EnumEstadoTramites.Cancelado;
            string notarioNombre = $"{notariausuario.Persona.Nombres} {notariausuario.Persona.Apellidos}";
            dynamic auxDatosAdicionales = JsonConvert.DeserializeObject<object>(tramite.DatosAdicionales);
            if (auxDatosAdicionales == null)
                auxDatosAdicionales = new { MotivoCancelacion = pinFirmaDTO.MotivoRechazo, CanceladoPor = notarioNombre };
            else
            {
                auxDatosAdicionales.MotivoCancelacion = pinFirmaDTO.MotivoRechazo;
                auxDatosAdicionales.CanceladoPor = notarioNombre;
            }

            tramite.DatosAdicionales = JsonConvert.SerializeObject(auxDatosAdicionales);
            _tramiteRepositorio.Modificar(tramite);
            _tramiteRepositorio.UnidadDeTrabajo.Commit();
            return result;
        }

        private async Task LogConsultaNotariaSegura(ActaNotarialSeguraRequest request, string idTramite, bool result)
        {

            _consultaNotariaSegura.Agregar(new ConsultaNotariaSegura
            {
                TramiteId = idTramite,
                TramiteIdHash = request.Tramiteid,
                NotariaId = request.NotariaId,
                Email = request.Email,
                FechaConsulta = request.FechaTramite,
                EncontroArchivo = result
            });

            await _consultaNotariaSegura.UnidadDeTrabajo.CommitAsync();
        }
    }
}