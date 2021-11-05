using Aplicacion.ContextoPrincipal.Contrato;
using Aplicacion.ContextoPrincipal.Mapper;
using Aplicacion.ContextoPrincipal.Servicio;
using AutoMapper;
using Dominio.ContextoPrincipal.ContratoRepositorio;
using Infraestructura.AgenteServicios.AgenteANI;
using Infraestructura.ContextoPrincipal.Repositorios;
using Infraestructura.ContextoPrincipal.UnidadDeTrabajo;
using Infraestructura.Transversal;
using Infraestructura.Transversal.Adaptador;
using Infraestructura.Transversal.Correo;
using Infraestructura.Transversal.Log.Contratos;
using Infraestructura.Transversal.Log.Implementacion;
using Infraestructura.Transversal.Log.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Infraestructura.ContextoPrincipal.Repositorios.Parametricas;
using Aplicacion.ContextoPrincipal.Contrato.Transaccional;
using Aplicacion.ContextoPrincipal.Servicio.Transaccional;
using Dominio.ContextoPrincipal.ContratoRepositorio.Transaccional;
using Dominio.ContextoPrincipal.ContratoRepositorio.StoredProcedures;
using Infraestructura.ContextoPrincipal.Repositorios.StoredProcedures;
using ServiciosDistribuidos.ContextoPrincipal.Filtro;
using plicacion.ContextoPrincipal.Servicio;
using Infraestructura.AgenteReconoser;
using TSAIntegracion;
using TSAIntegracion.Entities;
using GeneradorQR;
using Infraestructura.ContextoPrincipal.Repositorios.Transaccional;
using Infraestructura.ContextoArchivos.UnidadDeTrabajo;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas.Archivos;
using Infraestructura.ContextoPrincipal.Repositorios.Parametricas.Archivos;
using Infraestructura.PowerBI;
using Infraestructura.Storage.Interfaces;
using Infraestructura.Storage.Impl;
using Infraestructura.Cosmos.Interfaces;
using Infraestructura.Cosmos.Impl;
using HtmlToPdf;
using Dominio.ContextoPrincipal.ContratoRepositorio.Parametricas;
using Aplicacion.TareasAutomaticas.Contrato.Transaccional;
using Aplicacion.TareasAutomaticas.Servicio.Tansaccional;
using Aplicacion.ContextoPrincipal.Servicio.Rest;
using Aplicacion.ContextoPrincipal.Contrato.Rest;
using Infraestructura.ContextoPrincipal.Repositorios.Log;
using Dominio.ContextoPrincipal.ContratoRepositorio.Log;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios.Interfaces;
using Aplicacion.ContextoPrincipal.DigitalizacionNotairal.Servicios;
using PdfTronUtils;
using Aplicacion.ContextoPrincipal.Contrato.Parametricas;
using Aplicacion.ContextoPrincipal.Servicio.Parametricas;
using Aplicacion.ContextoPrincipal.Modelo.Parametricas;

namespace ServiciosDistribuidos.ContextoPrincipal
{
    public static class StartupExtensions
    {
        public static void AgregarServiciosDeAplicaciones(this IServiceCollection services)
        {
            services.AddScoped<IPdfTronService, PdfTronService>();
            services.AddScoped<ITipoIdentificacionServicio, TipoIdentificacionServicio>();
            services.AddScoped<ITipoTramiteServicio, TipoTramiteServicio>();
            services.AddScoped<ICategoriaServicio, CategoriaServicio>();
            services.AddScoped<ITramiteServicio, TramiteServicio>();
            services.AddScoped<IMaquinaServicio, MaquinaServicio>();
            services.AddScoped<IUbicacionServicio, UbicacionServicio>();
            services.AddScoped<INotariaServicio, NotariaServicio>();
            services.AddScoped<INotarioServicio, NotarioServicio>();
            services.AddScoped<IComparecienteServicio, ComparecienteServicio>();
            services.AddScoped<INotariasUsuarioServicio, NotariasUsuarioServicio>();
            services.AddScoped<IPersonasServicio, PersonasServicio>();
            services.AddScoped<IActaNotarialServicio, ActaNotarialServicio>();
            services.AddScoped<ITemplateServicio, TemplateServicio>();
            services.AddScoped<IPlantillaActaServicio, PlantillaActaServicio>();
            services.AddScoped<IConsultaNotariaSeguraServicio, ConsultaNotariaSeguraServicio>();
            services.AddScoped<IGenerarActaServicio, GenerarActaServicio>();
            services.AddScoped<IUsuarioAdministracionServicio, UsuarioAdministracionServicio>();
            services.AddScoped<ICertificadoServicio, CertificadoServicio>();
            services.AddScoped<IPortalVirtualServicio, PortalVirtualServicio>();
            //services.AddScoped<IMiFirmaRestApiService, MiFirmaRestApiService>();

            services.AddScoped<IConvenioNotariaVirtualServicio, ConvenioNotariaVirtualServicio>();
            services.AddScoped<IActoNotarialServicio, ActoNotarialServicio>();
            

        }

