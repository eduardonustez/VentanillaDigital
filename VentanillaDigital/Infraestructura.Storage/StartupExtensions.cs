using Azure.Storage.Blobs;
using Infraestructura.Storage.Impl;
using Infraestructura.Storage.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructura.Storage
{
    public static class StartupExtensions
    {
        public static void AgregarStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHistoricosStorageInfraestructura, HistoricosStorageInfraestructura>();
            services.AddScoped(x => new BlobServiceClient(configuration.GetSection("BlobStorage")["ConnectionString"]));
        }
    }
}
