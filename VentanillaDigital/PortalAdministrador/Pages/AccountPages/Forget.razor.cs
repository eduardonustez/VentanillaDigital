using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Services;

namespace PortalAdministrador.Pages.AccountPages
{
    public partial class Forget : ComponentBase
    {
        private RecoveryAccount account = new RecoveryAccount();
        private string ForgetMessage;
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAccountService AccountService { get; set; }
        public bool esEnlaceEnviado = false;


        public async Task HandleReestablecerContrasena()
        {
            string resultado = await AccountService.RecoveryPassword(account.Email);
            if (string.IsNullOrEmpty(resultado))
            {
                esEnlaceEnviado = true;
                string email = EnmascararEmail(account.Email);
                ForgetMessage = $"Se ha enviado un correo electrónico a { email }. Sigue las instrucciones para reestablecer la contraseña";
                Open();
                await Task.FromResult(true);
            }
            else
            {
                esEnlaceEnviado = false;
                ForgetMessage = resultado;
                await Task.FromResult(false);
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

        private string EnmascararEmail(string email)
        {
            string usuario = email;
            string[] separada = usuario.Split('@');
            int inicio = 3;
            int final = 3;
            int longitud;
            if (separada[0].Length > inicio + final)
                longitud = separada[0].Length - final - inicio;
            else
                longitud = 1;

            separada[0] = separada[0].Remove(inicio, longitud).Insert(inicio, new string('*', longitud));
            usuario = string.Join("@", separada);
            return usuario;
        }
    }
}