        public static void AgregarRepositoriosDeInfraestructura(this IServiceCollection services
            , IConfiguration configuration)
        {
            services.AddScoped<ITipoIdentificacionRepositorio, TipoIdentificacionRepositorio>();
            services.AddScoped<ITipoTramiteRepositorio, TipoTramiteRepositorio>();
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<ITramiteRepositorio, TramiteRepositorio>();
            services.AddScoped<ITramitePortalVirtualMensajeRepositorio, TramitePortalVirtualMensajeRepositorio>();
            services.AddScoped<IMaquinaRepositorio, MaquinaRepositorio>();
            services.AddScoped<IConvenioRNECRepositorio, ConvenioRNECRepositorio>();
            services.AddScoped<IUbicacionRepositorio, UbicacionRepositorio>();
            services.AddScoped<IPersonasRepositorio, PersonasRepositorio>();
            services.AddScoped<INotariasUsuarioRepositorio, NotariasUsuarioRepositorio>();
            services.AddScoped<INotariaRepositorio, NotariaRepositorio>();
            services.AddScoped<ITemplateRepositorio, TemplateRepositorio>();
            services.AddScoped<IComparecienteRepositorio, ComparecienteRepositorio>();
            services.AddScoped<IProcedimientoAlmacenadoRepositorio, ProcedimientosAlmacenadosRepositorio>();
            services.AddScoped<INotarioRepositorio, NotarioRepositorio>();
            services.AddScoped<IDocumentoPrevioRepositorio, DocumentoPrevioRepositorio>();
            services.AddScoped<IDocumentoPendienteAutorizarRepositorio, DocumentoPendienteAutorizarRepositorio>();
            services.AddScoped<IActaNotarialRepositorio, ActaNotarialRepositorio>();
            services.AddScoped<IComparecienteRepositorio, ComparecienteRepositorio>();
            services.AddScoped<IPlantillaActaRepositorio, PlantillaActaRepositorio>();
            services.AddScoped<IParametricaRepositorio, ParametricaRepositorio>();
            services.AddScoped<IArchivoRepositorio, ArchivoRepositorio>();
            services.AddScoped<ISelloNotariaRepositorio, SelloNotariaRepositorio>();
            services.AddScoped<IGrafoNotarioRepositorio, GrafoNotarioRepositorio>();
            services.AddScoped<IConsultaNotariaSeguraRepositorio, ConsultaNotariaRepositorio>();
            services.AddScoped<IUsuarioAdministracionRepositorio, UsuarioAdministracionRepositorio>();
            services.AddScoped<ICertificadoRepositorio, CertificadoRepositorio>();
            services.AddScoped<IConvenioNotariaVirtualRepositorio, ConvenioNotariaVirtualRepositorio>();
            services.AddScoped<IPortalVirtualRepositorio, PortalVirtualRepositorio>();
            services.AddScoped<IRecaudoTramiteVirtualRepositorio, RecaudoTramiteVirtualRepositorio>();
            services.AddScoped<IArchivosPortalVirtualRepositorio, ArchivosPortalVirtualRepositorio>();
            services.AddScoped<IEstadoTramiteVirtualRepositorio, EstadoTramiteVirtualRepositorio>();
            services.AddScoped<ITipoTramiteVirtualRepositorio, TipoTramiteVirtualRepositorio>();
            services.AddScoped<ILogTramitePortalVirtualRepositorio, LogTramitePortalVirtualRepositorio>();
            services.AddScoped<IActoNotarialRepositorio, ActoNotarialRepositorio>();
            services.AddScoped<IActoPorTramiteRepositorio, ActoPorTramiteRepositorio>();
            services.AddScoped<ITipoArchivoTramiteVirtualRepositorio, TipoArchivoTramiteVirtualRepositorio>();

            services.AddScoped<IUsuarioTokenPortalAdministradorRepositorio, UsuarioTokenPortalAdministradorRepositorio>();

            var ConfigServidorCorreo = configuration.GetSection("ConfigServidorCorreo");


            ServidorCorreo servidor = new ServidorCorreo()
            {
                host = ConfigServidorCorreo["Host"],
                port = ConfigServidorCorreo["Port"],
                fromaddress = ConfigServidorCorreo["FromAddress"],
                fromname = ConfigServidorCorreo["FromName"],
                username = ConfigServidorCorreo["Username"],
                password = ConfigServidorCorreo["Password"]
            };

            services.AddSingleton<IManejadorCorreos>(new ManejadorCorreos(servidor));

            services.AddTransient<IDigitalizacionNotarialServicio>(x =>
                new DigitalizacionNotarialServicio(configuration.GetSection("ServiciosExternos").GetSection("DigitalizacionNotairal")));

            IConfigurationSection appSettingsSection = configuration.GetSection("ConfiguracionMiFirma");
            services.Configure<MiFirmaSettings>(appSettingsSection);
        }
        public static void AgregarAgenteServicios(this IServiceCollection services, IConfiguration configuration)
        {
            ANIParametersModel parameters = new ANIParametersModel()
            {
                Uri = configuration.GetValue<String>("ServiciosExternos:ANI:Uri"),
                Username = configuration.GetValue<String>("ServiciosExternos:ANI:Username"),
                Password = configuration.GetValue<String>("ServiciosExternos:ANI:Password"),
                Aplicacion = configuration.GetValue<String>("ServiciosExternos:ANI:Aplicacion")
            };
            services.AddSingleton<IAgenteANI>(new AgenteANI(parameters));

            services.AddScoped<IAgenteReconoser, AgenteReconoser>();

        }

