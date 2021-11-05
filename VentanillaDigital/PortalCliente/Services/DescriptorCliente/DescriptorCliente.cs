using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Services.DescriptorCliente
{
    public class DescriptorCliente:IDescriptorCliente
    {
        private IJSRuntime JSRuntime { get; set; }

        public DescriptorCliente (IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
            EsMovil = JSRuntime.InvokeAsync<string>("detectOS").AsTask()
                .ContinueWith(t => t.Result == "Android");
        }

        public Task<bool> EsMovil { get; private set; }
    }
}
