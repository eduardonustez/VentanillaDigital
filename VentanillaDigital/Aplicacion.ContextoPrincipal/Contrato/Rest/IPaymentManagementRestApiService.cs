using Aplicacion.ContextoPrincipal.Modelo.Rest;
using System;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Rest
{
    public interface IPaymentManagementRestApiService : IDisposable
    {
        Task<string> CreatePaymentLink(CreatePaymentLinkModel model);
        Task<string> CreateMultiplePayments(CreateMultiplePaymentsModel model);
        Task<string> UpdateNotaryProcedure(UpdateNotaryProcedureModel model);
        Task<string> EnvioArchivoFirmadoNotarioAutorizadoMiFirma(EnvioArchivoFirmadoNotarioAutorizadoModel envioArchivoFirmado);
    }
}
