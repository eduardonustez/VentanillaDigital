using Newtonsoft.Json;
using RestSharp;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Entidades;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Extensiones;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios
{
    public class DigitalizacionNotarialServicio : IDigitalizacionNotarialServicio
    {
        public DigitalizacionNotarialServicio(Microsoft.Extensions.Configuration.IConfigurationSection configuration)
        {
            this._url = configuration["url"];
            this._configuration = configuration;
        }

        public ActoNotarialProtocoloResponse ActoNotarialProtocolo(ActoNotarialProtocoloRequest request)
        {
            try
            {
                return this.ExecuteClient<ActoNotarialProtocoloResponse>
                (
                    this._configuration.GetSection("endpoints")["actoProtocolo"],
                    request.ToSerialize(),
                    Method.POST,
                    null
                );
            }
            catch (Exception exception)
            {
                return new ActoNotarialProtocoloResponse
                {
                    Cod_respuesta = 0,
                    Id_repositorio = exception.Message
                };
            }
        }

        public ServiceInfoResponse ServiceInfo()
        {
            try
            {
                return this.ExecuteClient<ServiceInfoResponse>
                (
                    this._configuration.GetSection("endpoints")["serviceInfo"],
                    null,
                    Method.GET,
                    new Dictionary<string, string>()
                    {
                        { "api-user", this._configuration["apiUser"] },
                        { "api-key", this._configuration["apiKey"] }
                    }
                );
            }
            catch (Exception exception)
            {
                return new ServiceInfoResponse
                {
                    ValidationStatus = exception.Message
                };
            }
        }

        public CheckInResponse CheckIn(CheckInRequest request)
        {
            try
            {
                return this.ExecuteClient<CheckInResponse>
                (
                    this._configuration.GetSection("endpoints")["checkin"],
                    request.ToSerialize(),
                    Method.POST,
                    new Dictionary<string, string>()
                    {
                        { "api-user", request.ApiUser },
                        { "api-key", request.ApiKey }
                    }
                );
            }
            catch(Exception exception)
            {
                return new CheckInResponse
                {
                    CheckInStatus = exception.Message
                };
            }
        }

        private T ExecuteClient<T>(string endPoint, string jsonRquest = null,
            Method method = Method.GET, Dictionary<string, string> headers = null)
        {
            if(string.IsNullOrEmpty(this._url))
            {
                return default;
            }

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            RestClient restClient = new RestClient(this._url);

            var restRequest = new RestRequest(endPoint, method);
            restClient.ReadWriteTimeout = 120000;
            restClient.Timeout = 120000;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Accept", "*/*");
            restRequest.AddHeader("Connection", "keep-alive");
            restRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");

            if(headers != null)
            {
                headers.ToList().ForEach(h =>
                {
                    restRequest.AddHeader(h.Key, h.Value);
                });
            }

            if (!string.IsNullOrEmpty(jsonRquest))
            {
                //restRequest.AddParameter("application/json", jsonRquest, ParameterType.RequestBody);
                restRequest.AddJsonBody(jsonRquest);
            }

            IRestResponse restResponse = restClient.Execute(restRequest);

            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonConvert.DeserializeObject<T>(restResponse.Content);

                case HttpStatusCode.BadRequest:
                case HttpStatusCode.NotFound:
                    return JsonConvert.DeserializeObject<T>(restResponse.Content);
                default:
                    throw new ArgumentException($"StatusCode:{restResponse.StatusCode} | Content:{restResponse.Content}");

            }
        }

        private string _url;
        private readonly Microsoft.Extensions.Configuration.IConfigurationSection _configuration;
    }
}