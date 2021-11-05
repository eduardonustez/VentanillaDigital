using ApiGateway.Contratos.Models.Certificado;
using ApiGateway.Contratos.Models.Reportes;
using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.Notario
{

    public interface ICertificadoService
    {
        Task<SolicitudCertificadoDto> ObtenerDatosSolicitud();
        Task<bool> RegistrarSolicitud(SolicitudCertificadoDto solicitud);
        Task<string> ObtenerAutorizacion(SolicitudCertificadoDto solicitud);
        Task<string> ObtenerContrato(SolicitudCertificadoDto solicitud);
        Task<IEnumerable<CertificadoDTO>> ObtenerCertificados();
        Task<bool> ActualizarCertificadoNotario(int idCertificado);
    }
}