        public static void AgregarPowerBIServicios(this IServiceCollection services, IConfiguration configuration)
        {
            ConfiguracionReportesPowerBI parameters = new ConfiguracionReportesPowerBI();
            var valuesSection = configuration.GetSection("ConfigPowerBI:Reportes");
            parameters.AuthorityUrl = configuration.GetValue<string>("ConfigPowerBI:AuthorityUrl");
            parameters.ResourceUrl = configuration.GetValue<string>("ConfigPowerBI:ResourceUrl");
            parameters.ApiUrl = configuration.GetValue<string>("ConfigPowerBI:ApiUrl");
            parameters.EmbedUrlBase = configuration.GetValue<string>("ConfigPowerBI:EmbedUrlBase");
            parameters.Reportes = new List<ConfiguracionReporte>();
            foreach (IConfigurationSection section in valuesSection.GetChildren())
            {
                parameters.Reportes.Add(new ConfiguracionReporte
                {
                    TipoReporte = section.GetValue<string>("TipoReporte"),
                    esMasterUser = section.GetValue<bool>("esMasterUser"),
                    ApplicationId = section.GetValue<Guid>("ApplicationId"),
                    WorkspaceId = section.GetValue<Guid>("WorkspaceId"),
                    ReportId = section.GetValue<Guid>("ReportId"),
                    Username = section.GetValue<string>("Username"),
                    Password = section.GetValue<string>("Password"),
                    ApplicationSecret = section.GetValue<string>("ApplicationSecret"),
                    Tenant = section.GetValue<Guid>("Tenant"),
                    UnobtrusiveJavaScriptEnabled = section.GetValue<bool>("UnobtrusiveJavaScriptEnabled"),
                    UrlVaultAzure = section.GetValue<string>("UrlVaultAzure"),
                    CertifiedName = section.GetValue<string>("CertifiedName"),
                    esCertificate = section.GetValue<bool>("esCertificate")
                });
            }

            services.AddSingleton<IReporteService>(new ReporteService(parameters));
        }

