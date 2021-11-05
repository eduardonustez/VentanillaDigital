using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.Transversales
{
    public partial class Tab
    {
        [CascadingParameter]
        public TabSelector Parent { get; set; }

        [CascadingParameter]
        public RenderFragment<Tab> NavigatorLink { get; set; }


        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public bool Disabled { get; set; } = false;

        [Parameter]
        public bool Display { get; set; } = true;

        [Parameter]
        public EventCallback<bool> DisabledChanged { get; set; }

        [Parameter]
        public EventCallback OnNext { get; set; }

        public bool Active { get => Parent.Current == Name; }

        protected override async Task OnInitializedAsync()
        {
            await Parent.AddTab(this);
            await base.OnInitializedAsync();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            bool disabled, display;
            bool currDisabled, currDisplay;
            bool disabledObt, displayObt;

            disabledObt = parameters.TryGetValue<bool>("Disabled", out disabled);
            displayObt = parameters.TryGetValue<bool>("Display", out display);

            currDisabled = Disabled;
            currDisplay = Display;

            await base.SetParametersAsync(parameters);

            if ((disabledObt && disabled != currDisabled)
                || (displayObt && display != currDisplay))
            {
                await Parent.UpdateNextAndPreviousDisabled();
            }
        }

        public async Task Select ()
        {
            await Parent.Select(Name);
        }
    }
}
