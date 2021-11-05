using GeneradorQR;
using HerramientasFirmaDigital.Abstraccion;
using HerramientasFirmaDigital.Interno;
using HerramientasFirmaDigital.Interno.Abstraccion;
using HtmlToPdf;
using System;
using TSAIntegracion;
using TSAIntegracion.Abstraccion;
using TSAIntegracion.Entities;

namespace HerramientasFirmaDigital
{
    public class FirmaEnPaginaAdicional : IFirmaEnPaginaAdicional
    {
        private readonly IGeneradorPaginaFirma _generadorPaginaFirma;
        private readonly IAdjuntadorPdfFactory _adjuntadorPdfFactory;
        private readonly IEstampadorDeTiempo _estampadorDeTiempo;
        private readonly ITSAConfig _tsaConfig;

        public FirmaEnPaginaAdicional(IHtmlToPdf htmlToPdf,
            IGeneradorQR generadorQR,
            IEstampadorDeTiempo estampadorDeTiempo,
            ITSAConfig tsaConfig) :
            this(new GeneradorPaginaFirma(generadorQR, htmlToPdf),
                new AdjuntadorPdfItextFactory(),
                estampadorDeTiempo,
                tsaConfig)
        { }

        internal FirmaEnPaginaAdicional(
            IGeneradorPaginaFirma generadorPaginaFirma,
            IAdjuntadorPdfFactory adjuntadorPdfFactory,
            IEstampadorDeTiempo estampadorDeTiempo,
            ITSAConfig tsaConfig)
        {
            _generadorPaginaFirma = generadorPaginaFirma;
            _adjuntadorPdfFactory = adjuntadorPdfFactory;
            _estampadorDeTiempo = estampadorDeTiempo;
            _tsaConfig = tsaConfig;
        }

        public byte[] FirmarEnNuevaPagina(
            byte[] documento,
            DatosFirma datosFirma)
        {
            var nuevaPagina = _generadorPaginaFirma.GenerarPagina(datosFirma);

            var archivoCompleto =
                _adjuntadorPdfFactory.Obtener(documento)
                    .Adjuntar(nuevaPagina)
                    .Generar();

            var myConfig = new TSAConfig()
            {
                Algorithm = _tsaConfig.Algorithm,
                Url = _tsaConfig.Url,
                Username = _tsaConfig.Username,
                Password = _tsaConfig.Password,
                CertificateSerialNumber = datosFirma.SerialCertificado
            };

            try
            {
                return _estampadorDeTiempo.AgregarEstampaCronologica(
                    archivoCompleto, 
                    myConfig, 
                    datosFirma.NombreCompleto);
            }
            catch (Exception ex)
            {
                return archivoCompleto;
            }
        }
    }
}
