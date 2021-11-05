using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using PortalAdministrador.Helper;
using PortalAdministrador.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.AccountPages
{
    public partial class NewPassword : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAccountService AccountService { get; set; }

        [Parameter]
        public string pass { get; set; }
        public string confirpass { get; set; }
        bool banFail = false;
        private string Token = string.Empty;
        private string Email = string.Empty;
        private string NewPasswordMessage;        
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        protected override void OnInitialized()
        {
            bool esToken = NavigationManager.TryGetQueryString("token", out Token);
            bool esEmail = NavigationManager.TryGetQueryString("email", out Email);
            if (!esToken || !esEmail)
            {
                NavigationManager.NavigateTo("/");
            }
        }

        protected async Task HandleCambiarContrasena()
        {
            if (pass.Equals(confirpass))
            {
                string resultado = await AccountService.ResetPassword(Token, pass, confirpass, Email).ConfigureAwait(false);
                if (string.IsNullOrEmpty(resultado))
                {
                    banFail = false;
                    NewPasswordMessage = string.Empty;
                    Open();
                }
                NewPasswordMessage = resultado;
                banFail = true;
            }
            else
            {
                NewPasswordMessage = "Las contraseñas no coinciden";
                banFail = true;
            }
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
            NavigationManager.NavigateTo("/");
        }

    }
}
