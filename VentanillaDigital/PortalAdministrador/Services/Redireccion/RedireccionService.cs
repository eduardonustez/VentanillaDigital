using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Services.Notario;
using System.Threading.Tasks;

namespace PortalAdministrador.Services.RedireccionLogin
{

    public class RedireccionService : IRedireccionService
    {
        INotarioService _notarioService;

        AuthenticationStateProvider _authenticationStateProvider;

        NavigationManager _navigationManager;

        public RedireccionService(INotarioService notarioService,
            AuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager)
        {
            _notarioService = notarioService;
            _authenticationStateProvider = authenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task IrAPaginaInicial()
        {
            var usuarioAutenticado = await ((CustomAuthenticationStateProvider)_authenticationStateProvider)?.GetAuthenticatedUser();

            if (usuarioAutenticado.Rol == "ADMIN" || usuarioAutenticado.Rol == "USER") _navigationManager.NavigateTo("tramite");
        }
    }
}
