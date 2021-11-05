using ApiGateway.Filtro;
using ApiGateway.Helper;
using ApiGateway.Policies;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OTPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ApiRestGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region StartupExtensions
            services.AgregarAutenticacion(Configuration);
            services.AgregarSwagger();
            services.AgregarValidacionModelos();
            services.AgregarAdaptadorFactory();
            services.AgregarOTP(Configuration);
            services.AgregarUtilidades();
            services.AgregarKeyManagerClient(Configuration);
            #endregion

            services.AddControllers().AddFluentValidation();
            services.AddHttpContextAccessor();
            services.AddScoped<IHttpClientHelper, HttpClientHelper>();
            services.AddScoped<CustomExceptionFilterAttribute>();

            if (Configuration.GetSection("Seguridad:IgnorarCertificados")?.Value == "true")
            {
                ServicePointManager.ServerCertificateValidationCallback =
                   delegate (object sender, X509Certificate certificate, X509Chain
                        chain, SslPolicyErrors sslPolicyErrors)
                   {
                       return true;
                   };

                services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                });
            }
            else
            {
                services.AddHttpClient("HttpClientWithSSLUntrusted");
            }

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            PolicyBasedAuthorization.AddPolicies(services);

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiRest Gateway V1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
