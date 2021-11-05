using Aplicacion.TareasAutomaticas.Contrato.Transaccional;
using Aplicacion.TareasAutomaticas.Servicio.Tansaccional;
using AutoMapper;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas.Archivos;
using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using HtmlToPdf;
using Infraestructura.ContextoPrincipal.Repositorios;
using Infraestructura.ContextoPrincipal.Repositorios.Parametricas;
using Infraestructura.ContextoPrincipal.Repositorios.Parametricas.Archivos;
using Infraestructura.ContextoPrincipal.Repositorios.StoredProcedures;
using Infraestructura.ContextoPrincipal.Repositorios.Transaccional;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Transversal;
using Infraestructura.Transversal.Adaptador;
using GeneradorQR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiciosDistribuidos.TareasAutomaticas.Jobs;
using System;
using TSAIntegracion.Entities;
using Microsoft.Extensions.Logging;
using Infraestructura.Transversal.Log.Modelo;
using Infraestructura.KeyManager;

namespace ServiciosDistribuidos.TareasAutomaticas
{
    public static class StartupExtensions
    {
        public static void AgregarServiciosDeAplicaciones(this IServiceCollection services)
        {
            services.AddScoped<IGenerarActaServicio, GenerarActaServicio>();
        }

        public static void AgregarRepositoriosDeInfraestructura(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.AddScoped<ITramiteRepositorio, TramiteRepositorio>();
            services.AddScoped<INotariasUsuarioRepositorio, NotariasUsuarioRepositorio>();
            services.AddScoped<INotariaRepositorio, NotariaRepositorio>();
            services.AddScoped<IComparecienteRepositorio, ComparecienteRepositorio>();
            services.AddScoped<INotarioRepositorio, NotarioRepositorio>();
            services.AddScoped<IDocumentoPendienteAutorizarRepositorio, DocumentoPendienteAutorizarRepositorio>();
            services.AddScoped<IActaNotarialRepositorio, ActaNotarialRepositorio>();
            services.AddScoped<IPlantillaActaRepositorio, PlantillaActaRepositorio>();
        }

        //public static void AgregarLocalizationFactory(this IServiceCollection services)
        //{
        //    services.AddScoped<CustomExceptionFilterAttribute>();
        //    services.AddScoped<TokenValidationFilterAttribute>();
        //    //LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
        //}

        public static void AgregarContextoBD(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(UnidadTrabajo));
            int timeOutSeconds = 1200;
            try
            {
                string timeOutMinutes = configuration.GetValue<String>("ConfigTimeout:BD");
                timeOutSeconds = Convert.ToInt32(timeOutMinutes) * 60;
            }
            catch { }

            services.AddDbContext<UnidadTrabajo>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSeconds)));
        }

        public static void AgregarUtilidadesTransversales(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGeneradorQR, GeneradorQR.GeneradorQR>();
            services.AddSingleton<IHtmlToPdf, HtmlToPdf.HtmlToPdf>();
            var certificadoConfig = configuration.GetSection("ConfigCertificado");
            services.AddSingleton<ITSAConfig>(new TSAConfig()
            {
                Algorithm = certificadoConfig[nameof(TSAConfig.Algorithm)],
                Url = certificadoConfig[nameof(TSAConfig.Url)],
                Username = certificadoConfig[nameof(TSAConfig.Username)],
                Password = certificadoConfig[nameof(TSAConfig.Password)],
                CertificateSerialNumber = certificadoConfig[nameof(TSAConfig.CertificateSerialNumber)],
                CertificatePassword = certificadoConfig[nameof(TSAConfig.CertificatePassword)],
                Location = certificadoConfig[nameof(TSAConfig.Location)],
                Reason = certificadoConfig[nameof(TSAConfig.Reason)]
            });

        }

        public static void AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options)
            where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            var config = new ScheduleConfig<T>();
            options.Invoke(config);

            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
        }
    }
}