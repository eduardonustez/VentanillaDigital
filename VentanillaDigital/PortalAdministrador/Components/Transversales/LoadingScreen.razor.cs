using Microsoft.AspNetCore.Components;
using PortalAdministrador.Services.LoadingScreenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.Transversales
{
    public partial class LoadingScreen
    {

        private bool _show;

        private string _message;

        protected override Task OnInitializedAsync()
        {
            LoadingScreenService.ShowEvent += Show;
            LoadingScreenService.HideEvent += Hide;
            return base.OnInitializedAsync();
        }

        private void Show (object sender, string message)
        {
            _message = message;
            _show = true;
            StateHasChanged();
        }

        private void Hide (object sender, EventArgs args)
        {
            _show = false;
            StateHasChanged();
        }
    }
}
