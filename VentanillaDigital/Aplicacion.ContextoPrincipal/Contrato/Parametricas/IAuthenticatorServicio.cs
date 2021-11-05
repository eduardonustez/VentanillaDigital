using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Microsoft.AspNetCore.Mvc;
using ServiciosDistribuidos.ContextoPrincipal.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato.Parametricas
{
    public interface IAuthenticatorServicio:IDisposable
    {
        Task<IActionResult> VerificarUsuarioOTP(TramiteCreateDTO tramite);

        Task<ResponseDTO> CreateUserToken(string numeroDocumento);
        Task<ResponseDTO> AutenticarOtp(OtpAuthenticator authModel);
    }
}
