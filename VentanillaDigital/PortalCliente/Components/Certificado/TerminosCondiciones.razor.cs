using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PortalCliente.Services.Notario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Contratos.Models.Certificado;
using BlazorInputFile;
using System.IO;

namespace PortalCliente.Components.Certificado
{
    public partial class TerminosCondiciones : ComponentBase
    {
        [Parameter]
        public bool Aceptado{get;set;}
        [Parameter]
        public EventCallback<bool> aceptadoChanged { get; set; }
        [Inject]
        IJSRuntime JsRuntime{get;set;}
        protected override async Task OnInitializedAsync()
        {
            //await JsRuntime.InvokeVoidAsync("scrollTyc");
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("scrollTyc");
        }
        public void Aceptar(){
            aceptadoChanged.InvokeAsync(true);

        }
      
    }
}