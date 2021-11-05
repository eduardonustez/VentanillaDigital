using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTPClient
{
    public static class StartupExtensions
    {       
        public static void AgregarOTP(this IServiceCollection services, IConfiguration configuration)
        {

            var servicioUri =
                configuration.GetSection("ConfiguracionOTP:URI").Value;
            services.AddHttpClient<IOTPClient, OTPClient>(
                client => { 
                    client.BaseAddress = new Uri(servicioUri); 
                });

        }
    }
}