        public static void AgregarAdaptadorFactory(this IServiceCollection services)
        {

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
                // Número -> Texto
                mc.CreateMap<Byte, string>().ConvertUsing(new CifradorTransformador<Byte>());
                mc.CreateMap<Int16, string>().ConvertUsing(new CifradorTransformador<Int16>());
                mc.CreateMap<Int32, string>().ConvertUsing(new CifradorTransformador<Int32>());
                mc.CreateMap<Int64, string>().ConvertUsing(new CifradorTransformador<Int64>());
                mc.CreateMap<Guid, string>().ConvertUsing(new CifradorTransformador<Guid>());

                // Texto -> Número
                mc.CreateMap<string, Byte>().ConvertUsing(new DescifradorTransformador<Byte>());
                mc.CreateMap<string, Int16>().ConvertUsing(new DescifradorTransformador<Int16>());
                mc.CreateMap<string, Int32>().ConvertUsing(new DescifradorTransformador<Int32>());
                mc.CreateMap<string, Int64>().ConvertUsing(new DescifradorTransformador<Int64>());
                mc.CreateMap<string, Guid>().ConvertUsing(new DescifradorTransformador<Guid>());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IAdaptadorTipoFactory, AdaptadorTipoAutoMapperFactory>();
            AdaptadorTipoFactory.SetCurrent(services.BuildServiceProvider().GetService<IAdaptadorTipoFactory>());

        }
        public static void AgregarContextoBD(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(nameof(UnidadTrabajo));
            var connectionStringArchivos = configuration.GetConnectionString(nameof(UnidadTrabajoArchivos));
            int timeOutSeconds = 1200;
            try
            {
                string timeOutMinutes = configuration.GetValue<String>("ConfigTimeout:BD");
                timeOutSeconds = Convert.ToInt32(timeOutMinutes) * 60;
            }
            catch { }

            services.AddDbContext<UnidadTrabajo>(options => options.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSeconds)));
            services.AddDbContext<UnidadTrabajoArchivos>(options => options.UseSqlServer(connectionStringArchivos, sqlServerOptions => sqlServerOptions.CommandTimeout(timeOutSeconds)));
        }
        public static void AgregarSerilogFactory(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration != null)
            {

                services.AddSingleton<ISerilogFactory, SerilogLoggerFactory>();
                services.BuildServiceProvider().GetService<ILogger>();
                var serilogConfigExcepciones = configuration.GetSection("SerilogConfigExcepciones");
                var serilogConfigEventos = configuration.GetSection("SerilogConfigEventos");

                var configuracionExcepciones = new SerilogConfig()
                {
                    ConnectionStrings = configuration.GetConnectionString("SerilogContexto"),
                    NombreSchema = serilogConfigExcepciones[nameof(SerilogConfig.NombreSchema)],
                    NombreTabla = serilogConfigExcepciones[nameof(SerilogConfig.NombreTabla)]

                };
                var configuracionEventos = new SerilogConfig()
                {
                    ConnectionStrings = configuration.GetConnectionString("SerilogContexto"),
                    NombreSchema = serilogConfigEventos[nameof(SerilogConfig.NombreSchema)],
                    NombreTabla = serilogConfigEventos[nameof(SerilogConfig.NombreTabla)]
                };

                SerilogFactory.SetCurrent(new SerilogLoggerFactory(configuracionExcepciones, configuracionEventos));
            }

        }

        public static void AgregarLocalizationFactory(this IServiceCollection services)
        {
            services.AddScoped<CustomExceptionFilterAttribute>();
            services.AddScoped<TokenValidationFilterAttribute>();
            services.AddScoped<TokenValidationAdministrationAttribute>();
            services.AddScoped<TokenValidationAdministrationRoleAttribute>();
            //LocalizationFactory.SetCurrent(new ResourcesManagerFactory());
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


    }
}
