using Microsoft.AspNetCore.Components;
using System;
namespace PortalCliente.Components.Input
{
    public partial class CInputButton : ComponentBase
    {
        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public EventCallback<bool> ButtonClickChanged { get; set; }

        [Parameter]
        public bool showSpinner { get; set; }
        [Parameter]
        public string useclass { get; set; }


        protected async void onClickButton(){
            await ButtonClickChanged.InvokeAsync(true);
        }

    }
}