using Infraestructura.Transversal.Log.Enumeracion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Cors;
using PortalCliente.Services.Biometria.Models;
using PortalCliente.Services.Biometria.Models.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria
{
    public class RNECService : IRNECService
    {
        private readonly HttpClient client;
        protected readonly ITrazabilidadService _trazabilidadService;
        protected readonly AuthenticationStateProvider _authenticationStateProvider;

        public RNECService(HttpClient client, ITrazabilidadService trazabilidadService, AuthenticationStateProvider authenticationStateProvider)
        {
            this.client = client;
            this._trazabilidadService = trazabilidadService;
            this._authenticationStateProvider = authenticationStateProvider;
        }

        private async Task<int> Captura(Dedo dedo, short captura)
        {
            var request = new CapturarHuellaRequest()
            {
                Cedula = "123456789", // NO IMPORTA
                Dedo = (short)dedo,
                Captura = captura,
                Ticks = DateTime.Now.Ticks
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var httpResponse = await client.PostAsJsonAsync("CapturarHuella", request);
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                {
                    Tiempo = timeSpan.ToString(@"m\:ss\.fff")
                };
                Task.Run(() => AgregarLog("Captura", "CapturarHuellaRequest", informacionTraza));
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<CapturarHuellaResponse>();
                    if (response.Respuesta.ToUpper() == "OK")
                    {
                        return response.Calidad;
                    }
                    else
                    {
                        Task.Run(() => CrearExcepcion("Captura", JsonSerializer.Serialize(request), new ArgumentException(response.Detalle)));
                        return 0;
                    }
                }
                else
                {
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
                }
            }catch(Exception ex)
            {
                Task.Run(() => CrearExcepcion("Captura", JsonSerializer.Serialize(request), new ArgumentException("Servicio Reconoser no responde")));
                return 0; 
            }
        }

        public Task<int> Captura1(Dedo dedo)
        {
            return Captura(dedo, 1);
        }

        public Task<int> Captura2(Dedo dedo)
        {
            return Captura(dedo, 2);
        }

        public async Task<string> ObtenerATDP(Guid? convenioId = null)
        {
            var request = new ObtenerFormatoAutorizacionRequest()
            {
                Cedula = "123456789", // NO IMPORTA
                IdConvenio = convenioId,
                Ticks = DateTime.Now.Ticks
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var httpResponse = await client.PostAsJsonAsync("ObtenerFormatoAutorizacion", request);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"ConvenioId: {convenioId}"
            };
            Task.Run(() => AgregarLog("ObtenerATDP", "ObtenerFormatoAutorizacionRequest", informacionTraza));
            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<ObtenerFormatoAutorizacionResponse>();
                if (response.Respuesta.ToUpper() == "OK")
                {
                    return response.TextoFormato;
                }
                else
                {
                    throw new ApplicationException(response.Detalle);
                }
            }
            else
            {
                throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        public async Task<ConsultarEstadoResponse> ConsultarEstado()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var httpResponse = await client.GetAsync("ConsultarEstado");
                stopwatch.Stop();
                TimeSpan timeSpan = stopwatch.Elapsed;
                InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
                {
                    Tiempo = timeSpan.ToString(@"m\:ss\.fff")
                };
                Task.Run(() => AgregarLog("ConsultarEstado", "", informacionTraza));
                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<ConsultarEstadoResponse>();
                    return response;
                }
                else
                {
                    return new ConsultarEstadoResponse() { Estado = "Bad Request", Version = "0", Propiedades = null };
                }
            }
            catch
            {
                return new ConsultarEstadoResponse() { Estado = "Bad Request", Version = "0", Propiedades = null };
            }

        }


        public async Task<ValidacionResponse> ValidarIdentidad(ValidacionRequest validacionRequest)
        {
            var request = new ValidarIdentidadRequest()
            {
                Cedula = validacionRequest.Ciudadano,
                Asesor = validacionRequest.Asesor,
                IdConvenio = validacionRequest.ConvenioId,
                Oficina = validacionRequest.Oficina,
                Producto = validacionRequest.ProductoId,
                Ticks = DateTime.Now.Ticks,
                Grafo = validacionRequest.Grafo
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var httpResponse = await client.PostAsJsonAsync("ValidarIdentidad", request);
            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;
            InformacionTrazaModel informacionTraza = new InformacionTrazaModel()
            {
                Tiempo = timeSpan.ToString(@"m\:ss\.fff"),
                DatosAdicionalesTraza = $"Cedula: {request.Cedula}, Oficina: {request.Oficina}"
            };
            Task.Run(() => AgregarLog("ValidarIdentidad", "ValidadIdentidadRequest", informacionTraza));
            /*string urlText = $"ValidarIdentidad/{v.Ciudadano}/{v.Asesor}/{v.Oficina}/{v.ProductoId}/{DateTime.Now.Ticks}/";

            var httpResponse = await client.GetAsync(urlText);*/

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<ValidarIdentidadResponse>();
                if (response.Respuesta.ToUpper() == "OK")
                {
                    var ret = new ValidacionResponse()
                    {
                        Documento = response.Documento,
                        // FechaExpedicion = response.FechaExpedicion,
                        NutValidacion = response.IdPeticion.ToString(),
                        Validado = response.Huellas?.Any(h=>h.Score >=32)??false
                    };
                    if(ret.Validado)
                    {
                        ret.PrimerApellido = response.PrimerApellido;
                        ret.SegundoApellido = response.SegundoApellido;
                        ret.PrimerNombre = response.PrimerNombre;
                        ret.SegundoNombre = response.SegundoNombre;
                        ret.Vigencia = ObtenerEstadoRNEC(response.Vigencia);
                    }
                    if (response.Huellas != null)
                    {
                        ret.Huellas = response.Huellas.Select(
                            h => new Huella()
                            {
                                Dedo = (Dedo)h.NumeroDedo,
                                Detalle = h.Error,
                                Hit = h.Score >= 32,
                                Score = h.Score
                            }).ToArray();
                    }

                    return ret;
                }
                else
                {
                    Task.Run(() => CrearExcepcion("ValidarIdentidad", JsonSerializer.Serialize(request), new ArgumentException(response.Detalle)));
                    throw new ApplicationException(response.Detalle);
                }
            }
            else
            {
                throw new HttpRequestException(httpResponse.StatusCode.ToString());
            }
        }

        private string ObtenerEstadoRNEC(string codVigencia)
        {
            int vigencia;
            string res;
            if (int.TryParse(codVigencia, out vigencia))
            {
                switch (vigencia)
                {
                    case 0:
                        res = "VIGENTE"; break;
                    case 12:
                        res = "Baja por Pérdida o Suspensión de los Derechos Políticos"; break;
                    case 14:
                        res = "Baja por Interdicción Judicial por Demencia"; break;
                    case 21:
                        res = "Cancelada por Muerte"; break;
                    case 22:
                        res = "Cancelada por Doble Cedulación"; break;
                    case 23:
                        res = "Cancelada por Suplantación o Falsa Identidad"; break;
                    case 24:
                        res = "Cancelada por Menoría de Edad"; break;
                    case 25:
                        res = "Cancelada por Extranjería"; break;
                    case 26:
                        res = "Cancelada por Mala Elaboración"; break;
                    case 27:
                        res = "Cancelada por Reasignación o cambio de sexo"; break;
                    case 51:
                        res = "Cancelada por Muerte Facultad Ley 1365 2009"; break;
                    case 52:
                        res = "Cancelada por Intento de Doble Cedulación NO Expedida"; break;
                    case 53:
                        res = "Cancelada por Falsa Identidad o Suplantación NO Expedida"; break;
                    case 54:
                        res = "Cancelada por Menoría de Edad NO Expedida"; break;
                    case 55:
                        res = "Cancelada por Extranjería NO Expedida"; break;
                    case 56:
                        res = "Cancelada por Mala Elaboración No Expedida"; break;
                    case 88:
                        res = "PENDIENTE POR ESTAR EN PROCESO"; break;
                    case 99:
                        res = "EN PROCESO DE ELABORACION"; break;
                    default:
                        res = "N/A"; break;
                }

                return res;
            }
            else
                return codVigencia;
        }

        public Task ReiniciarCaptor()
        {
            return Task.CompletedTask;
        }

        private async Task AgregarLog(string metodo, string entidad, InformacionTrazaModel informacionAdicional)
        {
            try{
                var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
                InformationModel objTraza = new InformationModel("1",
                "Portal Cliente",
                "RNECService",
                metodo,
                entidad,
                null,
                JsonSerializer.Serialize(informacionAdicional), state.User.Identity.Name);
                await _trazabilidadService.CrearTraza(objTraza);
            }catch{}
        }
        private async Task CrearExcepcion(string metodo, string entidad,Exception exception)
        {
            try
            {
                var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
                ErrorModel objExc = new ErrorModel(Guid.NewGuid().ToString(),exception,
                PoliticaCapaEnum.PortalCliente,
                "RNECService",
                metodo,
                entidad,
                state.User.Identity.Name);
                await _trazabilidadService.CrearExcepcion(objExc);
            }
            catch { }
        }
    }
}
