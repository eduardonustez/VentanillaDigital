using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Contrato.Rest;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios.Interfaces;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Aplicacion.ContextoPrincipal.Servicio.Rest;
using Aplicacion.Nucleo.Base;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Log;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad;
using Dominio.ContextoPrincipal.Entidad.Log;
using Dominio.ContextoPrincipal.Entidad.Parametricas;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using GenericExtensions;
using HashidsNet;
using HerramientasFirmaDigital;
using HerramientasFirmaDigital.Abstraccion;
using Infraestructura.Transversal;
using Infraestructura.Transversal.Cache;
using Infraestructura.Transversal.Correo;
using Infraestructura.Transversal.Helpers;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PdfTronUtils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Aplicacion.ContextoPrincipal.Servicio.Transaccional
{
    public class PortalVirtualServicio : BaseServicio, IPortalVirtualServicio
    {
        #region Mensajes
        const string objetoNullActualizacion = "Objeto null lo sentimos.";
        const string errorActualizarTramite = "Error al actualizar el tramite virtual";
        const string errorActualizarArchivos = "Error al insertar los nuevos archivos";
        const string tramiteNoValido = "Tramite virtual invalido";
        const string notariaIncorrecta = "Notaria invalida ó no pertenece a Notaria Digital";
        const string estadoIncorrecta = "Estado de tramite invalido";
        const string opcionArchivoNull = "Archivo null invalido";
        const string CoordenadasInvalidas = "la ubicación enviada no pertenece a la Ciudad de";
        const string ErrorObtenerCoordenadas = "lo sentimos no fue posible validar su ubicación por favor intente de nuevo, si el error persiste comuniquese con el administrador";
        #endregion

        private readonly IRecaudoTramiteVirtualRepositorio _recaudoTramiteVirtualRepositorio;
        private readonly IPortalVirtualRepositorio _portalVirtualRepositorio;
        private readonly IArchivosPortalVirtualRepositorio _archivosPortalVirtualRepositorio;
        private readonly ITipoTramiteVirtualRepositorio _tipoTramiteVirtualRepositorio;
        private readonly IEstadoTramiteVirtualRepositorio _estadoTramiteVirtualRepositorio;
        private readonly IConvenioNotariaVirtualRepositorio _convenioNotariaVirtualRepositorio;
        private readonly ITipoIdentificacionRepositorio _tipoIdentificacionRepositorio;
        private readonly IPersonasRepositorio _personasRepositorio;
        private readonly ILogTramitePortalVirtualRepositorio _logTramitePortalVirtualRepositorio;
        private IPaymentManagementRestApiService _miFirmaRestApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IFirmaEnPaginaAdicional _firmaEnPaginaAdicional;
        private readonly INotariasUsuarioRepositorio _notariasUsuarioRepositorio;
        private readonly IDigitalizacionNotarialServicio _digitalizacionNotarialServicio;
        private readonly IActoPorTramiteRepositorio _actoPorTramiteRepositorio;
        private readonly ImplementedCache _cache;
        private readonly IActoNotarialServicio _actoNotarialServicio;
        private readonly IPdfFiller _pdfFiller;
        private readonly IPdfTronService _pdfTronService;
        private readonly ITipoArchivoTramiteVirtualRepositorio _tipoArchivoTramiteVirtualRepositorio;
        private readonly ITramitePortalVirtualMensajeRepositorio _tramitePortalVirtualMensajeRepositorio;
        private readonly ITemplateServicio _templateServicio;
        private readonly IManejadorCorreos _manejadorCorreos;
        private readonly ITramiteRepositorio _tramiteRepositorio;
        private readonly IComparecienteRepositorio _comparecienteRepositorio;
        private readonly IActoNotarialRepositorio _actoNotarialRepositorio;

        public PortalVirtualServicio(IPortalVirtualRepositorio portalVirtualRepositorio
            , IRecaudoTramiteVirtualRepositorio recaudoTramiteVirtualRepositorio
            , IArchivosPortalVirtualRepositorio archivosPortalVirtualRepositorio
            , ITipoTramiteVirtualRepositorio tipoTramiteVirtualRepositorio
            , IEstadoTramiteVirtualRepositorio estadoTramiteVirtualRepositorio
            , IConvenioNotariaVirtualRepositorio convenioNotariaVirtualRepositorio
            , ITipoIdentificacionRepositorio tipoIdentificacionRepositorio
            , IPersonasRepositorio personasRepositorio
            //, IMiFirmaRestApiService miFirmaRestApiService
            , ILogTramitePortalVirtualRepositorio logTramitePortalVirtualRepositorio
            , IHttpContextAccessor httpContextAccessor
            , IConfiguration configuration
            , IPdfTronService pdfTronService
            , IFirmaEnPaginaAdicional firmaEnPaginaAdicional
            , IDigitalizacionNotarialServicio digitalizacionNotarialServicio
            , ImplementedCache cache
            , INotariasUsuarioRepositorio notariasUsuarioRepositorio
            , IActoPorTramiteRepositorio actoPorTramiteRepositorio
            , IActoNotarialServicio actoNotarialServicio
            , IPdfFiller pdfFiller
            , ITemplateServicio templateServicio
            , ITramitePortalVirtualMensajeRepositorio tramitePortalVirtualMensajeRepositorio
            , ITipoArchivoTramiteVirtualRepositorio tipoArchivoTramiteVirtualRepositorio
            , ITramiteRepositorio tramiteRepositorio
            , IActoNotarialRepositorio actoNotarialRepositorio
            , IComparecienteRepositorio comparecienteRepositorio)
            : base(portalVirtualRepositorio
                  , archivosPortalVirtualRepositorio
                  , tipoTramiteVirtualRepositorio
                  , estadoTramiteVirtualRepositorio
                  , convenioNotariaVirtualRepositorio
                  , tipoIdentificacionRepositorio
                  , personasRepositorio
                  , tipoIdentificacionRepositorio
                  , logTramitePortalVirtualRepositorio
                  , actoPorTramiteRepositorio
                  , actoNotarialServicio
                  , templateServicio
                  , tipoArchivoTramiteVirtualRepositorio
                  , tramitePortalVirtualMensajeRepositorio
                  , recaudoTramiteVirtualRepositorio
                  , tramiteRepositorio
                  , actoNotarialRepositorio
                  , comparecienteRepositorio)
        {
            _cache = cache;
            _pdfFiller = pdfFiller;
            _configuration = configuration;
            _pdfTronService = pdfTronService;
            _templateServicio = templateServicio;
            _manejadorCorreos = new ManejadorCorreos(configuration.GetSection("ConfigServidorOlimpia").Get<ServidorCorreo>());
            _httpContextAccessor = httpContextAccessor;
            //_miFirmaRestApiService = new MiFirmaRestApiService(configuration["ServiciosExternos:MiFirma:Uri"] ?? ""); //miFirmaRestApiService;
            _firmaEnPaginaAdicional = firmaEnPaginaAdicional;
            _portalVirtualRepositorio = portalVirtualRepositorio;
            _actoPorTramiteRepositorio = actoPorTramiteRepositorio;
            _notariasUsuarioRepositorio = notariasUsuarioRepositorio;
            _digitalizacionNotarialServicio = digitalizacionNotarialServicio;
            _archivosPortalVirtualRepositorio = archivosPortalVirtualRepositorio;
            _tipoTramiteVirtualRepositorio = tipoTramiteVirtualRepositorio;
            _estadoTramiteVirtualRepositorio = estadoTramiteVirtualRepositorio;
            _convenioNotariaVirtualRepositorio = convenioNotariaVirtualRepositorio;
            _tipoIdentificacionRepositorio = tipoIdentificacionRepositorio;
            _recaudoTramiteVirtualRepositorio = recaudoTramiteVirtualRepositorio;
            _personasRepositorio = personasRepositorio;
            _logTramitePortalVirtualRepositorio = logTramitePortalVirtualRepositorio;
            _actoNotarialServicio = actoNotarialServicio;
            _tipoArchivoTramiteVirtualRepositorio = tipoArchivoTramiteVirtualRepositorio;
            _tramitePortalVirtualMensajeRepositorio = tramitePortalVirtualMensajeRepositorio;
            _tramiteRepositorio = tramiteRepositorio;
            _comparecienteRepositorio = comparecienteRepositorio;
            _actoNotarialRepositorio = actoNotarialRepositorio;
        }

        #region Contratos
        public async Task<ListaTramitePortalVirtualReturnDTO> ObtenerTramitePortalVirtual(DefinicionFiltro definicionFiltro)
        {
            var tramitesObtenidos = await _portalVirtualRepositorio.ObtenerTramitePortalVirtual(definicionFiltro);
            var resul = tramitesObtenidos.Adaptar<ListaTramitePortalVirtualReturnDTO>();

            resul.TramitesPortalVirtualReturn = JsonConvert.DeserializeObject<IEnumerable<TramitePortalVirtualReturnDTO>>(tramitesObtenidos.Resultado);

            return resul;
        }

        public async Task<decimal> TotalPagadoTramite(int tramitePortalVirtualId)
        {
            var valorPagado = await _recaudoTramiteVirtualRepositorio.ObtenerTodo()
                .Where(m => m.TramitePortalVirtualId == tramitePortalVirtualId)
                .SumAsync(m => m.ValorPagado);

            return valorPagado;
        }

        public async Task<bool> RegistrarCiudadano(TramitePortalVirtualCiudadanoDTO TramitePortalVirtual)
        {
            await ValidarRequestRegistroCiudadano(TramitePortalVirtual);

            var TramitePortalVirtualCiudadano = TramitePortalVirtual.Adaptar<TramitesPortalVirtual>();
            TramitePortalVirtualCiudadano.ActoPrincipalId = await _actoNotarialRepositorio.ObtenerActoNotarialId(TramitePortalVirtual.ActoPrincipalId.ToString());
            _portalVirtualRepositorio.Agregar(TramitePortalVirtualCiudadano);
            await _portalVirtualRepositorio.UnidadDeTrabajo.CommitAsync().ConfigureAwait(false);

            if (TramitePortalVirtualCiudadano.TramitesPortalVirtualId > 0)
            {
                var id = TramitePortalVirtualCiudadano.TramitesPortalVirtualId;
                int totalArchivos = InsertarArchivos(id, TramitePortalVirtual.Archivos);
                if (totalArchivos > 0)
                {
                    if (TramitePortalVirtual.ActosTramite != null
                        && TramitePortalVirtual.ActosTramite.Count > 0)
                    {
                        int resultadoActosTramite = InsertarActosPorTramite(TramitePortalVirtual.ActosTramite, id);
                        if (resultadoActosTramite > 0)
                        {
                            return true;
                        }

                        throw new Exception($"ValidarRequestRegistroCiudadano: resultadoActosTramite");
                        //return false;
                    }
                    return true;
                }
            };

            throw new Exception($"RegistrarCiudadano: TramitePortalVirtualCiudadano.TramitesPortalVirtualId");
        }

        public async Task<ActualizarTramiteVirtualResponseDTO> ActualizarTramiteVirtual(ActualizarTramiteVirtualDTO actualizarTramite)
        {
            ActualizarTramiteVirtualResponseDTO respuesta;

            respuesta = await ValidarPeticionActualizacionTramite(actualizarTramite);

            if (string.IsNullOrWhiteSpace(respuesta.MensajeError))
            {
                if (actualizarTramite.Archivos != null)
                {
                    if (actualizarTramite.BorrarArchivo
                    && actualizarTramite.Archivos.Count > 0)
                    {
                        bool esActualizadoTramite = await ActualizarArchivosYTramiteVirtual(actualizarTramite);
                        if (esActualizadoTramite)
                            respuesta.MensajeError = string.Empty;
                        else
                            respuesta.MensajeError = errorActualizarTramite;
                        respuesta.EsTramiteActualizado = esActualizadoTramite;
                    }

                    if (!actualizarTramite.BorrarArchivo && actualizarTramite.Archivos.Count > 0)
                    {
                        int esActualizadoEstadoTramite = await ActualizarEstadoTramiteVirtual(actualizarTramite);
                        if (esActualizadoEstadoTramite > 0)
                        {
                            bool esArchivosInsertados = await InsertarArchivosNuevos(actualizarTramite);
                            if (esArchivosInsertados)
                                respuesta.MensajeError = string.Empty;
                            else
                                respuesta.MensajeError = errorActualizarArchivos;

                            respuesta.EsTramiteActualizado = esArchivosInsertados;
                        }
                        else
                            respuesta.MensajeError = errorActualizarTramite;
                    }

                    if (!actualizarTramite.BorrarArchivo)
                    {
                        int esActualizadoEstadoTramite = await ActualizarEstadoTramiteVirtual(actualizarTramite);

                        if (esActualizadoEstadoTramite > 0)
                        {
                            respuesta.MensajeError = string.Empty;
                            respuesta.EsTramiteActualizado = true;
                        }
                        else
                        {
                            respuesta.MensajeError = errorActualizarTramite;
                            respuesta.EsTramiteActualizado = false;
                        }
                    }
                }
                else
                {
                    respuesta.MensajeError = opcionArchivoNull;
                    respuesta.EsTramiteActualizado = false;
                }
            }
            else
            {
                respuesta.EsTramiteActualizado = false;
                respuesta.MensajeError = respuesta.MensajeError;
            }
            return respuesta;
        }

        public async Task<bool> EnviarRecaudo(long recaudoTramiteVirtualId, EnviarRecaudoModel model)
        {
            var recaudo = await _recaudoTramiteVirtualRepositorio
                .GetOneAsync(m => m.RecaudoTramiteVirtualId == recaudoTramiteVirtualId, m => m.TramitesPortalVirtual);

            if (recaudo == null) throw new Exception("Recaudo no encontrado.");

            _recaudoTramiteVirtualRepositorio.UnidadDeTrabajo.Begin();

            if (model.Archivos != null && model.Archivos.Any())
            {

                foreach (var item in model.Archivos)
                {
                    _archivosPortalVirtualRepositorio.Agregar(new ArchivosPortalVirtual
                    {
                        Base64 = item.Data,
                        Formato = item.Formato,
                        Nombre = item.Nombre,
                        TramitesPortalVirtualId = recaudo.TramitePortalVirtualId,
                        TipoArchivo = 2
                    });
                }

                //await _archivosPortalVirtualRepositorio.UnidadDeTrabajo.CommitAsync();
            }

            List<string> archivosBase64 = new List<string>();

            if (model.ArchivosPortalVirtualId != null && model.ArchivosPortalVirtualId.Any())
            {
                var archivos = await _archivosPortalVirtualRepositorio.ObtenerTodo().Where(m => model.ArchivosPortalVirtualId.Contains(m.ArchivosPortalVirtualId)).ToListAsync();

                if (archivos.Any()) archivosBase64.AddRange(archivos.Select(m => m.Base64).ToList());
            }

            if (model.Archivos != null && model.Archivos.Any()) archivosBase64.AddRange(model.Archivos.Select(m => m.Data).ToList());


            var convenioNotaria = await _convenioNotariaVirtualRepositorio.ObtenerTodo()
                .Where(x => x.NotariaId == recaudo.TramitesPortalVirtual.NotariaId).FirstOrDefaultAsync();

            if (convenioNotaria == null) throw new Exception("Convenio Notaría no encontrado.");
            _miFirmaRestApiService = new PaymentManagementRestApiService(convenioNotaria.UrlApiMiFirma);
            var res = await _miFirmaRestApiService.CreatePaymentLink(new Modelo.Rest.CreatePaymentLinkModel
            {
                OrderAmount = (long)recaudo.ValorTotal,
                OrderTax = (long)recaudo.IVA,
                Concept = recaudo.Observacion,
                Cuandi = recaudo.TramitesPortalVirtual.CUANDI,
                Email = recaudo.Correo,
                NombreDestinatario = recaudo.NombreCompleto,
                DocumentBase64 = archivosBase64.Any() ? archivosBase64[0] : null,
                Document2Base64 = archivosBase64.Count >= 2 ? archivosBase64[1] : null,
                Document3Base64 = archivosBase64.Count >= 3 ? archivosBase64[2] : null,
                TransaccionId = recaudoTramiteVirtualId
            });

            recaudo.RespuestaServicio = res;
            recaudo.Estado = eEstadoRecaudo.Enviado;

            _recaudoTramiteVirtualRepositorio.Modificar(recaudo);
            await _recaudoTramiteVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            return true;
        }

        public async Task<bool> EliminarRecaudo(long recaudoTramiteVirtualId)
        {
            var recaudo = await _recaudoTramiteVirtualRepositorio.GetOneAsync(m => m.RecaudoTramiteVirtualId == recaudoTramiteVirtualId);

            if (recaudo == null) throw new Exception("Recaudo no encontrado");

            _recaudoTramiteVirtualRepositorio.Eliminar(recaudo, true);
            await _recaudoTramiteVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            return true;
        }

        public async Task<ArchivoTramiteVirtual> ConsultarArchivoTramiteVirtualPorId(long archivoId)
        {
            var archivo = await _archivosPortalVirtualRepositorio.ObtenerArchivoPortalVirtual(archivoId);

            if (archivo != "[]")
            {
                return JsonConvert.DeserializeObject<IEnumerable<ArchivoTramiteVirtual>>(archivo).FirstOrDefault();
            }
            else
            {
                return new ArchivoTramiteVirtual();
            }
        }

        public async Task<TramiteVirtualModel> ConsultarTramiteVirtualPorId(ConsultarTramiteDTO consultarTramite)
        {
            var tramite = await _portalVirtualRepositorio.ObtenerTodo()
                //.Where(m => m.TramitesPortalVirtualId == consultarTramite.TramiteId && m.NotariaId == consultarTramite.NotariaId)
                .Where(m => m.TramitesPortalVirtualId == consultarTramite.TramiteId)
                .Include("EstadoTramiteVirtual")
                .Include("TipoTramiteVirtual")
                .Include("ActoPrincipal")
                .FirstOrDefaultAsync();

            if (tramite == null) throw new Exception("Trámite no encontrado");

            var archivos = await _archivosPortalVirtualRepositorio.ObtenerTodo()
                .Where(m => m.TramitesPortalVirtualId == consultarTramite.TramiteId && m.TipoArchivo != 9)
                .Select(m => new ArchivoTramiteVirtual
                {
                    ArchivosPortalVirtualId = m.ArchivosPortalVirtualId,
                    Formato = m.Formato,
                    Nombre = m.Nombre,
                    TipoArchivo = m.TipoArchivo,
                    TipoNombre = m.TipoArchivoTramiteVirtual.Nombre
                })
                .ToListAsync();

            var tipoIdentificacion = await _tipoIdentificacionRepositorio.GetOneAsync(m => m.TipoIdentificacionId == tramite.TipoDocumento);

            return new TramiteVirtualModel
            {
                TramiteVirtualID = tramite.TramiteVirtualID,
                TramiteVirtualGuid = tramite.TramiteVirtualGuid,
                EstadoTramiteVirtualId = tramite.EstadoTramiteVirtualId,
                NotariaId = tramite.NotariaId,
                NumeroDocumento = tramite.NumeroDocumento,
                TipoDocumento = tramite.TipoDocumento,
                TipoDocumentoNombre = tipoIdentificacion?.Nombre,
                TipoTramiteVirtualId = tramite.TipoTramiteVirtualId,
                DatosAdicionales = tramite.DatosAdicionales,
                CUANDI = tramite.CUANDI,
                EstadoNombre = tramite.EstadoTramiteVirtual?.Nombre,
                Fecha = tramite.FechaCreacion,
                TipoTramiteVirtualNombre = tramite.TipoTramiteVirtual?.Nombre,
                Archivos = archivos,
                ActoPrincipalId = tramite.ActoPrincipalId,
                ActoPrincipalNombre = $"{tramite.ActoPrincipal?.Codigo} - {tramite.ActoPrincipal?.Nombre}"
            };
        }

        public async Task<IEnumerable<TramiteVirtualMensajeModel>> ConsultarMensajesTramiteVirtual(long tramiteId)
        {
            var mensajes = await _tramitePortalVirtualMensajeRepositorio.Obtener(m => m.TramitePortalVirtualId == tramiteId);

            return mensajes?.Select(m => new TramiteVirtualMensajeModel
            {
                EsNotario = m.EsNotario,
                Fecha = m.FechaCreacion,
                Mensaje = m.Mensaje,
                TramitePortalVirtualMensajeId = m.TramitePortalVirtualMensajeId,
                Usuario = m.UsuarioCreacion
            }).OrderByDescending(m => m.Fecha).ToList();
        }

        public async Task<IEnumerable<RecaudoTramiteModel>> ConsultarRecaudosTramite(long tramiteId)
        {
            var res = await _recaudoTramiteVirtualRepositorio.Obtener(m => m.TramitePortalVirtualId == tramiteId);

            return res?.Select(m => new RecaudoTramiteModel
            {
                TramitePortalVirtualId = m.TramitePortalVirtualId,
                Correo = m.Correo,
                CUS = m.CUS,
                Estado = GetStatusName(m.Estado),
                FechaPagado = m.FechaPagado,
                NombreCompleto = m.NombreCompleto,
                NumeroIdenficacion = m.NumeroIdenficacion,
                Observacion = m.Observacion,
                RecaudoTramiteVirtualId = m.RecaudoTramiteVirtualId,
                TipoIdentificacion = m.TipoIdentificacion,
                ValorPagado = m.ValorPagado,
                ValorTotal = m.ValorTotal
            }).ToList();
        }

        public async Task<ResponseCrearRecaudo> GuardarRecaudo(CrearRecaudoTramiteVirtualModel body)
        {
            if (body.TramitePortalVirtualId == 0 || string.IsNullOrEmpty(body.Correo) || string.IsNullOrEmpty(body.NumeroIdentificacion)
                || body.TipoIdentificacion == 0 || body.Valor == 0) throw new Exception("Por favor complete los datos.");

            if (body.Valor < 100 || body.Valor > 100000000) throw new Exception("Valor no válido.");

            _recaudoTramiteVirtualRepositorio.Agregar(new RecaudoTramiteVirtual
            {
                TramitePortalVirtualId = body.TramitePortalVirtualId,
                Correo = body.Correo,
                NombreCompleto = body.NombreCliente,
                TipoIdentificacion = body.TipoIdentificacion,
                NumeroIdenficacion = body.NumeroIdentificacion,
                Estado = eEstadoRecaudo.Generado,
                ValorTotal = body.Valor,
                IVA = body.IVA,
                Observacion = body.Observacion,
                UsuarioCreacion = _httpContextAccessor.HttpContext.Request.Query["userId"].ToString()
            });

            await _recaudoTramiteVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            return new ResponseCrearRecaudo { Status = true };
        }

        public async Task<bool> ActualizarPago(long recaudoTramiteVirtualId, ActualizarPagoModel model)
        {
            var item = await _recaudoTramiteVirtualRepositorio.GetOneAsync(m => m.RecaudoTramiteVirtualId == recaudoTramiteVirtualId);

            if (item == null) throw new Exception("Recaudo no encontrado");

            if (item.ValorTotal < model.ValorPagado) throw new Exception("El valor pagado no debe ser mayor al valor total");

            item.ValorPagado = model.ValorPagado;
            item.CUS = model.Referencia;
            item.Estado = (eEstadoRecaudo)model.Estado;

            _recaudoTramiteVirtualRepositorio.Modificar(item);
            await _recaudoTramiteVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            return true;
        }

        public async Task<string> ConsultarUrlSubirArchivosMiFirma(long notariaId)
        {
            var convenioNotaria = await _convenioNotariaVirtualRepositorio.ObtenerTodo()
                .Where(x => x.NotariaId == notariaId).FirstOrDefaultAsync();

            if (convenioNotaria == null) throw new Exception("Convenio Notaría no encontrado.");

            var currentMillis = DateTime.Now.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;

            return $"{convenioNotaria.UrlSubirDocumentosMiFirma}?q={currentMillis}";
        }


        public async Task<TramiteVirtualModel> CambiarEstadoCliente(long tramiteId, List<UploadFileModel> files, bool estado, string usuario, string mensaje)
        {
            var tramite = await _portalVirtualRepositorio.GetOneAsync(m => m.TramitesPortalVirtualId == tramiteId,
                m => m.TipoTramiteVirtual, m => m.ActoPrincipal);

            var tipoArchivoTramiteVirtual = _tipoArchivoTramiteVirtualRepositorio.ObtenerTodo();

            if (tramite == null) throw new Exception("Trámite no encontrado");

            _portalVirtualRepositorio.UnidadDeTrabajo.Begin();
            tramite.EstadoTramiteVirtualId = estado ? 2 : 1;

            _tramitePortalVirtualMensajeRepositorio.Agregar(new TramitePortalVirtualMensaje
            {
                TramitePortalVirtualId = tramite.TramitesPortalVirtualId,
                Mensaje = mensaje,
                UsuarioCreacion = usuario,
                EsNotario = false
            });

            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    _archivosPortalVirtualRepositorio.Agregar(new ArchivosPortalVirtual
                    {
                        Base64 = file.Data,
                        Formato = file.Formato,
                        Nombre = file.Nombre,
                        TramitesPortalVirtualId = (int)tramiteId,
                        TipoArchivo = 1
                    });
                }
            }

            _portalVirtualRepositorio.Modificar(tramite);
            await _portalVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            return await ConsultarTramiteVirtualPorId(new ConsultarTramiteDTO { TramiteId = tramiteId, NotariaId = tramite.NotariaId });
        }

        public async Task<TramiteVirtualModel> CambiarEstado(long tramiteId, CambiarEstadoTramiteVirtualModel body)
        {
            var tramite = await _portalVirtualRepositorio.GetOneAsync(m => m.TramitesPortalVirtualId == tramiteId,
                m => m.TipoTramiteVirtual, m => m.ActoPrincipal);

            var tipoArchivoTramiteVirtual = _tipoArchivoTramiteVirtualRepositorio.ObtenerTodo();

            if (tramite == null) throw new Exception("Trámite no encontrado");

            _portalVirtualRepositorio.UnidadDeTrabajo.Begin();

            var estadoAnterior = tramite.EstadoTramiteVirtualId;

            // Validar tipo archivo - Autenticación

            await ActualizarTipoArchivoAutenticacion(tramite, body.Estado);

            // Actos notariales
            await AgregarActualizarActosNotariales(tramite.TramitesPortalVirtualId, tramite.EstadoTramiteVirtualId, body.ActosNotariales);

            tramite.EstadoTramiteVirtualId = body.Estado;
            tramite.ActoPrincipalId = body.ActoPrincipalId;

            if (tramite.EstadoTramiteVirtualId == 17)
            {
                if (string.IsNullOrEmpty(body.Razon)) throw new Exception("El mensaje es obligatorio.");

                if (body.Files != null && body.Files.Count > 0)
                {
                    foreach (var item in body.Files)
                    {
                        _archivosPortalVirtualRepositorio.Agregar(new ArchivosPortalVirtual
                        {
                            Base64 = item.Data,
                            Formato = item.Formato,
                            Nombre = item.Nombre,
                            TramitesPortalVirtualId = (int)tramiteId,
                            TipoArchivo = (short)item.Type
                        });
                    }
                }

                tramite.DetalleCambioEstado = body.Precio.ToString();
                var userId = _httpContextAccessor.HttpContext.Request.Query["userId"].ToString();

                _tramitePortalVirtualMensajeRepositorio.Agregar(new TramitePortalVirtualMensaje
                {
                    TramitePortalVirtualId = tramite.TramitesPortalVirtualId,
                    Mensaje = body.Razon.Trim(),
                    UsuarioCreacion = userId,
                    EsNotario = true
                });

                if (!string.IsNullOrEmpty(tramite.DatosAdicionales))
                {
                    var datosAdicionalesArr = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(tramite.DatosAdicionales);

                    if (datosAdicionalesArr == null) throw new Exception("No se encontró información del compareciente.");

                    var personaCorreo = "";
                    var nombreCliente = "";
                    int cn = 0;
                    foreach (Newtonsoft.Json.Linq.JObject comparecienteObj in datosAdicionalesArr)
                    {
                        //[{"FullName":"Eduar Aderty Restrepo Lucuara","Email":"eduar.restrepo@olimpiait.com"}]
                        if (comparecienteObj.ContainsKey("FullName"))
                        {
                            nombreCliente = $"{comparecienteObj.SelectToken("FullName")}";
                        }
                        if (comparecienteObj.ContainsKey("Email"))
                        {
                            personaCorreo = $"{comparecienteObj.SelectToken("Email")}";
                        }

                        if (!string.IsNullOrEmpty(personaCorreo))
                        {
                            var hashemail = new Hashids("EMAILCLIENTE", 12, "abcdefghijklmnopqrstuvwxyz0123456789");
                            var encodedNotaria = hashemail.EncodeHex(tramite.NotariaId.ToString());
                            var encodedNumber = hashemail.EncodeHex(cn.ToString());
                            var returnUrl = $"{_configuration["UrlPortalVirtual"]}tramitesportalvirtual/revisar/{hashemail.EncodeLong(tramiteId)}?nt={encodedNotaria}&cn={encodedNumber}";
                            await EnviarCorreoNotificacionTramiteCliente(returnUrl, personaCorreo, nombreCliente);
                        }
                        cn++;
                    }
                }
            }
            else if (tramite.EstadoTramiteVirtualId == 15)
            {
                tramite.DetalleCambioEstado = body.Razon.ToString();
            }
            else if (tramite.EstadoTramiteVirtualId == (int)EstadoTramiteVirtual.FirmadoNotarioAutorizado)
            {
                string notarioValido = ValidarUbicacionNotario(tramite.NotariaId, body);
                if (!string.IsNullOrEmpty(notarioValido))
                    throw new Exception(notarioValido);

                var userId = _httpContextAccessor.HttpContext.Request.Query["userId"].ToString();

                if (tramite.TipoTramiteVirtualId == 5)
                {
                    //Guardamos el testamento cifrado con la clave indicada
                    if (body.Files == null || body.Files.Count == 0) throw new Exception("No se envió el testamento");

                    if (string.IsNullOrEmpty(body.ClaveTestamento)) throw new Exception("No se envió la clave para el testamento");

                    _archivosPortalVirtualRepositorio.Agregar(new ArchivosPortalVirtual
                    {
                        Base64 = Cifrador.CifradoVariable(body.Files.FirstOrDefault().Data, body.ClaveTestamento), //cifrarlo
                        Formato = body.Files.FirstOrDefault().Formato,
                        Nombre = body.Files.FirstOrDefault().Nombre,
                        TramitesPortalVirtualId = (int)tramiteId,
                        TipoArchivo = tipoArchivoTramiteVirtual.Where(x => x.Nombre == "Testamento Cerrado").Select(c => c.TipoArchivoTramiteVirtualId).FirstOrDefault(), //Testamento cerrado
                    });
                }
                else if (tramite.TipoTramiteVirtualId == 1)
                {
                    //Borramos los archivos tipo 1
                    var archivos = await _archivosPortalVirtualRepositorio
                        .Obtener(m => m.TramitesPortalVirtualId == tramite.TramitesPortalVirtualId && m.TipoArchivo == 1);

                    if (archivos.Any())
                    {
                        foreach (var item in archivos) _archivosPortalVirtualRepositorio.Eliminar(item, true);
                    }
                }
                if (tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Compraventa
                    || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Matrimonio
                    || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoAbierto
                    || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoCerrado)
                {
                    tramite.DatosAdicionalesCierre = body.DatosAdicionalesCierre;
                }

                var minutas = await _archivosPortalVirtualRepositorio
                        .Obtener(m => m.TramitesPortalVirtualId == tramite.TramitesPortalVirtualId && m.TipoArchivo == 3);

                if (minutas.Any())
                {
                    var actosNotarialesPorTramite = await _actoPorTramiteRepositorio.ObtenerTodo()
                        .Where(m => m.TramitePortalVirtualId == tramiteId)
                        .Select(m => new Tuple<string, string>(m.ActoNotarial.Nombre, m.Cuandi))
                        .AsNoTracking()
                        .ToListAsync();

                    if (tramite.ActoPrincipalId.HasValue)
                        actosNotarialesPorTramite.Insert(0, new Tuple<string, string>(tramite.ActoPrincipal.Nombre, tramite.CUANDI));

                    foreach (var item in minutas)
                    {
                        string documentoBase64 = item.Base64;
                        Dictionary<string, string> datosPdf = null;

                        if ((tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Compraventa
                            || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.Matrimonio
                            || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoAbierto
                            || tramite.TipoTramiteVirtualId == (int)TipoTramiteVirtual.TestamentoCerrado)
                            && !string.IsNullOrEmpty(tramite.DatosAdicionalesCierre))
                        {
                            try
                            {
                                datosPdf = JsonConvert.DeserializeObject<Dictionary<string, string>>(tramite.DatosAdicionalesCierre);
                                //var documentoCmaposLLenos = _pdfFiller.FillPdf(Convert.FromBase64String(item.Base64), datosPdf);
                                var documentoCmaposLLenos = _pdfTronService.FillPdf(Convert.FromBase64String(item.Base64), datosPdf);
                                documentoBase64 = Convert.ToBase64String(documentoCmaposLLenos);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (tramite.TipoTramiteVirtualId == 6)
                        {
                            var res = await ObtenerFirmaYSelloNotario(body.UserName, body.NotariaId);
                            var notariaUsuario = res.notariaUsuarios;
                            var dselloNotaria = res.selloNotaria;

                            var documentoConSellos = _pdfFiller.StampAllPages(Convert.FromBase64String(documentoBase64), notariaUsuario.Notario.GrafoArchivo.Contenido, dselloNotaria.Contenido);
                            documentoBase64 = Convert.ToBase64String(documentoConSellos);
                        }

                        bool esFirmadoElectronico = false;
                        if (tramite.TipoTramiteVirtualId.Equals((int)TipoTramiteVirtual.Autenticacion))
                        {
                            esFirmadoElectronico = await _archivosPortalVirtualRepositorio.ObtenerTodo()
                                                            .Where(m => m.TramitesPortalVirtualId == tramite.TramitesPortalVirtualId && m.TipoArchivo == 0)
                                                            .AnyAsync();
                        }

                        var minutaFirmada = await FirmarDocumento(
                            userId,
                            body.NotariaId,
                            body.UserName,
                            documentoBase64,
                            tramite.CUANDI,
                            tramite.TipoTramiteVirtual.Nombre,
                            actosNotarialesPorTramite,
                            datosPdf != null ? datosPdf["matricula"] : "",
                            tramite.TipoTramiteVirtualId,
                            esFirmadoElectronico
                        );

                        item.Base64 = minutaFirmada;
                        item.TipoArchivo = 4;
                        _archivosPortalVirtualRepositorio.Modificar(item);
                    }
                }

                var responseSnr = await EnviarMinutasSNR(
                    body.NotariaId,
                    minutas,
                    tramite.TramitesPortalVirtualId,
                    tramite.TipoTramiteVirtualId,
                    tramite.CUANDI,
                    tramite.EstadoTramiteVirtualId,
                    tramite.TipoTramiteVirtual.Nombre,
                    tramite.ActoPrincipal?.Codigo
                );
                _logTramitePortalVirtualRepositorio.Agregar(new LogTramitePortalVirtual
                {
                    TramitePortalVirtualId = tramite.TramitesPortalVirtualId,
                    UsuarioCreacion = userId,
                    ClaveTestamentoCerrado = !string.IsNullOrEmpty(body.ClaveTestamento) ? Cifrador.CifradoVariable(body.ClaveTestamento, _configuration["EncryptAesKey"]) : null,
                    EnvioSNR = responseSnr.envioSnr,
                    //EnvioSNR = true,
                    Lat = body.Coordenadas.Lat,
                    Lng = body.Coordenadas.Lng,
                    EstadoTramiteVirtualId = tramite.EstadoTramiteVirtualId,
                    LogResponseSNR = responseSnr.log,
                    //LogResponseSNR = "Bien"
                });
            }

            else if (tramite.EstadoTramiteVirtualId == (int)EstadoTramiteVirtual.MinutaSubida)
            {
                NotificarPagoMiFirma(tramite.TramiteVirtualGuid);
            }

            _portalVirtualRepositorio.Modificar(tramite);
            await _portalVirtualRepositorio.UnidadDeTrabajo.CommitAsync();

            if (tramite.EstadoTramiteVirtualId == (int)EstadoTramiteVirtual.FirmadoNotarioAutorizado)
            {
                await NotificarFirmadoNotarioAutorizadoMiFirma(tramite.TramiteVirtualGuid, tramite.TramitesPortalVirtualId);
            }

            return await ConsultarTramiteVirtualPorId(new ConsultarTramiteDTO { TramiteId = tramiteId, NotariaId = tramite.NotariaId });
        }

        private async Task ActualizarTipoArchivoAutenticacion(TramitesPortalVirtual tramite, int estado)
        {
            if (tramite.TipoTramiteVirtualId.Equals((int)TipoTramiteVirtual.Autenticacion) && estado.Equals((int)EstadoTramiteVirtual.PendienteAutorizar))
            {
                var minutas = await _archivosPortalVirtualRepositorio
                        .Obtener(m => m.TramitesPortalVirtualId == tramite.TramitesPortalVirtualId && m.TipoArchivo == 0);
                if (minutas.Any())
                {
                    foreach (var item in minutas)
                    {
                        item.TipoArchivo = 3; //Minuta Abierta
                        _archivosPortalVirtualRepositorio.Modificar(item);
                    }
                }
            }
        }

        private async Task NotificarFirmadoNotarioAutorizadoMiFirma(string TramiteVirtualGuid, long TramitesPortalVirtualId)
        {
            Modelo.Rest.ResponseModel responseModel = new Modelo.Rest.ResponseModel();
            try
            {
                var files = new List<Modelo.Rest.FilesModel>();
                var minutaFirmadaAutorizada = (await _archivosPortalVirtualRepositorio
                        .Obtener(m => m.TramitesPortalVirtualId == TramitesPortalVirtualId && m.TipoArchivo == 4)).ToList();
                if (minutaFirmadaAutorizada.Count > 0)
                {
                    foreach (var item in minutaFirmadaAutorizada)
                    {
                        files.Add(new Modelo.Rest.FilesModel
                        {
                            Format = item.Formato,
                            Base64 = item.Base64,
                            Name = item.Nombre
                        });
                    }

                    string url = _configuration["NotificarFirmadoNotarioAutorizadoMiFirma"];
                    _miFirmaRestApiService = new PaymentManagementRestApiService(url);
                    var model = new Modelo.Rest.EnvioArchivoFirmadoNotarioAutorizadoModel
                    {
                        notaryProcedureID = TramiteVirtualGuid,
                        state = (int)EstadoTramiteVirtualMiFirma.FirmadoNotarioAutorizado,
                        Files = files
                    };

                    var res = await _miFirmaRestApiService.EnvioArchivoFirmadoNotarioAutorizadoMiFirma(model);
                    responseModel = JsonConvert.DeserializeObject<Modelo.Rest.ResponseModel>(res);
                }

            }
            catch (Exception e)
            {
                responseModel.error = true;
                responseModel.message = e.Message;
            }
            //return responseModel.error;
        }

        private async void NotificarPagoMiFirma(string TramiteVirtualGuid)
        {
            try
            {
                string url = _configuration["NotificarMiFirmaPagoRealizado"];
                _miFirmaRestApiService = new PaymentManagementRestApiService(url);
                var res = await _miFirmaRestApiService.UpdateNotaryProcedure(new Modelo.Rest.UpdateNotaryProcedureModel
                {
                    notaryProcedureID = TramiteVirtualGuid,
                    state = (int)EstadoTramiteVirtualMiFirma.Pagado
                });
                var responseModel = JsonConvert.DeserializeObject<Modelo.Rest.ResponseModel>(res);
            }
            catch (Exception e)
            {
            }
        }

        private string ValidarUbicacionNotario(int notariaId, CambiarEstadoTramiteVirtualModel body)
        {
            string respueta = string.Empty;
            var coordenadasNotaria = _convenioNotariaVirtualRepositorio.Obtener(x => x.NotariaId == notariaId).Result.FirstOrDefault();

            var DatosNotario = ObtenerDatosNotario(notariaId, body.UserId, body.UserName);

            if (coordenadasNotaria != null)
            {
                if (body.Coordenadas.Lat < coordenadasNotaria.Latitud1
                || body.Coordenadas.Lat > coordenadasNotaria.Latitud2
                    ||
                body.Coordenadas.Lng < coordenadasNotaria.Longitud1
                || body.Coordenadas.Lng > coordenadasNotaria.Longitud2
                )
                    respueta = $"{ObtenerGeneroNotario(DatosNotario?.Persona?.Genero)},  {CoordenadasInvalidas} {DatosNotario?.Notaria?.CirculoNotaria}";
            }
            else
                respueta = $"{ ObtenerGeneroNotario(DatosNotario?.Persona?.Genero)}, {ErrorObtenerCoordenadas}";

            return respueta;
        }

        private NotariaUsuarios ObtenerDatosNotario(int notariaId, string userId, string userName)
        {
            var notariaUsuario = _cache.GetFromCache($"Notario_Grafo:{userName}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                .Where(m => !m.IsDeleted
                && m.UserEmail == userName
                && m.NotariaId == notariaId
                && m.UsuariosId == userId)
                .Include(m => m.Persona)
                .Include(m => m.Notaria)
                .FirstOrDefaultAsync()).Result;
            return notariaUsuario;
        }

        private async Task AgregarActualizarActosNotariales(int tramiteId, int estadoTramiteVirtualId, List<ActoPorTramiteModel> actosNotariales)
        {
            if (estadoTramiteVirtualId == 1 && actosNotariales != null && actosNotariales.Count > 0)
            {
                short count = actosNotariales.Max(m => m.Orden);
                count += 1;

                foreach (var item in actosNotariales.Where(m => m.ActoPorTramiteId == 0).ToList())
                {
                    var r = _actoPorTramiteRepositorio.Agregar(new ActoPorTramite
                    {
                        TramitePortalVirtualId = tramiteId,
                        ActoNotarialId = item.ActoNotarialId,
                        Cuandi = "",
                        Orden = count
                    });

                    count += 1;
                    await _actoPorTramiteRepositorio.UnidadDeTrabajo.SaveChangesAsync();
                    item.ActoPorTramiteId = r.ActoPorTramiteId;
                }

                var actosNotarialesPorTramite = await _actoPorTramiteRepositorio.ObtenerTodo()
                    .Where(m => m.TramitePortalVirtualId == tramiteId)
                    .ToListAsync();

                if (actosNotarialesPorTramite.Any())
                {
                    foreach (var item in actosNotariales.Where(m => m.ActoPorTramiteId > 0).ToList())
                    {
                        var acto = actosNotarialesPorTramite.Find(m => m.ActoPorTramiteId == item.ActoPorTramiteId);

                        if (acto != null)
                        {
                            acto.ActoNotarialId = item.ActoNotarialId;
                            acto.Cuandi = item.Cuandi;
                            _actoPorTramiteRepositorio.Modificar(acto);
                        }
                    }

                    foreach (var item in actosNotarialesPorTramite)
                    {
                        if (!actosNotariales.Any(m => m.ActoPorTramiteId == item.ActoPorTramiteId))
                        {
                            _actoPorTramiteRepositorio.Eliminar(item, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Envía las minutas al SNR
        /// </summary>
        /// <returns></returns>
        private async Task<(string log, bool envioSnr)> EnviarMinutasSNR(
            long notariaId,
            IEnumerable<ArchivosPortalVirtual> minutas,
            int tramitesPortalVirtualId,
            int tipoTramiteVirtualId,
            string cuandi,
            int estadoTramiteVirtualId,
            string tipoTramiteVirtual,
            string codigoActo)
        {
            string logsnr = null;
            bool envioSnr = false;

            if (minutas.Any())
            {
                var convenioNotaria = await _convenioNotariaVirtualRepositorio.ObtenerTodo().Where(x => x.NotariaId == notariaId).FirstOrDefaultAsync();

                if (convenioNotaria == null) throw new Exception("Convenio Notaría no encontrado.");

                if (string.IsNullOrEmpty(convenioNotaria.IdNotariaSNR) || string.IsNullOrEmpty(convenioNotaria.AutorizacionAutenticacionSNR)
                    || string.IsNullOrEmpty(convenioNotaria.ApiUserSNR) || string.IsNullOrEmpty(convenioNotaria.ApiKeySNR)) throw new Exception("SNR Keys no encontradas.");

                dynamic responseLog = new ExpandoObject();

                var res = _digitalizacionNotarialServicio.ActoNotarialProtocolo(new DigitalizacionNotairal.Entidades.ActoNotarialProtocoloRequest
                {
                    IdNotaria = convenioNotaria.IdNotariaSNR,
                    AutorizacionAutenticacion = convenioNotaria.AutorizacionAutenticacionSNR,
                    TipoDeDocumento = 1,
                    Codigo_acto = int.Parse(ObtenerCodigoActo(tipoTramiteVirtualId)),
                    Fecha_acto = DateTime.Now.ToString("yyyy-MM-dd"),
                    Cuandi = cuandi,
                    Consecutivo = tramitesPortalVirtualId.ToString(),
                    MatriculaRelacionada = cuandi, //"50C-54654",
                    Factura_recibo = "454566876",
                    Palabras_claves = $"{tipoTramiteVirtual.ToUpper()}; {cuandi}; {tramitesPortalVirtualId}",//"ESCRITURA; 056546",
                    Datos_interesados = $"{cuandi}; {estadoTramiteVirtualId}; {DateTime.Now.ToString("yyyy-MM-dd")}"//"DAVIVIENDA; NIT344543545"
                });

                if (res.Cod_respuesta != 1) throw new Exception($"Error al enviar el documento a la SNR: {ObtenerRespuestaSNR(res.Cod_respuesta)}");

                responseLog.notariaProtcolo = res;
                responseLog.checkInResponses = new List<DigitalizacionNotairal.Entidades.CheckInResponse>();

                foreach (var item in minutas)
                {
                    var resCheckIn = _digitalizacionNotarialServicio.CheckIn(new DigitalizacionNotairal.Entidades.CheckInRequest
                    {
                        DocumentType = "NOTDIG-GENERICO",
                        CheckInType = "BASE64",
                        FileName = $"{(item.Nombre.Contains(".pdf") ? item.Nombre : $"{item.Nombre}.pdf")}",
                        FileContent = item.Base64,
                        Tags = new DigitalizacionNotairal.Entidades.Tag[]
                        {
                                new DigitalizacionNotairal.Entidades.Tag
                                {
                                    Label = "ID_REPOSITORIO",
                                    Value = res.Id_repositorio
                                }
                        },
                        ApiUser = convenioNotaria.ApiUserSNR,
                        ApiKey = convenioNotaria.ApiKeySNR
                    });

                    responseLog.checkInResponses.Add(resCheckIn);
                }

                logsnr = JsonConvert.SerializeObject(responseLog);
                envioSnr = true;
            }

            return (logsnr, envioSnr);
        }

        public async Task<TestamentoModel> ObtenerTestamento(long tramiteId, string claveTestamento)
        {
            try
            {
                var archivo = await _archivosPortalVirtualRepositorio.ObtenerTodo()
                 .Where(m => m.TramitesPortalVirtualId == tramiteId && m.TipoArchivo == 9)
                 .Select(m => new TestamentoModel
                 {
                     Base64 = Cifrador.DescifradoVariable(m.Base64, claveTestamento),
                     Formato = m.Formato
                 })
                 .FirstOrDefaultAsync();

                return archivo;
            }
            catch
            {
                throw new Exception("Clave incorrecta");
            }
        }

        public async Task<ValidaTramiteResponseDTO> ValidarTramitePersona(ValidarTramitePersonaDTO validarTramitePersona)
        {
            ValidaTramiteResponseDTO responseDTO = new ValidaTramiteResponseDTO();
            var Persona = (await _personasRepositorio.Obtener(
                x => x.TipoIdentificacionId == validarTramitePersona.TipoIdentificacionId
                && x.NumeroDocumento == validarTramitePersona.NumeroDocumento)).FirstOrDefault();

            if (Persona != null)
            {
                var comparecientes = (await _comparecienteRepositorio.Obtener(x => x.PersonaId == Persona.PersonaId)).ToList().OrderByDescending(t => t.TramiteId).ThenByDescending(t => t.FechaCreacion);
                Tramite tramite = null;
                foreach (var item in comparecientes)
                {
                    var ultimoTramite = await _tramiteRepositorio.GetOneAsync(x => x.TramiteId == item.TramiteId && x.NotariaId == validarTramitePersona.NotariaId);

                    if (ultimoTramite != null)
                    {
                        tramite = ultimoTramite;
                        break;
                    }
                }
                if (tramite != null)
                {
                    responseDTO.TramiteExiste = true;
                    responseDTO.FechaCreacionTramite = tramite.FechaCreacion;
                }
                else
                {
                    responseDTO.TramiteExiste = false;
                    responseDTO.FechaCreacionTramite = null;
                }
            }
            else
            {
                responseDTO.TramiteExiste = false;
                responseDTO.FechaCreacionTramite = null;
            }
            return responseDTO;
        }
        #endregion

        #region Metodos Privados
        private async Task EnviarCorreoNotificacionTramiteCliente(string returnUrl, string email, string nombreCliente)
        {
            var destinatarios = new List<string>();
            destinatarios.Add(email);
            string cuerpoHMTL = _templateServicio.ObtenerTemplateNotificacionTramiteCliente(returnUrl, nombreCliente);

            string htmldocument = cuerpoHMTL;
            var htmlView = AlternateView.CreateAlternateViewFromString(htmldocument, null, "text/html");
            await _manejadorCorreos.EnviarCorreo(destinatarios
                    , "Notificación Tramite"
                    , htmldocument, htmlView);
        }
        private string ObtenerRespuestaSNR(int cod_respuesta) => cod_respuesta switch
        {
            2 => "Error en estructura json",
            3 => "Autorizacion Autenticación no corresponde",
            4 => "Cuandi Repetido",
            5 => "No existe el id_repositorio",
            8 => "No se ha enviado información de prueba",
            9 => "No existe código del acto",
            10 => "Tipo de documento no existe",
            _ => "Respuesta no encontrada"
        };

        private string ObtenerCodigoActo(int tipoTramiteVirtualId) => tipoTramiteVirtualId switch
        {
            1 => "00000301",
            2 => "00000511",
            3 => "01250000",
            4 => "00000250",
            5 => "00000251",
            6 => "00000546",
            _ => ""
        };

        private string ObtenerGeneroNotario(int? GeneroId) => GeneroId switch
        {
            1 => "Señor Notario",
            2 => "Señora Notaria",
            _ => "Señor Notario"
        };

        private string ObtenerGeneroNotarioFirmaDocumento(int? GeneroId) => GeneroId switch
        {
            1 => "El Notario",
            2 => "La Notaria",
            _ => "El Notario"
        };

        private async Task<string> FirmarDocumento(
            string userId,
            long notariaId,
            string username,
            string documento,
            string CUANDI,
            string tipoTramite,
            List<Tuple<string, string>> actosNotariales,
            string matricula,
            int tipoTramiteId,
            bool esFirmadoElectronico)
        {
            var res = await ObtenerFirmaYSelloNotario(username, notariaId);
            var notariaUsuario = res.notariaUsuarios;
            var dselloNotaria = res.selloNotaria;

            var convenioNotaria = await _convenioNotariaVirtualRepositorio.ObtenerTodo().Where(x => x.NotariaId == notariaId).FirstOrDefaultAsync();

            if (convenioNotaria == null) throw new Exception("Convenio Notaría no encontrado.");

            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Administrador" && string.IsNullOrEmpty(convenioNotaria.SerialCertificado)) throw new Exception("Serial Certificado no encontrado.");

            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Notario Encargado" && string.IsNullOrEmpty(convenioNotaria.SerialCertificadoNotarioEncargado)) throw new Exception("Serial Certificado Notario encargado no encontrado.");

            var sb = new StringBuilder();
            foreach (var item in actosNotariales)
            {
                sb.AppendLine($"<tr><td>{item.Item1}</td><td>{item.Item2}</td></tr>");
            }

            object NumeroNotaria;
            if (notariaUsuario?.Notaria?.NumeroNotaria == 0)
                NumeroNotaria = "Única";
            else
                NumeroNotaria = notariaUsuario?.Notaria?.NumeroNotaria;

            string body = ObtenerCuerpoDocumentoAFirmar(sb, matricula, notariaUsuario, NumeroNotaria, tipoTramiteId, esFirmadoElectronico);

            var datosFirma = new DatosFirma()
            {
                Cargo = $"{notariaUsuario.Cargo} de la Notaría {NumeroNotaria} del Círculo de {notariaUsuario.Notaria.CirculoNotaria}",
                NombreCompleto = $"{notariaUsuario.Persona.Nombres} {notariaUsuario.Persona.Apellidos}",
                SerialCertificado = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value == "Administrador" ?
                    convenioNotaria.SerialCertificado :
                    convenioNotaria.SerialCertificadoNotarioEncargado,
                TextoFirma = body,
                Titulo = $"DILIGENCIA DE TIPO {tipoTramite.ToUpper()}",
                UrlQR = $"Notaría {NumeroNotaria} Círculo de {notariaUsuario.Notaria.CirculoNotaria} - {CUANDI}",
                Grafo = notariaUsuario.Notario.GrafoArchivo.Contenido,
                FormatoGrafo = notariaUsuario.Notario.GrafoArchivo.Extension,
                Sello = dselloNotaria.Contenido,
                FormatoSello = dselloNotaria.Extension
            };

            var documentoFirmado = _firmaEnPaginaAdicional.FirmarEnNuevaPagina(Convert.FromBase64String(documento), datosFirma);

            if (documentoFirmado.Length == 0) throw new Exception("No se pudo firmar el documento.");

            return Convert.ToBase64String(documentoFirmado);
        }

        private async Task<(NotariaUsuarios notariaUsuarios, Dominio.ContextoPrincipal.Entidad.Parametricas.Archivos.SelloNotaria selloNotaria)>
            ObtenerFirmaYSelloNotario(string username, long notariaId)
        {
            var notariaUsuario = await _cache.GetFromCache($"Notario_Grafo_con_grafo:{username}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                .Where(m => !m.IsDeleted && m.UserEmail == username && m.NotariaId == notariaId)
                .Include(m => m.Notario.GrafoArchivo)
                .Include(m => m.Persona)
                .Include(m => m.Notaria)
                .FirstOrDefaultAsync());

            if (notariaUsuario?.Notario?.GrafoArchivo == null) throw new Exception("El notario no tiene un grafo asignado");

            var dselloNotaria = await _cache.GetFromCache($"Sello:{notariaId}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                .Where(m => !m.IsDeleted && m.NotariaId == notariaId)
                .Select(nu => nu.Notaria.SelloArchivo)
                .FirstOrDefaultAsync());

            if (dselloNotaria == null) throw new Exception("La notaría no tiene sello asignado");

            return (notariaUsuario, dselloNotaria);
        }

        private string ObtenerCuerpoDocumentoAFirmar(StringBuilder sb, string matricula, NotariaUsuarios notariaUsuario, object numeroNotaria, int tipoTramiteId, bool esFirmadoElectronico)
        {
            string body;
            if (tipoTramiteId == 3) // compra venta
            {
                // Matricula
                body = $@"Escritura pública ({matricula}) 
{NumberHelper.NumeroALetras(double.Parse(matricula))} del 
{DateTime.Now.ToString("dd \\de MMMM \\de yyyy", new CultureInfo("es-CO"))}, 
otorgada en la Notaria {numeroNotaria} 
del Circulo de {notariaUsuario?.Notaria?.CirculoNotaria}
<br><br>
<table>
        <thead>
            <th>Acto Notarial</th>
            <th>CUANDI</th>
        </thead>
        <tbody>
{sb}
        </tbody>
</table>";
            }
            else
            {
                string textoFirma;
                if (tipoTramiteId == (int)TipoTramiteVirtual.Autenticacion)
                    textoFirma = esFirmadoElectronico ? " y firmaron de manera electrónica el documento que precede" : "";
                else
                    textoFirma = " y firmaron de manera electrónica el documento que precede";

                body = $@"{ObtenerGeneroNotarioFirmaDocumento(notariaUsuario?.Persona?.Genero)} certifica que el día {DateTime.Now.ToString("dd \\de MMMM \\de yyyy"
                    , new CultureInfo("es-CO"))}
, mediante diligencia virtual, comparecieron los firmantes{textoFirma}.
</br></br>
<table>
        <thead>
            <th>Acto Notarial</th>
            <th>CUANDI</th>
        </thead>
        <tbody>
{sb}
        </tbody>
</table>";
            }
            return body;
        }

        private async Task<bool> ValidarRequestRegistroCiudadano(TramitePortalVirtualCiudadanoDTO tramitePortalVirtual)
        {
            bool resultado = false;

            var esGuidValido = Guid.TryParse(tramitePortalVirtual.TramiteVirtualGuid, out Guid guid);
            if (!esGuidValido) throw new Exception($"ValidarRequestRegistroCiudadano: esGuidValido");

            var esNotariaValida = (await _convenioNotariaVirtualRepositorio.Obtener(x => x.NotariaId == tramitePortalVirtual.NotariaId && !x.IsDeleted)).Any();
            if (!esNotariaValida) throw new Exception($"ValidarRequestRegistroCiudadano: esNotariaValida");

            var esTipoTramiteValido = (await _tipoTramiteVirtualRepositorio.Obtener(x => x.TipoTramiteID == tramitePortalVirtual.TipoTramiteVirtualId)).Any();
            if (!esTipoTramiteValido) throw new Exception($"ValidarRequestRegistroCiudadano: esTipoTramiteValido");

            var esEstadoTramiteValido = (await _estadoTramiteVirtualRepositorio.Obtener(x => x.EstadoTramiteID == tramitePortalVirtual.EstadoTramiteVirtualId)).Any();
            if (!esEstadoTramiteValido) throw new Exception($"ValidarRequestRegistroCiudadano: esEstadoTramiteValido");

            var esTramiteUnico = (await _portalVirtualRepositorio.Obtener(x => x.TramiteVirtualGuid == tramitePortalVirtual.TramiteVirtualGuid && x.NotariaId == tramitePortalVirtual.NotariaId)).Any();
            if (esTramiteUnico) throw new Exception($"ValidarRequestRegistroCiudadano: esTramiteUnico");

            var esActoTramiteValido = ValidarRequestActoTramite(tramitePortalVirtual.ActosTramite);
            if (!esActoTramiteValido) throw new Exception($"ValidarRequestRegistroCiudadano: esActoTramiteValido");

            bool esMimeTypeValido = ValidarMimeTypeArchivo(tramitePortalVirtual.Archivos);
            if (!esMimeTypeValido) throw new Exception($"ValidarRequestRegistroCiudadano: esMimeTypeValido");

            return !resultado;
        }

        private bool ValidarMimeTypeArchivo(List<ArchivosDTO> archivos)
        {
            bool resultado = false;
            string mimeTypesValidos = _configuration["MimeTypeArchivoPortalVirtual"];
            string[] mimeTypes = { };
            if (!string.IsNullOrEmpty(mimeTypesValidos))
            {
                mimeTypes = mimeTypesValidos.Split('|');
                if (archivos != null && archivos.Count > 0)
                {
                    foreach (var item in archivos)
                    {
                        bool esMimeTypeValido = mimeTypes.Where(x => x == item.Formato).Any();
                        if (!esMimeTypeValido)
                            return resultado;
                    }
                    return !resultado;
                }
            }

            return resultado;
        }

        private bool ValidarRequestActoTramite(List<ActoTramite> actosTramite)
        {
            bool resultado = true;
            if (actosTramite != null
                && actosTramite.Count > 0)
            {
                var esOrdenRepetido = actosTramite.Select(x => x.Orden).Distinct().ToArray();
                if (esOrdenRepetido.Length != actosTramite.Count)
                    return !resultado;
                var listaActos = _actoNotarialServicio.ObtenerTodosActosNotariales().Result;

                foreach (var item in actosTramite)
                {
                    if (item.ActoNotarialId <= 0)
                        return !resultado;
                    if (string.IsNullOrWhiteSpace(item.CUANDI))
                        return !resultado;
                    else
                    {
                        var esUnico = _actoPorTramiteRepositorio.Obtener(x => x.Cuandi == item.CUANDI).Result.Any();
                        if (esUnico)
                            return !resultado;
                    }

                    var esActoValido = listaActos.Where(a => a.ActoNotarialId == item.ActoNotarialId).Any();
                    if (!esActoValido)
                        return !resultado;
                }
            }

            return resultado;
        }

        private int InsertarArchivos(int TramitesPortalVirtualId, List<ArchivosDTO> archivos)
        {
            var ArchivosPortalVirtual = new List<ArchivosPortalVirtual>();
            foreach (var item in archivos)
            {
                ArchivosPortalVirtual.Add(new ArchivosPortalVirtual
                {
                    Nombre = item.Nombre,
                    Base64 = item.Base64,
                    TipoArchivo = (short)item.TipoArchivo,
                    Formato = item.Formato,
                    FechaCreacion = DateTime.Now,
                    FechaModificacion = DateTime.Now,
                    TramitesPortalVirtualId = TramitesPortalVirtualId
                });
            }
            _archivosPortalVirtualRepositorio.Agregar(ArchivosPortalVirtual);
            var insercionArchivos = _archivosPortalVirtualRepositorio.UnidadDeTrabajo.Commit();
            return insercionArchivos;
        }

        private async Task<TramitesPortalVirtual> ObtenerTramiteVirtual(int NotariaId, string TramiteVirtualGuid, string CUANDI)
        {
            return (await _portalVirtualRepositorio.Obtener(x => x.NotariaId == NotariaId && x.TramiteVirtualGuid == TramiteVirtualGuid && x.CUANDI == CUANDI)).FirstOrDefault();
        }

        private async Task<bool> ActualizarArchivosYTramiteVirtual(ActualizarTramiteVirtualDTO actualizarTramite)
        {
            bool respuesta = false;

            int TramitesPortalVirtualId = await ActualizarEstadoTramiteVirtual(actualizarTramite);

            if (TramitesPortalVirtualId > 0)
            {
                bool resultadoActualizacionArchivos = await EliminarEInsertarArchivos(actualizarTramite, TramitesPortalVirtualId);
                if (resultadoActualizacionArchivos)
                    respuesta = resultadoActualizacionArchivos;
            }
            return respuesta;
        }

        private async Task<bool> EliminarEInsertarArchivos(ActualizarTramiteVirtualDTO actualizarTramite, int TramitesPortalVirtualId)
        {
            bool resultado = false;
            var listaArchivos = (await _archivosPortalVirtualRepositorio.Obtener(x => x.TramitesPortalVirtualId == TramitesPortalVirtualId)).ToList();
            if (listaArchivos.Count > 0)
            {
                int resultadoArchivosEliminados = 0;
                foreach (var item in listaArchivos)
                {
                    _archivosPortalVirtualRepositorio.Eliminar(item, fisico: true);
                    resultadoArchivosEliminados = await _archivosPortalVirtualRepositorio.UnidadDeTrabajo.CommitAsync();
                    resultadoArchivosEliminados++;
                }
                if (resultadoArchivosEliminados > 0)
                {
                    var ListaArchivos = actualizarTramite.Archivos.Adaptar<ArchivosDTO>().ToList();
                    int resultadoInsercionArchivos = InsertarArchivos(TramitesPortalVirtualId, ListaArchivos);
                    if (resultadoInsercionArchivos > 0)
                        resultado = true;
                }
            }
            return resultado;
        }

        private async Task<bool> InsertarArchivosNuevos(ActualizarTramiteVirtualDTO actualizarTramite)
        {
            bool resultado = false;
            var tramiteVirtualBD = await ObtenerTramiteVirtual(actualizarTramite.NotariaId, actualizarTramite.TramiteVirtualGuid, actualizarTramite.CUANDI);
            if (tramiteVirtualBD != null)
            {
                var ListaArchivos = actualizarTramite.Archivos.Adaptar<ArchivosDTO>().ToList();
                int resultadoInsercionArchivos = InsertarArchivos(tramiteVirtualBD.TramitesPortalVirtualId, ListaArchivos);
                if (resultadoInsercionArchivos > 0)
                    resultado = true;
            }
            return resultado;
        }

        private async Task<ActualizarTramiteVirtualResponseDTO> ValidarPeticionActualizacionTramite(ActualizarTramiteVirtualDTO actualizarTramite)
        {
            ActualizarTramiteVirtualResponseDTO responseDTO = new ActualizarTramiteVirtualResponseDTO();
            if (actualizarTramite != null)
            {
                var tramiteVirtual = await ObtenerTramiteVirtual(actualizarTramite.NotariaId, actualizarTramite.TramiteVirtualGuid, actualizarTramite.CUANDI);
                if (tramiteVirtual != null)
                {
                    var esNotariaValida = (await _convenioNotariaVirtualRepositorio.Obtener(x => x.NotariaId == actualizarTramite.NotariaId && !x.IsDeleted)).Any();
                    if (esNotariaValida)
                    {
                        var esEstadoTramiteValido = (await _estadoTramiteVirtualRepositorio.Obtener(x => x.EstadoTramiteID == actualizarTramite.EstadoTramiteVirtualId)).Any();
                        if (!esEstadoTramiteValido)
                            responseDTO.MensajeError = estadoIncorrecta;
                    }
                    else
                        responseDTO.MensajeError = notariaIncorrecta;
                }
                else
                    responseDTO.MensajeError = tramiteNoValido;
            }
            else
                responseDTO.MensajeError = objetoNullActualizacion;
            return responseDTO;
        }

        private async Task<int> ActualizarEstadoTramiteVirtual(ActualizarTramiteVirtualDTO actualizarTramite)
        {
            int respuesta = 0;

            var tramiteVirtualOld = ObtenerTramiteVirtual(actualizarTramite.NotariaId, actualizarTramite.TramiteVirtualGuid, actualizarTramite.CUANDI).Result;
            if (tramiteVirtualOld != null)
            {
                tramiteVirtualOld.EstadoTramiteVirtualId = actualizarTramite.EstadoTramiteVirtualId;
                if (!string.IsNullOrEmpty(actualizarTramite.DatosAdicional))
                    tramiteVirtualOld.DatosAdicionales = actualizarTramite.DatosAdicional;

                _portalVirtualRepositorio.Modificar(tramiteVirtualOld);
                int resultadoActualizacion = await _portalVirtualRepositorio.UnidadDeTrabajo.CommitAsync();
                if (resultadoActualizacion > 0)
                {
                    respuesta = tramiteVirtualOld.TramitesPortalVirtualId;
                }
            }
            return respuesta;
        }

        private int InsertarActosPorTramite(List<ActoTramite> actosTramite, int id)
        {
            List<ActoPorTramite> ActosPorTramite = new List<ActoPorTramite>();
            foreach (var item in actosTramite)
            {
                ActosPorTramite.Add(new ActoPorTramite
                {
                    TramitePortalVirtualId = id,
                    ActoNotarialId = item.ActoNotarialId,
                    Cuandi = item.CUANDI,
                    Orden = (short)item.Orden
                });
            }
            _actoPorTramiteRepositorio.Agregar(ActosPorTramite.OrderBy(x => x.Orden));
            return _actoPorTramiteRepositorio.UnidadDeTrabajo.Commit();
        }

        private string GetStatusName(eEstadoRecaudo estado) => estado switch
        {
            eEstadoRecaudo.Generado => "GENERADO",
            eEstadoRecaudo.Anulado => "ANULADO",
            eEstadoRecaudo.Enviado => "ENVIADO",
            eEstadoRecaudo.Pagado => "PAGADO",
            eEstadoRecaudo.Rechazado => "RECHAZADO",
            _ => ""
        };

        enum TipoTramiteVirtual
        {
            Matrimonio = 1,
            Autenticacion = 2,
            Compraventa = 3,
            TestamentoAbierto = 4,
            TestamentoCerrado = 5,
            DocumentoPrivado = 6
        }

        enum EstadoTramiteVirtual
        {
            Creado = 1,
            RevisadoCliente = 2,
            PendientePorPagar = 3,
            Pagado = 5,
            MinutaSubida = 7,
            PendienteAutorizar = 9,
            FirmadoNotarioAutorizado = 11,
            Rechazado = 15,
            RevisarCliente = 17
        }

        enum EstadoTramiteVirtualMiFirma
        {
            Pagado = 102,
            FirmadoNotarioAutorizado = 105
        }
        #endregion

    }
}
