using PortalAdministrador.Services.Biometria.Models;
using PortalAdministrador.Services.Biometria.Models.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.Biometria
{
    public class RNECMovilService : IRNECService
    {
        private readonly HttpClient client;

        public RNECMovilService(HttpClient client)
        {
            this.client = client;
            Console.WriteLine("URL RECONOSER ACTUAL:" + client.BaseAddress.AbsoluteUri);
        }

        private async Task<int> Captura(Dedo dedo, short captura)
        {
            var request = new CapturaMovilRequest()
            {
                
                NumeroDedo = (int) dedo,
                TipoDedo = (int) captura,
                SerialDispositivo = "1530I173349"
            };

            var httpResponse = await client.PostAsJsonAsync("SolicitudOlimpia", request, 
                new System.Text.Json.JsonSerializerOptions()
                {
                    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
                });

            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<CapturaMovilResponse>();
                if (response.DescripcionError == null)
                {
                    return 170;
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

            try
            {
                var httpResponse = await client.PostAsJsonAsync("Ingreso", request);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var response = await httpResponse.Content.ReadFromJsonAsync<ValidarIdentidadMovilResponse>();


                    if (response.DescripcionError == "0")
                    {

                        var res = new ValidacionResponse()
                        {
                            Documento = request.NumeroDocumento,
                            // FechaExpedicion = response.FechaExpedicion,
                            NutValidacion = response.IdPeticion.ToString(),
                            Validado = response.Resultado == "HIT"

                        };

                        if (response.Resultado == "HIT")
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
                            Hit = b.Resultado == "HIT",
                            Detalle = b.Error,
                            Score = b.Score
                        }).ToArray();

                        return res;


                    }
                    else
                    {
                        throw new ApplicationException(response.DescripcionError);
                    }
                }
                else
                {
                    throw new HttpRequestException(httpResponse.StatusCode.ToString());
                }
            }
            catch
            {
                return null;
            }           
        }

        public Task<ConsultarEstadoResponse> ConsultarEstado()
        {
            return Task.FromResult(new ConsultarEstadoResponse()
            {
                Estado = "OK",
                Version = "0.0"
            });
        }
    }
}
