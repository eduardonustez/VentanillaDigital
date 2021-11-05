using Microsoft.AspNetCore.Components;
using PortalCliente.Components.RegistroTramite;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Services;
using System.Linq;

namespace PortalCliente.Shared
{
    public partial class NavOperario : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider _authenticationStateProvider { get; set; }

        [Parameter]
        public EventCallback<string> onListenClickMenu { get; set; }
        [Parameter]
        public bool usePreventDefault { get; set; }

        [Parameter]
        public bool NotariaConvenioVirtual { get; set; }
        string lastlinkClicked = "";

        void onClickMenu()
        {
            onListenClickMenu.InvokeAsync(lastlinkClicked);
        }
        void onClickLink(string url)
        {
            lastlinkClicked = url;
        }
    }
}