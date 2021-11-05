using AutoMapper;
using Infraestructura.Transversal.Adaptador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortalCliente.Mapper;
using System.Text;

namespace PortalCliente
{
    public static class StartupExtensions
    {
        public static void AgregarAdaptadorFactory(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IAdaptadorTipoFactory, AdaptadorTipoAutoMapperFactory>();
            AdaptadorTipoFactory.SetCurrent(services.BuildServiceProvider().GetService<IAdaptadorTipoFactory>());
        }
    }
}
