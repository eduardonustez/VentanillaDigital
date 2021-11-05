using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
namespace PortalCliente.Components.RegistroTramite
{
    public partial class CancelarTramiteModal : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        int motivoSelec;
        string MotivoCancelacion { get; set; }
        [Parameter]
        public EventCallback<string> MotivoCancelacionChanged { get; set; }
        private Dictionary<int, string> opcMotivo = new Dictionary<int, string>();

        protected override void OnInitialized()
        {
            FillMotivos();
            base.OnInitialized();
        }


        private void FillMotivos()
        {
            opcMotivo.Add(1, "Interrupción voluntaria del compareciente");
            opcMotivo.Add(2, "Imposibilidad de capturar huella");
            opcMotivo.Add(3, "Incumplimiento de requisitos");
            opcMotivo.Add(4, "Otro");
        }
        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "Show";
            ShowBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }
        async void setMotivoCancelacion()
        {
            if (motivoSelec > 0 && motivoSelec < 4)
                MotivoCancelacion = opcMotivo[motivoSelec];
            await MotivoCancelacionChanged.InvokeAsync(MotivoCancelacion);
            await Js.InvokeVoidAsync("removerTramiteDeURL");

            MotivoCancelacion = "";
            motivoSelec = 0;
            Close();
        }

    }
}