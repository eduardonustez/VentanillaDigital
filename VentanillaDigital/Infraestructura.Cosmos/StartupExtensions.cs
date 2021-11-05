using Infraestructura.Cosmos.Impl;
using Infraestructura.Cosmos.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Cosmos
{
    public static class StartupExtensions
    {
        public static void AgregarCosmos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHistoricosCosmosInfraestructura, HistoricosCosmosInfraestructura>();
            services.AddScoped(x => {
                CosmosClientOptions clientOptions = new CosmosClientOptions()
                {
                    SerializerOptions = new CosmosSerializationOptions()
                    {
                        IgnoreNullValues = true
                    },
                    ConnectionMode = ConnectionMode.Gateway,
                };
                return new CosmosClient(configuration.GetSection("CosmosDB")["ConnectionString"], clientOptions);
            });
        }
    }
}
