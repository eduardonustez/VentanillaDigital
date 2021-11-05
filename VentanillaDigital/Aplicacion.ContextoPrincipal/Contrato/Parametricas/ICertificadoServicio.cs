using Aplicacion.ContextoPrincipal.Modelo;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Transaccional;
using Infraestructura.Transversal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacion.ContextoPrincipal.Contrato
{
    public interface ICertificadoServicio:IDisposable
    {
        Task<SolicitudCertificadoDTO> ObtenerDatosSolicitud(string userId);
        Task<string> RegistrarSolicitud(CertificadoCreateDTO solicitud);
        Task<CertificadoSelectedDTO> ObtenerCertificadoNotario(string usuarioId);
        Task ActualizarCertificadoNotario(CertificadoSelectedDTO certificadoSelected);
        Task ActualizarUsuarioNotario(CertificadoSelectedDTO certificadoSelected);
    }
}
