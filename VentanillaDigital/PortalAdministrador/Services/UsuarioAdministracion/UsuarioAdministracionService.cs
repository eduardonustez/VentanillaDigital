using ApiGateway.Contratos.Models;
using ApiGateway.Contratos.Models.Account;
using Blazored.SessionStorage;
using GenericExtensions;
using Infraestructura.Transversal.Models;
using PortalAdministrador.Data.Account;
using PortalAdministrador.Services;
using System;
using System.Threading.Tasks;

namespace PortalAdministracion.Services.UsuarioAdministracion
{
    public class UsuarioAdministracionService : IUsuarioAdministracionService
    {
        private readonly ICustomHttpClient _customHttpClient;

        public UsuarioAdministracionService(ISessionStorageService sessionStorageService, ICustomHttpClient customHttpClient)
        {
            _customHttpClient = customHttpClient;

        }

        public async Task<PaginableResponse<UsuarioAdministracionModel>> ObtenerUsuariosAdministracionPaginado(DefinicionFiltro definicionFiltro)
        {
            var resultado = await _customHttpClient.PostJsonAsync<PaginableResponse<UsuarioAdministracionModel>>("/api/UsuarioAdministracion/ObtenerPaginado", definicionFiltro);
            return resultado;
        }

        public async Task<UsuarioCreacionPortalAdminResponseDTO> UsuarioRegistroPortalAdmin(UsuarioPortalAdmin user)
        {
            var resultado = await _customHttpClient.PostJsonAsync<UsuarioCreacionPortalAdminResponseDTO>($"/api/UsuarioAdministracion/CrearUsuario", user);
            return resultado;
        }

        public async Task<UsuarioPortalAdminResponseDTO> ObtenerUsuarioPortalAdmin(long UsuarioAdminId)
        {
            try
            {
                var resultado = await _customHttpClient.GetJsonAsync<UsuarioPortalAdminResponseDTO>($"/api/UsuarioAdministracion/ObtenerUsuarioPortalAdmin/{UsuarioAdminId}");
                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UsuarioCreacionPortalAdminResponseDTO> ActualizarUsuarioPortalAdmin(UsuarioPortalAdmin user)
        {
            return await _customHttpClient.PostJsonAsync<UsuarioCreacionPortalAdminResponseDTO>($"/api/UsuarioAdministracion/ActualizarUsuarioAdmin/", user);
        }

        public async Task<bool> EliminarUsuarioPortalAdmin(EliminarUsuarioPortalAdmin eliminarUsuarioPortal)
        {
            return await _customHttpClient.PostJsonAsync<bool>($"/api/UsuarioAdministracion/EliminarUsuarioAdmin/", eliminarUsuarioPortal);
        }

        public async Task<bool> NotificacionPwdUsuarioPortalAdmin(string email)
        {
            NotificacionPasswordUsuarioPortalAdminModel notificarUsuarioAdmin = new NotificacionPasswordUsuarioPortalAdminModel { Email = email };
            return await _customHttpClient.PostJsonAsync<bool>($"/api/UsuarioAdministracion/NotificacionPwdUsuarioPortalAdmin/", notificarUsuarioAdmin);
        }

        public async Task<string> AsignarClaveUsuarioPortalAdmin(string token, string clave, string confirmacionClave, string email)
        {
            try
            {
                AsignarClaveUsuarioAdminRequest asignarPasswordUsuario = new AsignarClaveUsuarioAdminRequest
                {
                    Code = token
                    , Password = clave
                    , ConfirmPassword = confirmacionClave
                    , Email = email
                };
                return await _customHttpClient.PostJsonAsync<string>($"/api/UsuarioAdministracion/AsignarPasswordAdministracion/", asignarPasswordUsuario);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
