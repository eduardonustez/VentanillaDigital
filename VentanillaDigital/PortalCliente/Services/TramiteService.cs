using PortalCliente.Data;
using System;
using System.Threading.Tasks;
using Blazored.SessionStorage;
using ApiGateway.Models.Transaccional;
using ApiGateway.Models;
using System.Collections.Generic;
using ApiGateway.Contratos.Models.Archivos;
using Infraestructura.Transversal.Models;
using System.Net.Http;
using PortalCliente.Services.Biometria.Models;
using ApiGateway.Contratos.Models;
using Newtonsoft.Json;
using Infraestructura.Transversal.Log.Modelo;
using System.Threading;
using Microsoft.AspNetCore.Components.Authorization;
using System.Diagnostics;

namespace PortalCliente.Services
{
    public class TramiteService : ITramiteService
    {
        private readonly ICustomHttpClient _customHttpClient;
        protected ITrazabilidadService _trazabilidadService;
        protected AuthenticationStateProvider _authenticationStateProvider;

        public TramiteService(ISessionStorageService sessionStorageService, ICustomHttpClient customHttpClient, ITrazabilidadService trazabilidadService, AuthenticationStateProvider authenticationStateProvider)
        {
            _customHttpClient = customHttpClient;
            _trazabilidadService = trazabilidadService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<TramitePendienteAutorizacionModel> ObtenerPendientesAutorizacion(DefinicionFiltro definicionFiltro, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var resultado = await _customHttpClient.PostJsonAsync<TramitePendienteAutorizacionModel>("/api/Notario/ObtenerPendientesAutorizacion", definicionFiltro, cancellationToken);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff")
            };
            await AgregarLog("ObtenerPendientesAutorizacion", "DefinicionFiltro", informacionTraza);
            return resultado;
        }

        public async Task<TramitePendienteAutorizacionModel> ObtenerTramitesEnProceso(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var resultado = await _customHttpClient.PostJsonAsync<TramitePendienteAutorizacionModel>("/api/Notario/ObtenerTramitesEnProcesoPaginado", filtroTramites, cancellationToken);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff")
            };
            await AgregarLog("ObtenerTramitesEnProcesoPaginado", "DefinicionFiltro", informacionTraza);
            return resultado;
        }

