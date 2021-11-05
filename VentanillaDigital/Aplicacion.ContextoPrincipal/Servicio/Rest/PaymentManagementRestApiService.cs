using Aplicacion.ContextoPrincipal.Contrato.Rest;
using Aplicacion.ContextoPrincipal.Modelo.Rest;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Servicio.Rest
{
    public class PaymentManagementRestApiService : RestClient, IPaymentManagementRestApiService
    {
        #region Constructor
        public PaymentManagementRestApiService(string url)
            : base(url)
        {
            this.Timeout = 120000;
            this.ReadWriteTimeout = 120000;
            ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;
        }

        #endregion

        #region Contratos
        public async Task<string> CreatePaymentLink(CreatePaymentLinkModel model)
        {
            var request = new RestRequest("createPaymentLink");
            request.AddJsonBody(model);
            var response = await ExecutePostAsync(request);

            if (!response.IsSuccessful)
                Throw(response);
            return response.Content;
        }

        public async Task<string> CreateMultiplePayments(CreateMultiplePaymentsModel model)
        {
            var request = new RestRequest("createMultiplePayments");
            request.AddJsonBody(model);
            var response = await ExecutePostAsync(request);

            if (!response.IsSuccessful)
                Throw(response);

            return response.Content;
        }

        public async Task<string> UpdateNotaryProcedure(UpdateNotaryProcedureModel model)
        {
            var request = new RestRequest("UpdateNotaryProcedure");
            request.AddJsonBody(model);
            var response = await ExecutePostAsync(request);

            if (!response.IsSuccessful)
                Throw(response);

            return response.Content;
        }


        public async Task<string> EnvioArchivoFirmadoNotarioAutorizadoMiFirma(EnvioArchivoFirmadoNotarioAutorizadoModel envioArchivoFirmado)
        {
            var request = new RestRequest("SendDocumentsNotaryProcedure");
            request.AddJsonBody(envioArchivoFirmado);
            var response = await ExecutePostAsync(request);

            if (!response.IsSuccessful)
                Throw(response);

            return response.Content;
        }
        #endregion

        #region Metodos privados
        private void Throw(IRestResponse response)
        {
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new Exception(response.ErrorMessage);
            }

            throw new Exception(response.Content);
        }

        #endregion


        public void Dispose()
        {

        }
    }
}
