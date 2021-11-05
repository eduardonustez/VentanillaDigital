using GeneradorQR;
using HerramientasFirmaDigital.Abstraccion;
using HtmlToPdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TSAIntegracion;
using TSAIntegracion.Abstraccion;
using TSAIntegracion.Entities;

namespace HerramientasFirmaDigital.InyeccionDeDependencias
{
    public static class StartupExtensions
    {
        public static void AnadirHerramientasFirmaDigital(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IGeneradorQR, GeneradorQR.GeneradorQR>();
            services.AddSingleton<IHtmlToPdf, HtmlToPdf.HtmlToPdf>();
            services.AddSingleton<IAdjuntadorPdfFactory, AdjuntadorPdfItextFactory>();
            services.AddScoped<IEstampadorDeTiempo, EstampadorDeTiempo>();
            services.AddScoped<IPdfFiller, PdfFiller>();
            services.AddScoped<IFirmaEnPaginaAdicional, FirmaEnPaginaAdicional>(
                (prov) => new FirmaEnPaginaAdicional(
                    prov.GetRequiredService<IHtmlToPdf>(),
                    prov.GetRequiredService<IGeneradorQR>(),
                    prov.GetRequiredService<IEstampadorDeTiempo>(),
                    GetTSAConfig(configuration)
                ));
        }

        private static ITSAConfig GetTSAConfig(IConfiguration configuration)
        {
            var certificadoConfig = configuration.GetSection("ConfigCertificado");
            return new TSAConfig()
            {
                Algorithm = certificadoConfig[nameof(TSAConfig.Algorithm)],
                Url = certificadoConfig[nameof(TSAConfig.Url)],
                CertificateSerialNumber = certificadoConfig[nameof(TSAConfig.CertificateSerialNumber)],
                CertificatePassword = certificadoConfig[nameof(TSAConfig.CertificatePassword)],
                Location = certificadoConfig[nameof(TSAConfig.Location)],
                Reason = certificadoConfig[nameof(TSAConfig.Reason)],
                Password = certificadoConfig[nameof(TSAConfig.Password)],
                Username = certificadoConfig[nameof(TSAConfig.Username)],
            };
        }

    }
}
