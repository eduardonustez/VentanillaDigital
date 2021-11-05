using OTPClient.Models;
using System;
using System.Threading.Tasks;

namespace OTPClient
{
    public interface IOTPClient
    {
        Task<OTPResponse> GenerarCodigoOTP(string correo, string celular);
        Task<OTPValidationResponse> ValidacionOTP(Guid identificador, string textoOTP);
    }
}