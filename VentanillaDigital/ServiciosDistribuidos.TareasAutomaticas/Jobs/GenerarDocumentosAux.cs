using Aplicacion.TareasAutomaticas.Contrato.Transaccional;
using Aplicacion.TareasAutomaticas.Enums;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.Entidad.Transaccional;
using GenericExtensions;
using Infraestructura.KeyManager;
using Infraestructura.KeyManager.Models;
using Infraestructura.Transversal.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TSAIntegracion;
using TSAIntegracion.Entities;

namespace ServiciosDistribuidos.TareasAutomaticas.Jobs
{
    public class GenerarDocumentosAux : CronJobService
    {

        private readonly ILogger<GenerarDocumentosAux> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ImplementedCache _cache;
        private readonly IKeyManagerClient _keyManagerClient;
        private const string CACHE_NOTARIA_USUARIO = "NOTARIA_USUARIO_{0}";

        public GenerarDocumentosAux(IScheduleConfig<GenerarDocumentosAux> config,
            ILogger<GenerarDocumentosAux> logger,
            ImplementedCache cache,
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory,
            IKeyManagerClient keyManagerClient)
            : base(config.CronExpression, config.TimeZoneInfo, logger)
        {
            _logger = logger;
            _cache = cache;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            _keyManagerClient = keyManagerClient;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(GenerarDocumentos.JOBIniciado, "GenerarDocumentosAux starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(30000);

                _logger.LogInformation(GenerarDocumentos.JOBIniciaProcesoActas, $"GenerarDocumentosAux is working.");
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (bool.Parse(_configuration["EjecutarParalelo"])) await GenerarDocumentoParaleloAsync();
                    else await GenerarDocumentoAsync();
                }

                //return Task.CompletedTask;
            }
            catch { }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(GenerarDocumentos.JOBDeteniendo, "GenerarDocumentosAux is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private async Task GenerarDocumentoParaleloAsync()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var documentoAutorizadoRepositorio = scope.ServiceProvider.GetService<IDocumentoPendienteAutorizarRepositorio>();
                    int cantidad = int.Parse(_configuration["CantidadDocumentoGenerar"]);
                    int tiempoExpiraCacheFirmaNotario;
                    int.TryParse(_configuration["ExpiraCacheFirmaNotarioEnMinutos"], out tiempoExpiraCacheFirmaNotario);
                    tiempoExpiraCacheFirmaNotario = tiempoExpiraCacheFirmaNotario == 0 ? 60 : tiempoExpiraCacheFirmaNotario;

                    _logger.LogInformation(GenerarDocumentos.JOBMarcandoActasPendientes, $"GenerarDocumentosAux - GenerarDocumentoParaleloAsync Marcando Actas Pendientes");
                    var documentosPendientes = await documentoAutorizadoRepositorio.ObtenerProximasSpAsync(cantidad);

                    if (documentosPendientes != null && documentosPendientes.Any())
                    {
                        _logger.LogInformation(GenerarDocumentos.JOBActasEncontradas, $"GenerarDocumentosAux - GenerarDocumentoParaleloAsync Actas Encontradas: {documentosPendientes.Count}");
                        var cantidadHilos = int.Parse(_configuration["CantidadMaximoHilos"]);

                        Parallel.ForEach(documentosPendientes, new ParallelOptions { MaxDegreeOfParallelism = cantidadHilos }, async documentoPendienteAutorizarId =>
                        {
                            var stopWatch = new Stopwatch();
                            stopWatch.Restart();

                            using (var scopeParallel = _serviceScopeFactory.CreateScope())
                            {
                                var _documentoAutorizadoRepositorio = scopeParallel.ServiceProvider.GetService<IDocumentoPendienteAutorizarRepositorio>();
                                var _actaNotarialService = scopeParallel.ServiceProvider.GetService<IGenerarActaServicio>();
                                var _notariasUsuarioRepositorio = scopeParallel.ServiceProvider.GetService<INotariasUsuarioRepositorio>();
                                var _tramiteRepositorio = scopeParallel.ServiceProvider.GetService<ITramiteRepositorio>();
                                var _tsaConfig = scopeParallel.ServiceProvider.GetService<ITSAConfig>();

                                DocumentoPendienteAutorizar item = null;
                                try
                                {
                                    item = await _documentoAutorizadoRepositorio.GetOneAsync(m => m.DocumentoPendienteAutorizarId == documentoPendienteAutorizarId);
                                    //_logger.LogInformation($"GenerarDocumentosAux - GenerarDocumentoParaleloAsync Recorriendo item: {item.TramiteId} en {DateTime.Now:hh:mm:ss}");

                                    var timeScope = stopWatch.ElapsedMilliseconds;
                                    stopWatch.Restart();

                                    var notariaUsuario = await _cache
                                        .GetFromCache(string.Format(CACHE_NOTARIA_USUARIO, item.NotarioUsuarioId),
                                        () => _notariasUsuarioRepositorio
                                            .ObtenerTodo()
                                            .Where(nu => nu.NotariaUsuariosId == item.NotarioUsuarioId)
                                            .Select(m => new
                                            {
                                                UserEmail = m.UserEmail,
                                                NotariaId = m.NotariaId,
                                                Pin = m.Notario.Pin,
                                                CertificadoId = m.Notario.Certificadoid ?? 0
                                                //GrafoArchivo = m.Notario.GrafoArchivo.ToDataUrl(),
                                                //SelloArchivo = m.Notaria.SelloArchivo.ToDataUrl()
                                            })
                                            .FirstOrDefaultAsync(), tiempoExpiraCacheFirmaNotario
                                        );

                                    var dgrafoNotario = await _cache.GetFromCache($"Grafo:{notariaUsuario.UserEmail}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                                        .Where(m => !m.IsDeleted && m.UserEmail == notariaUsuario.UserEmail)
                                        .Select(nu => nu.Notario.GrafoArchivo)
                                        .FirstOrDefaultAsync(), tiempoExpiraCacheFirmaNotario);

                                    if (dgrafoNotario == null) throw new Exception("El notario no tiene un grafo asignado");

                                    var dselloNotaria = await _cache.GetFromCache($"Sello:{item.NotariaId}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                                        .Where(m => !m.IsDeleted && m.NotariaId == item.NotariaId)
                                        .Select(nu => nu.Notaria.SelloArchivo)
                                        .FirstOrDefaultAsync());

                                    if (dselloNotaria == null) throw new Exception("La notaría no tiene sello asignado");

                                    var grafoNotario = dgrafoNotario.ToDataUrl();
                                    var selloNotaria = dselloNotaria.ToDataUrl();

                                    var timeNotariaUsuario = stopWatch.ElapsedMilliseconds;
                                    stopWatch.Restart();

                                    if (string.IsNullOrEmpty(grafoNotario))
                                    {
                                        item.Estado = EstadoDocumento.ERROR;
                                        item.Error = "Notario no encontrado";
                                        _documentoAutorizadoRepositorio.Modificar(item);
                                    }
                                    else
                                    {
                                        var archivo = await _actaNotarialService.GenerarArchivoPrevioAsync(item.TramiteId, item.UsarSticker, notariaUsuario.UserEmail, grafoNotario, selloNotaria);
                                        var timeArchivo = stopWatch.ElapsedMilliseconds;
                                        stopWatch.Restart();

                                        if (archivo.archivo != null)
                                        {
                                            string archivoConEstampa = null;
                                            try
                                            {
                                                if (notariaUsuario.CertificadoId > 0)
                                                {
                                                    int myPin = 0;
                                                    int.TryParse(notariaUsuario.Pin, out myPin);
                                                    string signResul = await _keyManagerClient.SignDocument(new SignDocumentRequest()
                                                    {
                                                        certificateId = notariaUsuario.CertificadoId,
                                                        fileEncoded = archivo.archivo,
                                                        pin = myPin
                                                    });
                                                    archivoConEstampa = signResul ?? TSA.AgregarEstampaCronologicaString(archivo.archivo, _tsaConfig, "Firma");
                                                }
                                                else
                                                {
                                                    archivoConEstampa = TSA.AgregarEstampaCronologicaString(archivo.archivo, _tsaConfig, "Firma");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _logger.LogError(GenerarDocumentos.JOBErrorEstampa, $"GenerarDocumentosAux - GenerarDocumentoParaleloAsync {DateTime.Now:hh:mm:ss} ErrorServicioEstampa. Error:{ex.ToString()} ");
                                            }
                                            var timeEstampa = stopWatch.ElapsedMilliseconds;
                                            stopWatch.Restart();

                                            var tramite = await _tramiteRepositorio.GetOneAsync(m => m.TramiteId == item.TramiteId);
                                            tramite.EstadoTramiteId = (int)EnumEstadoTramites.Autorizado;
                                            tramite.ActaNotarial = new MetadataArchivo()
                                            {
                                                Nombre = $"Acta Notarial Tramite {tramite.TramiteId}",
                                                Extension = ".pdf",
                                                Tamanio = 0,
                                                Ruta = "",
                                                Archivo = new Archivo()
                                                {
                                                    Contenido = archivoConEstampa ?? archivo.archivo
                                                }
                                            };

                                            var timeTramite = stopWatch.ElapsedMilliseconds;
                                            stopWatch.Restart();
                                            _tramiteRepositorio.Modificar(tramite);
                                            item.Generado = true;
                                            item.FechaGeneracion = DateTime.Now;
                                            item.Estado = EstadoDocumento.GENERADO;
                                            var timeGuardarActaNotarial = stopWatch.ElapsedMilliseconds;
                                            stopWatch.Stop();

                                            if (!string.IsNullOrEmpty(archivo.tiempos))
                                            {
                                                dynamic times = JsonConvert.DeserializeObject(archivo.tiempos);
                                                times.tScope = timeScope;
                                                times.tNotariaUsuario = timeNotariaUsuario;
                                                times.tArchivo = timeArchivo;
                                                times.tEstampa = timeEstampa;
                                                times.tTramite = timeTramite;
                                                times.tgActaNotarial = timeGuardarActaNotarial;
                                                item.Seguimiento = JsonConvert.SerializeObject(times);
                                            }

                                            _documentoAutorizadoRepositorio.Modificar(item);
                                        }
                                    }
                                }
                                catch (Exception ex) when (item != null)
                                {
                                    _logger.LogError(GenerarDocumentos.JOBErrorLogicaGeneracionActa, $"GenerarDocumentos - GenerarDocumentoParaleloAsync Error logica generación actas: {ex.ToString()}");
                                    item.Estado = EstadoDocumento.ERROR;
                                    item.Error = ex.ToDetailedString();
                                    _documentoAutorizadoRepositorio.Modificar(item);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(GenerarDocumentos.JOBErrorLogicaGeneracionActa, $"GenerarDocumentos - GenerarDocumentoParaleloAsync Error logica generación actas: {ex.ToString()}");
                                }

                                try
                                {
                                    await _documentoAutorizadoRepositorio.UnidadDeTrabajo.CommitAsync();
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(GenerarDocumentos.JOBErrorAlGuardarActa, $"GenerarDocumentosAux - GenerarDocumentoParaleloAsync Generación de acta: Error al guardar los cambios. {ex.ToString()}");
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(GenerarDocumentos.JOBErrorGeneralGenerarDocumento, $"GenerarDocumentosAux - GenerarDocumentoParaleloAsync Catch General. {ex.ToString()}");
            }
        }

        private async Task GenerarDocumentoAsync()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var _documentoAutorizadoRepositorio = scope.ServiceProvider.GetService<IDocumentoPendienteAutorizarRepositorio>();
                    var _actaNotarialService = scope.ServiceProvider.GetService<IGenerarActaServicio>();
                    var _notariasUsuarioRepositorio = scope.ServiceProvider.GetService<INotariasUsuarioRepositorio>();
                    var _tramiteRepositorio = scope.ServiceProvider.GetService<ITramiteRepositorio>();
                    var _tsaConfig = scope.ServiceProvider.GetService<ITSAConfig>();
                    int cantidad = int.Parse(_configuration["CantidadDocumentoGenerar"]);
                    int tiempoExpiraCacheFirmaNotario;
                    int.TryParse(_configuration["ExpiraCacheFirmaNotarioEnMinutos"], out tiempoExpiraCacheFirmaNotario);
                    tiempoExpiraCacheFirmaNotario = tiempoExpiraCacheFirmaNotario == 0 ? 60 : tiempoExpiraCacheFirmaNotario;

                    _logger.LogInformation(GenerarDocumentos.JOBMarcandoActasPendientes, $"GenerarDocumentosAux - GenerarDocumentoAsync Marcando Actas Pendientes");
                    var documentosPendientes = await _documentoAutorizadoRepositorio.ObtenerProximasSpAsync(cantidad);

                    if (documentosPendientes != null && documentosPendientes.Any())
                    {
                        _logger.LogInformation(GenerarDocumentos.JOBActasEncontradas, $"GenerarDocumentosAux - GenerarDocumentoAsync Actas Encontradas: {documentosPendientes.Count}");

                        foreach (var DocumentoPendienteAutorizarId in documentosPendientes)
                        {
                            var stopWatch = new Stopwatch();
                            stopWatch.Restart();

                            var item = await _documentoAutorizadoRepositorio.GetOneAsync(m => m.DocumentoPendienteAutorizarId == DocumentoPendienteAutorizarId);

                            try
                            {
                                var timeDocumentoPendiente = stopWatch.ElapsedMilliseconds;
                                stopWatch.Restart();

                                //var notariaUsuario = await _cache
                                //    .GetFromCache(string.Format(CACHE_NOTARIA_USUARIO, item.NotarioUsuarioId),
                                //    () => _notariasUsuarioRepositorio
                                //        .GetOneAsync(nu => nu.NotariaUsuariosId == item.NotarioUsuarioId,
                                //        nu => nu.Notario.GrafoArchivo,
                                //        nu => nu.Notaria.SelloArchivo));

                                var notariaUsuario = await _cache
                                    .GetFromCache(string.Format(CACHE_NOTARIA_USUARIO, item.NotarioUsuarioId),
                                    () => _notariasUsuarioRepositorio
                                        .ObtenerTodo()
                                        .Where(nu => nu.NotariaUsuariosId == item.NotarioUsuarioId)
                                        .Select(m => new
                                        {
                                            UserEmail = m.UserEmail,
                                            NotariaId = m.NotariaId,
                                            //GrafoArchivo = m.Notario.GrafoArchivo.ToDataUrl(),
                                            //SelloArchivo = m.Notaria.SelloArchivo.ToDataUrl()
                                        })
                                        .FirstOrDefaultAsync(), tiempoExpiraCacheFirmaNotario
                                    );

                                var dgrafoNotario = await _cache.GetFromCache($"Grafo:{notariaUsuario.UserEmail}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                                        .Where(m => !m.IsDeleted && m.UserEmail == notariaUsuario.UserEmail)
                                        .Select(nu => nu.Notario.GrafoArchivo)
                                        .FirstOrDefaultAsync(), tiempoExpiraCacheFirmaNotario);

                                if (dgrafoNotario == null) throw new Exception("El notario no tiene un grafo asignado");

                                var dselloNotaria = await _cache.GetFromCache($"Sello:{item.NotariaId}", () => _notariasUsuarioRepositorio.ObtenerTodo()
                                    .Where(m => !m.IsDeleted && m.NotariaId == item.NotariaId)
                                    .Select(nu => nu.Notaria.SelloArchivo)
                                    .FirstOrDefaultAsync());

                                if (dselloNotaria == null) throw new Exception("La notaría no tiene sello asignado");

                                var grafoNotario = dgrafoNotario.ToDataUrl();
                                var selloNotaria = dselloNotaria.ToDataUrl();

                                var timeNotariaUsuario = stopWatch.ElapsedMilliseconds;
                                stopWatch.Restart();

                                if (string.IsNullOrEmpty(grafoNotario))
                                {
                                    item.Estado = EstadoDocumento.ERROR;
                                    item.Error = "Notario no encontrado";
                                    _documentoAutorizadoRepositorio.Modificar(item);
                                }
                                else
                                {
                                    var stopWatchArchivo = new Stopwatch();
                                    stopWatchArchivo.Start();
                                    var archivo = await _actaNotarialService.GenerarArchivoPrevioAsync(item.TramiteId, item.UsarSticker, notariaUsuario.UserEmail, grafoNotario, selloNotaria);

                                    var timeArchivo = stopWatch.ElapsedMilliseconds;
                                    stopWatch.Restart();

                                    if (archivo.archivo != null)
                                    {
                                        string archivoConEstampa = null;
                                        try
                                        {
                                            archivoConEstampa = TSA.AgregarEstampaCronologicaString(archivo.archivo, _tsaConfig, "Firma");
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger.LogError(GenerarDocumentos.JOBErrorEstampa, $"GenerarDocumentosAux - GenerarDocumentoAsync {DateTime.Now:hh:mm:ss} ErrorServicioEstampa. Error:{ex.ToString()} ");
                                        }
                                        var timeEstampa = stopWatch.ElapsedMilliseconds;
                                        stopWatch.Restart();

                                        var tramite = await _tramiteRepositorio.GetOneAsync(m => m.TramiteId == item.TramiteId);
                                        tramite.EstadoTramiteId = (int)EnumEstadoTramites.Autorizado;
                                        tramite.ActaNotarial = new MetadataArchivo()
                                        {
                                            Nombre = $"Acta Notarial Tramite {tramite.TramiteId}",
                                            Extension = ".pdf",
                                            Tamanio = 0,
                                            Ruta = "",
                                            Archivo = new Archivo()
                                            {
                                                Contenido = archivoConEstampa ?? archivo.archivo
                                            }
                                        };

                                        var timeTramite = stopWatch.ElapsedMilliseconds;
                                        stopWatch.Restart();
                                        _tramiteRepositorio.Modificar(tramite);
                                        item.Generado = true;
                                        item.FechaGeneracion = DateTime.Now;
                                        item.Estado = EstadoDocumento.GENERADO;
                                        var timeGuardarActaNotarial = stopWatchArchivo.ElapsedMilliseconds;
                                        stopWatchArchivo.Stop();

                                        if (!string.IsNullOrEmpty(archivo.tiempos))
                                        {
                                            dynamic times = JsonConvert.DeserializeObject(archivo.tiempos);
                                            times.tConsulta = timeDocumentoPendiente;
                                            times.tNotariaUsuario = timeNotariaUsuario;
                                            times.tArchivo = timeArchivo;
                                            times.tEstampa = timeEstampa;
                                            times.tTramite = timeTramite;
                                            times.tgActaNotarial = timeGuardarActaNotarial;
                                            item.Seguimiento = JsonConvert.SerializeObject(times);
                                        }

                                        _documentoAutorizadoRepositorio.Modificar(item);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(GenerarDocumentos.JOBErrorLogicaGeneracionActa, $"GenerarDocumentoAsync - GenerarDocumentoAsync Error logica generación actas: {ex.ToString()}");
                                item.Estado = EstadoDocumento.ERROR;
                                item.Error = ex.ToDetailedString();
                                _documentoAutorizadoRepositorio.Modificar(item);
                            }

                            try
                            {
                                await _documentoAutorizadoRepositorio.UnidadDeTrabajo.CommitAsync();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(GenerarDocumentos.JOBErrorLogicaGeneracionActa, $"GenerarDocumentosAux - GenerarDocumentoAsync Generación de acta: Error al guardar los cambios. {ex.ToString()}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(GenerarDocumentos.JOBErrorGeneralGenerarDocumento, $"GenerarDocumentosAux - GenerarDocumentoAsync Error Obteniendo Documento Autorizar. Error:{ex.ToString()} ");
            }
        }
    }
}
