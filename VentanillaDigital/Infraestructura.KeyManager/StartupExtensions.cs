using Infraestructura.KeyManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Infraestructura.KeyManager
{
    public static class StartupExtensions
    {
        public static void AgregarKeyManagerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var _keyManagerConfig = configuration.GetSection("ConfiguracionKeyManager");
            var servicioUri = _keyManagerConfig["URI"];
            string Usuario = _keyManagerConfig["Usuario"];
            string Contrasena = _keyManagerConfig["Contrasena"];

            services.AddSingleton(new UserLoginRequest(Usuario,Contrasena));

            services.AddHttpClient<IKeyManagerClient, KeyManagerClient>(
                client => {
                    client.BaseAddress = new Uri(servicioUri);
                });
        }
    }
}
