using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalCliente.Components.Transversales
{
    public partial class ModalQuestion : ComponentBase
    {
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        int motivoSelec;
        string MotivoCancelacion { get; set; }
        [Parameter]
        public string ModalTitle { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public EventCallback OnOkClick { get; set; }

        protected override void OnInitialized()
        {

            base.OnInitialized();
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

        public async Task Ok()
        {
            this.Close();
            await OnOkClick.InvokeAsync(null);
        }

    }
}