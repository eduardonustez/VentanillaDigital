using Microsoft.AspNetCore.Components;
using PortalCliente.Services;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using System.Text;

namespace PortalCliente.Components.Notario
{
    public partial class PinCertificado : ComponentBase
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        [Inject]
        public IActaNotarialService actaNotarialService { get; set; }
        [Parameter]
        public long TramiteId { get; set; }

        private string _pin;
        [Parameter]
        public string Pin
        {
            get => _pin;
            set
            {
                if (_pin == value) return;

                _pin = value;
                PinChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> PinChanged { get; set; }

        [Parameter]
        public string ClaseFirma { get; set; }

        [Parameter]
        public bool ModalAutorizar { get; set; }

        [Parameter]
        public bool ModalCambioFirma { get; set; }
        bool preventDefault = false;

        private byte codAscii;
        string _p1;
        string p1
        {
            get => _p1;
            set
            {
                if (_p1 == value) return;

                _p1 = value;
                SetPin();
            }
        }
        string _p2;
        string p2
        {
            get => _p2;
            set
            {
                if (_p2 == value) return;
                _p2 = value;
                SetPin();
            }
        }
        string _p3;
        string p3
        {
            get => _p3;
            set
            {
                if (_p3 == value) return;
                _p3 = value;
                SetPin();
            }
        }
        string _p4;
        string p4
        {
            get => _p4;
            set
            {
                if (_p4 == value) return;
                _p4 = value;
                SetPin();
            }
        }
        string tipoDefecto = "password";
        string iconoVisibilidad = "visibility";
        bool errors = false;
        private readonly string idModal = "modalForm";
        private readonly string idInput = "inputUno";

        protected override void OnInitialized()
        {
            LimpiarInputs();
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
                await JsRuntime.InvokeVoidAsync("focusInputModal", idModal, idInput);
                await JsRuntime.InvokeVoidAsync("skipToNextInput",ClaseFirma);
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
            if (codAscii == 69)
            {
                // action for enter
            }
        }
        private void SetPin(){
            Pin = string.Concat(p1, p2, p3, p4);
        }
    }
}
