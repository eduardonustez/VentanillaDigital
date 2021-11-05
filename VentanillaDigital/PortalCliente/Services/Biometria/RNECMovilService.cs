using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Biometria.Models;
using PortalCliente.Services.Biometria.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalCliente.Services.Biometria
{
    public class RNECMovilService : IRNECService
    {
        private readonly HttpClient _client;
        private readonly IJSRuntime _jsRuntime;

        public RNECMovilService(HttpClient client, IJSRuntime jsRuntime)
        {
            _client = client;
            _jsRuntime = jsRuntime;
        }

        private async Task<int> Captura(Dedo dedo, short captura)
        {
            var request = new CapturaMovilRequest()
            {
                
                NumeroDedo = (int) dedo,
                TipoDedo = (int) captura,
                SerialDispositivo = "1530I173349"
            };

            var httpResponse = await _client.PostAsJsonAsync("SolicitudOlimpia", request, 
                new System.Text.Json.JsonSerializerOptions()
                {
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                });

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<CapturaMovilResponse>();
                if (response.DescripcionError == null)
                {
                    return response.CalidadHuella;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new HttpRequestException(httpResponse.StatusCode.ToString());
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

        

        public async Task<ValidacionResponse> ValidarIdentidad(ValidacionRequest validacionRequest)
        {
            var request = new ValidarIdentidadMovilRequest()
            {
                
                Convenio = validacionRequest.ConvenioId.ToString(),
                IdOficina = validacionRequest.Oficina,
                IdProducto = validacionRequest.ProductoId,
                IdCliente = validacionRequest.ClienteId,         
                Login = validacionRequest.Asesor,
                NumeroDocumento = validacionRequest.Ciudadano
                      
                 
            };

            var httpResponse = await _client.PostAsJsonAsync("Ingreso", request);

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<ValidarIdentidadMovilResponse>();

                if (response.DescripcionError == "0" && response.CodigoError == null)
                {

                    var res = new ValidacionResponse()
                    {
                        Documento = request.NumeroDocumento,
                        // FechaExpedicion = response.FechaExpedicion,
                        NutValidacion = response.IdPeticion.ToString(),
                        Validado = response.Biometrias?.Any(b => b.Score >= 32) ?? false
                    };

                    if (res.Validado)
                    {
                        res.PrimerNombre = response.Candidato.PrimerNombre;
                        res.SegundoNombre = response.Candidato.SegundoNombre;
                        res.PrimerApellido = response.Candidato.PrimerApellido;
                        res.SegundoApellido = response.Candidato.SegundoApellido;
                        res.Vigencia = response.Candidato.Vigencia?.ToUpper();
                    }
                    res.Huellas = response.Biometrias?.Select(b => new Huella()
                    {
                        Dedo = (Dedo)b.IdSubtipo,
                        Hit = b.Score >= 32,
                        Detalle = b.Error,
                        Score = b.Score
                    }).ToArray();

                    return res;


                }
                else if(response.DescripcionError != "0")
                {
                    throw new ApplicationException(response.DescripcionError);
                }
                else
                {
                    throw new ApplicationException($"Se presentó un error durante la validación con RNEC. Verifique el número de documento y si el error persiste comuníquese con soporte al cliente.\nCódigo de error: {response.CodigoError}");
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
                var httpResponse = await _client.GetAsync("EstadoAgente");

                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseMovil = await httpResponse.Content.ReadFromJsonAsync<ConsultarEstadoMovilResponse>();

                    var response = new ConsultarEstadoResponse()
                    {
                        Estado = "OK",
                        Version = responseMovil.Version,
                        Propiedades = new PropiedadConsulta[]
                        {
                            new PropiedadConsulta()
                            {
                                Key = "IPLocal",
                                Value = responseMovil.Ip
                            },
                            new PropiedadConsulta()
                            {
                                Key = "MacAddress",
                                Value = responseMovil.Imei
                            },
                            new PropiedadConsulta()
                            {
                                Key = "CaptorDetectado",
                                Value = responseMovil.Captor.ToString()
                            }
                        }
                    };

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

        public async Task ReiniciarCaptor()
        {
            _jsRuntime.InvokeAsync<object>("openFrame","olimpiait://apagar");
            try
            {
                await _client.GetAsync("release");
            }
            catch { }
            finally
            {
                await Task.Delay(2000);
                _jsRuntime.InvokeAsync<object>("openFrame", "olimpiait://reconoser");
            }

        }
    }
}
