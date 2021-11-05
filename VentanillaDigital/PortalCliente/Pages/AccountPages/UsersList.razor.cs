using ApiGateway.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using PortalCliente.Data.Account;
using PortalCliente.Functions;
using PortalCliente.Services;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortalCliente.Pages.AccountPages
{
    public partial class UsersList : ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider _sessionStorageService { get; set; }

        [Inject]
        private IAccountService accountService { get; set; }

        [Inject]
        NotificationService notificationService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected bool IsDisabledSubmitEmail { get; set; } = false;
        protected string IsDisabledRol { get; set; } = string.Empty;

        int rol = 0;
        string genero = "";
        int tipoIdentificacion = 0;
        string identificacion = "";
        string nombres = "";
        string apellidos = "";
        string celular = "";
        string correo = "";
        string cargo = "";
        string area = "";
        int notaria = 0;
        string Estado = string.Empty;
        string Mensaje = string.Empty;
        string EstadoEliminacion = string.Empty;
        string MensajeEliminacion = string.Empty;
        public bool usuariosEliminados = false;
        bool showSpinner;

        bool isNew = true;
        public string ModalBtnsJustification = "";
        public string ModalTitle = "";
        public string ModalSubmitBtn = "";
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;

        public string ModalDisplayRemove = "none;";
        public string ModalClassRemove = "";
        public bool ShowBackdropRemove = false;

        public string ModalDisplayCreateEditUser = "none;";
        public string ModalClassCreateEditUser = "";
        public bool ShowBackdropCreateEditUser = false;

        public string mensajeErrorClase = "";
        public string errorInputClase = "";

        public string modalCrearUsuarioId = "modalCrearUsuario";

        bool showModal = false;
        bool userNotFound = false;

        List<AspNetUsersResponseDTO> usuarios;
        AspNetUsersResponseDTO selectedUser;
        object[,] registros;

        [Parameter]
        public UserDeleteList ListaUsuariosSeleccionados { get; set; }

        string[] columnas = { "Tipo", "Identificación", "Nombre completo", "Rol", "Contacto", "" };
        protected override async Task OnInitializedAsync()
        {
            await ObtenerUsuarios();
        }

        async Task ObtenerUsuarios()
        {
            AuthenticatedUser userName = await GetAuthenticatedUser();
            notaria = int.Parse(userName.Notaria);

            usuarios = await accountService.ListarUsuariosRegistrados(notaria);

            if (usuarios != null)
            {
                registros = usuarios.To2DArray(u => u.Id, u => u.TipoDocumento, u => u.NumeroIdentificacion, u => u.NombreCompleto.ToUpper(), u => u.RolName, u => u.Email);
            }

            StateHasChanged();
        }

        public async Task<AuthenticatedUser> GetAuthenticatedUser()
        {
            var userActual = (CustomAuthenticationStateProvider)_sessionStorageService;
            return await userActual.GetAuthenticatedUser();
        }

        protected void closeModal()
        {
            showModal = false;

            rol = 0;
            tipoIdentificacion = 0;
            identificacion = "";
            nombres = "";
            apellidos = "";
            celular = "";
            correo = "";
            cargo = "";
            area = "";

            userNotFound = false;
            StateHasChanged();
        }

        protected void CloseModalEditCreateUser()
        {
            rol = 0;
            tipoIdentificacion = 0;
            identificacion = "";
            nombres = "";
            apellidos = "";
            celular = "";
            correo = "";
            cargo = "";
            area = "";
            userNotFound = false;
            StateHasChanged();
        }

        public async Task ClosedModalWindow(string Id)
        {
            await JsRuntime.InvokeVoidAsync("cerrarModal", Id);
        }

        protected void validateInput(ChangeEventArgs e)
        {
            celular = e.Value.ToString();

            if ((celular.Length < 7) || (celular.Length > 12))
            {
                errorInputClase = "error-input";
                mensajeErrorClase = "mostrar";
            }
            else
            {
                errorInputClase = "";
                mensajeErrorClase = "";
            }

        }

        private async Task<bool> SaveUser()
        {
            try
            {
                IsDisabledRol = string.Empty;
                if (!Regex.IsMatch(correo, @"^[a-zA-Z0-9_\.-]+@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}", RegexOptions.IgnoreCase))
                {
                    return false;
                }
                if (isNew)
                {
                    var user = new UserAccount
                    {
                        RolId = rol,
                        TipoIdentificacionId = tipoIdentificacion,
                        EmailNotaria = correo,
                        EmailPersonal = correo,
                        NumeroDocumento = identificacion,
                        Nombres = nombres,
                        Apellidos = apellidos,
                        NumeroCelular = celular,
                        Area = area,
                        Cargo = cargo,
                        EsUsuarioNotaria = true,
                        personaDatos = null,
                        NotariaId = notaria,
                        Genero = string.IsNullOrEmpty(genero) ? 0 : (sbyte)Enum.Parse<Genero>(genero)
                    };
                    showSpinner = true;
                    await accountService.UserRegister(user);
                    showSpinner = false;
                    Estado = "Usuario creado";
                    Mensaje = "Se ha creado el usuario " + correo + " en su Notaria. Se enviará un correo electrónico al funcionario para ingresar al sistema.";
                }
                else
                {
                    AuthenticatedUser userName = await GetAuthenticatedUser();
                    if (userName.Rol != "Administrador" && selectedUser.RolName == "Administrador")
                    {
                        rol = 1;
                    }
                    var user = new UpdateUserAccount
                    {
                        Id = selectedUser != null ? selectedUser.Id : "",
                        RolId = rol,
                        TipoIdentificacionId = tipoIdentificacion,
                        EmailNotaria = correo,
                        EmailPersonal = correo,
                        EmailAnterior = selectedUser.Email,
                        NumeroDocumento = identificacion,
                        Nombres = nombres,
                        Apellidos = apellidos,
                        NumeroCelular = celular,
                        Area = area,
                        Cargo = cargo,
                        EsUsuarioNotaria = true,
                        personaDatos = null,
                        NotariaId = notaria,
                        Genero = string.IsNullOrEmpty(genero) ? 0 : (sbyte)Enum.Parse<Genero>(genero)
                    };
                    showSpinner = true;
                    await accountService.UserUpdate(user);
                    showSpinner = false;
                    Estado = "Usuario actualizado";
                    Mensaje = "Se ha actualizado correctamente los datos del usuario " + correo;
                }

                await ObtenerUsuarios();
                CloseModalEditCreateUser();
                await ClosedModalWindow(modalCrearUsuarioId);
                ShowMessage(NotificationSeverity.Success, Estado, Mensaje);
                return true;
            }
            catch (Exception e)
            {
                if (isNew)
                {
                    Estado = "Error al crear el usuario";
                    Mensaje = $"Error al crear el usuario {correo}. {e.Message}";
                }
                else
                {
                    Estado = "Error al editar el usuario";
                    Mensaje = $"Error al editar el usuario {correo}.\nRazón: {e.Message}";
                }
                showSpinner = false;
                CloseModalEditCreateUser();
                await ClosedModalWindow(modalCrearUsuarioId);
                ShowMessage(NotificationSeverity.Error, Estado, Mensaje);
                await ObtenerUsuarios();
                return await Task.FromResult(false);
            }
        }

        protected void ShowMessage(NotificationSeverity notificationSeverity, string Summary, string Message)
        {
            var message = new NotificationMessage()
            {
                Severity = notificationSeverity,
                Summary = $"{Summary}",
                Detail = $"{Message}",
                Duration = 4000
            };
            notificationService.Notify(message);
        }

        public void setCreateModal()
        {
            ModalTitle = "Nuevo usuario";
            ModalSubmitBtn = "Crear usuario";
            ModalBtnsJustification = "justify-content-end";
            IsDisabledRol = string.Empty;
            isNew = true;
            selectedUser = null;
            closeModal();
            StateHasChanged();
        }

        async void setUpdateModal(string id)
        {
            IsDisabledSubmitEmail = false;
            ModalTitle = "Editar usuario";
            ModalSubmitBtn = "Editar usuario";
            ModalBtnsJustification = "justify-content-between";
            isNew = false;
            selectedUser = usuarios.Find(u => id == u.Id);
            AuthenticatedUser userName = await GetAuthenticatedUser();
            if (userName.Rol != "Administrador" && selectedUser.RolName == "Administrador")
                IsDisabledRol = "none";
            else
                IsDisabledRol = string.Empty;
            switch (selectedUser.RolName)
            {
                case "Administrador":
                case "Notario Principal":
                    rol =1;
                    break;
                case "Notario Encargado":
                    rol = 3;
                    break;
                default:
                    rol = 2;
                    break;
            }
            //rol = selectedUser.RolName == "Notario Encargado" ? 3 : 2;
            tipoIdentificacion = 1;
            correo = selectedUser.Email;
            identificacion = selectedUser.NumeroIdentificacion;
            nombres = selectedUser.Nombres;
            apellidos = selectedUser.Apellidos;
            celular = selectedUser.NumeroCelular;
            area = selectedUser.Area;
            cargo = selectedUser.Cargo;
            genero = selectedUser.Genero == null ? "" : Enum.Parse<Genero>(selectedUser?.Genero.ToString()).ToString();
            StateHasChanged();
        }

        public async Task<bool> sendVerificationEmail()
        {
            try
            {
                IsDisabledSubmitEmail = true;
                await accountService.RecoveryPassword(selectedUser.Email);
                Estado = "Correo enviado";
                Mensaje = "Se ha enviado un correo para el ingreso del usuario a la cuenta " + correo;
                await ClosedModalWindow(modalCrearUsuarioId);
                ShowMessage(NotificationSeverity.Success, Estado, Mensaje);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Estado = "Correo no enviado";
                Mensaje = "Ha ocurrido un error al enviar el correo para el ingreso del usuario a la cuenta. " + e.Message;
                await ClosedModalWindow(modalCrearUsuarioId);
                ShowMessage(NotificationSeverity.Error, Estado, Mensaje);
                return await Task.FromResult(false);
            }
        }

        private void CheckboxClickedUsuarios(string Identificacion, object esChecked)
        {
            if ((bool)esChecked)
            {
                if (!ListaUsuariosSeleccionados.Identificacion.Contains(Identificacion))
                {
                    ListaUsuariosSeleccionados.Identificacion.Add(Identificacion);
                }
            }
            else
            {
                if (ListaUsuariosSeleccionados.Identificacion.Contains(Identificacion))
                {
                    ListaUsuariosSeleccionados.Identificacion.Remove(Identificacion);
                }
            }
            StateHasChanged();
        }

        private void OpenRemoveUsers(string id)
        {
            ModalDisplayRemove = "block;";
            ModalClassRemove = "Show";
            ShowBackdropRemove = true;
            selectedUser = usuarios.Find(u => id == u.Id);
            rol = selectedUser.RolName == "Notario Encargado" ? 3 : 2;
            tipoIdentificacion = 1;
            correo = selectedUser.Email;
            identificacion = selectedUser.NumeroIdentificacion;
            nombres = selectedUser.Nombres;
            apellidos = selectedUser.Apellidos;
            celular = selectedUser.NumeroCelular;
            EstadoEliminacion = "Eliminar usuario";
            MensajeEliminacion = $"¿Desea eliminar al usuario {correo}?";
        }

        private async Task RemoveUsersSelected()
        {
            if (selectedUser != null)
            {
                UserDelete ud = new UserDelete()
                {
                    Id = selectedUser.Id,
                    Email = selectedUser.Email
                };

                try
                {
                    Estado = "Eliminar usuario";
                    var resultado = await accountService.UserDelete(ud);
                    if (resultado)
                    {
                        Mensaje = $"Se ha eliminado correctamente al usuario {selectedUser.Email}";
                        ShowMessage(NotificationSeverity.Info, string.Empty, Mensaje);
                        await ObtenerUsuarios();
                    }
                    else
                    {
                        Mensaje = $"Error al eliminar al usuario {selectedUser.Email}, si el error persiste por favor comunicarse con el administrador";
                        ShowMessage(NotificationSeverity.Error, string.Empty, Mensaje);
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        Estado = "Error";
                        Mensaje = ex.Message;
                        ShowMessage(NotificationSeverity.Error, string.Empty, Mensaje);
                    }
                }
                finally
                {
                    CloseRemoveUsers();
                }
            }
        }

        public void CloseRemoveUsers()
        {
            EstadoEliminacion = string.Empty;
            MensajeEliminacion = string.Empty;
            ModalDisplayRemove = "none";
            ModalClassRemove = "";
            ShowBackdropRemove = false;
            selectedUser = null;
            closeModal();
            StateHasChanged();
        }
    }

    public enum Genero
    {
        Masculino = 1,
        Femenino = 2
    }
}
