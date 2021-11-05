using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Services;
using PortalAdministrador.Services.RedireccionLogin;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.AccountPages
{
    public partial class Login : ComponentBase
    {
        private UserLogin user = new UserLogin();
        private string LoginMessage;
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        public IAccountService AccountService { get; set; }
        [Inject]
        public IRedireccionService RedireccionService { get; set; }
        private Version Version { get => Assembly.GetExecutingAssembly().GetName().Version; }
        private string VersionStr { get => $"{Version.Major}.{Version.Minor}.{Version.Build}{Version.Revision}"; }
        bool showSpinner;

        protected override async Task OnInitializedAsync()
        {
            var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            if (state.User.Identity.IsAuthenticated)
            {
                await RedireccionService.IrAPaginaInicial();
            }
        }

        private async Task<bool> Redirect(AuthenticatedUser user)
        {
            showSpinner = false;
            Console.WriteLine($"😂😂 {user.IsAuthenticated}");
            if (user != null && user.IsAuthenticated)
            {
                await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(user);
                await RedireccionService.IrAPaginaInicial();
                return true;
            }

            LoginMessage = "Usuario o Contraseña incorrectos. Verifica e intenta nuevamente";
            return false;
        }

        private async Task<bool> ValidateUser()
        {
            LoginMessage = null;
            showSpinner = true;
            var authenticatedUser = await AccountService.Login(user);
            return await Redirect(authenticatedUser);
        }
    }
}
