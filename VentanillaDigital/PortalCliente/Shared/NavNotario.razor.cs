using ApiGateway.Contratos.Models.NotariaVirtual;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PortalCliente.Shared
{
    public partial class NavNotario : ComponentBase
    {
        [Inject]
        public ITramiteVirtualService _tramitesVirtualService { get; set; }

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