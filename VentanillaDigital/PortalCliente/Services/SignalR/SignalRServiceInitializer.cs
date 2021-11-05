using Extensions.Hosting.AsyncInitialization;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PortalCliente.Services.Recursos;

namespace PortalCliente.Services.SignalR
{
    public class SignalRServiceInitializer : IAsyncInitializer
    {
        private ISignalRv2Service _signalRv2Service { get; set; }
        public SignalRServiceInitializer(ISignalRv2Service SignalRv2Service,
            IRecursosService recursosService)
        {
            _signalRv2Service = SignalRv2Service;
        }
        public async Task InitializeAsync()
        {
            await _signalRv2Service.Initialize();
        }
    }
}
