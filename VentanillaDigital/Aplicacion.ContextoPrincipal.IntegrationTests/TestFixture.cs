﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Aplicacion.ContextoPrincipal.IntegrationTests
{
    class TestFixture:IDisposable
    {
        private readonly TestServer _server;
        public HttpClient Client { get; }
        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseStartup<ServiciosDistribuidos.ContextoPrincipal.Startup>()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "..\\..\\..\\..\\..\\..\\ServiciosDistribuidos.ContextoPrincipal"));
                    config.AddJsonFile("appsettings.json");
                });
            _server = new TestServer(builder);
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:8888");
        }
        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}
