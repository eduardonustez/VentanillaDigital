using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Infraestructura.ContextoPrincipal.Repositorios.Transaccional;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.KeyManager;
using Infraestructura.Transversal.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServiciosDistribuidos.TareasAutomaticas.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiciosDistribuidos.TareasAutomaticas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AgregarServiciosDeAplicaciones();
            services.AgregarRepositoriosDeInfraestructura(Configuration);

            services.AgregarUtilidadesTransversales(Configuration);
            services.AddHttpContextAccessor();

            services.AgregarContextoBD(Configuration);
            services.AgregarKeyManagerClient(Configuration);
            services.AddMemoryCache();
            services.AddSingleton<ImplementedCache>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Servicios Distribuidos Jobs", Version = "v1" });
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCronJob<GenerarDocumentos>((config) =>
            {
                config.CronExpression = Configuration["GenerarDocumentosCronExpression"];
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });

            services.AddCronJob<GenerarDocumentosAux>((config) =>
            {
                config.CronExpression = Configuration["GenerarDocumentosCronExpression"];
                config.TimeZoneInfo = TimeZoneInfo.Local;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
