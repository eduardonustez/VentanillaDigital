using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;



namespace PortalCliente.Components.RegistroTramite
{
    public partial class CapturaFoto
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        private string TextoBoton { get => Capturando ? "Tomar foto" : "Iniciar nueva captura"; }
        private bool Capturando { get; set; } = false;

        [Parameter]
        public string Foto { get; set; }

        [Parameter]
        public EventCallback<string> FotoChanged { get; set; }

        [Parameter]
        public EventCallback<bool> ReadyChanged { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if(string.IsNullOrEmpty(Foto))
                    await Capturar();
            }
        }

        private async Task Capturar()
        {
            if (!Capturando)
            {
                Capturando = await JsRuntime.InvokeAsync<bool>("IniciarCaptura");
                await ReadyChanged.InvokeAsync(false);
            }
            else
            {
                Foto = await JsRuntime.InvokeAsync<string>("TerminarCaptura");
                await FotoChanged.InvokeAsync(Foto);
                await ReadyChanged.InvokeAsync(true);
                Capturando = false;
            }
            StateHasChanged();
        }
        private async Task cambiar()
        {
            await JsRuntime.InvokeAsync<string>("cambiarcamra");
            //await ReadyChanged.InvokeAsync(true);
        }
    }
}
