using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using Infraestructura.Transversal.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PortalAdministracion.Services.UsuarioAdministracion;
using PortalAdministrador.Components.Shared;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Validators;
using Radzen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Pages.Usuarios
{
    public partial class UsuariosPortalAdmin : ComponentBase
    {
        [Inject]
        IJSRuntime Js { get; set; }

        [Inject]
        IUsuarioAdministracionService _usuarioAdministracionService { get; set; }

        [Inject]
        Radzen.NotificationService notificationService { get; set; }

        #region Propiedades
        public List<(string, string)> Columns { get; set; } = new List<(string, string)>
        {
            ("Correo", "Login")
            , ("Perfil", "Rol")
        };
        public GridControl<UsuarioAdministracionModel> Grid { get; set; }
        public bool consultando { get; set; }
        public string CorreoFilter { get; set; }
        public List<UsuarioAdministracionModel> Usuarios { get; set; }
        public int TotalPages { get; set; }
        public long TotalRows { get; set; }
        UsuarioPortalAdminResponseDTO selectedUser;
        string Estado = string.Empty;
        string Mensaje = string.Empty;
        bool esUsuarioCreado;
        UsuarioPortalAdmin userAccount = ResetUser();
        string TituloModal;
        public string modalCrearUsuarioId = "createUserModalCenter";
        public string deleteUserModalCenter = "deleteUserModalCenter";
        public bool FiltrosVisibles { get; set; } = true;
        bool showSpinner;

        #endregion

        #region Overrides
        protected override async Task OnInitializedAsync()
        {
            await ConsultarUsuarios(1);
        }
        #endregion

        public void Refrescar()
        {
            CorreoFilter = string.Empty;
            Grid.ResetIndex();
            //await LimpiarCampos();
        }
        public async void OnChangePage(int indice)
        {
            await ConsultarUsuarios(indice);
        }
        private async Task OnEditClick(UsuarioAdministracionModel userAccount2)
        {
            TituloModal = "Actualizar información de usuario portal Administrador";
            userAccount = ResetUser();
            esUsuarioCreado = true;
            var usuarioSeleccionado = await _usuarioAdministracionService.ObtenerUsuarioPortalAdmin(userAccount2.Id);
            if (usuarioSeleccionado != null)
            {
                userAccount.Email = usuarioSeleccionado.Login;
                userAccount.Rol = usuarioSeleccionado.Rol;
                userAccount.Id = usuarioSeleccionado.UsuarioAdministracionId;
                await Js.InvokeVoidAsync("cerrarModal", "createUserModalCenter");
                StateHasChanged();
            }
            else
            {
                Estado = "No se ha encontrado el usuario.";
                Mensaje = "No se ha encontrado el usuario solicitado. Probablemente haya sido eliminado o no es permitido editar su propio usuario";
                ShowMessage(NotificationSeverity.Warning, Estado, Mensaje);
            }
        }

        public async Task MostrarFiltros(bool mostrarOverlay = true)
        {
            await Js.InvokeVoidAsync("mostrarFiltros", mostrarOverlay);
        }

        public async Task ConsultarUsuarios(int indice)
        {
            consultando = true;
            var model = new DefinicionFiltro
            {
                IndicePagina = indice,
                TotalRegistros = 0,
                RegistrosPagina = 10,
                Filtros = new List<Filtro>()
            };

            if (!string.IsNullOrEmpty(CorreoFilter)) model.Filtros.Add(new Filtro("Login", CorreoFilter));

            var res = await _usuarioAdministracionService.ObtenerUsuariosAdministracionPaginado(model);

            if (res != null)
            {
                Usuarios = res.Data?.ToList();
                TotalRows = res.TotalRows;
                TotalPages = res.Pages;
            }

            consultando = false;

            StateHasChanged();
        }

        void ConfigurarModal()
        {
            esUsuarioCreado = false;
            userAccount = ResetUser();
            TituloModal = "Nuevo notario principal";
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

        private async Task OnDeleteItem(UsuarioAdministracionModel usuarioModel)
        {
            selectedUser = await _usuarioAdministracionService.ObtenerUsuarioPortalAdmin(usuarioModel.Id);
            if (selectedUser != null)
            {
                await Js.InvokeVoidAsync("cerrarModal", deleteUserModalCenter);
            }
            else
            {
                Estado = "No se ha encontrado el usuario.";
                Mensaje = "No se ha encontrado el usuario solicitado. Probablemente haya sido eliminado o no es permitido eliminar su propio usuario";
                ShowMessage(NotificationSeverity.Warning, Estado, Mensaje);
            }
        }

        public async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await Filtrar();
            }
        }

        public async Task LimpiarCampos()
        {
            CorreoFilter = string.Empty;
            await ConsultarUsuarios(1);
        }

        public async Task Filtrar()
        {
            await ConsultarUsuarios(indice: 1);
            Grid.ResetIndex();
            await MostrarFiltros(false);
        }

        static UsuarioPortalAdmin ResetUser()
        {
            return new UsuarioPortalAdmin()
            {
                Email = string.Empty,
                //EsUsuarioNotaria = true,
                Rol = "Administrador"
            };
        }

        private async Task HandleValidSubmit()
        {
            await SaveUserAlt();
        }

        private async Task SaveUserAlt()
        {
            showSpinner = true;
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                if (esUsuarioCreado)
                {
                    await _usuarioAdministracionService.ActualizarUsuarioPortalAdmin(userAccount);
                    Estado = "Usuario actualizado";
                    Mensaje = "Se ha actualizado correctamente los datos del usuario " + userAccount.Email;
                }
                else
                {
                    var respuestaCreacion = await _usuarioAdministracionService.UsuarioRegistroPortalAdmin(userAccount);
                    if (!respuestaCreacion.Error)
                    {
                        //Notificar Email
                        var esNotificado = await _usuarioAdministracionService.NotificacionPwdUsuarioPortalAdmin(userAccount.Email);
                        if (esNotificado)
                        {
                            Estado = "Usuario creado";
                            Mensaje = "Se ha creado el usuario " + userAccount.Email + ". Se enviará un correo electrónico al funcionario para ingresar al sistema.";
                        }
                        else
                        {
                            Estado = "Error notificación";
                            Mensaje = "Error al intentar notificar al usuario " + userAccount.Email + ". Por favor eliminar el usuario e intentar de nuevo realizar la creación del usuario.";
                        }
                    }
                    else
                    {
                        Estado = "Error al crear el usuario";
                        Mensaje = $"Error al crear el usuario {userAccount.Email}, intente de nuevo";
                    }
                }
                await ClosedModalWindow(modalCrearUsuarioId);
                await LimpiarCampos();
                ShowMessage(NotificationSeverity.Success, Estado, Mensaje);
                showSpinner = false;
                esUsuarioCreado = false;
                userAccount = ResetUser();
                StateHasChanged();
            }
            catch (Exception e)
            {
                Estado = "Error al crear el usuario";
                Mensaje = $"{e.Message}";
                showSpinner = false;
                await ClosedModalWindow(modalCrearUsuarioId);
                ShowMessage(NotificationSeverity.Error, Estado, Mensaje);
                await LimpiarCampos();
                userAccount = ResetUser();
                esUsuarioCreado = false;
                StateHasChanged();
            }
        }
        public async Task ClosedModalWindow(string Id)
        {
            await Js.InvokeVoidAsync("cerrarModal", Id);
        }

        private async Task RemoveUsersSelected()
        {
            if (selectedUser != null)
            {
                EliminarUsuarioPortalAdmin usuarioAEliminar = new EliminarUsuarioPortalAdmin()
                {
                    Id = selectedUser.UsuarioAdministracionId,
                    Email = selectedUser.Login,
                    Rol = selectedUser.Rol
                };

                try
                {
                    showSpinner = true;
                    Estado = "Eliminar usuario";
                    var resultado = await _usuarioAdministracionService.EliminarUsuarioPortalAdmin(usuarioAEliminar);
                    if (resultado)
                    {
                        Mensaje = $"Se ha eliminado correctamente al usuario {selectedUser.Login}";
                        ShowMessage(NotificationSeverity.Info, string.Empty, Mensaje);
                    }
                    else
                    {
                        Mensaje = $"Error al eliminar al usuario {selectedUser.Login}, si el error persiste por favor comunicarse con el administrador";
                        ShowMessage(NotificationSeverity.Error, string.Empty, Mensaje);
                    }
                    await LimpiarCampos();
                }
                catch (Exception ex)
                {
                    Estado = "Error";
                    Mensaje = ex.Message;
                    ShowMessage(NotificationSeverity.Error, string.Empty, Mensaje);
                    showSpinner = false;
                }
                finally
                {
                    showSpinner = false;
                    await Js.InvokeVoidAsync("cerrarModal", deleteUserModalCenter);
                    StateHasChanged();
                }
            }
        }
    }
}
