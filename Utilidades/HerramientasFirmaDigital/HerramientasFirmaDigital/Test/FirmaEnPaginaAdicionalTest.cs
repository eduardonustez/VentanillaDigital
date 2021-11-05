using HerramientasFirmaDigital.Abstraccion;
using HerramientasFirmaDigital.Interno.Abstraccion;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TSAIntegracion.Abstraccion;
using TSAIntegracion.Entities;
using Xunit;

namespace HerramientasFirmaDigital.Test
{
    public class FirmaEnPaginaAdicionalTest
    {
        private FirmaEnPaginaAdicional _sut;

        private readonly byte[] _documentoEntrada;
        private readonly Mock<IGeneradorPaginaFirma> _mGeneradorPaginaFirma;
        private readonly byte[] _paginaGenerada; 
        private readonly byte[] _documentoFinal;
        private readonly byte[] _documentoFirmado;
        private readonly Mock<IAdjuntadorPdf> _mAdjuntadorPdf;
        private readonly Mock<IAdjuntadorPdfFactory> _mAdjuntadorPdfFactory;
        private readonly Mock<IEstampadorDeTiempo> _mEstampadorDeTiempo;
        private readonly TSAConfig _tsaConfig;
        public FirmaEnPaginaAdicionalTest()
        {
            _mGeneradorPaginaFirma = new Mock<IGeneradorPaginaFirma>();
            _mAdjuntadorPdf = new Mock<IAdjuntadorPdf>();
            _mAdjuntadorPdfFactory = new Mock<IAdjuntadorPdfFactory>();
            _mEstampadorDeTiempo = new Mock<IEstampadorDeTiempo>();
            _tsaConfig = GetUnimportantTSAConfig();

            _documentoEntrada = new byte[] { 1 };
            _paginaGenerada = new byte[] { 2, 3 };
            _documentoFinal = new byte[] { 1, 2, 3 };
            _documentoFirmado = new byte[] { 1, 2, 3, 4 };

            _sut = new FirmaEnPaginaAdicional(
                _mGeneradorPaginaFirma.Object,
                _mAdjuntadorPdfFactory.Object,
                _mEstampadorDeTiempo.Object,
                _tsaConfig);

            _mAdjuntadorPdfFactory
                .Setup(factory => factory.Obtener(It.IsAny<byte[]>()))
                .Returns(_mAdjuntadorPdf.Object);

            _mAdjuntadorPdf
                .Setup(adjuntador => adjuntador.Adjuntar(It.IsAny<byte[]>()))
                .Returns(_mAdjuntadorPdf.Object);

            _mAdjuntadorPdf
                .Setup(adjuntador => adjuntador.Generar())
                .Returns(_documentoFinal);

            _mGeneradorPaginaFirma
                .Setup(generador => generador.GenerarPagina(It.IsAny<DatosFirma>()))
                .Returns(_paginaGenerada);

            _mEstampadorDeTiempo
                .Setup(estampador =>
                    estampador.AgregarEstampaCronologica(It.IsAny<byte[]>(),
                                                        It.IsAny<ITSAConfig>(),
                                                        It.IsAny<string>()))
                    .Returns(_documentoFirmado);
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldCallGenerarPaginaWithInput(DatosFirma datosFirma)
        {
            _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);

            _mGeneradorPaginaFirma.Verify(generador => generador.GenerarPagina(datosFirma));
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldCallObtenerWithInput(DatosFirma datosFirma)
        {
            _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);

            _mAdjuntadorPdfFactory.Verify(factory => factory.Obtener(_documentoEntrada));
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldCallAdjuntarAndThenGenerar(DatosFirma datosFirma)
        {
            _mAdjuntadorPdf
                .Setup(adjuntador => adjuntador.Generar())
                .Callback(() => _mAdjuntadorPdf.Verify(
                    adjuntador => adjuntador.Adjuntar(_paginaGenerada))
                );

            _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);

            _mAdjuntadorPdf
                .Verify(adjuntador => adjuntador.Generar());
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldCallEstamparWithTSAConfigExceptCertificate(DatosFirma datosFirma)
        {
            string esperado = datosFirma.SerialCertificado;
            _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);


            _mEstampadorDeTiempo.Verify(
                estampador => estampador.AgregarEstampaCronologica(
                    It.IsAny<byte[]>(),
                    It.Is<ITSAConfig>(
                        cfg => 
                        cfg.Algorithm == UNIMPORTANT_ALGORITHM &&
                        cfg.Url == UNIMPORTANT_URL &&
                        cfg.Username == UNIMPORTANT_USERNAME &&
                        cfg.Password == UNIMPORTANT_PASSWORD &&
                        cfg.CertificateSerialNumber == esperado),
                    It.IsAny<string>()));
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldCallEstamparWithCompleteDocument(DatosFirma datosFirma)
        {
            var esperado = _documentoFinal;
            _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);


            _mEstampadorDeTiempo.Verify(
                estampador => estampador.AgregarEstampaCronologica(
                    _documentoFinal,
                    It.IsAny<ITSAConfig>(),
                    It.IsAny<string>()));
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldReturnTheResultFromEstampar(DatosFirma datosFirma)
        {
            var esperado = _documentoFirmado;
            var real = _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);

            Assert.Equal(esperado, real);
        }

        [Theory]
        [MemberData(nameof(ObtenerDatos.DatosFirmaPrueba), MemberType = typeof(ObtenerDatos))]
        public void FirmarEnNuevaPaginaShouldReturnTheCompleteDocumentIfEstamparThrows(DatosFirma datosFirma)
        {
            var esperado = _documentoFinal;

            _mEstampadorDeTiempo
                .Setup(estampador => estampador.AgregarEstampaCronologica(It.IsAny<byte[]>(),
                                                                        It.IsAny<ITSAConfig>(),
                                                                        It.IsAny<string>()))
                .Throws(new Exception());

            var real = _sut.FirmarEnNuevaPagina(_documentoEntrada, datosFirma);

            Assert.Equal(esperado, real);
        }

        private static readonly string UNIMPORTANT_ALGORITHM = "SHA";
        private static readonly string UNIMPORTANT_URL = "https://www.test.com";
        private static readonly string UNIMPORTANT_USERNAME = "test";
        private static readonly string UNIMPORTANT_PASSWORD = "123456";
        private TSAConfig GetUnimportantTSAConfig()
        {
            return new TSAConfig()
            {
                Algorithm = UNIMPORTANT_ALGORITHM,
                Url = UNIMPORTANT_URL,
                Username = UNIMPORTANT_USERNAME,
                Password = UNIMPORTANT_PASSWORD
            };
        }
    }
}
