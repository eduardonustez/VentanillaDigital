using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalCliente.Helper
{
    public static class IJSRuntimeExtensionsMethods 
    {
        public static async ValueTask InicializarTimerInactivo<T>(this IJSRuntime js,
            DotNetObjectReference<T> dotNetObjectReference) where T : class
        {
            await js.InvokeVoidAsync("timerInactivo", dotNetObjectReference);
        }
    }
}