        public async Task<TramitePendienteAutorizacionModel> ObtenerTramitesAutorizados(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            TramitePendienteAutorizacionModel resultado = new TramitePendienteAutorizacionModel();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                resultado = await _customHttpClient.PostJsonAsync<TramitePendienteAutorizacionModel>("/api/Notario/ObtenerTramitesAutorizadoPaginado", filtroTramites, cancellationToken);

            }
            catch (Exception ex)
            {
                string ms = ex.Message;
            }
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff")
            };
            await AgregarLog("ObtenerTramitesAutorizadoPaginado", "DefinicionFiltro", informacionTraza);
            return resultado;
        }


        public async Task<TramitePendienteAutorizacionModel> ObtenerTramitesPendientes(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            TramitePendienteAutorizacionModel resultado = new TramitePendienteAutorizacionModel();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                resultado = await _customHttpClient.PostJsonAsync<TramitePendienteAutorizacionModel>("/api/Notario/ObtenerTramitesPendientesAutPaginado", filtroTramites, cancellationToken);

            }
            catch (Exception ex)
            {
                string ms = ex.Message;
            }
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff")
            };
            await AgregarLog("ObtenerTramitesPendientesAutPaginado", "DefinicionFiltro", informacionTraza);
            return resultado;
        }


        public async Task<TramitePendienteAutorizacionModel> ObtenerTramitesRechazados(FiltroTramites filtroTramites, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            TramitePendienteAutorizacionModel resultado = new TramitePendienteAutorizacionModel();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                resultado = await _customHttpClient.PostJsonAsync<TramitePendienteAutorizacionModel>("/api/Notario/ObtenerTramitesRechazadosPaginado", filtroTramites, cancellationToken);

            }
            catch (Exception ex)
            {
                string ms = ex.Message;
            }
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff")
            };
            await AgregarLog("ObtenerTramitesRechazadosPaginado", "DefinicionFiltro", informacionTraza);
            return resultado;
        }



        public async Task<Data.Tramite> ObtenerTramite(long tramiteId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var resul = await _customHttpClient.GetJsonAsync<TramiteReturn>($"/api/Tramite/ObtenerTramite/{tramiteId}");
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                IdTramite = tramiteId
            };
            await AgregarLog("ObtenerTramite", "tramiteId", informacionTraza);
            return new Data.Tramite()
            {
                TramiteId = resul.TramiteId,
                CantidadComparecientes = resul.CantidadComparecientes,
                ComparecienteActualPos = resul.ComparecienteActual,
                ComparecientesCompletos = resul.ComparecientesCompletos,
                UsarSticker = resul.UsarSticker,
                FueraDeDespacho = resul.FueraDeDespacho,
                DireccionComparecencia = resul.DireccionComparecencia,
                DatosAdicionales = resul.DatosAdicionales,
                TipoTramite = new TipoTramite()
                {
                    TipoTramiteId = resul.TipoTramite.TipoTramiteId,
                    Nombre = resul.TipoTramite.Nombre,
                    Descripcion = resul.TipoTramite.Descripcion,
                    CodigoTramite = resul.TipoTramite.CodigoTramite,
                    ProductoReconoserId = resul.TipoTramite.ProductoReconoserId
                },
                Comparecientes = ExtraerComparecientes(resul.Comparecientes),
            };
        }

        private List<Compareciente> ExtraerComparecientes(List<ComparecienteReturnDTO> comparecienteReturnDTOs)
        {
            var resul = new List<Compareciente>();
            foreach (var c in comparecienteReturnDTOs)
            {
                var objCompareciente = new Compareciente()
                {
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    NumeroIdentificacion = c.NumeroIdentificacion,
                    Foto = c.Foto,
                    Firma = c.Firma,
                    FechaCreacion = c.FechaCreacion,
                    MotivoSinBiometria = c.MotivoSinBiometria,
                    TramiteSinBiometria = c.TramiteSinBiometria,
                    TipoIdentificacion = new TipoIdentificacion()
                    {
                        TipoIdentificacionId = c.TipoIdentificacion.TipoIdentificacionId,
                        Nombre = c.TipoIdentificacion.Nombre,
                        Abreviatura = c.TipoIdentificacion.Abreviatura
                    },
                    ComparecienteActualPos = c.Posicion,
                };
                resul.Add(objCompareciente);
            }
            return resul;
        }
        public async Task<long> CrearTramite(Data.Tramite tramite)
        {
            try
            {
                if (tramite.TramiteId == 0)
                {
                    TramiteModel nuevoTramite = new TramiteModel()
                    {
                        CantidadComparecientes = tramite.CantidadComparecientes,
                        TipoTramiteId = tramite.TipoTramite.TipoTramiteId,
                        DatosAdicionales = tramite.DatosAdicionales,
                        UsarSticker = tramite.UsarSticker,
                        FueraDeDespacho = tramite.FueraDeDespacho,
                        DireccionComparecencia = tramite.DireccionComparecencia
                    };
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var resulNuevoTramite = await _customHttpClient.PostJsonAsync<TramiteReturn>("Tramite/CrearTramite", nuevoTramite);
                    stopwatch.Stop();
                    TimeSpan timeSpan = stopwatch.Elapsed;
                    InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                    {
                        Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                        IdTramite = resulNuevoTramite.TramiteId
                    };
                    await AgregarLog("CrearTramite", "TramiteModel", informacionTraza);
                    return resulNuevoTramite.TramiteId;
                }
                return tramite.TramiteId;
            }
            catch (Exception ex)
            {
                InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                {
                    DatosAdicionalesTraza = ex.InnerException?.Message ?? ex.Message
                };
                await AgregarLog("CrearTramite", "CrearTramiteException", informacionTraza);
                return -1;
            }
        }

        private async Task AgregarLog(string metodo, string entidad, InformacionTrazaModel informacionAdicional)
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            InformationModel objTraza = new InformationModel("1",
               "Portal Cliente",
               "TramiteService",
               metodo,
               entidad,
               null,
               JsonConvert.SerializeObject(informacionAdicional), state.User.Identity.Name);
            await _trazabilidadService.CrearTraza(objTraza);
        }

        public async Task<bool> CrearCompareciente(long tramiteId, Compareciente comparecienteToCreate)
        {
            long resul = 0;
            try
            {
                Foto foto = new Foto()
                {
                    Nombre = string.Concat("Foto_", comparecienteToCreate.NumeroIdentificacion),
                    Imagen = comparecienteToCreate.Foto,
                    Extension = ".jpg",
                    Tamano = 0
                };
                Foto firma = new Foto()
                {
                    Nombre = string.Concat("Firma_", comparecienteToCreate.NumeroIdentificacion),
                    Imagen = comparecienteToCreate.Firma,
                    Extension = ".jpg",
                    Tamano = 0
                };
                Foto ImagenDocumento = new Foto()
                {
                    Nombre = string.Concat("Documento_", comparecienteToCreate.NumeroIdentificacion),
                    Imagen = comparecienteToCreate.Documento,
                    Extension = ".jpg",
                    Tamano = 0
                };

                ComparecienteCreateRequest comparecienteCreateRequest = new ComparecienteCreateRequest()
                {
                    TramiteId = tramiteId,
                    TipoDocumentoId = comparecienteToCreate.TipoIdentificacion.TipoIdentificacionId,
                    NumeroDocumento = comparecienteToCreate.NumeroIdentificacion,
                    Foto = foto,
                    Firma = firma,
                    ImagenDocumento = ImagenDocumento,
                    PeticionRNEC = comparecienteToCreate.TransaccionRNECId,
                    Nombres = comparecienteToCreate.Nombres,
                    Apellidos = comparecienteToCreate.Apellidos,
                    NumeroCelular = comparecienteToCreate.NumeroCelular,
                    Email = comparecienteToCreate.Email,
                    //Excepciones = comparecienteToCreate.Excepciones.Adaptar<ExcepcionHuellaModel>(),
                    Excepciones = comparecienteToCreate.Excepciones != null ? ConvertExcepcionHuellaToExcepcionHuellaModel(comparecienteToCreate.Excepciones) : null,
                    TramiteSinBiometria = comparecienteToCreate.TramiteSinBiometria,
                    MotivoSinBiometria = comparecienteToCreate.MotivoSinBiometria,
                    ComparecienteActualPos = comparecienteToCreate.ComparecienteActualPos,
                    HitDedo1 = comparecienteToCreate.DedoUnoResultado == null ? (bool?)null : comparecienteToCreate.DedoUnoResultado == "HIT",
                    HitDedo2 = comparecienteToCreate.DedoDosResultado == null ? (bool?)null : comparecienteToCreate.DedoDosResultado == "HIT",
                    Vigencia = comparecienteToCreate.ResulHuellasDetalle,
                    NombreDigitado = comparecienteToCreate.NombreCompletoDigitado
                };
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var resultado = await _customHttpClient.PostJsonAsync<TramiteReturn>("Compareciente/CrearCompareciente", comparecienteCreateRequest);
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                {
                    Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                    IdTramite = tramiteId
                };
                await AgregarLog("CrearCompareciente", "ComparecienteCreateRequest", informacionTraza);
                return true;
            }
            catch
            {
                return false;
            }

        }

        private ExcepcionHuellaModel ConvertExcepcionHuellaToExcepcionHuellaModel(ExcepcionHuella excepcion)
        {
            ExcepcionHuellaModel excepcionHuellaModel = new ExcepcionHuellaModel();
            excepcionHuellaModel.Descripcion = excepcion.Descripcion;
            excepcionHuellaModel.Dedos = new EnumDedos[excepcion.Dedos.Length];
            List<EnumDedos> auxDedos = new List<EnumDedos>();
            foreach (var excepcion_dedo in excepcion.Dedos)
            {
                switch (excepcion_dedo)
                {
                    case Dedo.PulgarDerecho:
                        auxDedos.Add(EnumDedos.PulgarDerecho);
                        break;
                    case Dedo.IndiceDerecho:
                        auxDedos.Add(EnumDedos.IndiceDerecho);
                        break;
                    case Dedo.MedioDerecho:
                        auxDedos.Add(EnumDedos.CorazonDerecho);
                        break;
                    case Dedo.AnularDerecho:
                        auxDedos.Add(EnumDedos.AnularDerecho);
                        break;
                    case Dedo.MeniqueDerecho:
                        auxDedos.Add(EnumDedos.MeñiqueDerecho);
                        break;
                    case Dedo.PulgarIzquierdo:
                        auxDedos.Add(EnumDedos.PulgarIzquierdo);
                        break;
                    case Dedo.IndiceIzquierdo:
                        auxDedos.Add(EnumDedos.IndiceIzquierdo);
                        break;
                    case Dedo.MedioIzquierdo:
                        auxDedos.Add(EnumDedos.CorazonIzquierdo);
                        break;
                    case Dedo.AnularIzquierdo:
                        auxDedos.Add(EnumDedos.AnularIzquierdo);
                        break;
                    case Dedo.MeniqueIzquierdo:
                        auxDedos.Add(EnumDedos.MeñiqueIzquierdo);
                        break;
                }
            }
            excepcionHuellaModel.Dedos = auxDedos.ToArray();
            return excepcionHuellaModel;
        }

        public async Task<bool> ActualizarTramite(Data.Tramite tramite)
        {
            try
            {
                if (tramite.TramiteId != 0)
                {
                    TramiteModel nuevoTramite = new TramiteModel()
                    {
                        TramiteId = tramite.TramiteId,
                        CantidadComparecientes = tramite.CantidadComparecientes,
                        TipoTramiteId = tramite.TipoTramite.TipoTramiteId,
                        DatosAdicionales = tramite.DatosAdicionales
                    };
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var resulNuevoTramite = await _customHttpClient.SendJsonAsync(HttpMethod.Post, "Tramite/ActualizarTramite", nuevoTramite);
                    stopwatch.Stop();
                    TimeSpan timeSpan = stopwatch.Elapsed;
                    InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                    {
                        Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                        IdTramite = tramite.TramiteId
                    };
                    await AgregarLog("ActualizarTramite", "TramiteModel", informacionTraza);
                    return resulNuevoTramite.StatusCode == System.Net.HttpStatusCode.NoContent;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

}
