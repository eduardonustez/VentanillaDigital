using Microsoft.AspNetCore.Components;
using PortalAdministrador.Services;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace PortalAdministrador.Components.Notario
{
    public partial class PinFirma : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }
        [Parameter]
        public long TramiteId { get; set; }

        [Parameter]
        public EventCallback<string> SetPin { get; set; }

        [Parameter]
        public string ClaseFirma { get; set; }

        [Parameter]
        public bool ModalAutorizar { get; set; }

        [Parameter]
        public bool ModalCambioFirma { get; set; }
        bool preventDefault = false;

        private byte codAscii;

        string p1 = "";
        string p2 = "";
        string p3 = "";
        string p4 = "";
        string tipoDefecto = "password";
        string iconoVisibilidad = "visibility";
        bool errors = false;

        async void Firmar()
        {
            var pinCode = string.Concat(p1, p2, p3, p4);
            if (pinCode.Length == 4)
            {
                LimpiarInputs();
                await SetPin.InvokeAsync(pinCode);
                await JsRuntime.InvokeVoidAsync("focusPrimerInput");
            }
            else
            {
                errors = true;
            }
        }

        async void LimpiarInputs()
        {
            errors = false;
            p1 = p2 = p3 = p4 = "";
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JsRuntime.InvokeVoidAsync("skipToNextInput");
                await JsRuntime.InvokeVoidAsync("skipToNextInputRechazar");
            }
        }

        public void VerTodo()
        {
            tipoDefecto = tipoDefecto == "password" ? "tel" : "password";
            iconoVisibilidad = iconoVisibilidad == "visibility" ? "visibility_off" : "visibility";
        }

        private void KeyDown(KeyboardEventArgs args)
        {

            codAscii = Encoding.ASCII.GetBytes(args.Key.ToString())[0];

            if ((codAscii >= 48 && codAscii <= 57) ||
                 codAscii == 66 || codAscii == 68 ||
                 codAscii == 65 || codAscii == 84)
            {

                this.preventDefault = false;
            }
            else
            {
                this.preventDefault = true;

            }

        }

    }
}
