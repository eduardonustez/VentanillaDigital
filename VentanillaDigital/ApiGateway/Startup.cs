using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using ApiGateway.Filtro;
using ApiGateway.Helper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OTPClient;
using ApiGateway.Policies;
using Infraestructura.KeyManager;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


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


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway Ventanilla Digital");
                c.RoutePrefix = "api";
            });
            

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseTokenValidator();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
