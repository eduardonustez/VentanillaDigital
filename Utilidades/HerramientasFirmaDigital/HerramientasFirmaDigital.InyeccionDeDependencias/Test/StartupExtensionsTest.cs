
using HerramientasFirmaDigital.Abstraccion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using Xunit;

namespace HerramientasFirmaDigital.InyeccionDeDependencias.Test
{
    public class StartupExtensionsTest
    {
        [Fact]
        public void AnadirHerramientasFirmaDigitalShouldRegisterFirmaEnPaginaAdicional()
        {
            var config = Mock.Of<IConfiguration>();
            Mock.Get(config)
                .Setup(cfg => cfg.GetSection("ConfigCertificado"))
                .Returns(Mock.Of<IConfigurationSection>());

            IServiceCollection services = new ServiceCollection();

            services.AnadirHerramientasFirmaDigital(config);

            var serviceProvider = services.BuildServiceProvider();
            var firmaEnPaginaAdicional = serviceProvider.GetService<IFirmaEnPaginaAdicional>();

            Assert.NotNull(firmaEnPaginaAdicional);
        }
    }
}
