using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Components.TransaccionesVirtuales
{
    public partial class ModalFormVirtual : ComponentBase
    {
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        int motivoSelec;

        [Inject]
        IJSRuntime Js { get; set; }

        [Inject]
        public ITramiteVirtualService tramitesVirtualService { get; set; }

        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        string MotivoCancelacion { get; set; }

        [Parameter]
        public string CustomModalClass { get; set; }

        [Parameter]
        public string CustomStyleModal { get; set; }

        [Parameter]
        public string ModalTitle { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        AuthenticationState context { get; set; }


        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        async Task ObtenerInformacion(string titulo, string cuandi, List<ComparecientesIFrame> comparecientes)
        {
            context = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string notariaId = context.User.Claims.Where(x => x.Type == "NotariaId").Select(x => x.Value).FirstOrDefault();
            long notariaID = 0;
            if (!string.IsNullOrEmpty(notariaId))
                long.TryParse(notariaId, out notariaID);
            var objetoMiFirma = await tramitesVirtualService.ObtenerInformacionMiFirma(notariaID);
            if (objetoMiFirma != null)
            {
                objetoMiFirma.Titulo = titulo;
                objetoMiFirma.CUANDI = cuandi;
                objetoMiFirma.Comparecientes = comparecientes;
                await Js.InvokeVoidAsync("VisualizarModelTramiteVirtual", objetoMiFirma);
            }
        }

        public async void Open(string titulo, string cuandi, List<ComparecientesIFrame> listPersonas)
        {
            ModalDisplay = "block;";
            ModalClass = "Show";
            ShowBackdrop = true;
            await ObtenerInformacion(titulo, cuandi, listPersonas);
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }

    }
}