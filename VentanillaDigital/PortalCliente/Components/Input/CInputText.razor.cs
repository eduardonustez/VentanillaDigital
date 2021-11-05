using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
namespace PortalCliente.Components.Input
{
    public partial class CInputText : ComponentBase
    {
        [Parameter]
        public string Text { get; set; }
        // {
        //     get => _text;
        //     set
        //     {
        //         if (_text == value) return;

        //         _text = value;
        //         TextChanged.InvokeAsync(value);
        //     }
        // }

        [Parameter]
        public EventCallback<string> TextChanged { get; set; }

        [Parameter]
        public string Placeholder { get; set; }

        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public bool IsDisabled { get; set; }

        private string _text;

        private async Task OnInputChange(ChangeEventArgs args)
        {
             Text = (string)args.Value;
             await TextChanged.InvokeAsync(Text);
        }
    }
}