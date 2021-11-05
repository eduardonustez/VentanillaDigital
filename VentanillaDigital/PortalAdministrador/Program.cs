using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Services;
using PortalAdministrador.Services.Biometria;
using ApiGateway.Contratos;
using ApiGateway.Cliente;
using Radzen;
using PortalAdministrador.Services.Wacom;
using PortalAdministrador.Services.Recursos;
using Microsoft.JSInterop;
using Microsoft.Extensions.Hosting;
using PortalAdministrador.Services.Notaria;
using PortalAdministrador.Services.Notario;
using PortalAdministrador.Services.RedireccionLogin;
using ApiGateway;
using PortalAdministrador.HttpHandler;
using Microsoft.AspNetCore.Components;
using PortalAdministrador.Services.Parametrizacion;
using PortalAdministrador.Services.LoadingScreenService;
using System.Threading;
using PortalAdministrador.Services.DescriptorCliente;
using PortalAdministracion.Services.UsuarioAdministracion;

namespace PortalAdministrador
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var gatewayApiUri = builder.HostEnvironment.BaseAddress + "api/";

            builder.Services.AddHttpClient<ICustomHttpClient, CustomHttpClient>
                ( client => { client.BaseAddress = new Uri(gatewayApiUri); })
                .AddHttpMessageHandler(s=>s.GetService< SpinnerHttpHandler>());

            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<ITramiteService, TramiteService>();
            builder.Services.AddScoped<IUsuarioAdministracionService, UsuarioAdministracionService>();
            builder.Services.AddScoped<IActaNotarialService, ActaNotarialService>();
            builder.Services.AddScoped<IParametricasService, ParametricasService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRecursosService, RecursosService>();
            builder.Services.AddScoped<INotariaService, NotariaService>();
            builder.Services.AddScoped<ITrazabilidadService, TrazabilidadService>();
            builder.Services.AddScoped<INotarioService, NotarioService>();
            builder.Services.AddScoped<IRedireccionService, RedireccionService>();
            builder.Services.AddScoped<IMachineService, MachineService>();
            builder.Services.AddScoped<IParametrizacionServicio, ParametrizacionServicio>();
            builder.Services.AddScoped<IConfiguracionesService, ConfiguracionesService>();
            builder.Services.AddScoped<LoadingScreenService>();
            builder.Services.AddScoped<IRNECService, RNECProxyService>();
            builder.Services.AddScoped<IDescriptorCliente, DescriptorCliente>();
            builder.Services.AddSingleton<WacomService>();
            builder.Services.AddSingleton<IWacomService>(b=> b.GetService<WacomService>());
            builder.Services.AddAsyncInitializer<WacomServiceInitializer>();

            builder.Services.AddHttpClient<IAccountServiceClient, AccountServiceClient>
                (client => { client.BaseAddress = new Uri(gatewayApiUri); });
            builder.Services.AddHttpClient<IRecursosService, RecursosService>
                (client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });

            var reconoSerServicioFijasUri = 
                builder.Configuration.GetSection("ConfiguracionServiciosAPI:ReconoSer").Value;

            builder.Services.AddHttpClient<RNECService>
                (client => { client.BaseAddress = new Uri(reconoSerServicioFijasUri); });

            var reconoSerServicioMovilesUri =
                builder.Configuration.GetSection("ConfiguracionServiciosAPI:ReconoSerMovil").Value;

            builder.Services.AddHttpClient<RNECMovilService>
                (client => { client.BaseAddress = new Uri(reconoSerServicioMovilesUri); });


            /*var reconoSerMovilServicioUri =
                builder.Configuration.GetSection("ConfiguracionServiciosAPI:ReconoSerMovil").Value;

            builder.Services.AddHttpClient<IRNECService, RNECMovilService>
                (client => { client.BaseAddress = new Uri(reconoSerMovilServicioUri); });*/

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddOptions();
            //builder.Services.AddAuthorizationCore();
            builder.Services.AddAuthenticationCore();
            builder.Services.AddScoped<HttpClient>();
            builder.Services.AgregarAdaptadorFactory();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("RequireAdminOnly", policy =>
                      policy.RequireRole("Administrador"));

                options.AddPolicy("RequireNotario", policy =>
                {
                    policy.RequireRole("Administrador","Notario Encargado");
                });
            });

            builder.Services.AddScoped<SpinnerHttpHandler>();

            var host = builder.Build();
            await host.InitAsync();
            await host.RunAsync();
            //builder.Services.AddBlazorise(options =>{options.ChangeTextOnKeyPress = true;})
            //  .AddBootstrapProviders()
            //  .AddFontAwesomeIcons();


            //builder.Services.AddSingleton(new HttpClient
            //{
            //    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            //});


            //var host = builder.Build();

            //host.Services
            //  .UseBootstrapProviders()
            //  .UseFontAwesomeIcons();

            //await host.RunAsync();
        }
    }
}
