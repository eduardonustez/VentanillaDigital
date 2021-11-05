using AutoMapper;
using Infraestructura.Transversal.Adaptador;
using Microsoft.Extensions.DependencyInjection;
using PortalAdministrador.Mapper;

namespace PortalAdministrador
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
