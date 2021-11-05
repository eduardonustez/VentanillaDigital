using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalCliente.Components.Transversales;
using PortalCliente.Data;
using PortalCliente.Data.Account;
using PortalCliente.Services;
using PortalCliente.Services.Notario;
using PortalCliente.Services.RedireccionLogin;
using PortalCliente.Services.Parametrizacion;
namespace PortalCliente.Pages.AccountPages
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

        [Inject]
        public INotarioService NotarioService { get; set; }
        [Inject]
        IParametrizacionServicio _parametrizacion{get;set;}

        private Version Version { get => Assembly.GetExecutingAssembly().GetName().Version; }

        private string VersionStr { get => $"{Version.Major}.{Version.Minor}.{Version.Build}{Version.Revision}"; }

        public Compareciente Compareciente { get; set; } = Compareciente.ObtenerNuevoCompareciente();

        private Guid? IdentificadorOTP { get; set; }
        private string OTP { get; set; }

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
            if (user != null)
            {
                if (user.IsAuthenticated && user.IdentificadorOTP == Guid.Empty)
                {
                    await ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(user);
                    await _parametrizacion.RegistrarMaquina();
                    await RedireccionService.IrAPaginaInicial();
                    return true;
                }
                else if (user.IsAuthenticated)
                {
                    IdentificadorOTP = user.IdentificadorOTP;
                    return true;
                }
                else
                {
                    if (IdentificadorOTP == null)
                        LoginMessage = "Usuario o Contraseña incorrectos. Verifica e intenta nuevamente";
                    else
                        LoginMessage = "PIN incorrecto. Verifica e intenta nuevamente";

                    return false;
                }

            }
            LoginMessage = "Algo ha salido mal, por favor vuelve a intentarlo";
            return false;
        }

        private async Task<bool> ValidateUser()
        {
            LoginMessage = null;
            showSpinner = true;
            AuthenticatedUser authenticatedUser;
            if (IdentificadorOTP != null)
            {
                authenticatedUser = await AccountService.Login(user, IdentificadorOTP.Value, OTP);
                return await Redirect(authenticatedUser);
            }

            authenticatedUser = await AccountService.Login(user);
            return await Redirect(authenticatedUser);
        }

    }
}
