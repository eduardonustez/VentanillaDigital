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
using PortalCliente.Services;
using PortalCliente.Services.Biometria;
using ApiGateway.Contratos;
using ApiGateway.Cliente;
using Radzen;
using PortalCliente.Services.Wacom;
using PortalCliente.Services.Recursos;
using Microsoft.JSInterop;
using Microsoft.Extensions.Hosting;
using PortalCliente.Services.Notaria;
using PortalCliente.Services.Notario;
using PortalCliente.Services.RedireccionLogin;
using ApiGateway;
using PortalCliente.HttpHandler;
using Microsoft.AspNetCore.Components;
using PortalCliente.Services.Parametrizacion;
using PortalCliente.Services.LoadingScreenService;
using System.Threading;
using PortalCliente.Services.DescriptorCliente;
using PortalCliente.Services.SignalR;

namespace PortalCliente
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            var gatewayApiUri = builder.HostEnvironment.BaseAddress + "api/";

            builder.Services.AddHttpClient<ICustomHttpClient, CustomHttpClient>
                (client => { client.BaseAddress = new Uri(gatewayApiUri); })
                .AddHttpMessageHandler(s => s.GetService<SpinnerHttpHandler>());
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            //builder.Services.AddScoped<ISignalRv2Service, SignalRv2Service>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddScoped<ITramiteService, TramiteService>();
            builder.Services.AddScoped<IActaNotarialService, ActaNotarialService>();
            builder.Services.AddScoped<IParametricasService, ParametricasService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRecursosService, RecursosService>();
            builder.Services.AddScoped<INotariaService, NotariaService>();
            builder.Services.AddScoped<INotarioService, NotarioService>();
            builder.Services.AddScoped<IRedireccionService, RedireccionService>();
            builder.Services.AddScoped<IMachineService, MachineService>();
            builder.Services.AddScoped<IParametrizacionServicio, ParametrizacionServicio>();
            builder.Services.AddScoped<IConfiguracionesService, ConfiguracionesService>();
            builder.Services.AddScoped<LoadingScreenService>();
            //builder.Services.AddScoped<ITrazabilidadService, TrazabilidadService>();
            builder.Services.AddScoped<IRNECService, RNECProxyService>();
            builder.Services.AddScoped<IDescriptorCliente, DescriptorCliente>();
            builder.Services.AddScoped<ICertificadoService, CertificadoService>();
            builder.Services.AddScoped<ITramiteVirtualService, TramiteVirtualService>();
            builder.Services.AddSingleton<ISignalRv2Service>(sp => {
                var jSRuntime = sp.GetRequiredService<IJSRuntime>();
                var configuracion = sp.GetRequiredService<IConfiguration>();
                return new SignalRv2Service(configuracion, jSRuntime);
            });
            builder.Services.AddAsyncInitializer<SignalRServiceInitializer>();
            //builder.Services.AddSingleton<WacomService>();
            //builder.Services.AddSingleton<WacomAgenteService>();
            builder.Services.AddSingleton<WacomService>();
            builder.Services.AddSingleton<WacomAgenteService>();
            builder.Services.AddSingleton<IWacomServiceFactory, WacomServiceFactory>();
            //builder.Services.AddSingleton<IWacomService>(b => b.GetService<WacomService>());
            builder.Services.AddAsyncInitializer<WacomServiceInitializer>();
            

            builder.Services.AddHttpClient<IAccountServiceClient, AccountServiceClient>
                (client => { client.BaseAddress = new Uri(gatewayApiUri); });
            builder.Services.AddHttpClient<IRecursosService, RecursosService>
                (client => { client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<ITrazabilidadService, TrazabilidadService>
                (client => { client.BaseAddress = new Uri(gatewayApiUri); });

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
                    policy.RequireRole("Administrador", "Notario Encargado");
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
