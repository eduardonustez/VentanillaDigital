using ApiGateway.Contratos.Models.Certificado;
using ApiGateway.Contratos.Models.Reportes;
using ApiGateway.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PortalCliente.Services.Notario
{
    public class CertificadoService : ICertificadoService
    {
        private readonly ICustomHttpClient _customHttpClient;


        public CertificadoService(ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;
        }
        public async Task<SolicitudCertificadoDto> ObtenerDatosSolicitud()
        {
            var resultado = await _customHttpClient.GetJsonAsync<SolicitudCertificadoDto>("Certificado/ObtenerDatosSolicitud");
            return resultado;
        }
        public async Task<bool> RegistrarSolicitud(SolicitudCertificadoDto solicitud)
        {
            return await _customHttpClient.PostJsonAsync<bool>("Certificado/RegistrarSolicitud", solicitud);
        }
        public async Task<string> ObtenerAutorizacion(SolicitudCertificadoDto solicitud)
        {
            var resultado = await _customHttpClient.PostJsonAsync<string>("Certificado/ObtenerAutorizacion",solicitud);
            return resultado;
        }
        public async Task<string> ObtenerContrato(SolicitudCertificadoDto solicitud)
        {
            var resultado = await _customHttpClient.PostJsonAsync<string>("Certificado/ObtenerContrato", solicitud);
            return resultado;
        }

        public async Task<IEnumerable<CertificadoDTO>> ObtenerCertificados()
        {
            var resultado = await _customHttpClient.GetJsonAsync<IEnumerable<CertificadoDTO>>("Certificado/ObtenerCertificados");
            return resultado;
        }

        public async Task<bool> ActualizarCertificadoNotario(int idCertificado)
        {
            var resultado = await _customHttpClient.PostJsonAsync<bool>($"Certificado/ActualizarCertificadoNotario/{idCertificado}",null);
            return resultado;
        }
    }
}
